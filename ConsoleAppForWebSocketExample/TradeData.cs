using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppForWebSocketExample
{
    [DataContract]
    public class TradeData
    {
        [DataMember(Name = "e")]
        public string E { get; set; }

        [DataMember(Name = "data")]
        public ComplexData Data { get; set; }
    }

    [DataContract]
    public class ComplexData
    {
        [DataMember(Name = "buy_total")]
        public long BuyTotal { get; set; }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "pair")]
        public string Pair { get; set; }

        [DataMember(Name = "buy")]
        public List<List<double>> Buy { get; set; } //mb [] not needed List<List<double>>

        [DataMember(Name = "sell")]
        public List<List<double>> Sell { get; set; }

        [DataMember(Name = "sell_total")]
        public long SellTotal { get; set; }
    }

    /*[DataContract]
    public class ComplexElement
    {
        [DataMember(Name = "0")]
        public double Zero { get; set; }

        [DataMember(Name = "1")]
        public long One { get; set; }
    }*/
}
