using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPiCurrencies.Models
{
    public class CurrencyPairs
    {
        public string  pairName { get; set; }
        public float pairValue { get; set; }
        public DateTime time { get; set; }
    }
}
