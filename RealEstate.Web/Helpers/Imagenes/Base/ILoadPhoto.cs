namespace RealEstate.Web.Helpers.Imagenes.Base
{
    public interface ILoadPhoto<Dto, T>
    {
        Task<Dto> LoadPhoto(Dto dto, T entity);

    }
}
