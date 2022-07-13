using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_exchange_engine_simple.Engine
{
    internal class Trade
    {
        internal string TakerOrderId { get; set; }
        internal string MakerOrderId { get; set; }
        internal decimal Amount { get; set; }
        internal decimal Price { get; set; }
        public override string ToString()
        {
            return $"TaskerOrderId:{TakerOrderId},MakerOrderId{MakerOrderId},Amount:{Amount},Price:{Price}";
        }
    }
}
