using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crypto_exchange_engine_simple.Engine
{
    internal class OrderBook
    {
        public OrderBook(Order[] sellerOrders, Order[] buyerOrders)
        {
            this.sellerOrders = sellerOrders;
            this.buyerOrders = buyerOrders;
        }

        readonly Order[] sellerOrders;
        readonly Order[] buyerOrders;

        internal Task<OrderBook> AddBuyOrder(Order order)
        {
            var n = buyerOrders.Length;
            var i = 0;
            for (i =n-1 ; i >=0; i--)
            {
                var buyOrder = buyerOrders[i];
                if (buyOrder.Price < order.Price)
                    break;
            }
            if (i == n - 1)
            {
                buyerOrders.Append(order);
            }
            else
            {
                buyerOrders.Prepend(order);
            }
            return Task.FromResult(this);
        }

        internal Task<OrderBook> AddSellOrder(Order order)
        {
            var n = sellerOrders.Length;
            var i = 0;
            for (i = n-1; i >= 0 ; i--)
            {
                var sellOrder = sellerOrders[i];
                if(sellOrder.Price > order.Price)
                {
                    break;
                }
            }
            if (i == n - 1)
            {
                sellerOrders.Append(order);
            }
            else
            {
                sellerOrders.Prepend(order);
            }
            return Task.FromResult(this);
        }

        internal Task RemoveBuyOrder(int index)
        {
            var len = buyerOrders.Length;
            for (int i = index; i > len-1; i--)
            {
                buyerOrders[i] = buyerOrders[i+1];
            }
            return Task.CompletedTask;
        }
        internal Task RemoveSellOrder(int index)
        {
            var len = sellerOrders.Length;
            for (int i = index; i > len - 1; i--)
            {
                sellerOrders[i] = sellerOrders[i + 1];
            }
            return Task.CompletedTask;
        }

        internal Task<Trade[]> Process(Order order)
        {
            if (order.Side == 1)
            {
                return ProcessLimitBuy(order);
            }
            return ProcessLimitSell(order);
        }

        internal Task<Trade[]> ProcessLimitBuy(Order order)
        {
            var trades = new List<Trade>();
            var n = sellerOrders.Length;
            if (n != 0 || sellerOrders[n - 1].Price <= order.Price)
            {
                for (int i = n - 1; i >= 0; i--)
                {
                    var sellOrder = sellerOrders[i];
                    if (sellOrder.Price > order.Price)
                        break;
                    if (sellOrder.Amount >= order.Amount) 
                    {
                        trades.Add(new Trade { Amount = order.Amount, Price = sellOrder.Price, MakerOrderId = sellOrder.Id, TakerOrderId = order.Id }); //已撮合写入成交单
                        sellOrder.Amount -= order.Amount;
                        if (sellOrder.Amount == 0)
                        {
                            RemoveSellOrder(i);
                            Console.WriteLine($"已成交:{order.ToString()},对手id:{sellerOrders[i].Id}");
                        }
                        return Task.FromResult(trades.ToArray());
                    }
                    if (sellOrder.Amount < order.Amount)  
                    {
                        trades.Add(new Trade
                        {
                            Amount = sellOrder.Amount,
                            Price = sellOrder.Price,
                            MakerOrderId = sellOrder.Id,
                            TakerOrderId = order.Id
                        });
                        order.Amount -= sellOrder.Amount;  
                        RemoveSellOrder(i);
                        Console.WriteLine($"已成交:{sellerOrders[i].ToString()},对手id:{order.Id}");
                        continue;
                    }
                }
            }
            AddBuyOrder(order); 
            return Task.FromResult(trades.ToArray());
        }

        internal Task<Trade[]> ProcessLimitSell(Order order)
        {
            var trades = new List<Trade>();
            var n = buyerOrders.Length;
            if(n!=0|| buyerOrders[n-1].Price >= order.Price)
            {
                for (int i = n-1; i >=0; i--)
                {
                    var buyerOrder = buyerOrders[i];
                    if(buyerOrder.Price < order.Price)
                    {
                        break;
                    }
                    if(buyerOrder.Amount >= order.Amount)
                    {
                        trades.Add(new Trade { TakerOrderId = order.Id, MakerOrderId = buyerOrder.Id, Amount = order.Amount, Price = buyerOrder.Price });
                        buyerOrder.Amount -= buyerOrder.Amount;
                        if (buyerOrder.Amount == 0)
                        {
                            RemoveBuyOrder(i);
                            Console.WriteLine($"已成交:{order.ToString()},对手id:{buyerOrders[i].Id}");
                        }
                        return Task.FromResult(trades.ToArray());
                    }
                    if(buyerOrder.Amount < order.Amount)
                    {
                        trades.Add(new Trade { TakerOrderId = order.Id, MakerOrderId = buyerOrder.Id, Amount = buyerOrder.Amount, Price = buyerOrder.Price });
                        order.Amount -= buyerOrder.Amount;
                        RemoveBuyOrder(i);
                        Console.WriteLine($"已成交:{buyerOrders[i].ToString()},对手id:{order.Id}");
                        continue;
                    }
                }
            }
            AddSellOrder(order);
            return Task.FromResult(trades.ToArray());
        }
    }
}
