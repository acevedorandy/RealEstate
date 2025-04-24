using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstate.Application.Features.mejora.Commands.SaveMejoras
{
    /// <summary>
    ///  Parametros para la creacion de una mejora
    /// </summary>
    public class SaveMejorasCommand : IRequest<int>
    {
        /// <example>
        ///  Balcón
        /// </example>
        [SwaggerParameter(Description = "Nombre de la mejora")]
        public string Nombre { get; set; }
        /// <example>
        ///  Balcón frontal con vista a al mar
        /// </example>
        [SwaggerParameter(Description = "Una Descripcion de la mejora")]
        public string Descripcion { get; set; }
    }

    public class SaveMejorasCommandHandler : IRequestHandler<SaveMejorasCommand, int>
    {
        private readonly IMejorasRepository _mejorasRepository;
        private readonly IMapper _mapper;

        public SaveMejorasCommandHandler(IMejorasRepository mejorasRepository, IMapper mapper)
        {
            _mejorasRepository = mejorasRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(SaveMejorasCommand request, CancellationToken cancellationToken)
        {
            var mejora = _mapper.Map<Mejoras>(request);
            var result = await _mejorasRepository.Save(mejora);

            if (!result.Success)
                throw new ApplicationException(result.Message ?? "Error al guardar la mejora.");

            return mejora.MejoraID;
        }
    }
}
