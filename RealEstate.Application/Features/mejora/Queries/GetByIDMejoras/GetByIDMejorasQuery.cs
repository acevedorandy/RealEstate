using MediatR;
using RealEstate.Persistance.Models.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.mejora.Queries.GetByIDMejoras
{
    /// <summary>
    ///  Parametros para la obtener una mejora
    /// </summary>
    public class GetByIDMejorasQuery : IRequest<MejorasModel>
    {
        /// <example>
        ///  8
        /// </example>
        [SwaggerParameter(Description = "Ingrese el Id de la mejora que se desea obtener")]
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
