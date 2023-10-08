using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using System.Data;
using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZAS_V2.Controllers
{
    public class AutenticacionController : Controller
    {
        //Conexion a la BD..
        private readonly IConfiguration _IConfig;
        private IUsuario usuarioprocess;
        public AutenticacionController(IConfiguration iConfig)
        {
            _IConfig = iConfig;
            usuarioprocess = new RepoUsuario();

        }//fin del constructor..


        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Logueo()
        {
            //instanciamos la clase ClassUsuarioModel
            Usuario usu = new Usuario();
            return View(usu);
        }
        [HttpPost]
        public async Task<IActionResult> Logueo(Usuario reg)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_IConfig["ConnectionStrings:cn"]))
            {
                //aplicamos una condicion...
                if (string.IsNullOrEmpty(reg.NOMBRE_USUARIO) || string.IsNullOrEmpty(reg.CONTRA_USUARIO))
                {
                    ModelState.AddModelError("", "Ingresar los datos solicitados");
                }
                else
                {
                    try
                    {
                        //aperturamos la base de datos...
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("SP_SEGURIDAD_USUARIO", cn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //agregamos los parametros...
                        cmd.Parameters.AddWithValue("@usu", reg.NOMBRE_USUARIO);
                        cmd.Parameters.AddWithValue("@clav", reg.CONTRA_USUARIO);
                        //realizamos la ejecucion...
                        SqlDataReader dr = cmd.ExecuteReader();
                        //aplicamos una segunda condicion...
                        if (dr.Read())
                        {
                            string usuario = reg.NOMBRE_USUARIO;
                            List<Rol> rolesDelUsuario = ObtenerRolesDeLaBaseDeDatos(usuario);

                            // Aplicamos las claims (notificadores)
                            var claims = new List<Claim>
                             {
                                new Claim(ClaimTypes.Name, usuario)
                              };

                            // Agregamos cada rol como una reclamación
                            foreach (var rol in rolesDelUsuario)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, rol.NOMBRE)); // Suponiendo que el nombre del rol se encuentra en la propiedad "Nombre" de la clase Rol
                            }

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                           
                            // Redireccionamos
                            return RedirectToAction("listar_base", "Base_General");
                        }
                        else
                        {
                            //envie mensaje..
                            ModelState.AddModelError("", "Datos ingresados no son validos");

                        }//fin de la segunda condicion...
                    }
                    catch (Exception ex)
                    {
                        mensaje = ex.Message;

                    }    //fin de catch...

                }   //fin de la condicion...

            }  //fin del using....

            //almacenamos el mensaje  para enviarlos a la vista
            ViewBag.mensaje = mensaje;
            return View();

        }  //fin del metodo INDEX POST...
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Logueo", "Autenticacion");
        }
        private List<Rol> ObtenerRolesDeLaBaseDeDatos(string usuario)
        {
            List<Rol> roles = new List<Rol>();
            Usuario? User= usuarioprocess.BuscarPorNombreUsuario(usuario);
            string connectionString = _IConfig["ConnectionStrings:cn"];

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_LISTAR_ROLES_USUARIO", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_USUARIO", User.ID_USUARIO);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            roles.Add(new Rol
                            {
                                ID_ROL = dr.GetInt32(0), // Asegúrate de tener la estructura correcta de tu tabla de roles
                                NOMBRE = dr.GetString(1) // Suponiendo que la columna 1 contiene el nombre del rol
                            });
                        }
                    }
                }
            }

            return roles;
        }
        public IActionResult Mensaje()
        {
            return View();
        }
    }
}
