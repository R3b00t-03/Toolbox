using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinger
{
    public class PingResultsItemTemplate
    {
        public string Host { get; set; }
        public float roundTripTime { get; set; }
        public bool Pingable { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
