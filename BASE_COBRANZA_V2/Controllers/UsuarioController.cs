using BASE_COBRANZA_V2.Models.Beans;
using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BASE_COBRANZAS_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public class UsuarioController : Controller
    {
        private IUsuario usuarioprocess;
        private IRol rolprocess;
        public UsuarioController() { usuarioprocess = new RepoUsuario(); rolprocess = new RepoRol(); }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> listar_usuario(int p)
        {
            int nr = 100;
            int tr = usuarioprocess.ListaUsuario().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => usuarioprocess.ListaUsuario().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Usuario());
        }
        //Realizamos el POST para enviarle el usuario que queremos registrar
        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {

                return View(usuario);
            }
            ViewBag.mensaje = usuarioprocess.Agregar(usuario);
            return RedirectToAction("listar_usuario", "Usuario");

        }
        [HttpGet]
        //************creamos la vista
        public IActionResult Detail(int id)
        {
            //almacenamos el registro recuperado
            Usuario usuario = usuarioprocess.Buscar(id);
            //aplicamos una condicion...
            if (usuario == null) return RedirectToAction("listar_usuario", "Usuario");
            return View(usuario);
        }  //fin del metodo edit GET
        
        [HttpGet]
        //************creamos la vista
        public IActionResult Edit(int id)
        {
            //almacenamos el registro recuperado
            Usuario usuario = usuarioprocess.Buscar(id);
            //aplicamos una condicion...
            if (usuario == null) return RedirectToAction("listar_usuario", "Usuario");
            var roles_disponibles = rolprocess.ListaRol()
               .Select(r => new SelectListItem
               {
                   Value = r.ID_ROL.ToString(),
                   Text = r.NOMBRE
               }).ToList();

            // Asignar la lista de SelectListItems a ViewBag.DistritosDisponibles
            ViewBag.roles_disponibles = roles_disponibles;
            return View(usuario);
        }  //fin del metodo edit GET
        [HttpPost]
        public IActionResult Edit(Usuario model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = usuarioprocess.Actualizar(model);
            return RedirectToAction("listar_usuario", "Usuario");

        }   //fin del metodo edit POST...
        /**/
        
        [HttpPost]
        public IActionResult Delete(int ID_USUARIO)
        {
            ViewBag.mensaje = usuarioprocess.Eliminar(ID_USUARIO);
            return RedirectToAction("listar_usuario", "Usuario");
        }
        [HttpGet]
        public async Task<IActionResult> AsignarRol(Usuario usuario)
        {
            usuario = usuarioprocess.Buscar(usuario.ID_USUARIO);
            List<Rol> roles = new List<Rol>();
            roles = usuario.roles;
            Usuarios_Roles usuarios_roles = new Usuarios_Roles();
            usuarios_roles.ID_USUARIO = usuario.ID_USUARIO;
            usuarios_roles.NOMBRE_USUARIO = usuario.NOMBRE_USUARIO;
            usuarios_roles.RolesAsignados = roles;
            ViewBag.roles = roles;
            ViewBag.roles_disponibles = new SelectList(await Task.Run(() => rolprocess.ListaRol()), "ID_ROL", "NOMBRE", usuario.ID_USUARIO);
            return View(usuarios_roles);
        }

        [HttpPost]
        public IActionResult AsignarRol(Usuarios_Roles usuarios_roles)
        {
            Usuario usuario = usuarioprocess.Buscar(usuarios_roles.ID_USUARIO);
            Rol rol = rolprocess.Buscar(usuarios_roles.ID_ROL); // Debes tener un proceso similar para buscar un distrito por su ID

            if (usuario != null && rol != null)
            {
                usuario.roles.Add(rol);
                ViewBag.mensaje = usuarioprocess.AsignarRol(rol, usuario);

                return RedirectToAction("Edit", new { id = usuarios_roles.ID_USUARIO });
            }

            else
            {
                // Maneja el caso en que el procurador o el distrito no se encuentren
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Delete_Rol(string NOMBRE, int ID_USUARIO)
        {
            Rol rol = usuarioprocess.RolXNombre(NOMBRE);
            ViewBag.mensaje = usuarioprocess.Eliminar_Rol(rol.ID_ROL, ID_USUARIO);
            return RedirectToAction("listar_usuario", "Usuario");
        }
    }
}
