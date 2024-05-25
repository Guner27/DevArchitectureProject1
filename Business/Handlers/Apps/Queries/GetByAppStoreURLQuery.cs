
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Logging;
namespace Business.Handlers.Apps.Queries
{

    public class GetByAppStoreURLQuery : IRequest<IDataResult<App>>
    {
        public string AppStoreURL { get; set; }

        public class GetByAppStoreURLQueryHandler : IRequestHandler<GetByAppStoreURLQuery, IDataResult<App>>
        {
            private readonly IAppRepository _appRepository;
            private readonly IMediator _mediator;

            public GetByAppStoreURLQueryHandler(IAppRepository appRepository, IMediator mediator)
            {
                _appRepository = appRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<App>> Handle(GetByAppStoreURLQuery request, CancellationToken cancellationToken)
            {
                var app = await _appRepository.GetAsync(p => p.AppStoreURL == request.AppStoreURL);

                return new SuccessDataResult<App>(app);
            }
        }
    }
}
