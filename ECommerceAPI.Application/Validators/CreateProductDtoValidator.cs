using ECommerceAPI.Application.DTOs;
using FluentValidation;

namespace ECommerceAPI.Application.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ürün adı boş olamaz.")
            .MaximumLength(100)
            .WithMessage("Ürün adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Açıklama en fazla 500 karakter olabilir.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Fiyat 0'dan büyük olmalıdır.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stok miktarı negatif olamaz.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("Geçerli bir kategori seçilmelidir.");
    }
}