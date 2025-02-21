using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catelogs
{
    public class CourseCreateInput
    {
        [Display(Name = "Course Name")]
        public string Name { get; set; }

        [Display(Name = "Course Description")]
        public string Description { get; set; }

        [Display(Name = "Course Price")]
        public decimal Price { get; set; }

        public string Picture { get; set; }

        public string UserId { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Course Category")]
        public string CategoryId { get; set; }

        [Display(Name = "Course Picture")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
