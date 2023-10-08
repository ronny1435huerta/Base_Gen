using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BASE_COBRANZA_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class Demanda_principalController : Controller
    {
        private IDemanda_principal statusarbitrajeprocess;
        public Demanda_principalController() { statusarbitrajeprocess = new RepoDemanda_principal(); }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> listar_demanda_principal(int p)
        {
            int nr = 100;
            int tr = statusarbitrajeprocess.ListaDemanda_principal().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => statusarbitrajeprocess.ListaDemanda_principal().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Status_demanda_principal());
        }
        //Realizamos el POST para enviarle el procurador que queremos registrar
        [HttpPost]
        public IActionResult Create(Status_demanda_principal Demanda_principal)
        {
            if (!ModelState.IsValid)
            {

                return View(Demanda_principal);
            }
            ViewBag.mensaje = statusarbitrajeprocess.Agregar(Demanda_principal);
            return RedirectToAction("listar_demanda_principal", "Demanda_principal");

        }
        [HttpGet]
        //************creamos la vista
        public IActionResult Detail(int id)
        {
            //almacenamos el registro recuperado
            Status_demanda_principal Demanda_principal = statusarbitrajeprocess.Buscar(id);
            //aplicamos una condicion...
            if (Demanda_principal == null) return RedirectToAction("listar_demanda_principal", "Demanda_principal");
            return View(Demanda_principal);
        }  //fin del metodo edit GET


        [HttpGet]
        //************creamos la vista
        public IActionResult Edit(int id)
        {
            //almacenamos el registro recuperado
            Status_demanda_principal Demanda_principal = statusarbitrajeprocess.Buscar(id);
            //aplicamos una condicion...
            if (Demanda_principal == null) return RedirectToAction("listar_demanda_principal", "Demanda_principal");
            return View(Demanda_principal);
        }  //fin del metodo edit GET
        [HttpPost]
        public IActionResult Edit(Status_demanda_principal model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = statusarbitrajeprocess.Actualizar(model);
            return RedirectToAction("listar_demanda_principal", "Demanda_principal");

        }   //fin del metodo edit POST...
        /**/

        [HttpPost]
        public IActionResult Delete(int ID_DEMANDA_PRINCIPAL)
        {
            ViewBag.mensaje = statusarbitrajeprocess.Eliminar(ID_DEMANDA_PRINCIPAL);
            return RedirectToAction("listar_demanda_principal", "Demanda_principal");
        }
    }
}
