
using Business.Handlers.Apps.Commands;
using FluentValidation;

namespace Business.Handlers.Apps.ValidationRules
{

    public class CreateAppValidator : AbstractValidator<CreateAppCommand>
    {
        public CreateAppValidator()
        {
            RuleFor(x => x.AppName).NotEmpty();
            RuleFor(x => x.AppStoreURL).NotEmpty();
            RuleFor(x => x.PlayStoreURL).NotEmpty();

        }
    }
    public class UpdateAppValidator : AbstractValidator<UpdateAppCommand>
    {
        public UpdateAppValidator()
        {
            RuleFor(x => x.AppName).NotEmpty();
            RuleFor(x => x.AppStoreURL).NotEmpty();
            RuleFor(x => x.PlayStoreURL).NotEmpty();

        }
    }
}