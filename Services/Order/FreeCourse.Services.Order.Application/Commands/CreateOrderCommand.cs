using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Shared.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Commands
{
    public class CreateOrderCommand : IRequest<Response<CreatedOrderDto>>
    {
        // user'ın id'si
        public string BuyerId { get; set; }
        // odeme numarası
        // public int PaymentNo { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }

        public AddressDto Address { get; set; }
    }
}
