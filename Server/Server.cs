using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Server
{
    public interface IServer
    {
        void ExecuteServer();

    }
    public class Server : IServer
    {
        private readonly IServerService _serverService;
        public Server(IServerService serverService)
        {
            _serverService = serverService;
        }
        public void ExecuteServer()
        {
            try
            {
                IPAddress ipAd = IPAddress.Parse("192.168.15.53");
                // use local m/c IP address, and 
                // use the same in the client

                /* Initializes the Listener */
                TcpListener myList = new TcpListener(IPAddress.Any, 13);

                /* Start Listeneting at the specified port */
                myList.Start();

                Console.WriteLine("The server is running at port 13...");
                Console.WriteLine("The local End point is  :" +
                                  myList.LocalEndpoint);
                Console.WriteLine("Waiting for a connection.....");


                Socket s = myList.AcceptSocket();

                Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

                var sw = new Stopwatch();
                sw.Start();

                NetworkStream ns = new NetworkStream(s);

                byte[] bytes = new byte[105000000];
                int k = s.Receive(bytes);
                Console.WriteLine("Recieved...");
                _serverService.ConvertData(bytes);

                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes("The string was recieved by the server."));
                Console.WriteLine("\nSent Acknowledgement");

                /* clean up */
                s.Close();
                myList.Stop();

                sw.Stop();
                Console.WriteLine("Tempo gasto : " + sw.ElapsedMilliseconds.ToString() + " milisegundos total");
                Console.WriteLine("Salvo com sucesso.");
                System.Console.ReadKey();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
                System.Console.ReadKey();
            }

        }
    }
}
