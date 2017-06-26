using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppForWebSocketExample
{
    [DataContract]
    public class Data
    {
        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "text")]
        public TextComplex Text { get; set; }
    }

    [DataContract]
    public class TextComplex
    {
        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "content")]
        public ContentComplex Content { get; set; }

    }

    public class ContentComplex
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "score")]
        public int Score { get; set; }

        [DataMember(Name = "user_id")]
        public long UserId { get; set; }

        [DataMember(Name = "vote")]
        public int Vote { get; set; }
    }
}
