using System.Net.Http.Json;
using System.Threading.Tasks;
using Application.Features.Requisitions.Commands.CreateRequisition;
using Application.Features.Requisitions.Dtos;
using Domain.Entities.Requisitions;
using FluentAssertions;
using Identity.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Shouldly;

namespace App.Core.Tests.Acceptance.API.Features.Requisition;
public class RequisitionControllerTests : IAsyncLifetime
{
    private ApiApplicationFactory _application;
    private HttpClient _client;
    public async Task InitializeAsync()
    {
        _application = new ApiApplicationFactory();

        using (var scope = _application.Services.CreateScope())
        {
            var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await appContext.Database.EnsureDeletedAsync();
            await appContext.Database.EnsureCreatedAsync();

            var identityContext = scope.ServiceProvider.GetRequiredService<TalentSuiteIdentityDbContext>();
            await identityContext.Database.EnsureDeletedAsync();
            await identityContext.Database.EnsureCreatedAsync();

            appContext.RequisitionWorkType.Add(new RequisitionWorkType { Id = 1, WorkType = "Full-time" });
            appContext.RequisitionHardware.Add(new RequisitionHardware { Id = 1, Hardware = "Laptop" });
            appContext.RequisitionStatus.Add(new RequisitionStatus { Id = 1, Status = "Open" });
            appContext.RequisitionCurrency.Add(new RequisitionCurrency { Id = 1, Currency = "USD" });
            appContext.RequisitionInterval.Add(new RequisitionInterval { Id = 1, Interval = "Annual" });
            appContext.RequisitionLocation.Add(new RequisitionLocation { Id = 1, Location = "Medellin" });
            appContext.RequisitionLocationType.Add(new RequisitionLocationType { Id = 1, LocationType = "Medellin" });
        }

        _client = _application.CreateClient();
    }
    public async Task DisposeAsync()
    {
        _client?.Dispose();
        await _application.DisposeAsync();
    }


    //[Fact]
    //public async Task RequisitionRequest_CreatesRequisition()
    //{
    //    CreateRequisitionCommand request = RequisitionTestData.GetValidCreateRequisitionCommand("600601");

    //    var response = await _client.PostAsJsonAsync("/api/Requisition", request);

    //    var errorContent = await response.Content.ReadAsStringAsync();
    //    System.Diagnostics.Debug.WriteLine($"Response Status: {response.StatusCode}");
    //    System.Diagnostics.Debug.WriteLine($"Response Content: {errorContent}");

    //    if (!response.IsSuccessStatusCode)
    //    {
    //        throw new Exception($"Status: {response.StatusCode}, Content: {errorContent}");
    //    }

    //    response.EnsureSuccessStatusCode();

    //    var reqResponse = await response.Content.ReadFromJsonAsync<CreateRequisitionDto>();
    //    reqResponse.Id.ShouldBeGreaterThan(0);
    //    reqResponse.ReqCode.ShouldNotBeNull();
    //    reqResponse.ReqCode.ShouldBe("600601");
    //    reqResponse.Name.ShouldBe("Test Requisition");
    //    reqResponse.StatusId.ShouldBe(1);
    //    reqResponse.WorkTypeId.ShouldBe(1);
    //    reqResponse.HardwareId.ShouldBe(1);
    //    reqResponse.Headcount.ShouldBe(10);
    //    reqResponse.Locationid.ShouldBe(1);
    //    reqResponse.Department.ShouldBe("IT");
    //    reqResponse.HiringManager.ShouldBe("UserHiringManager");
    //    reqResponse.Owner.ShouldBe("UserRequisitionOwner");
    //    reqResponse.CompensationBandMin.ShouldBe(1000);
    //    reqResponse.CompensationBandMax.ShouldBe(5000);
    //    reqResponse.CompensationBandCurrencyId.ShouldBe(1);
    //    reqResponse.CompensationBandIntervalId.ShouldBe(1);
    //    reqResponse.Description.ShouldBe("ReqDescription");
    //    reqResponse.BackfillName.ShouldBe("ReqBackfillName");
    //}
}