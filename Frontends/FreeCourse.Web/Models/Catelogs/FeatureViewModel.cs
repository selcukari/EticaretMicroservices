using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catelogs
{
    public class FeatureViewModel
    {
        [Display(Name = "Kurs süre")]
        public int Duration { get; set; }
    }
}
