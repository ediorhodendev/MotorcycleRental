using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Configuration
{
    public class KafkaOptions
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
    }
}
