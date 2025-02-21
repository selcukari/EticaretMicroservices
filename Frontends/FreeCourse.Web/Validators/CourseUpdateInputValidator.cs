using FluentValidation;
using FreeCourse.Web.Models.Catelogs;

namespace FreeCourse.Web.Validators
{
    public class CourseUpdateInputValidator: AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("namespace cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("description cannot be empty");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("duration cannot be empty");

            // 12345678.12 =>  şekilde 2 basamak ondalıklı sayı
            RuleFor(x => x.Price).NotEmpty().WithMessage("price cannot be empty").ScalePrecision(2, 10).WithMessage("incorrect currency format");
        }
    }
}
