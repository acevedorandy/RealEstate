using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.propiedad.Queries.GetByIDPropiedades
{
    public class GetByIDPropiedadesQuery : IRequest<PropiedadesModel>
    {
        public int PropiedadID { get; set; }
    }

    public class GetByIDPropiedadesHandler : IRequestHandler<GetByIDPropiedadesQuery, PropiedadesModel>
    {
        private readonly IPropiedadesRepository _propiedadesRepository;

        public GetByIDPropiedadesHandler(IPropiedadesRepository propiedadesRepository)
        {
            _propiedadesRepository = propiedadesRepository;
        }

        public async Task<PropiedadesModel> Handle(GetByIDPropiedadesQuery request, CancellationToken cancellationToken)
        {
            var result = await _propiedadesRepository.GetById(request.PropiedadID);

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "La propiedad no fue encontrada.");

            return (PropiedadesModel)result.Data;
        }
    }
}
