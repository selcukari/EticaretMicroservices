using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            try
            {
                //paymentDto ile ödeme işlemi gerçekleştir. gonderilen kuyruk ismi(create-order-service), send oldugu icin command tek service gonder
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

                var createOrderMessageCommand = new CreateOrderMessageCommand
                {
                    BuyerId = paymentDto.Order.BuyerId,
                    Province = paymentDto.Order.Address.Province,
                    District = paymentDto.Order.Address.District,
                    Street = paymentDto.Order.Address.Street,
                    Line = paymentDto.Order.Address.Line,
                    ZipCode = paymentDto.Order.Address.ZipCode,
                    OrderItems = paymentDto.Order.OrderItems?.Select(x => new OrderItem
                    {
                        PictureUrl = x.PictureUrl,
                        Price = x.Price,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName
                    }).ToList()
                };

                // rabbitMq kuyruga mesajı gönder, orderservice kuyrugu dinleyecek ve message'ı alacak, ama order service ayakta degilse message
                // kuyrukta bekleyecek orderservice ayaga kalktıgında message'ı alacak ve islemi gerceklestirecek
                await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

                // burda sipariş no donebiliriz ve web tarafında sipariş no ile siparişin durumunu sorgulayabiliriz(web-ordercontroller da)
                return CreateActionResultInstance(Shared.Dto.Response<NoContent>.Success(200));
            }
            catch (Exception ex)
            {
                // loglama yapılacak
                return CreateActionResultInstance(Shared.Dto.Response<NoContent>.Fail(ex.Message, 500));
            }
        }
    }
}
