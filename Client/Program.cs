using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        private const int portNum = 13;
        private const string hostName = "192.168.15.53";
        public static int Main(string[] args)
        {
            try
            {
                var client = new TcpClient(hostName, portNum);

                Console.WriteLine("Connected");
                Console.Write("Enter the string to be transmitted : ");

                string text = System.IO.File.ReadAllText(@"C:\Users\bruna.oliveira\source\repos\access_split.log");
                //var s = System.IO.File.GetBytes ?
                NetworkStream stm = client.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(text);
                Console.WriteLine("Transmitting.....");

                stm.Write(ba, 0, ba.Length);

                byte[] bytes = new byte[1024];
                int bytesRead = stm.Read(bytes, 0, bytes.Length);

                Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRead));

                client.Close();

                Console.WriteLine("Press any key to exit.");
                System.Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }
    }
}
