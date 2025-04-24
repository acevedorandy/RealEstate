using MediatR;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Features.mejora.Queries.GetAllMejoras
{
    /// <summary>
    ///  No necesario tener parametros aqui
    /// </summary>
    public class GetAllMejorasQuery : IRequest<IEnumerable<MejorasModel>>
    {
    }

    public class GetAllMejorasQueryHandler : IRequestHandler<GetAllMejorasQuery, IEnumerable<MejorasModel>>
    {
        private readonly IMejorasRepository _mejorasRepository;

        public GetAllMejorasQueryHandler(IMejorasRepository mejorasRepository)
        {
            _mejorasRepository = mejorasRepository;
        }

        public async Task<IEnumerable<MejorasModel>> Handle(GetAllMejorasQuery request, CancellationToken cancellationToken)
        {
            var result = await _mejorasRepository.GetAll();

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "Error al obtener las mejoras.");
            return (IEnumerable<MejorasModel>)result.Data;

        }
    }
}
