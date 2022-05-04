using System;
using System.Threading.Tasks;

using BlazorApp1.Application.Common;
using BlazorApp1.Application.Items;
using BlazorApp1.Application.Items.Commands;
using BlazorApp1.Application.Services;
using BlazorApp1.Domain.Events;
using BlazorApp1.Infrastructure.Persistence;

using Duende.IdentityServer.EntityFramework.Options;

using Microsoft.EntityFrameworkCore;

using NSubstitute;

using Shouldly;

using Xunit;

namespace Tests;

public class ItemsTest
{
    [Fact]
    public async Task CreateItem_ItemCreated()
    {
        // Arrange

        var user = new BlazorApp1.Domain.ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "test@email.com",
            NormalizedUserName = "TEST@EMAIL.COM"
        };

        var fakeDomainEventService = Substitute.For<IDomainEventService>();

        var fakeCurrentUserService = Substitute.For<ICurrentUserService>();
        fakeCurrentUserService.UserId.Returns(x => user.Id);

        var fakeDateTimeService = Substitute.For<IDateTimeService>();
        fakeDateTimeService.Now.Returns(x => DateTime.Now);

        var duendeOptions = Substitute.For<Microsoft.Extensions.Options.IOptions<OperationalStoreOptions>>();
        duendeOptions.Value.Returns(x => new OperationalStoreOptions()
        {
            DeviceFlowCodes = new TableConfiguration("DeviceCodes")
        });

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "Test")
           .Options;

        using IApplicationDbContext context = new ApplicationDbContext(options, duendeOptions, fakeDomainEventService, fakeCurrentUserService, fakeDateTimeService);

        context.Users.Add(user);

        var commandHandler = new CreateItemCommand.Handler(context);

        var initialHandoverCount = await context.Items.CountAsync();

        // Act

        var createItemCommand = new CreateItemCommand("Test", "Blah");

        ItemDto item = await commandHandler.Handle(createItemCommand, default);

        // Assert

        var newHandoverCount = await context.Items.CountAsync();

        newHandoverCount.ShouldBeGreaterThan(initialHandoverCount);

        // Has Domain Event been published ?

        await fakeDomainEventService
            .Received(1)
            .Publish(Arg.Is<ItemCreatedEvent>(d => d.ItemId == item.Id));
    }
}