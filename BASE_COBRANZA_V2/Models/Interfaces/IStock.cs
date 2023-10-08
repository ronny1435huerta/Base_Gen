using BASE_COBRANZA_V2.Models.Beans;

namespace BASE_COBRANZA_V2.Models.Interfaces
{
    public interface IStock
    {
        string guardar_stock(Stock stock);
        IEnumerable<Stock> ListaStock();
        int Obtenerid();
        string Actualizar(Stock stock);
        Stock Buscar(int id);
    }
}
