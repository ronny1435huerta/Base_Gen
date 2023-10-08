
using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BASE_COBRANZAS_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class Tipo_impulsoController : Controller
    {
        private ITipo_impulso tipoimpulsoprocess;
        public Tipo_impulsoController() { tipoimpulsoprocess = new RepoTipo_impulso(); }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> listar_tipo_impulso(int p)
        {
            int nr = 100;
            int tr = tipoimpulsoprocess.ListaTipo_impulso().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => tipoimpulsoprocess.ListaTipo_impulso().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Tipo_impulso());
        }
        //Realizamos el POST para enviarle el procurador que queremos registrar
        [HttpPost]
        public IActionResult Create(Tipo_impulso Tipo_impulso)
        {
            if (!ModelState.IsValid)
            {

                return View(Tipo_impulso);
            }
            ViewBag.mensaje = tipoimpulsoprocess.Agregar(Tipo_impulso);
            return RedirectToAction("listar_tipo_impulso", "Tipo_impulso");

        }
        [HttpGet]
        //************creamos la vista
        public IActionResult Detail(int id)
        {
            //almacenamos el registro recuperado
            Tipo_impulso Tipo_impulso = tipoimpulsoprocess.Buscar(id);
            //aplicamos una condicion...
            if (Tipo_impulso == null) return RedirectToAction("listar_tipo_impulso", "Tipo_impulso");
            return View(Tipo_impulso);
        }  //fin del metodo edit GET


        [HttpGet]
        //************creamos la vista
        public IActionResult Edit(int id)
        {
            //almacenamos el registro recuperado
            Tipo_impulso Tipo_impulso = tipoimpulsoprocess.Buscar(id);
            //aplicamos una condicion...
            if (Tipo_impulso == null) return RedirectToAction("listar_tipo_impulso", "Tipo_impulso");
            return View(Tipo_impulso);
        }  //fin del metodo edit GET
        [HttpPost]
        public IActionResult Edit(Tipo_impulso model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = tipoimpulsoprocess.Actualizar(model);
            return RedirectToAction("listar_tipo_impulso", "Tipo_impulso");

        }   //fin del metodo edit POST...
        /**/

        [HttpPost]
        public IActionResult Delete(int ID_TIPO_IMPULSO)
        {
            ViewBag.mensaje = tipoimpulsoprocess.Eliminar(ID_TIPO_IMPULSO);
            return RedirectToAction("listar_tipo_impulso", "Tipo_impulso");
        }

    }
}
