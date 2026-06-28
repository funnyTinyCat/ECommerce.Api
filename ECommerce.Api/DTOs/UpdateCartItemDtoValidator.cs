using FluentValidation;

namespace ECommerce.Api.DTOs;

public class UpdateCartItemDtoValidator :  AbstractValidator<UpdateCartItemDto>
{
    public UpdateCartItemDtoValidator()
    {
        RuleFor (x => x.Quantity )
            .GreaterThan(0);
    }
}