using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using ConsoleApp1.Models;

namespace ConsoleApp1.Module
{
    public abstract class TcpServer
    {
        public abstract List<SocketData> responseMessage(Socket socket, TCPmessage receivemessage);
        private Socket serversocket = null;
        private byte[] receivedata;
        private List<Socket> clist = new List<Socket>();
        private int servertport=3300;
        public TcpServer()
        {
            receivedata = new byte[33200];
            //this.serverport = serverport;
            Connect();
        }

        private void Connect()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any,servertport);
                serversocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serversocket.Bind(ep);
                serversocket.Listen(5);
                serversocket.BeginAccept(accept_callback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void accept_callback(IAsyncResult ar)
        {
            Socket client = serversocket.EndAccept(ar);
            clist.Add(client);
            client.BeginReceive(receivedata, 0, receivedata.Length,SocketFlags.None, receive_callback, client);
            serversocket.BeginAccept(accept_callback, null);

        }
        private void receive_callback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndReceive(ar);
            TCPmessage tcpmessage = new TCPmessage(receivedata);
            List<SocketData> sendclient = responseMessage(client,tcpmessage);
            try
            {
                foreach (SocketData s in sendclient)
                {
                    Socket socket = s.socket;
                    byte[] sendmessage = s.message;
                    socket.BeginSend(sendmessage,0,sendmessage.Length,SocketFlags.None,send_callback,socket);
                }
                if (client.Connected)
                {
                    client.BeginReceive(receivedata, 0, receivedata.Length, SocketFlags.None, receive_callback, client);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


        }
        private void send_callback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
