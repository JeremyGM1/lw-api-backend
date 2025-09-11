using App.Core.Tests.Unit.Application.Features.Shared;
using App.Core.Tests.Unit.Application.Mocks.DataFactory;
using Application.Features.Shared.Dtos.UserDtos;
using Application.Features.Users.Queries.GetUserById;
using Moq;
using Persistence.Interfaces;
using Shouldly;
using static App.Core.Tests.Unit.Application.Mocks.MockUserRepository;

namespace App.Core.Tests.Unit.Application.Features.User.Queries;
public class GetUserWithIdQueryHandlerTest : UserHandlerTestBase<GetUserByIdQueryHandler>
{
    [Fact]
    public async Task GetUserWithIdTest()
    {
        var user = TestDataFactory.CreateTestUser(
            id: "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d",
            firstName: "John",
            lastName: "Doe",
            email: "john.doe@company.com"
        );

        var handler = new GetUserByIdQueryHandler(_mapper, _mockRepo.Object);

        var result = await handler.Handle(new GetUserByIdQuery(user.Id), CancellationToken.None);

        result.ShouldBeOfType<UserDto>();
        result.ShouldNotBeNull();
        result.Id.ShouldBe(user.Id);
        result.FirstName.ShouldBe(user.FirstName);
        result.LastName.ShouldBe(user.LastName);
        result.Email.ShouldBe(user.Email);
    }
}