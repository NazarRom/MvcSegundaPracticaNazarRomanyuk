using MvcSegundaPracticaNazarRomanyuk.Models;
using System.Data.SqlClient;
using System.Data;

#region PROCEDURE
//create procedure SP_INSERT_COMIC_SQL
//(@idcom int, @nombre nvarchar(50),@img nvarchar(600),@desc nvarchar(500))
//as
//insert into COMICS values (@idcom, @nombre, @img, @desc)
//go
#endregion
namespace MvcSegundaPracticaNazarRomanyuk.Repositories
{
    public class RepositoryComicsSQL : IRepositoryComics
    {
        private DataTable TablaComic;
        private SqlDataAdapter adapter;
        private SqlConnection cn;
        private SqlCommand com;
        public RepositoryComicsSQL()
        {
            string connectionString =
                @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            string sql = "select * from comics";
            this.adapter = new SqlDataAdapter(sql, connectionString);
            this.TablaComic = new DataTable();
            this.adapter.Fill(this.TablaComic);

            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }
        public List<Comic> GetComics()
        {
            var consulta = from datos in this.TablaComic.AsEnumerable()
                           select new Comic
                           {
                               IdComic = datos.Field<int>("IDCOMIC"),
                               Nombre = datos.Field<string>("NOMBRE"),
                               Imagen = datos.Field<string>("IMAGEN"),
                               Descripcion = datos.Field<string>("DESCRIPCION")
                           };
            return consulta.ToList();
        }

        private int GetMaxId()
        {
            var maximo = (from datos in this.TablaComic.AsEnumerable()
                          select datos).Max(x => x.Field<int>("IDCOMIC")) + 1;
            return maximo;
        }

        public void InsertComic(int idcomic, string nombre, string imagen, string descripcion)
        {
            int max = GetMaxId();
            SqlParameter idpam = new SqlParameter("@idcom", max);
            this.com.Parameters.Add(idpam);

            SqlParameter idnombre = new SqlParameter("@nombre", nombre);
            this.com.Parameters.Add(idnombre);

            SqlParameter idimagen = new SqlParameter("@img", imagen);
            this.com.Parameters.Add(idimagen);

            SqlParameter idpdescripcion = new SqlParameter("@desc", descripcion);
            this.com.Parameters.Add(idpdescripcion);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC_SQL";

            this.cn.Open();
            this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
