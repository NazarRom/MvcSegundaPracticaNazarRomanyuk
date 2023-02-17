using MvcSegundaPracticaNazarRomanyuk.Models;
using System.Data.SqlClient;
using System.Data;

#region PROCEDURE
//create procedure SP_INSERT_COMIC_SQL
//(@nombre nvarchar(50),@img nvarchar(600),@desc nvarchar(500))
//as
//DECLARE @id INT
//SELECT @id = MAX(IDCOMIC)+1 FROM COMICS
//insert into COMICS values (@id, @nombre, @img, @desc)
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
        //constructor de la clase en el que inicualizo los objetos, declaro la conexion y una consulta para el linq
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
        //metodo que devuelve una lista del modelo comic
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
       
        //metodo para insertar
        public void InsertComic(string nombre, string imagen, string descripcion)
        {
         
            

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
