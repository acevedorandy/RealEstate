using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.mejora.Commands.RemoveMejoras
{
    /// <summary>
    ///  Parametros para la eliminacion de una mejora
    /// </summary>
    public class RemoveMejorasCommand : IRequest<int>
    {
        /// <example>
        ///  2
        /// </example>
        [SwaggerParameter(Description = "Id de la mejora que se desea eliminar")]
        [JsonIgnore]
        public int MejoraID { get; set; }
    }

    public class RemoveMejorasCommandHandler : IRequestHandler<RemoveMejorasCommand, int>
    {
        private readonly IMejorasRepository _mejorasRepository;
        private readonly IMapper _mapper;

        public RemoveMejorasCommandHandler(IMejorasRepository mejorasRepository, IMapper mapper)
        {
            _mejorasRepository = mejorasRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(RemoveMejorasCommand request, CancellationToken cancellationToken)
        {
            if (request.MejoraID <= 0)
                throw new ArgumentException("ID inválido para mejora.");

            var mejoraGetBy = await _mejorasRepository.GetById(request.MejoraID);

            if (!mejoraGetBy.Success || mejoraGetBy.Data == null)
                throw new InvalidOperationException("La mejora no existe.");

            var mejora = _mapper.Map<Mejoras>(mejoraGetBy.Data);
            await _mejorasRepository.Remove(mejora);

            return request.MejoraID;
        }
    }
}
