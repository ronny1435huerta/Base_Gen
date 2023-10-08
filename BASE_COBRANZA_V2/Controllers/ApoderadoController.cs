using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BASE_COBRANZA_V2.ContApoderadolers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class ApoderadoController : Controller
    {
        private IApoderado Apoderadoprocess;
        public ApoderadoController() { Apoderadoprocess = new RepoApoderado(); }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> listar_Apoderado(int p)
        {
            int nr = 100;
            int tr = Apoderadoprocess.ListaApoderado().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => Apoderadoprocess.ListaApoderado().Skip(p * nr).Take(nr)));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Apoderado());
        }
        //Realizamos el POST para enviarle el Apoderado que queremos registrar
        [HttpPost]
        public IActionResult Create(Apoderado Apoderado)
        {
            if (!ModelState.IsValid)
            {

                return View(Apoderado);
            }
            ViewBag.mensaje = Apoderadoprocess.Agregar(Apoderado);
            return RedirectToAction("listar_Apoderado", "Apoderado");

        }
        [HttpGet]
        //************creamos la vista
        public IActionResult Detail(int id)
        {
            //almacenamos el registro recuperado
            Apoderado Apoderado = Apoderadoprocess.Buscar(id);
            //aplicamos una condicion...
            if (Apoderado == null) return RedirectToAction("listar_Apoderado", "Apoderado");
            return View(Apoderado);
        }  //fin del metodo edit GET


        [HttpGet]
        //************creamos la vista
        public IActionResult Edit(int id)
        {
            //almacenamos el registro recuperado
            Apoderado Apoderado = Apoderadoprocess.Buscar(id);
            //aplicamos una condicion...
            if (Apoderado == null) return RedirectToAction("listar_Apoderado", "Apoderado");
            return View(Apoderado);
        }  //fin del metodo edit GET
        [HttpPost]
        public IActionResult Edit(Apoderado model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = Apoderadoprocess.Actualizar(model);
            return RedirectToAction("listar_Apoderado", "Apoderado");

        }   //fin del metodo edit POST...
        /**/

        [HttpPost]
        public IActionResult Delete(int ID_Apoderado)
        {
            ViewBag.mensaje = Apoderadoprocess.Eliminar(ID_Apoderado);
            return RedirectToAction("listar_Apoderado", "Apoderado");
        }

    }
}

