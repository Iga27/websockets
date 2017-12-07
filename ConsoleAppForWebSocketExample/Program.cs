using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Net;
//nuget Json.net

namespace ConsoleAppForWebSocketExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var ws = new ClientWebSocket())
            { 
            #region sendMessage(Cex.io)
            var subscribe = new Subsribe();
            subscribe.E = "subscribe";
            subscribe.Rooms = new ComplexRooms { Zero = "tickers", One = "pair-BTC-USD" };

            var init = new Init();
            init.E = "init-ohlcv";
            init.I = "15m";
            init.Rooms = new Complex { Zero = "pair-BTC-USD" };

            var unsubscribe = new Subsribe();
            unsubscribe.E = "unsubscribe";
            unsubscribe.Rooms = new ComplexRooms { Zero = "tickers", One = "pair-BTC-USD" };

            var jsonSubscribe = JsonConvert.SerializeObject(subscribe);
            var jsonUnsubscribe = JsonConvert.SerializeObject(unsubscribe);
            var jsonInit = JsonConvert.SerializeObject(init);

            
            #endregion


            //var uri = new Uri("wss://ws.cex.io/ws");
            var uri = new Uri("wss://ws.cex.io/ws"); //"wss://pushstream.tradingview.com/message-pipe-ws/public"
           
            ws.ConnectAsync(uri, CancellationToken.None).Wait();

            //look at what browser sends and what servers sends(different colors)
             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonSubscribe)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonInit)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonUnsubscribe)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonSubscribe)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonInit)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();

           
            var buffer = new byte[3000];
                while (true)
                {
                    var segment = new ArraySegment<byte>(buffer);


                    var result = ws.ReceiveAsync(segment, CancellationToken.None).Result;
                    //ws.ReceiveAsync(segment, CancellationToken.None).Wait();


                    int count = result.Count;
                    while (!result.EndOfMessage)
                    {
                        if (count >= buffer.Length)
                            break; //skip this message

                        segment = new ArraySegment<byte>(buffer, count, buffer.Length - count); //write in buffer begining from count
                        result = ws.ReceiveAsync(segment, CancellationToken.None).Result;
                        count += result.Count;
                    }

                    var message = Encoding.UTF8.GetString(buffer, 0, count); //buffer

                    TradeData data;
                    if (message.Contains("md") && !message.Contains("md_groupped"))
                    {
                        data = JsonConvert.DeserializeObject<TradeData>(message);
                        Console.Write(data.Data.Buy[0][0]);    // 0-49 0-1
                        Console.WriteLine(" " + data.Data.BuyTotal);

                    }
                }
                  
                    
                  // Console.WriteLine(message);
             } 

        }

         
    }
}