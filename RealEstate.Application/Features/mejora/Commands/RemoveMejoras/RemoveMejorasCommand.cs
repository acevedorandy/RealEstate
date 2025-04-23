using MediatR;
using RealEstate.Domain.Entities.dbo;
using RealEstate.Persistance.Interfaces.dbo;

namespace RealEstate.Application.Features.mejora.Commands.RemoveMejoras
{
    public class RemoveMejorasCommand : IRequest<int>
    {
        public int MejoraID { get; set; }
    }

    public class RemoveMejorasCommandHandler : IRequestHandler<RemoveMejorasCommand, int>
    {
        private readonly IMejorasRepository _mejorasRepository;

        public RemoveMejorasCommandHandler(IMejorasRepository mejorasRepository)
        {
            _mejorasRepository = mejorasRepository;
        }

        public async Task<int> Handle(RemoveMejorasCommand request, CancellationToken cancellationToken)
        {
            if (request.MejoraID <= 0)
                throw new ArgumentException("ID inválido para mejora.");

            var mejoraGetBy = await _mejorasRepository.GetById(request.MejoraID);

            if (!mejoraGetBy.Success || mejoraGetBy.Data == null)
                throw new InvalidOperationException("La mejora no existe.");

            var mejora = (Mejoras)mejoraGetBy.Data;
            await _mejorasRepository.Remove(mejora);

            return mejora.MejoraID;
        }
    }
}
