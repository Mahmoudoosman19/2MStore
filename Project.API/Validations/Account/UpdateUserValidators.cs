
using Project.BLL.Dtos.Account;

namespace Project.API.Validations.Account
{
    public class UpdateUserValidators : AbstractValidator<UpdateUserDto>
    {


        public UpdateUserValidators()
        {
            ApplyValidationRules();

        }

        public void ApplyValidationRules()
        {

            RuleFor(x => x.UserName).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null")
                .Matches(@"^[a-zA-Z]{3}[a-zA-Z0-9_]{0,9}$").WithMessage("first three characters must be letters followed by up to 15 letters or numbers or _  Example:Abc_ab123");


            RuleFor(x => x.Phone)
    .Matches(@"^(?:010|011|012|015)[0-9]{8}$")
    .WithMessage("Phone number must start with 010, 011,015 or 012 and be followed by 8 digits.");

            RuleFor(x => x.Street)
      .NotEmpty().WithMessage("{PropertyName} is Not Empty")
      .NotNull().WithMessage("{PropertyName} is Not Null");

            RuleFor(x => x.City)
      .NotEmpty().WithMessage("{PropertyName} is Not Empty")
      .NotNull().WithMessage("{PropertyName} is Not Null");

            RuleFor(x => x.Country)
      .NotEmpty().WithMessage("{PropertyName} is Not Empty")
      .NotNull().WithMessage("{PropertyName} is Not Null");




        }
    }
}