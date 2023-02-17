using MvcSegundaPracticaNazarRomanyuk.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
#region PROCEDURE
//create or replace procedure SP_INSERT_COMIC_ORACLE
//(p_nombre comics.nombre%type,
//p_img comics.imagen%type,
//p_desc comics.descripcion%type)
//as
//begin
// insert into COMICS values((select max(idcomic) +1 from comics), p_nombre, p_img, p_desc);
//commit;
//end; 
#endregion
namespace MvcSegundaPracticaNazarRomanyuk.Repositories
{
    public class RepositoryComicsOracle : IRepositoryComics
    {
        private OracleConnection cn;
        private OracleCommand com;
        private OracleDataAdapter adapter;
        private DataTable TablaComic;
        //constructor de la clase en el que inicualizo los objetos, declaro la conexion y una consulta para el linq
        public RepositoryComicsOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE;Persist Security Info=false;User ID=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;

            string sql = "select * from comics";
            this.adapter = new OracleDataAdapter(sql, connectionString);
            this.TablaComic = new DataTable();
            this.adapter.Fill(this.TablaComic);
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

            OracleParameter idimagen = new OracleParameter(":img", imagen);
            this.com.Parameters.Add(idimagen);

            OracleParameter idpdescripcion = new OracleParameter(":desc", descripcion);
            this.com.Parameters.Add(idpdescripcion);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC_ORACLE";

            this.cn.Open();
            this.com.ExecuteNonQuery();

            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
