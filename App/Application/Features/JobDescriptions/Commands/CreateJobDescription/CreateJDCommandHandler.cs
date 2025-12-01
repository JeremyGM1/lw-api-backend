using AutoMapper;
using MediatR;
using Application.Common.Results.GenericErrors;
using Application.Common.Results.RequisitionErrors;
using Application.Common.Results;
using Application.Features.JobDescriptions.Dtos.Response;
using Application.Interfaces.Features.Common;
using Application.Interfaces.Features.JobDescriptions;
using Application.Interfaces.Features.Requisitions;
using Application.Interfaces.LoggingService;
using Domain.Entities.JobDescriptions;

namespace Application.Features.JobDescriptions.Commands.CreateJobDescription;

public class CreateJDCommandHandler(IMapper mapper, IJobDescriptionRepository jdRepository, IRequisitionRepository reqRepository, ICommonHelpers commonHelpers, IJobDescriptionLoggingService logger) : IRequestHandler<CreateJDCommand, Result<CreateJobDescriptionResponseDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IJobDescriptionRepository _jdRepository = jdRepository;
    private readonly IRequisitionRepository _reqRepository = reqRepository;
    private readonly ICommonHelpers _commonHelpers = commonHelpers;
    private readonly IJobDescriptionLoggingService _logger = logger;

    public async Task<Result<CreateJobDescriptionResponseDto>> Handle(CreateJDCommand request, CancellationToken cancellationToken)
    {
        var (userEmail, ipAddress, userAgent) = _commonHelpers.GetLoggingContext();
        try
        {
            var requisition = await _reqRepository.GetRequisitionById(request.RequisitionCode);
            if (requisition == null)
            {
                var reqError = RequisitionErrors.NotExistingRequisition(request.RequisitionCode);
                await _logger.LogFailureAsync("N/A", "Create", userEmail, ipAddress, userAgent, "Failure", reqError.Code, reqError.Description, request.RequisitionCode, request.Title);
                return Result<CreateJobDescriptionResponseDto>.Failure(reqError);
            }

            var mappedJD = _mapper.Map<JobDescription>(request);

            mappedJD.CreatedAt = DateTime.UtcNow;
            mappedJD.CreatedBy = userEmail;

            mappedJD.RequisitionCode = requisition.ReqCode;
            requisition.AddJobDescription(mappedJD);

            await _jdRepository.CreateAsync(mappedJD);

            var savedJD = await _jdRepository.GetJobDescriptionById(mappedJD.Id);

            var responseDto = _mapper.Map<CreateJobDescriptionResponseDto>(savedJD);

            await _logger.LogSuccessAsync(savedJD!.Id.ToString(), "Create", userEmail, ipAddress, userAgent, "Success", request.RequisitionCode, request.Title);

            return Result<CreateJobDescriptionResponseDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            var errorDescription = $"{ex.Message}" + (ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : "");
            await _logger.LogFailureAsync("N/A", "Create", userEmail, ipAddress, userAgent, "Failure", ex.Message, errorDescription, request.RequisitionCode, request.Title);
            return Result<CreateJobDescriptionResponseDto>.Failure(CommonErrors.UnexpectedException(errorDescription));
        }
    }
}