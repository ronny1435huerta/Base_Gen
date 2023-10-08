using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BASE_COBRANZAS_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class Status_poder_judicialController : Controller
    {
        private IStatus_poder_judicial statuspoderjudicialprocess;
        public Status_poder_judicialController() { statuspoderjudicialprocess = new RepoStatus_poder_judicial(); }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> listar_status_poder_judicial(int p)
        {
            int nr = 100;
            int tr = statuspoderjudicialprocess.ListaStatus_poder_judicial().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => statuspoderjudicialprocess.ListaStatus_poder_judicial().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new status_poder_judicial());
        }
        //Realizamos el POST para enviarle el procurador que queremos registrar
        [HttpPost]
        public IActionResult Create(status_poder_judicial status_poder_judicial)
        {
            if (!ModelState.IsValid)
            {

                return View(status_poder_judicial);
            }
            ViewBag.mensaje = statuspoderjudicialprocess.Agregar(status_poder_judicial);
            return RedirectToAction("listar_status_poder_judicial", "Status_poder_judicial");

        }
        [HttpGet]
        //************creamos la vista
        public IActionResult Detail(int id)
        {
            //almacenamos el registro recuperado
            status_poder_judicial status_poder_judicial = statuspoderjudicialprocess.Buscar(id);
            //aplicamos una condicion...
            if (status_poder_judicial == null) return RedirectToAction("listar_poder_status_judicial", "Status_poder_judicial");
            return View(status_poder_judicial);
        }  //fin del metodo edit GET


        [HttpGet]
        //************creamos la vista
        public IActionResult Edit(int id)
        {
            //almacenamos el registro recuperado
            status_poder_judicial status_poder_judicial = statuspoderjudicialprocess.Buscar(id);
            //aplicamos una condicion...
            if (status_poder_judicial == null) return RedirectToAction("listar_status_poder_judicial", "Status_poder_judicial");
            return View(status_poder_judicial);
        }  //fin del metodo edit GET
        [HttpPost]
        public IActionResult Edit(status_poder_judicial model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = statuspoderjudicialprocess.Actualizar(model);
            return RedirectToAction("listar_status_poder_judicial", "Status_poder_judicial");

        }   //fin del metodo edit POST...
        /**/

        [HttpPost]
        public IActionResult Delete(int ID_STATUS_PODER_JUDICIAL)
        {
            ViewBag.mensaje = statuspoderjudicialprocess.Eliminar(ID_STATUS_PODER_JUDICIAL);
            return RedirectToAction("listar_status_poder_judicial", "Status_poder_judicial");
        }

    }
}
