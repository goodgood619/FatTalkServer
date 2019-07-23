using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace ConsoleApp1.Models
{
    public class Clientdata
    {
        public Socket socket { get; set; }
        public int number { get; set; }
        public Clientdata(Socket socket,int number)
        {
            this.socket = socket;
            this.number = number;
        }
    }
}
