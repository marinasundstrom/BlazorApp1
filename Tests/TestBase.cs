using System;

using BlazorApp1.Application.Common;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Entities;
using BlazorApp1.Infrastructure.Persistence;
using BlazorApp1.Infrastructure.Persistence.Interceptors;

using Duende.IdentityServer.EntityFramework.Options;

using MediatR;

using Microsoft.EntityFrameworkCore;


namespace BlazorApp1.Application.Tests;

public abstract class TestBase
{
    protected readonly IMediator _fakeMediator;
    protected readonly IDomainEventService _fakeDomainEventService;
    protected readonly ICurrentUserService _fakeCurrentUserService;
    protected IDateTimeService _fakeDateTimeService;
    protected IUrlHelper _fakeUrlHelper;
    protected ITenantService _fakeTenantService;

    protected TestBase()
    {
        _fakeMediator = Substitute.For<IMediator>();

        _fakeDomainEventService = Substitute.For<IDomainEventService>();

        _fakeCurrentUserService = Substitute.For<ICurrentUserService>();

        _fakeDateTimeService = Substitute.For<IDateTimeService>();
        _fakeDateTimeService.Now.Returns(x => DateTime.Now);

        _fakeUrlHelper = Substitute.For<IUrlHelper>();
        _fakeUrlHelper.CreateImageUrl(Arg.Any<string>()).Returns(x => $"http://image/{x.Arg<string>()}");

        _fakeTenantService = Substitute.For<ITenantService>();
        _fakeTenantService.TenantId.Returns(x => "1");
    }

    protected static User CreateTestUser()
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "test@email.com",
            NormalizedUserName = "TEST@EMAIL.COM"
        };
    }

    protected IApplicationDbContext CreateDbContext(ITenantService fakeTenantService)
    {
        var duendeOptions = Substitute.For<Microsoft.Extensions.Options.IOptions<OperationalStoreOptions>>();
        duendeOptions.Value.Returns(x => new OperationalStoreOptions()
        {
            DeviceFlowCodes = new TableConfiguration("DeviceCodes")
        });

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "Test")
           .Options;

        return new ApplicationDbContext(options, duendeOptions, _fakeDomainEventService, fakeTenantService,
            new AuditableEntitySaveChangesInterceptor(_fakeCurrentUserService, _fakeDateTimeService, _fakeTenantService));
    }
}