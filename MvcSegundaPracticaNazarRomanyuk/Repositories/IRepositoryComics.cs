using MvcSegundaPracticaNazarRomanyuk.Models;

namespace MvcSegundaPracticaNazarRomanyuk.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetComics();
        void InsertComic(int idcomic, string nombre, string imagen, string descripcion);
    }
}
