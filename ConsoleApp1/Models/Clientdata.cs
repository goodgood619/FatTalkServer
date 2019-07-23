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
        public Socket socket;
        public int number;
        public Clientdata(Socket socket,int number)
        {
            this.socket = socket;
            this.number = number;
        }
    }
}
