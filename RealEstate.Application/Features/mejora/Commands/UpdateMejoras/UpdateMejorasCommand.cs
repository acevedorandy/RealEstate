using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;

namespace RealEstate.Application.Features.mejora.Commands.UpdateMejoras
{
    public class UpdateMejorasCommand : IRequest<MejorasModel>
    {
        [JsonIgnore]
        public int MejoraID { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }

    public class UpdateMejorasCommandHandler : IRequestHandler<UpdateMejorasCommand, MejorasModel>
    {
        private readonly IMejorasRepository _mejorasRepository;
        private readonly IMapper _mapper;

        public UpdateMejorasCommandHandler(IMejorasRepository mejorasRepository, IMapper mapper)
        {
            _mejorasRepository = mejorasRepository;
            _mapper = mapper;
        }

        public async Task<MejorasModel> Handle(UpdateMejorasCommand request, CancellationToken cancellationToken)
        {
            if (request.MejoraID <= 0)
                throw new ArgumentException("ID de mejora no válido.");

            var mejoraGet = await _mejorasRepository.GetById(request.MejoraID);

            if (!mejoraGet.Success || mejoraGet.Data == null)
                throw new InvalidOperationException("La mejora no existe.");

            var mejora = _mapper.Map<Mejoras>(mejoraGet.Data);

            _mapper.Map(request, mejora);

            var result = await _mejorasRepository.Update(mejora);

            if (!result.Success)
                throw new ApplicationException(result.Message ?? "Error al actualizar la mejora.");

            return result.Data;
        }
    }
}
