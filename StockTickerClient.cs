using ABXMockExchangeClient.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ABXMockExchangeClient
{
    class StockTickerClient
    {
        private readonly string _serverAddress;
        private readonly int _port;
        private readonly List<TickerData> _receivedData = new List<TickerData>();
        private int _lastSequence = 0;

        public StockTickerClient(string serverAddress, int port)
        {
            _serverAddress = serverAddress;
            _port = port;
        }

        public async Task StartAsync()
        {
            try
            {
                using (TcpClient client = new TcpClient(_serverAddress, _port))
                using (NetworkStream stream = client.GetStream())
                {
                    Console.WriteLine("Connected to the server...");

                    // Continuously read data from the server
                    await ReceiveDataAsync(stream);

                    // Once done, generate the JSON output
                    GenerateJsonOutput("output.json");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task ReceiveDataAsync(NetworkStream stream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                ProcessReceivedData(data);
            }
        }

        private void ProcessReceivedData(string data)
        {
            // Deserialize received data into TickerData object (assuming JSON format)
            TickerData ticker = JsonSerializer.Deserialize<TickerData>(data);

            if (ticker == null)
            {
                Console.WriteLine("Received malformed data.");
                return;
            }

            // Ensure sequence continuity
            if (ticker.Sequence != _lastSequence + 1)
            {
                Console.WriteLine($"Missing sequence: {_lastSequence + 1}");
                // Implement handling for missing sequences if needed (e.g., request missing data)
            }

            _receivedData.Add(ticker);
            _lastSequence = ticker.Sequence;
        }

        private void GenerateJsonOutput(string filePath)
        {
            var jsonData = JsonSerializer.Serialize(_receivedData, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(filePath, jsonData);
            Console.WriteLine($"Data saved to {filePath}");
        }
    }
}
