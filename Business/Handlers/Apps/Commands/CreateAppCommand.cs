
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Apps.ValidationRules;

namespace Business.Handlers.Apps.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAppCommand : IRequest<IResult>
    {

        public string AppName { get; set; }
        public string AppStoreURL { get; set; }
        public string PlayStoreURL { get; set; }


        public class CreateAppCommandHandler : IRequestHandler<CreateAppCommand, IResult>
        {
            private readonly IAppRepository _appRepository;
            private readonly IMediator _mediator;
            public CreateAppCommandHandler(IAppRepository appRepository, IMediator mediator)
            {
                _appRepository = appRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateAppValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateAppCommand request, CancellationToken cancellationToken)
            {
                var isThereAppRecord = _appRepository.Query().Any(u => u.AppName == request.AppName);

                if (isThereAppRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedApp = new App
                {
                    AppName = request.AppName,
                    AppStoreURL = request.AppStoreURL,
                    PlayStoreURL = request.PlayStoreURL,

                };

                _appRepository.Add(addedApp);
                await _appRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}