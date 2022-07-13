using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_exchange_engine_simple.Engine
{
    internal class Order
    {
        internal decimal Amount { get; set; }
        internal decimal Price { get; set; }
        internal string Id { get; set; }
        /// <summary>
        /// 0 现价卖单 、1 限价买单
        /// </summary>
        internal int Side { get; set; } 
        public Order(decimal amount,decimal price,string id,int side)
        {
            this.Amount = amount;
            this.Price = price;
            this.Side = side;
            this.Id = id;
        }
        public override string ToString()
        {
            return $"<Id:{Id},Side{Side},Amount:{Amount},Price:{Price}>";
        }
    }
}
