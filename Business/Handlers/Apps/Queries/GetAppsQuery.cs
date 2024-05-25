
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;

namespace Business.Handlers.Apps.Queries
{

    public class GetAppsQuery : IRequest<IDataResult<IEnumerable<App>>>
    {
        public class GetAppsQueryHandler : IRequestHandler<GetAppsQuery, IDataResult<IEnumerable<App>>>
        {
            private readonly IAppRepository _appRepository;
            private readonly IMediator _mediator;

            public GetAppsQueryHandler(IAppRepository appRepository, IMediator mediator)
            {
                _appRepository = appRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<App>>> Handle(GetAppsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<App>>(await _appRepository.GetListAsync());
            }
        }
    }
}