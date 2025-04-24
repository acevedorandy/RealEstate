using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using RealEstate.Persistance.Models.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.mejora.Commands.UpdateMejoras
{
    /// <summary>
    ///  Parametros para la actualizacion de una mejora
    /// </summary>
    public class UpdateMejorasCommand : IRequest<MejorasModel>
    {
        /// <example>
        ///  2
        /// </example>
        [JsonIgnore]
        [SwaggerParameter(Description = "Id de la mejora que se desea actualizar")]
        public int MejoraID { get; set; }

        [SwaggerParameter(Description = "Nuevo Nombre de la mejora")]
        public string Nombre { get; set; }

        [SwaggerParameter(Description = "Una ueva Descripcion para esta mejora")]
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
