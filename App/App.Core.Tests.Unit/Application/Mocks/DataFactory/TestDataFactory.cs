using Domain.Entities;

namespace App.Core.Tests.Unit.Application.Mocks.DataFactory;
public static class TestDataFactory
{
    public static User CreateTestUser(string id = null, string firstName = "test", string lastName = "User", string email = "test@company.com")
    {
        return new User
        {
            Id = id ?? Guid.NewGuid().ToString(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Roles = null
        };
    }
    public static List<User> CreateTestUserList()
    {
        return
        [
            CreateTestUser(
                id: "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d",
                firstName: "John",
                lastName: "Doe",
                email: "john.doe@company.com"
            ),
            CreateTestUser(
                id: "1a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p",
                firstName: "Jane",
                lastName: "Smith",
                email: "jane.smith@company.com"
            ),
            CreateTestUser(
                id: "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                firstName: "Bob",
                lastName: "Johnson",
                email: "bob.johnson@company.com"
            ),
        ];
    }
}