using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABXMockExchangeClient.Model
{
    public class TickerData
    {
        public int Sequence { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
