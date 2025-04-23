using AutoMapper;
using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Features.tipoVenta.Commands.UpdateTiposVenta
{
    public class UpdateTiposVentaCommand : IRequest<int>
    {
        public int TipoVentaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class UpdateTiposVentaCommandHandler : IRequestHandler<UpdateTiposVentaCommand, int>
    {
        private readonly ITiposVentaRepository _tiposVentaRepository;
        private readonly IMapper _mapper;

        public UpdateTiposVentaCommandHandler(ITiposVentaRepository tiposVentaRepository, IMapper mapper)
        {
            _tiposVentaRepository = tiposVentaRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateTiposVentaCommand request, CancellationToken cancellationToken)
        {
            if(request.TipoVentaID <= 0)
                throw new ArgumentException("ID del tipo de venta no válido.");

            var tipoGetBy = await _tiposVentaRepository.GetById(request.TipoVentaID);

            if(!tipoGetBy.Success || tipoGetBy.Data == null)
                throw new ArgumentException("ID del tipo de venta no existe.");

            var tipo = (TiposVenta)tipoGetBy.Data;
            _mapper.Map(request, tipo);

            var result = await _tiposVentaRepository.Update(tipo);

            if (!result.Success)
                throw new ApplicationException(result.Message ?? "Error al actualizar el tipo de venta.");

            return tipo.TipoVentaID;
        }
    }
}
