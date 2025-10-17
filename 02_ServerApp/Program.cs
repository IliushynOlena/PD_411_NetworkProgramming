using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _02_ServerApp
{
    class Server
    {
        List<IPEndPoint> members;
        IPEndPoint clientEndPoint = null;
        int port = 4040;
        const string JOIN_CMD = "$<join>";
        UdpClient server_udp;

        public Server()
        {
            members = new List<IPEndPoint>();
            server_udp = new UdpClient(port);
        }
        private void AddMembers(IPEndPoint member)
        {
            members.Add(member);
            Console.WriteLine("Member was added ");
        }

        public void Start()
        {
            while (true)
            {
                byte[] data = server_udp.Receive(ref clientEndPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"Message : {message}. From : {clientEndPoint}");
                switch (message)
                {
                    case JOIN_CMD:
                        AddMembers(clientEndPoint);
                        break;

                    default:
                        SendAll(data);
                        break;
                }

            }
        }
        private void SendAll(byte[] data)
        {
            foreach (IPEndPoint member in members)
            {
                server_udp.SendAsync(data, data.Length, member);
            }
        }


    }
    internal class Program
    {
        
        static void Main(string[] args)
        {
            //string ip
         
            Server server = new Server();
            server.Start();
           


        }
      
    }
}
