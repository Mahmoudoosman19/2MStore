namespace Project.API.Validations.Account
{
    public class ChangePasswordValidators : AbstractValidator<ChangePasswordDto>
    {

        public ChangePasswordValidators()
        {
            ApplyValidationRules();
        }


        public void ApplyValidationRules()
        {


            RuleFor(x => x.CurrentPassword)
           .NotEmpty().WithMessage("{PropertyName} is Not Empty")
           .NotNull().WithMessage("{ProperyName} is Not Null").WithName("Current Password");

            RuleFor(x => x.NewPassword)
         .NotEmpty().WithMessage("{PropertyName} is Not Empty")
         .NotNull().WithMessage("{ProperyName} is Not Null").WithName("New Password"); ;

            RuleFor(x => x.ConfirmPassword)
           .NotEmpty().WithMessage("{PropertyName} is Not Empty")
            .NotNull().WithMessage("{ProperyName} is Not Null")
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match.").WithName("Confirm Password");
        }


    }
}
