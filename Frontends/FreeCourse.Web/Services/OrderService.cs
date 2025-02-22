using FreeCourse.Shared.Dto;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayments;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;
using System.Net.Http;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;
        // private readonly IPhotoStockService _photoStockService; pictureUrl'lerin alınması için

        public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            try
            {
                var basket = await _basketService.Get();

                var paymentInfoInput = new PaymentInfoInput()
                {
                    CardName = checkoutInfoInput.CardName,
                    CardNumber = checkoutInfoInput.CardNumber,
                    Expiration = checkoutInfoInput.Expiration,
                    CVV = checkoutInfoInput.CVV,
                    TotalPrice = basket.TotalPrice
                };
                var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

                if (!responsePayment)
                {
                    // loglama yapılacak odeme yapılamadı
                    return new OrderCreatedViewModel() { Error = "Ödeme alınamadı", IsSuccessful = false };
                }

                var orderCreateInput = new OrderCreateInput()
                {
                    BuyerId = _sharedIdentityService.GetUserId,
                    Address = new AddressCreateInput { Province = checkoutInfoInput.Province, District = checkoutInfoInput.District, Street = checkoutInfoInput.Street, Line = checkoutInfoInput.Line, ZipCode = checkoutInfoInput.ZipCode },
                };

                basket.BasketItems.ForEach(x =>
                {
                    var orderItem = new OrderItemCreateInput { ProductId = x.CourseId, Price = x.GetCurrentPrice, PictureUrl = "", ProductName = x.CourseName };
                    orderCreateInput.OrderItems.Add(orderItem);
                });

                var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);

                if (!response.IsSuccessStatusCode)
                {
                    // loglama yapılacak odeme alındı ama sipariş oluşturulamadı ve 5 sn sonra tekrar denenecek(var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);) istek
                    // retry mekanizması yapılacak ve alttakı kodlar silinecek, cunku odeme yapıldı
                    return new OrderCreatedViewModel() { Error = "Sipariş oluşturulamadı", IsSuccessful = false };
                }

                var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();

                orderCreatedViewModel.Data.IsSuccessful = true;
                var deneme = await _basketService.Delete();

                if (!deneme)
                {
                    // loglama yapılacak sepet silinemedi
                    // retry mekanizması yapılacak ve alttakı kodlar silinecek, cunku odeme yapıldı ve sipariş oluşturuldu
                    return new OrderCreatedViewModel() { Error = "Sepet silinemedi", IsSuccessful = false };
                }
                return orderCreatedViewModel.Data;
            }
            catch (Exception ex)
            {
                // loglama yapılacak
                return new OrderCreatedViewModel() { Error = "Sipariş oluşturulamadı", IsSuccessful = false };
            }
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

            if (response.Data == null)
            {
                return new List<OrderViewModel>();
            }

            return response.Data;
        }
        //rabbitMQ gonderilecek
        public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();
            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput { Province = checkoutInfoInput.Province, District = checkoutInfoInput.District, Street = checkoutInfoInput.Street, Line = checkoutInfoInput.Line, ZipCode = checkoutInfoInput.ZipCode },
            };

            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput { ProductId = x.CourseId, Price = x.GetCurrentPrice, PictureUrl = "", ProductName = x.CourseName };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice,
                Order = orderCreateInput
            };

            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            if (!responsePayment)
            {
                // loglama yapılacak odeme yapılamadı
                return new OrderSuspendViewModel() { Error = "Ödeme alınamadı", IsSuccessful = false };
            }

            var deleteBasket = await _basketService.Delete();

            if (!deleteBasket)
            {
                // loglama yapılacak sepet silinemedi ama ödeme alındı
                return new OrderSuspendViewModel() { Error = "Sepet silinemedi.", IsSuccessful = true };
            }
            return new OrderSuspendViewModel() { IsSuccessful = true };
        }
    }
}
