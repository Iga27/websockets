using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppForWebSocketExample
{
    [DataContract]
    public class Init
    {
        [DataMember(Name = "e")]
        public string E { get; set; }

        [DataMember(Name = "i")]
        public string I { get; set; }

        [DataMember(Name = "rooms")]
        public Complex Rooms { get; set; }
    }

     [DataContract]
    public class Complex
    {
        [DataMember(Name = "0")]
        public string Zero { get; set; }
    } 
}
