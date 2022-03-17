using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Validation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Wprowadź wartość!")
                .Length(5, 15)
                .Must((obj, value) => obj.Id == 0 && value != null ? !value.Contains("!") : true ).WithMessage("Nazwa nowego produktu nie może zawierać znaku wykrzyknika")
                .WithName("Nazwa produktu");
            RuleFor(x => x.Price).ExclusiveBetween(0, 100).ScalePrecision(2,4);
        }
    }
}
