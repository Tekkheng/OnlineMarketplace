using CartService.Models.RequestModels;
using FluentValidation;

public class AddToCartRequestValidator : AbstractValidator<AddToCartRequest>
{
    public AddToCartRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("ProductId must be valid.");
    }
}
