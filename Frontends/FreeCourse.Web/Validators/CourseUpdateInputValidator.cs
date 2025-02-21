using FluentValidation;
using FreeCourse.Web.Models.Catelogs;

namespace FreeCourse.Web.Validators
{
    public class CourseUpdateInputValidator: AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("isim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("açıklama alanı boş olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("süre alanı boş olamaz");

            // 12345678.12 =>  şekilde 2 basamak ondalıklı sayı
            RuleFor(x => x.Price).NotEmpty().WithMessage("fiyat alanı boş olamaz").ScalePrecision(2, 10).WithMessage("hatalı para formatı");
        }
    }
}
