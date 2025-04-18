using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Application.Enum;

namespace RealEstate.Web.Helpers.Otros
{
    public class SelectRol
    {
        public List<SelectListItem> Roles()
        {
            return Enum.GetValues(typeof(ClienteAgente))
                .Cast<ClienteAgente>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                }).ToList();
        }
    }
}
