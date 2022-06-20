using System.Threading.Tasks;

using BlazorApp1.Application.Common;
using BlazorApp1.Application.Items;
using BlazorApp1.Application.Items.Commands;
using BlazorApp1.Domain.Entities;
using BlazorApp1.Domain.Events;

using Microsoft.EntityFrameworkCore;


namespace BlazorApp1.Application.Tests;

public class ItemsTest : TestBase
{
    [Fact]
    public async Task CreateItem_ItemCreated()
    {
        // Arrange

        User user = CreateTestUser();

        _fakeCurrentUserService.UserId.Returns(x => user.Id);

        using IApplicationDbContext context = CreateDbContext(_fakeTenantService);

        context.Users.Add(user);

        context.Statuses.Add(new Status()
        {
            Id = 1,
            Name = "Created"
        });

        var commandHandler = new CreateItem.Handler(context, _fakeUrlHelper);

        var initialHandoverCount = await context.Items.CountAsync();

        // Act

        var createItemCommand = new CreateItem("Test", "Blah", 1);

        ItemDto item = await commandHandler.Handle(createItemCommand, default);

        // Assert

        var newHandoverCount = await context.Items.CountAsync();

        newHandoverCount.ShouldBeGreaterThan(initialHandoverCount);

        // Has Domain Event been published ?

        await _fakeDomainEventService
            .Received(1)
            .Publish(Arg.Is<ItemCreated>(d => d.ItemId == item.Id));
    }
}
