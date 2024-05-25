
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;


namespace Business.Handlers.Apps.Queries
{
    public class GetAppQuery : IRequest<IDataResult<App>>
    {
        public int Id { get; set; }

        public class GetAppQueryHandler : IRequestHandler<GetAppQuery, IDataResult<App>>
        {
            private readonly IAppRepository _appRepository;
            private readonly IMediator _mediator;

            public GetAppQueryHandler(IAppRepository appRepository, IMediator mediator)
            {
                _appRepository = appRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<App>> Handle(GetAppQuery request, CancellationToken cancellationToken)
            {
                var app = await _appRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<App>(app);
            }
        }
    }
}
