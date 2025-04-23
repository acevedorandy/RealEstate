using MediatR;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Features.mejora.Queries.GetByIDMejoras
{
    public class GetByIDMejorasQuery : IRequest<MejorasModel>
    {
        public int MejoraID { get; set; }
    }

    public class GetByIDMejorasQueryHandler : IRequestHandler<GetByIDMejorasQuery, MejorasModel>
    {
        private readonly IMejorasRepository _mejorasRepository;

        public GetByIDMejorasQueryHandler(IMejorasRepository mejorasRepository)
        {
            _mejorasRepository = mejorasRepository;
        }

        public async Task<MejorasModel> Handle(GetByIDMejorasQuery request, CancellationToken cancellationToken)
        {
            var result = await _mejorasRepository.GetById(request.MejoraID);

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "La mejora no fue encontrada.");

            return (MejorasModel)result.Data;
        }
    }
}
