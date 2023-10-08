using BASE_COBRANZA_V2.Models.Interfaces;
using NPOI.SS.Formula.Functions;
using System.Data;
using System.Security.Cryptography.Xml;

namespace BASE_COBRANZA_V2.Models.Beans
{
    public class Pago
    {      
        public int ID_PAGO { get; set; }
        public int ID_BASE { get; set; }
        public Decimal MONTO { get; set; }
        public string TIPO_PAGO { get; set; }
        public string CUENTA_BANCARIA { get; set; }
        public string NUMERO_OPERACION { get; set; }
        public DateTime FECHA_PAGO { get; set; }
        public string INSTANCIA_PAGO { get; set; }
        
    }
}
