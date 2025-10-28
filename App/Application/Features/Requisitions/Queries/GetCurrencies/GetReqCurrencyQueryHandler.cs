using Application.Features.Requisitions.Dtos.Lookup;
using Application.Interfaces.Features.Lookup.Requisition;
using AutoMapper;
using MediatR;

namespace Application.Features.Requisitions.Queries.GetCurrencies;
public class GetReqCurrencyQueryHandler(IMapper mapper, IReqCurrencyRepository reqCurrencyRepository) : IRequestHandler<GetReqCurrencyQuery, List<GetReqCurrencyDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IReqCurrencyRepository _reqCurrencyRepository = reqCurrencyRepository;

    public async Task<List<GetReqCurrencyDto>> Handle(GetReqCurrencyQuery request, CancellationToken cancellationToken)
    {
        var currencies = await _reqCurrencyRepository.GetCurrencies();

        var data = _mapper.Map<List<GetReqCurrencyDto>>(currencies);

        return data;
    }
}