using crypto_exchange_engine_simple.Engine;


var orderBook = new OrderBook(new Order[]{
    new Order (1,1,"1",1),
    new Order (2,2,"2",1),
    new Order (1,1,"3", 0),
    new Order (2,2,"4", 0)
},new Order[] {
    new Order (2,3,"5", 1),
    new Order (3,2,"6", 1),
    new Order (2,3,"7", 0),
    new Order (3,2,"8", 0),
});



Console.WriteLine("交易开始"); //TODO未完待续
for (int i = 9; i < 20; i++)
{
    var order = new Order(Random.Shared.Next(1, 10), Random.Shared.Next(1, 10), i.ToString(), Random.Shared.Next(0, 1));
    Console.WriteLine($"挂单:{order.ToString()}");
    var result = await orderBook.Process(order);
    Console.WriteLine("==========================================");
    foreach (var item in result)
    {
        Console.WriteLine($"交易信息:{item.ToString()}");
    }
    Console.WriteLine("==========================================");
    Console.WriteLine();
}


Console.Read();