using BASE_COBRANZA_V2.Models.Beans;
using BASE_COBRANZA_V2.Models.Interfaces;
using BASE_COBRANZA_V2.Models.Repository;
using BASE_COBRANZAS_V2.Models.Beans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace BASE_COBRANZA_V2.Controllers
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class ProcuradorController : Controller
    {
        //Instanciamos la interfaz e inicializamos el procurador por medio del repositorio designado a este
        private IProcurador procuradorprocess;
        public ProcuradorController() { procuradorprocess = new RepoProcurador(); }

        public IActionResult Index()
        {
            return View();
        }
        /*------------------------------------------------Aquí listamos y dirigimos hacia la pantalla de listado---------------------------------------------------*/
        public async Task<IActionResult> listar_procuradores(int p)
        {
            int nr = 100;
            int tr = procuradorprocess.ListaProcurador().Count();
            //De esta forma paginaremos nuestra vista para que nos muestre solo 5 registros por vista. Por ello se inició nr en 5
            int paginas = tr > 0 ? nr % tr > 0 ? tr / nr + 1 : tr / nr : 0;
            ViewBag.paginas = paginas;
            //ViewBag.tipo = await Task.Run(() => discoprocess.ListTipoDisco());
            return View(await Task.Run(() => procuradorprocess.ListaProcurador().Skip(p * nr).Take(nr)));
        }
        /*------------------------------------------------Aquí crearemos el formulario para el ingreso de los datos del procurador---------------------------------------------------*/
        //Realizamos la petición GET para mostrar el formulario
        [HttpGet]
        public IActionResult Create()
        {  
            return View(new Procurador());
        }
        //Realizamos el POST para enviarle el procurador que queremos registrar
        [HttpPost]
        public IActionResult Create(Procurador procurador)
        {
            if (!ModelState.IsValid)
            {
           
                return View(procurador);
            }
            ViewBag.mensaje = procuradorprocess.Agregar(procurador);
            return RedirectToAction("listar_procuradores", "Procurador");

        }

        /*------------------------------------------------Aquí actualizaremos el formulario para el ingreso de los datos del procurador---------------------------------------------------*/
        
        //************creamos la vista
        [HttpGet]
        public IActionResult Edit(int id)
        {
            
            // Almacenamos el registro recuperado
            Procurador procurador = procuradorprocess.Buscar(id);

            // Aplicamos una condición...
            if (procurador == null) return RedirectToAction("listar_procuradores", "Procurador");

            // Obtener la lista de distritos disponibles y convertirla en SelectListItems
            var distritosDisponibles = procuradorprocess.ListaDistrito()
                .Select(d => new SelectListItem
                {
                    Value = d.ID_DISTRITO.ToString(),
                    Text = d.NOMBRE
                }).ToList();

            // Asignar la lista de SelectListItems a ViewBag.DistritosDisponibles
            ViewBag.DistritosDisponibles = distritosDisponibles;

            return View(procurador);
           
          
        }
        [HttpPost]
        public IActionResult Edit(Procurador model)
        {
            //aplicamos una condicion...
            if (!ModelState.IsValid)
            {
                //retornamos
                return View(model);
            }   //fin de la condicion...
            ViewBag.mensaje = procuradorprocess.Actualizar(model);
            return RedirectToAction("listar_procuradores", "Procurador");

        }   //fin del metodo edit POST...
        /**/

        [HttpPost]
        public IActionResult Delete(int ID_PROCURADOR)
        {
            ViewBag.mensaje = procuradorprocess.Eliminar(ID_PROCURADOR);
            return RedirectToAction("listar_procuradores", "Procurador");
        }
        [HttpGet]
        public async Task<IActionResult> AsignarDistrito(Procurador procurador)
        {
            List<Distrito>distritos= new List<Distrito>();
            distritos = procurador.DISTRITOS;
            Procurador_Distrito procurador_Distrito = new Procurador_Distrito();
            procurador_Distrito.ID_PROCURADOR=procurador.ID_PROCURADOR;
            procurador_Distrito.NOMBRE_PROCURADOR = procurador.NOMBRE_PROCURADOR;
            procurador_Distrito.DistritosAsignados=distritos;
            ViewBag.Distritos = distritos;
            ViewBag.Distritos_disponibles = new SelectList(await Task.Run(() => procuradorprocess.ListaDistrito()), "ID_DISTRITO", "NOMBRE", procurador.ID_PROCURADOR);
            return View(procurador_Distrito);
        }

        [HttpPost]
        public IActionResult AsignarDistrito(Procurador_Distrito procurador_distrito)
        {
            Procurador procurador = procuradorprocess.Buscar(procurador_distrito.ID_PROCURADOR);
            Distrito distrito = procuradorprocess.BuscarDistrito(procurador_distrito.ID_DISTRITO); // Debes tener un proceso similar para buscar un distrito por su ID

            if (procurador != null && distrito != null)
            {
                procurador.DISTRITOS.Add(distrito);
                ViewBag.mensaje = procuradorprocess.AsignarDistrito(distrito, procurador);
                
                return RedirectToAction("Edit", new { id = procurador_distrito.ID_PROCURADOR });
            }
        
            else
            {
                // Maneja el caso en que el procurador o el distrito no se encuentren
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Delete_Distrito(string NOMBRE, int ID_PROCURADOR)
        {
            Distrito? distrito = procuradorprocess.BuscarDistritoXNombre(NOMBRE);
            ViewBag.mensaje = procuradorprocess.Eliminar_Distrito(distrito.ID_DISTRITO, ID_PROCURADOR);
            return RedirectToAction("listar_procuradores", "Procurador");
        }

    }
}
