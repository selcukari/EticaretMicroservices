using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Dto
{
    public class CourseCreateDto
    {
        public int Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }


        public FeatureDto Feature { get; set; }
        public string CategoryId { get; set; }

    }
}
