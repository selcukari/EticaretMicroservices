using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class CourseNameChangedEventConsumer: IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _orderDbContext;

        public CourseNameChangedEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            try
            {
                var orderItems = await _orderDbContext.OrderItems.Where(x => x.ProductId == context.Message.CourseId).ToListAsync();

                orderItems.ForEach(x =>
                {
                    x.UpdateOrderItem(context.Message.UpdatedName, x.PictureUrl, x.Price);
                });

                await _orderDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // For example, you can log the error using a logging framework
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Optionally, you can rethrow the exception if you want it to be handled by the caller
                throw;
            }
        }
    }
}
