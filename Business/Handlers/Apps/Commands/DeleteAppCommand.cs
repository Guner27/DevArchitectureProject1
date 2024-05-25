
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Business.Handlers.Apps.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteAppCommand : IRequest<IResult>
    {
        public int Id { get; set; }

        public class DeleteAppCommandHandler : IRequestHandler<DeleteAppCommand, IResult>
        {
            private readonly IAppRepository _appRepository;
            private readonly IMediator _mediator;

            public DeleteAppCommandHandler(IAppRepository appRepository, IMediator mediator)
            {
                _appRepository = appRepository;
                _mediator = mediator;
            }

            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(DeleteAppCommand request, CancellationToken cancellationToken)
            {
                var appToDelete = _appRepository.Get(p => p.Id == request.Id);

                _appRepository.Delete(appToDelete);
                await _appRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Deleted);
            }
        }
    }
}

