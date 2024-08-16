namespace ABXMockExchangeClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //server has to change
            string serverAddress = "exchange.abx.com";
            int port = 8080;

            StockTickerClient client = new StockTickerClient(serverAddress, port);
            await client.StartAsync();
        }
    }


}