using App.Core.Tests.Unit.Application.Features.Shared;
using App.Core.Tests.Unit.Application.Mocks.DataFactory;
using Application.Features.Shared.Dtos.UserDtos;
using Application.Features.Users.Queries.GetAllUsers;
using Domain.Entities;
using Shouldly;

namespace App.Core.Tests.Unit.Application.Features.User.Queries;
public class GetUserListsQueryHandlerTests : UserHandlerTestBase<GetUsersQueryHandler>
{
    [Fact]
    public async Task GetUserListTest()
    {
        const int numberOfUsersInList = 3;

        var handler = new GetUsersQueryHandler(_mapper, _mockRepo.Object, _mockAppLogger.Object);

        var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

        result.ShouldBeOfType<List<UserDto>>();
        result.Count.ShouldBe(numberOfUsersInList);
    }
}