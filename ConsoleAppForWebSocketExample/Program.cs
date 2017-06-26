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
            //ws.Options.SetRequestHeader("Connection", "Upgrade");
            //ws.Options.SetRequestHeader("Upgrade", "websocket");
            ws.Options.SetRequestHeader("Sec-WebSocket-Key", "jEmGLD4EmWMYdeeFCKA8BQ==");
            ws.Options.SetRequestHeader("Sec-WebSocket-Version", "13");
            //var uri = new Uri("wss://ws.cex.io/ws");
            var uri = new Uri("wss://ws.cex.io/ws"); //"wss://pushstream.tradingview.com/message-pipe-ws/public"
            ws.ConnectAsync(uri, CancellationToken.None).Wait();

            #region 
            /*var buffer = new byte[8000];
             var segment = new ArraySegment<byte>(buffer);
             while(ws.State==WebSocketState.Open)
            {
                if (ws.State == WebSocketState.Closed)
                    ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "my status description", CancellationToken.None);

                ws.ReceiveAsync(segment, CancellationToken.None).Wait();
               
                var result = Encoding.UTF8.GetString(buffer,0,8000);
      
                //Data data = JsonConvert.DeserializeObject<Data>(result);
                //Console.WriteLine(data.Id);
            } */
            #endregion

            #region sendMessage(Cex.io)
            /*var subscribe = new Subsribe();
            subscribe.E = "subscribe";
            subscribe.Rooms = new ComplexRooms {  Zero="tickers", One="pair-BTC-USD" } ;

            var init = new Init();
            init.E = "init-ohlcv";
            init.I = "15m";
            init.Rooms = new Complex { Zero="pair-BTC-USD" };

            var unsubscribe = new Subsribe();
            unsubscribe.E = "unsubscribe";
            unsubscribe.Rooms = new ComplexRooms {  Zero="tickers", One="pair-BTC-USD" } ;

            var jsonSubscribe = JsonConvert.SerializeObject(subscribe);
            var jsonUnsubscribe = JsonConvert.SerializeObject(unsubscribe);
            var jsonInit = JsonConvert.SerializeObject(init);

            ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonSubscribe)), WebSocketMessageType.Text, false, CancellationToken.None).Wait(); //???
            ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonInit)), WebSocketMessageType.Text, false, CancellationToken.None).Wait();
            ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonUnsubscribe)), WebSocketMessageType.Text, false, CancellationToken.None).Wait();
            ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonSubscribe)), WebSocketMessageType.Text, false, CancellationToken.None).Wait();
            ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonInit)), WebSocketMessageType.Text, false, CancellationToken.None).Wait();*/
                #endregion

             var buffer = new byte[8000];
             while (true)
             {
                 var segment = new ArraySegment<byte>(buffer);


                 var result =   ws.ReceiveAsync(segment, CancellationToken.None).Result;

                if (result.MessageType == WebSocketMessageType.Close)
                 {
                      ws.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "I don't do binary", CancellationToken.None).Wait();
                     return;
                 }

                 int count = result.Count;
                 while (!result.EndOfMessage)
                 {
                      if (count >= buffer.Length)
                     {
                          ws.CloseOutputAsync(WebSocketCloseStatus.InvalidPayloadData, "That's too long", CancellationToken.None).Wait();
                         return;
                     } 

                     segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
                     result =   ws.ReceiveAsync(segment, CancellationToken.None).Result;
                     count += result.Count;
                 }

                 var message = Encoding.UTF8.GetString(buffer, 0, count);
                 TradeData data = JsonConvert.DeserializeObject<TradeData>(message); //TradeData or Data
                 Console.WriteLine(data.E);
                 //Console.WriteLine(data.Id);
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