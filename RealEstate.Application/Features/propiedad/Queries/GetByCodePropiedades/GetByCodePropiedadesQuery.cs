using MediatR;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.propiedad.Queries.GetByCodePropiedades
{
    public class GetByCodePropiedadesQuery : IRequest<PropiedadesModel>
    {
        public string Codigo { get; set; }
    }

    public class GetByCodePropiedadesQueryHandler : IRequestHandler<GetByCodePropiedadesQuery, PropiedadesModel>
    {
        private readonly IPropiedadesRepository _propiedadesRepository;

        public GetByCodePropiedadesQueryHandler(IPropiedadesRepository propiedadesRepository)
        {
            _propiedadesRepository = propiedadesRepository;
        }

        public async Task<PropiedadesModel> Handle(GetByCodePropiedadesQuery request, CancellationToken cancellationToken)
        {
            var result = await _propiedadesRepository.GetPropertyByCode(request.Codigo);

            if (!result.Success || result.Data == null)
                throw new ApplicationException(result.Message ?? "La propiedad no fue encontrada.");

            return (PropiedadesModel)result.Data;
        }
    }


}
