using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppForWebSocketExample
{
    [DataContract]
    public class Subsribe
    {
        [DataMember(Name ="e")]
        public string E { get; set; }

        [DataMember(Name = "rooms")]
        public ComplexRooms Rooms { get; set; }
    }

     [DataContract]
    public class ComplexRooms
    {
        [DataMember(Name = "0")]
        public string Zero { get; set; }

        [DataMember(Name = "1")]
        public string One { get; set; }
    } 
}
