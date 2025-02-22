using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId = _sharedIdentityService.GetUserId });

                return CreateActionResultInstance(response);
            }
            catch (Exception ex)
            {
                // Hata loglaması yapılabilir
                // _logger.LogError(ex, "An error occurred while retrieving orders");

                return StatusCode(500, new { Message = "An error occurred while processing your request. Please try again later." });
            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand createOrderCommand)
        {
            try
            {
                var response = await _mediator.Send(createOrderCommand);

                if (!response.IsSuccessful)
                {
                    return StatusCode(500, new { Message = "An error occurred while processing your request. Please try again later." });
                }

                return CreateActionResultInstance(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"{ex.Message}" });
            }
        }
    }
}
