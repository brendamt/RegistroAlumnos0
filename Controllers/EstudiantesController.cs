using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using RegistroAlumnos0.Models;
using System.Web;


namespace RegistroAlumnos0.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly IConfiguration configuration;
        
        public EstudiantesController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        [HttpGet]
        public IActionResult Index()
        {
            
            List<EstudianteModel> ListaEstudiantes = new List<EstudianteModel>();
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("", con))
                    {
                        cmd.CommandText = "Select * From Estudiante where Estado =0";
                        con.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();

                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                ListaEstudiantes.Add(new EstudianteModel
                                {
                                    ID = Convert.ToInt32(sdr["Id"]),
                                    Nombre = sdr["Nombre"].ToString(),
                                    FechaNacimiento = sdr["FechaNacimiento"].ToString(),
                                    Curso = sdr["Curso"].ToString(),
                                    Estado = Convert.ToInt32(sdr["Estado"])
                                });
                            }
                        }

                    }

                }

                foreach (var Est in ListaEstudiantes)
                {
                    if (Est.Estado == 1)
                        Est.DescripEstado = "Inactivo";
                    else
                        Est.DescripEstado = "Activo";


                }
            }catch(Exception exp)
            {

            }
            return View(ListaEstudiantes);
        }
        [HttpGet]
        public IActionResult Agregar(int Id = 0)
        {
            EstudianteModel estudiante = new EstudianteModel();

            try
            {
                if (Id != 0)
                {
                    using (SqlConnection con = GetConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand("", con))
                        {
                            cmd.CommandText = $"Select * From Estudiante where Id ={Id}";
                            con.Open();
                            SqlDataReader sdr = cmd.ExecuteReader();

                            if (sdr.HasRows)
                            {
                                if (sdr.Read())
                                {


                                    estudiante.ID = Convert.ToInt32(sdr["Id"]);
                                    estudiante.Nombre = sdr["Nombre"].ToString();
                                    estudiante.FechaNacimiento = sdr["FechaNacimiento"].ToString();
                                    estudiante.Curso = sdr["Curso"].ToString();
                                    estudiante.Estado = Convert.ToInt32(sdr["Estado"]);

                                }
                            }

                        }
                    }
                }
                if (estudiante.Estado == 1)
                    estudiante.DescripEstado = "Inactivo";
                else
                    estudiante.DescripEstado = "Activo";
            }catch(Exception exp)
            {

            }
            return View(estudiante);


            } 
        [HttpPost]
        public IActionResult Agregar(EstudianteModel estudiante)
        {
            try
            {
                DateTime fecha = Convert.ToDateTime(estudiante.FechaNacimiento);// conversion de fecha

                estudiante.FechaNacimiento = string.Format(estudiante.FechaNacimiento, "dd/mm/aaaa");

                if (((DateTime.Today - fecha).TotalDays) / 365 >= 16)//calculo de edad
                {
                    if (estudiante.ID == 0)
                    {
                        using (SqlConnection con = GetConnection())
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("", con);
                            cmd.CommandText = $"INSERT INTO Estudiante (Nombre, FechaNacimiento,Curso) values('{estudiante.Nombre}','{estudiante.FechaNacimiento}'," +
                                $"'{estudiante.Curso}')";
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    else
                    {
                        using (SqlConnection con = GetConnection())
                        {
                            con.Open();
                            SqlCommand cmd = new SqlCommand("", con);
                            cmd.CommandText = $"UPDATE Estudiante SET Nombre = '{estudiante.Nombre}', FechaNacimiento='{estudiante.FechaNacimiento}',Curso ='{estudiante.Curso}' where Id = {estudiante.ID}";
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

                else
                {
                    return View(estudiante);
                }
            }catch(Exception exp)
            {

            }
            return RedirectToAction("Index");// redireccionar a la pagina principal
        }

        public IActionResult Eliminar(int id)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("", con);
                    cmd.CommandText = $"UPDATE Estudiante SET Estado = 1 where Id = {id}";
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }catch(Exception exp)
            {

            }
                return RedirectToAction("Index");// redireccionar a la pagina principal
            
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(configuration.GetConnectionString("ConexionSqlServer"));//Retornar conexion a la base de datos
        }
       
    }
}
