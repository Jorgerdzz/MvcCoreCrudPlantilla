using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using MvcCoreCrudPlantilla.Models;
using System.Data;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region STORE PROCEDURE
//alter procedure SP_PLANTILLA_UPSERT
//(@idHospital int, @idSala int, @idEmpleado int, @apellido nvarchar(50), @funcion nvarchar(50), @turno nvarchar(50), @salario int)
//as
//	declare @numEmpleado int
//	select @numEmpleado = EMPLEADO_NO from PLANTILLA where EMPLEADO_NO = @idEmpleado;

//if (@numEmpleado is null)
//    begin
//        insert into PLANTILLA values(@idHospital, @idSala, @idEmpleado, @apellido, @funcion, @turno, @salario)
//	end
//	else
//	begin
//		update PLANTILLA set HOSPITAL_COD = @idHospital, SALA_COD = @idSala,
//        APELLIDO = @apellido, FUNCION = @funcion, T = @turno, SALARIO = @salario
//		where EMPLEADO_NO = @idEmpleado
//	end
//go
#endregion

namespace MvcCoreCrudPlantilla.Repositories
{
    public class RepositoryPlantilla
    {
        private DataTable tablaPlantilla;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryPlantilla()
        {
            string connectionString = "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=HOSPITAL;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from PLANTILLA";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaPlantilla = new DataTable();
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            ad.Fill(this.tablaPlantilla);
        }

        public List<Plantilla> GetPlantilla()
        {
            var consulta = from datos in this.tablaPlantilla.AsEnumerable()
                           select datos;
            List<Plantilla> plantilla = new List<Plantilla>();
            foreach(var row in consulta)
            {
                Plantilla p = new Plantilla
                {
                    IdEmpleado = row.Field<int>("EMPLEADO_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Funcion = row.Field<string>("FUNCION"),
                    Turno = row.Field<string>("T"),
                    Salario = row.Field<int>("SALARIO"),
                    IdHospital = row.Field<int>("HOSPITAL_COD"),
                    IdSala = row.Field<int>("SALA_COD"),
                };
                plantilla.Add(p);
            }
            return plantilla;
        }

        public Plantilla FindEmpleadoPlantilla(int idEmpleado)
        {
            var consulta = from datos in this.tablaPlantilla.AsEnumerable()
                           where datos.Field<int>("EMPLEADO_NO") == idEmpleado
                           select datos;
            var row = consulta.First();
            Plantilla empleado = new Plantilla
            {
                IdEmpleado = row.Field<int>("EMPLEADO_NO"),
                Apellido = row.Field<string>("APELLIDO"),
                Funcion = row.Field<string>("FUNCION"),
                Turno = row.Field<string>("T"),
                Salario = row.Field<int>("SALARIO"),
                IdHospital = row.Field<int>("HOSPITAL_COD"),
                IdSala = row.Field<int>("SALA_COD"),
            };
            return empleado;
        }

        public List<string> GetFuncionesPlantilla()
        {
            var consulta = (from datos in this.tablaPlantilla.AsEnumerable()
                           select datos.Field<string>("FUNCION")).Distinct();
            List<string> funciones = consulta.ToList();
            return funciones;
        }

        public ResumenPlantilla GetEmpleadosPlantillaFuncion(string funcion)
        {
            var consulta = from datos in this.tablaPlantilla.AsEnumerable()
                           where datos.Field<string>("FUNCION") == funcion
                           select datos;
            if(consulta.Count() == 0)
            {
                ResumenPlantilla model = new ResumenPlantilla
                {
                    MaximoSalario = 0,
                    SumaSalarial = 0,
                    MediaSalarial = 0,
                    Empleados = null,
                };
                return model;
            }
            else
            {
                int salarioMaximo = consulta.Max(x => x.Field<int>("SALARIO"));
                int sumaSalarial = consulta.Sum(x => x.Field<int>("SALARIO"));
                double mediaSalarial = consulta.Average(x => x.Field<int>("SALARIO"));
                List<Plantilla> plantilla = new List<Plantilla>();
                foreach(var row in consulta)
                {
                    Plantilla empleado = new Plantilla
                    {
                        IdEmpleado = row.Field<int>("EMPLEADO_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Funcion = row.Field<string>("FUNCION"),
                        Turno = row.Field<string>("T"),
                        Salario = row.Field<int>("SALARIO"),
                        IdHospital = row.Field<int>("HOSPITAL_COD"),
                        IdSala = row.Field<int>("SALA_COD"),
                    };
                    plantilla.Add(empleado);
                }
                ResumenPlantilla model = new ResumenPlantilla
                {
                    MaximoSalario = salarioMaximo,
                    SumaSalarial = sumaSalarial,
                    MediaSalarial = mediaSalarial,
                    Empleados = plantilla
                };
                return model;
            }
        }

        public async Task InsertEmpleado(int idHospital, int idSala, int idEmpleado, string apellido, string funcion, string turno, int salario)
        {
            string sql = "SP_PLANTILLA_UPSERT";
            this.com.Parameters.AddWithValue("@idHospital", idHospital);
            this.com.Parameters.AddWithValue("@idSala", idSala);
            this.com.Parameters.AddWithValue("@idEmpleado", idEmpleado);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@funcion", funcion);
            this.com.Parameters.AddWithValue("@turno", turno);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task UpdateEmpleado(int idHospital, int idSala, int idEmpleado, string apellido, string funcion, string turno, int salario)
        {
            string sql = "SP_PLANTILLA_UPSERT";
            this.com.Parameters.AddWithValue("@idHospital", idHospital);
            this.com.Parameters.AddWithValue("@idSala", idSala);
            this.com.Parameters.AddWithValue("@idEmpleado", idEmpleado);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@funcion", funcion);
            this.com.Parameters.AddWithValue("@turno", turno);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

    }
}
