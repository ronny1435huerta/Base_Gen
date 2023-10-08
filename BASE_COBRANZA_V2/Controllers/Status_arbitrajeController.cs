using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BASE_COBRANZA_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class Status_arbitrajeController : Controller
    {
        private IStatus_arbitraje statusarbitrajeprocess;
        public Status_arbitrajeController() { statusarbitrajeprocess = new RepoStatus_arbitraje(); }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> listar_status_arbitraje(int p)
        {
            int nr = 100;
            int tr = statusarbitrajeprocess.ListaStatus_arbitraje().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => statusarbitrajeprocess.ListaStatus_arbitraje().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Status_arbitraje());
        }
        //Realizamos el POST para enviarle el procurador que queremos registrar
        [HttpPost]
        public IActionResult Create(Status_arbitraje Status_arbitraje)
        {
            if (!ModelState.IsValid)
            {

                return View(Status_arbitraje);
            }
            ViewBag.mensaje = statusarbitrajeprocess.Agregar(Status_arbitraje);
            return RedirectToAction("listar_status_arbitraje", "Status_arbitraje");

        }
        [HttpGet]
        //************creamos la vista
        public IActionResult Detail(int id)
        {
            //almacenamos el registro recuperado
            Status_arbitraje Status_arbitraje = statusarbitrajeprocess.Buscar(id);
            //aplicamos una condicion...
            if (Status_arbitraje == null) return RedirectToAction("listar_status_arbitraje", "Status_arbitraje");
            return View(Status_arbitraje);
        }  //fin del metodo edit GET


        [HttpGet]
        //************creamos la vista
        public IActionResult Edit(int id)
        {
            //almacenamos el registro recuperado
            Status_arbitraje Status_arbitraje = statusarbitrajeprocess.Buscar(id);
            //aplicamos una condicion...
            if (Status_arbitraje == null) return RedirectToAction("listar_status_arbitraje", "Status_arbitraje");
            return View(Status_arbitraje);
        }  //fin del metodo edit GET
        [HttpPost]
        public IActionResult Edit(Status_arbitraje model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = statusarbitrajeprocess.Actualizar(model);
            return RedirectToAction("listar_status_arbitraje", "Status_arbitraje");

        }   //fin del metodo edit POST...
        /**/

        [HttpPost]
        public IActionResult Delete(int ID_Status_arbitraje)
        {
            ViewBag.mensaje = statusarbitrajeprocess.Eliminar(ID_Status_arbitraje);
            return RedirectToAction("listar_status_arbitraje", "Status_arbitraje");
        }
    }
}
