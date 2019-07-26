using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1.Module
{
    public class MessengerServer : TcpServer
    {
        private const int serverport = 33212;
        private DBhelp dBhelp;
        private JsonHelp jsonHelp;
        public List<Clientdata> clientlist{get;set;}

        public MessengerServer()
        {
            //this.jsonHelp = new JsonHelp();
            this.dBhelp = new DBhelp();
            this.clientlist = new List<Clientdata>();
            //this.jsonHelp = new JsonHelp();
            Console.WriteLine($"Messenger Server Start : (Port: {serverport})");
        }
        public override List<SocketData> responseMessage(Socket socket, TCPmessage receivemessage)
        {
            List<SocketData> sendclient = new List<SocketData>();
            TCPmessage sendmessage = new TCPmessage();
            this.jsonHelp = new JsonHelp();
            switch (receivemessage.command)
            {
                case Command.login:
                    Dictionary<string, string> logininfo = jsonHelp.getlogininfo(receivemessage.message);
                    string id = logininfo[JsonName.ID];
                    string password = logininfo[JsonName.Password];
                    
                    bool idcheck = dBhelp.IsexistID(id);
                    bool passcheck = dBhelp.IsExistPassword(password);
                    bool validlogin = dBhelp.validLogin(id, password);

                    if (!idcheck && !validlogin) sendmessage.check = 0;
                    if (!passcheck && !validlogin) sendmessage.check = 1;
                    if (validlogin)
                    {
                        clientlist.Add(new Clientdata(socket,id)); //서버에 login을 했으니 정보를 추가해줘야함
                        sendmessage.check = 2;
                    }
                    if(!idcheck && !passcheck) sendmessage.check = 3;
                    
                    sendmessage.command = Command.login;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Join:
                    Dictionary<string,string> joininfo1=jsonHelp.getlogininfo(receivemessage.message);
                    Dictionary<string,string> joininfo2 = jsonHelp.getphonenick(receivemessage.message);
                    string joinid = joininfo1[JsonName.ID];
                    string joinpassword = joininfo1[JsonName.Password];
                    string joinnickname = joininfo2[JsonName.Nickname];
                    string joinphone = joininfo2[JsonName.Phone];
                    if(!dBhelp.IsexistID(joinid)) {
                        dBhelp.join(joinid,joinpassword,joinnickname,joinphone);
                        sendmessage.check= 1; //회원가입되었다는것을 의미
                    }
                    else sendmessage.check = 0;
                    sendmessage.command = Command.Join;
                    sendclient.Add(new SocketData(socket,sendmessage));
                    break;
                case Command.Idcheck:
                    Dictionary<string,string> idinfo = jsonHelp.getidinfo(receivemessage.message);
                    string checkid = idinfo[JsonName.ID];
                    if(!dBhelp.IsexistID(checkid)){
                        sendmessage.check = 1;
                    }
                    else sendmessage.check = 0;
                    sendmessage.command = Command.Idcheck;
                    sendclient.Add(new SocketData(socket,sendmessage));
                    break;
                case Command.logout:
                    //Clientdata LogoutData = clientlist.Find(x=> (x.id == receivemessage))

                    sendmessage.command = Command.logout;
                    sendclient.Add(new SocketData(socket,sendmessage));
                    break;
                case Command.Findid:
                    Dictionary<string, string> findidinfo = jsonHelp.getidinfo(receivemessage.message);
                    string findid = findidinfo[JsonName.ID];
                    if (dBhelp.IsexistID(findid))
                    {
                        string findpassword = dBhelp.Findpass(findid);
                        sendmessage.message = jsonHelp.logininfo(findid, findpassword);
                        sendmessage.check = 1;
                    }
                    else sendmessage.check = 0;
                    sendmessage.command = Command.Findid;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
            }

            return sendclient;
        }
    }
}
