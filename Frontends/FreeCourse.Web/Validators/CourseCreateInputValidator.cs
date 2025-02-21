using FluentValidation;
using FreeCourse.Web.Models.Catelogs;

namespace FreeCourse.Web.Validators
{
    public class CourseCreateInputValidator : AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("isim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("açıklama alanı boş olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("süre alanı boş olamaz");

            // 12345678.12 =>  şekilde 2 basamak ondalıklı sayı
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz.").GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.").Must(price => Decimal.Round(price, 2) == price).WithMessage("hatalı para formatı");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("kategori alanı seçiniz");
        }
    }
}
