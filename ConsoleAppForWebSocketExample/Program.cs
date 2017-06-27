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
             
            var ws = new ClientWebSocket();

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

             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonSubscribe)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonInit)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonUnsubscribe)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonSubscribe)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
             ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonInit)), WebSocketMessageType.Text, true, CancellationToken.None).Wait();

           
            var buffer = new byte[8000];
             while (true)
             {
                 var segment = new ArraySegment<byte>(buffer);


                var result =   ws.ReceiveAsync(segment, CancellationToken.None).Result;
                //ws.ReceiveAsync(segment, CancellationToken.None).Wait();
                 

                  int count = result.Count;
                 while (!result.EndOfMessage)
                 {
                    if (count >= buffer.Length)
                    {
                        break;
                    }
                    segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                     result =   ws.ReceiveAsync(segment, CancellationToken.None).Result;
                     count += result.Count;
                 } 

                 var message = Encoding.UTF8.GetString(buffer,0,count); //buffer

                TradeData data;
                if (message.Contains("md") && !message.Contains("md_groupped"))
                {
                    data = JsonConvert.DeserializeObject<TradeData>(message);
                    Console.Write(data.Data.Buy[0][0]);    // 0-49 0-1
                    Console.WriteLine(" " + data.Data.Buy[0][1]+" "+data.Data.BuyTotal);

                }
                  
                   //string e = data.E;
                    
                  // Console.WriteLine(message);
             } 

        }

        #region
        /* static async Task Client()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var socket = new ClientWebSocket();
            var uri = new Uri("wss://ws.cex.io/ws");


            //socket.Options.SetRequestHeader("Connection", "Upgrade");
             
            await socket.ConnectAsync(uri, CancellationToken.None);

            ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[5000]);
            WebSocketReceiveResult result = null;


            while (socket.State == WebSocketState.Open)
            {

                Console.WriteLine("hello");

                using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await
                            socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close response received",
                                CancellationToken.None);
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                        {
                           string jsonResult = "";
                            using (var reader = new StreamReader(ms, Encoding.UTF8))
                            {
                               jsonResult = reader.ReadToEnd();
                            }
                        TradeData results = JsonConvert.DeserializeObject<TradeData>(jsonResult);
                        //Console.WriteLine(results.Data.BuyTotal);

                        }
                    
                }
               // result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            }
        }*/
        #endregion
    }
}