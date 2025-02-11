using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dto
{
    public class CourseDto
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        public FeatureDto Feature { get; set; }
        public string CategoryId { get; set; }

        public CategoryDto Category { get; set; }
    }
}
