namespace Project.API.Validations.Account
{
    public class RegisterValidators : AbstractValidator<RegisterDto>
    {
        public RegisterValidators()
        {
            ApplyValidationRules();
        }
        public void ApplyValidationRules()
        {

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("{PropertyName} is Not Empty")
           .NotNull().WithMessage("{PropertyName} is Not Null");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("{PropertyName} is Not Empty")
           .NotNull().WithMessage("{PropertyName} is Not Null")
           .Matches(@"^[a-zA-Z]{3}[a-zA-Z0-9_]{0,20}$").WithMessage("first three characters must be letters followed by up to 20 letters or numbers or _  Example:Abc_ab123");

            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} is Not Empty")
                .NotNull().WithMessage("{PropertyName} is Not Null");
            //.Matches(@"^[a-zA-Z]{3,}[a-zA-Z._0-9]{0,20}[a-zA-Z0-9]{1,}(@gmail.com|@yahoo.com)$")
            //.WithMessage("Email must start with at least three alphabetic characters,allowed only '._' and numbers. Example :user123@(gmail.com|yahoo.com)");

            RuleFor(x => x.Phone)
    .Matches(@"^(?:010|011|012|015)[0-9]{8}$")
    .WithMessage("Phone number must start with 010, 011,015 or 012 and be followed by 8 digits.");

            RuleFor(x => x.Password)
.NotEmpty().WithMessage("{PropertyName} is Not Empty")
.NotNull().WithMessage("{PropertyName} is Not Null");

            RuleFor(x => x.ConfirmPassword)
      .NotEmpty().WithMessage("{PropertyName} is Not Empty")
      .NotNull().WithMessage("{PropertyName} is Not Null")
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.Try again !");


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

        public void ApplyCustomValidationRules()
        {
            //RuleFor(x => x.Name).MustAsync(async (Key, CancellationToken) => !await _studentServices.IsNameExist(Key)).WithMessage("Already Name Is Exist");
        }






    }
}
