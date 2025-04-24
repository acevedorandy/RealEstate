

using RealEstate.Application.Responses.identity.Base;

namespace RealEstate.Application.Responses.identity
{
    public class RegisterResponse : BaseResponse
    {
        public RegisterResponse()
        {
            HasError = false;
        }

        public dynamic Dynamic { get; set; }
    }
}
