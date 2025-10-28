using MediatR;
using Application.Features.Requisitions.Dtos.Lookup;

namespace Application.Features.Requisitions.Queries.GetCurrencies;

public class GetReqCurrencyQuery : IRequest<List<GetReqCurrencyDto>>;