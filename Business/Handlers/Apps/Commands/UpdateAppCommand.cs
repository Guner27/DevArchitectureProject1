
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Apps.ValidationRules;


namespace Business.Handlers.Apps.Commands
{


    public class UpdateAppCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public string AppStoreURL { get; set; }
        public string PlayStoreURL { get; set; }

        public class UpdateAppCommandHandler : IRequestHandler<UpdateAppCommand, IResult>
        {
            private readonly IAppRepository _appRepository;
            private readonly IMediator _mediator;

            public UpdateAppCommandHandler(IAppRepository appRepository, IMediator mediator)
            {
                _appRepository = appRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateAppValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateAppCommand request, CancellationToken cancellationToken)
            {
                var isThereAppRecord = await _appRepository.GetAsync(u => u.Id == request.Id);


                isThereAppRecord.AppName = request.AppName;
                isThereAppRecord.AppStoreURL = request.AppStoreURL;
                isThereAppRecord.PlayStoreURL = request.PlayStoreURL;


                _appRepository.Update(isThereAppRecord);
                await _appRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

