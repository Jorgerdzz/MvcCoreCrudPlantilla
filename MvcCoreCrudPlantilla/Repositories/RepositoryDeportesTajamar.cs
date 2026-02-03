using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcCoreCrudPlantilla.Models;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

#region FUNCTIONS
//CREATE OR ALTER FUNCTION fn_DEPORTES_TAJAMAR (@idUsuario INT)
//RETURNS TABLE
//AS
//RETURN
//(
//    SELECT  
//        U.NOMBRE,
//        U.APELLIDOS,
//        U.EMAIL,
//        U.IMAGEN,
//        A.nombre AS NOMBRE_ACTIVIDAD,
//        I.fecha_inscripcion,
//        I.quiere_ser_capitan,
//        C.NOMBRE AS NOMBRE_CURSO
//    FROM USUARIOSTAJAMAR U
//    INNER JOIN CURSOSTAJAMAR C
//        ON U.IDCURSO = C.IDCURSO
//    INNER JOIN INSCRIPCIONES I
//        ON U.IDUSUARIO = I.id_usuario
//    INNER JOIN EVENTO_ACTIVIDADES E
//        ON I.IdEventoActividad = E.IdEventoActividad
//    INNER JOIN ACTIVIDADES A
//        ON E.IdActividad = A.id_actividad
//    WHERE U.IDUSUARIO = @idUsuario
//);
//GO
#endregion

namespace MvcCoreCrudPlantilla.Repositories
{
    public class RepositoryDeportesTajamar
    {
        private DataTable tablaAlumnos;
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public RepositoryDeportesTajamar()
        {
            string connectionString = "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=PRACTICADEPORTES;User ID=SA;Trust Server Certificate=True";
            string sql = "select * from USUARIOSTAJAMAR";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaAlumnos = new DataTable();
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            ad.Fill(this.tablaAlumnos);
        }

        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in this.tablaAlumnos.AsEnumerable()
                           select datos;
            List<Usuario> usuarios = new List<Usuario>();
            foreach(var row in consulta)
            {
                Usuario user = new Usuario
                {
                    IdUsuario = row.Field<int>("IDUSUARIO"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Apellidos = row.Field<string>("APELLIDOs"),
                    Email = row.Field<string>("EMAIL"),
                    Imagen = row.Field<string>("IMAGEN"),
                    IdCurso = row.Field<int>("IDCURSO"),
                };
                usuarios.Add(user);
            }
            return usuarios;
        }

        public async Task<DatosUsuario> GetDatosUsuario(int idUsuario)
        {
            string sql = "SELECT * FROM fn_DEPORTES_TAJAMAR(@idUsuario)";
            this.com.Parameters.AddWithValue("@idUsuario", idUsuario);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            DatosUsuario user = new DatosUsuario();
            this.reader = await this.com.ExecuteReaderAsync();
            while (await this.reader.ReadAsync())
            {
                user.Nombre = this.reader["NOMBRE"].ToString();
                user.Apellidos = this.reader["APELLIDOS"].ToString();
                user.Email = this.reader["EMAIL"].ToString();
                user.Imagen = this.reader["IMAGEN"].ToString();
                user.NombreActividad = this.reader["NOMBRE_ACTIVIDAD"].ToString();
                user.FechaInscripcion = DateTime.Parse(this.reader["fecha_inscripcion"].ToString());
                user.Capitan = bool.Parse(this.reader["quiere_ser_capitan"].ToString());
                user.NombreCurso = this.reader["NOMBRE_CURSO"].ToString();
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return user;
        }

    }
}
