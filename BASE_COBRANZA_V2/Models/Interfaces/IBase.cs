using BASE_COBRANZA_V2.Models.Beans;
using BASE_COBRANZAS_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface IBase
    {
        IEnumerable<Base_General> ListaBase();

        //Agregamos un nuevo procurador
        string Actualizar(Base_General base_general);
        string Guardar_stock(int id_stock);

        //Buscamos un procurador por su id
        Base_General Buscar(int id);
        string Eliminar(int base_general);
        IEnumerable<Pago> ListaPago(int IdBase);
        string RegistrarPago(Pago pago);
        IEnumerable<Pago> ListaAllPago();
    }
}
