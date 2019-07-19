using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace ConsoleApp1.Models
{
    public class SocketData
    {
        public Socket socket { get; set; }
        public byte[] message { get; set; }
        public SocketData(Socket socket, TCPmessage message)
        {
            this.socket = socket;
            this.message = message.Tobytedata();
        }
    }
}
