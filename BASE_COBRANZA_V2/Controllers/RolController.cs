using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BASE_COBRANZAS_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class RolController : Controller
    {
        private IRol rolprocess;
        public RolController() { rolprocess = new RepoRol(); }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> listar_rol(int p)
        {
            int nr = 5;
            int tr = rolprocess.ListaRol().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => rolprocess.ListaRol().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Rol());
        }
        //Realizamos el POST para enviarle el rol que queremos registrar
        [HttpPost]
        public IActionResult Create(Rol rol)
        {
            if (!ModelState.IsValid)
            {

                return View(rol);
            }
            ViewBag.mensaje = rolprocess.Agregar(rol);
            return RedirectToAction("listar_rol", "Rol");

        }
        [HttpGet]
        //************creamos la vista
        public IActionResult Detail(int id)
        {
            //almacenamos el registro recuperado
            Rol rol = rolprocess.Buscar(id);
            //aplicamos una condicion...
            if (rol == null) return RedirectToAction("listar_rol", "Rol");
            return View(rol);
        }  //fin del metodo edit GET


        [HttpGet]
        //************creamos la vista
        public IActionResult Edit(int id)
        {
            //almacenamos el registro recuperado
            Rol rol = rolprocess.Buscar(id);
            //aplicamos una condicion...
            if (rol == null) return RedirectToAction("listar_rol", "Rol");
            return View(rol);
        }  //fin del metodo edit GET
        [HttpPost]
        public IActionResult Edit(Rol model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = rolprocess.Actualizar(model);
            return RedirectToAction("listar_rol", "Rol");

        }   //fin del metodo edit POST...
        /**/

        [HttpPost]
        public IActionResult Delete(int ID_ROL)
        {
            ViewBag.mensaje = rolprocess.Eliminar(ID_ROL);
            return RedirectToAction("listar_rol", "Rol");
        }

    }
}
