using FreeCourse.Web.Models.FakePayments;
using FreeCourse.Web.Services.Interfaces;
using System.Net.Http;

namespace FreeCourse.Web.Services
{
    public class PaymentService: IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<PaymentInfoInput>("fakepayments", paymentInfoInput);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // loglama yapılacak
                return false;
            }
            
        }
    }
}
