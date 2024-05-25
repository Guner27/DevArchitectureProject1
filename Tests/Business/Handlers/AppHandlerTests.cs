
using Business.Handlers.Apps.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Apps.Queries.GetAppQuery;
using Entities.Concrete;
using static Business.Handlers.Apps.Queries.GetAppsQuery;
using static Business.Handlers.Apps.Commands.CreateAppCommand;
using Business.Handlers.Apps.Commands;
using Business.Constants;
using static Business.Handlers.Apps.Commands.UpdateAppCommand;
using static Business.Handlers.Apps.Commands.DeleteAppCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class AppHandlerTests
    {
        Mock<IAppRepository> _appRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _appRepository = new Mock<IAppRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task App_GetQuery_Success()
        {
            //Arrange
            var query = new GetAppQuery();

            _appRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<App, bool>>>())).ReturnsAsync(new App()
//propertyler buraya yazılacak
//{																		
//AppId = 1,
//AppName = "Test"
//}
);

            var handler = new GetAppQueryHandler(_appRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.AppId.Should().Be(1);

        }

        [Test]
        public async Task App_GetQueries_Success()
        {
            //Arrange
            var query = new GetAppsQuery();

            _appRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<App, bool>>>()))
                        .ReturnsAsync(new List<App> { new App() { /*TODO:propertyler buraya yazılacak AppId = 1, AppName = "test"*/ } });

            var handler = new GetAppsQueryHandler(_appRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<App>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task App_CreateCommand_Success()
        {
            App rt = null;
            //Arrange
            var command = new CreateAppCommand();
            //propertyler buraya yazılacak
            //command.AppName = "deneme";

            _appRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<App, bool>>>()))
                        .ReturnsAsync(rt);

            _appRepository.Setup(x => x.Add(It.IsAny<App>())).Returns(new App());

            var handler = new CreateAppCommandHandler(_appRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _appRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task App_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateAppCommand();
            //propertyler buraya yazılacak 
            //command.AppName = "test";

            _appRepository.Setup(x => x.Query())
                                           .Returns(new List<App> { new App() { /*TODO:propertyler buraya yazılacak AppId = 1, AppName = "test"*/ } }.AsQueryable());

            _appRepository.Setup(x => x.Add(It.IsAny<App>())).Returns(new App());

            var handler = new CreateAppCommandHandler(_appRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task App_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateAppCommand();
            //command.AppName = "test";

            _appRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<App, bool>>>()))
                        .ReturnsAsync(new App() { /*TODO:propertyler buraya yazılacak AppId = 1, AppName = "deneme"*/ });

            _appRepository.Setup(x => x.Update(It.IsAny<App>())).Returns(new App());

            var handler = new UpdateAppCommandHandler(_appRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _appRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task App_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteAppCommand();

            _appRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<App, bool>>>()))
                        .ReturnsAsync(new App() { /*TODO:propertyler buraya yazılacak AppId = 1, AppName = "deneme"*/});

            _appRepository.Setup(x => x.Delete(It.IsAny<App>()));

            var handler = new DeleteAppCommandHandler(_appRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _appRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

