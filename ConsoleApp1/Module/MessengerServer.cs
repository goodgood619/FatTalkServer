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
        private const int serverport = 3300;
        private DBhelp dBhelp;
        private JsonHelp jsonHelp;
        public List<Clientdata> clientlist { get; set; }
        public List<Chattingdata> chattinglist { get; set; }
        public MessengerServer()
        {
            this.dBhelp = new DBhelp();
            this.clientlist = new List<Clientdata>();
            this.chattinglist = new List<Chattingdata>();
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
                    Dictionary<string, string> logininfo = jsonHelp.getidinfo(receivemessage.message);
                    Dictionary<string, string> logininfo2 = jsonHelp.getpasswordinfo(receivemessage.message);
                    string id = logininfo[JsonName.ID];
                    string password = logininfo2[JsonName.Password];

                    bool idcheck = dBhelp.IsexistID(id);
                    bool passcheck = dBhelp.IsExistPassword(password);
                    bool validlogin = dBhelp.validLogin(id, password);


                    if (!idcheck && !validlogin) sendmessage.check = 0;
                    if (!passcheck && !validlogin) sendmessage.check = 1;
                    if (validlogin)
                    {
                        bool duplicate = false;
                        foreach (Clientdata c in clientlist) //중복로그인 방지
                        {
                            if (c.id == id)
                            {
                                duplicate = true;
                                break;
                            }
                        }
                        if (duplicate) //중복로그인
                        {
                            sendmessage.check = 4;
                        }
                        else
                        {
                            clientlist.Add(new Clientdata(socket, id)); //서버에 login을 했으니 정보를 추가해줘야함
                            int usernumber = dBhelp.Getusernumber(id);
                            string nickname = dBhelp.Getnickname(usernumber);
                            sendmessage.Usernumber = usernumber;
                            sendmessage.message = jsonHelp.nickinfo(nickname);
                            sendmessage.check = 2;
                        }
                    }
                    if (!idcheck && !passcheck) sendmessage.check = 3;

                    sendmessage.command = Command.login;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Join:
                    Dictionary<string, string> Joinid = jsonHelp.getidinfo(receivemessage.message);
                    Dictionary<string, string> Joinpassword = jsonHelp.getpasswordinfo(receivemessage.message);
                    Dictionary<string, string> Joinnickname = jsonHelp.getnickinfo(receivemessage.message);
                    Dictionary<string, string> Joinphone = jsonHelp.getphoneinfo(receivemessage.message);
                    string joinid = Joinid[JsonName.ID];
                    string joinpassword = Joinpassword[JsonName.Password];
                    string joinnickname = Joinnickname[JsonName.Nickname];
                    string joinphone = Joinphone[JsonName.Phone];
                    int joinusernumber = dBhelp.Getjoinusernumber();
                    if (!dBhelp.IsexistID(joinid))
                    {
                        dBhelp.join(joinid, joinpassword, joinnickname, joinphone, joinusernumber);
                        sendmessage.check = 1; //회원가입되었다는것을 의미
                    }
                    else sendmessage.check = 0;
                    sendmessage.command = Command.Join;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Idcheck:
                    Dictionary<string, string> idinfo = jsonHelp.getidinfo(receivemessage.message);
                    string checkid = idinfo[JsonName.ID];
                    if (!dBhelp.IsexistID(checkid))
                    {
                        sendmessage.check = 1;
                    }
                    else sendmessage.check = 0;
                    sendmessage.command = Command.Idcheck;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Nicknamecheck:
                    Dictionary<string, string> nickinfo = jsonHelp.getnickinfo(receivemessage.message);
                    string checknickname = nickinfo[JsonName.Nickname];
                    if (!dBhelp.Isexistnickname(checknickname))
                    {
                        sendmessage.check = 1;
                    }
                    else sendmessage.check = 0;
                    sendmessage.command = Command.Nicknamecheck;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.logout:
                    Dictionary<string, string> logoutinfo = jsonHelp.getnickinfo(receivemessage.message);
                    string logoutid = dBhelp.Getid(logoutinfo[JsonName.Nickname]);
                    Clientdata LogoutData = clientlist.Find(x => (x.id == logoutid));
                    clientlist.Remove(LogoutData);
                    sendmessage.command = Command.logout;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Findid:
                    Dictionary<string, string> findidinfo = jsonHelp.getidinfo(receivemessage.message);
                    string findid = findidinfo[JsonName.ID];
                    Dictionary<string, string> findphoneinfo = jsonHelp.getphoneinfo(receivemessage.message);
                    string findphone = findphoneinfo[JsonName.Phone];
                    if (dBhelp.IsexistID(findid) && dBhelp.Getphone(findid) == findphone)
                    {
                        string findpassword = dBhelp.Findpass(findid);
                        sendmessage.message = jsonHelp.logininfo(findid, findpassword);
                        sendmessage.check = 1;
                    }
                    else sendmessage.check = 0;
                    sendmessage.command = Command.Findid;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Plusfriend:
                    Dictionary<string, string> plusidinfo1 = jsonHelp.getidinfo(receivemessage.message);
                    string userid = plusidinfo1[JsonName.ID];
                    Dictionary<string, string> plusidinfo2 = jsonHelp.getFnickinfo(receivemessage.message);
                    string plusid = plusidinfo2[JsonName.FID];
                    if (dBhelp.IsexistID(plusid)) // 아이디의 존재유무만 체크했지 아직 친구추가의 중복부분은 처리안함, 그 부분을 서버에서 가지고있어야함 친구목록을
                    {
                        if (plusid == userid) //추가하려는 아이디가 동일한경우
                        {
                            sendmessage.check = 2;
                        }
                        else if (!dBhelp.Plusid(plusid, userid)) //이미 친구를 추가한 아이디인 경우
                        {
                            sendmessage.check = 3;
                        }
                        else
                        {
                            //친구를 추가하려는데, 그 친구가 차단을 한경우
                            bool blockplusfriend = dBhelp.Blockplusfriend(userid, plusid);
                            if (!blockplusfriend)
                            {
                                string usernickname = dBhelp.Getnickname(userid);
                                string plusnickname = dBhelp.Getnickname(plusid);
                                dBhelp.plusfriend(userid, usernickname, plusid, plusnickname); //DB에 친구추가
                                sendmessage.message = jsonHelp.nickinfo(plusnickname);
                                sendmessage.check = 1;
                            }
                            else
                            {
                                sendmessage.check = 4; // 추가하려는 친구가 차단함
                   
                            }
                        }
                    }
                    else //아이디가 존재하지 않는 경우
                    {
                        sendmessage.check = 0;
                    }
                    sendmessage.command = Command.Plusfriend;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Refresh:
                    Dictionary<string, string> refreshinfo = jsonHelp.getnickinfo(receivemessage.message);
                    string refreshnickname = refreshinfo[JsonName.Nickname];
                    int refreshcnt = dBhelp.Refreshfriendcount(refreshnickname); //nickname의 친구명수
                    string[] fnickarray = dBhelp.Refreshnickarray(refreshnickname);
                    sendmessage.command = Command.Refresh;
                    sendmessage.message = jsonHelp.Refreshnickarrayinfo(fnickarray);
                    sendmessage.Friendcount = refreshcnt;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Removefriend:
                    Dictionary<string,string> dusernickname = jsonHelp.getnickinfo(receivemessage.message);
                    string rusernickname = dusernickname[JsonName.Nickname];
                    string[] removenickarray = jsonHelp.getdeletenickarrayinfo(receivemessage.message);
                    for(int i = 0; i < removenickarray.Length; i++)
                    {
                        dBhelp.deletenickarray(rusernickname, removenickarray[i]);
                    }
                    sendmessage.command = Command.Removefriend;
                    sendmessage.check = 1;
                    sendclient.Add(new SocketData(socket, sendmessage));
                    break;
                case Command.Makechat:
                    Dictionary<string, string> makechatnickname = jsonHelp.getnickinfo(receivemessage.message);
                    string musernickname = makechatnickname[JsonName.Nickname];
                    List<string> makenickarray = jsonHelp.getmakechatnickarrayinfo(receivemessage.message);
                    // 0. 방개설 요청한 닉네임으로부터 현재 친구로 등록되어있는지를 확인
                    // 1. 요청한 닉네임, 요청당한 닉네임들이 모두 로그인되어있는지를 확인
                    // 2. chattinglist에서 같은 닉네임들로 형성된 방이 있는지를 체크
                    // 3. 채팅방 개설
                    // 4. 초대한 친구들중 차단한 사람이 존재 하는지?
                    string[] currentFriendlist = dBhelp.Refreshnickarray(musernickname);
                    bool notregistered = false;
                    for(int i = 0; i < makenickarray.Count; i++)
                    {
                        bool check= currentFriendlist.Contains(makenickarray[i]);
                        if (!check)
                        {
                            notregistered = true;
                            break;
                        }
                    }
                    if (notregistered) //0번의 경우로 채팅방 개설 실패
                    {
                        sendmessage.check = 0;
                        sendmessage.command = Command.Makechat;
                        sendclient.Add(new SocketData(socket, sendmessage));
                    }
                    else
                    {
                        //1번을 체크(요청당한 닉네임들을 체크)
                        bool alllogin = true;
                        for(int i = 0; i < makenickarray.Count; i++)
                        {
                            bool check = clientlist.Exists(x => x.id == makenickarray[i]);
                            if (!check)
                            {
                                alllogin = false;
                                break;
                            }
                        }
                        if (!alllogin) // 모두가 들어오지않음
                        {
                            sendmessage.check = 1;
                            sendmessage.command = Command.Makechat;
                            sendclient.Add(new SocketData(socket, sendmessage));
                        }
                        else
                        {
                            makenickarray.Add(musernickname);
                            bool roompossible = false;
                            int makenickcnt = makenickarray.Count;
                            for(int i = 0; i < chattinglist.Count; i++)
                            {
                                List<string> currentroommembers = chattinglist[i].chatnickarray;
                                int cnt = 0;
                                for(int j = 0; j < currentroommembers.Count; j++)
                                {
                                    bool same = makenickarray.Contains(currentroommembers[j]);
                                    if (same) cnt++;
                                }
                                if (cnt == makenickcnt && currentroommembers.Count == makenickarray.Count)
                                {
                                    roompossible = true;
                                    break;
                                }
                            }
                            if (roompossible) //동일한 채팅방존재
                            {
                                sendmessage.check = 2;
                                sendmessage.command = Command.Makechat;
                                sendclient.Add(new SocketData(socket, sendmessage));
                            }
                            else{ //초대한 사람중에 차단한 사람이 존재하는지 -> 이거 그냥 Blockfriendlist쓰면됨(수정다시)
                                bool nochatMake = false;
                                for (int i = 0; i < currentFriendlist.Length; i++) {
                                    bool blockMakechatcheck = dBhelp.Blockmakechat(musernickname, currentFriendlist[i]);
                                    if(blockMakechatcheck == true)
                                    {
                                        nochatMake = true;
                                        break;
                                    }
                                }
                                if (nochatMake)
                                {
                                    sendmessage.check = 4;
                                    sendmessage.command = Command.Makechat;
                                    sendclient.Add(new SocketData(socket, sendmessage));
                                }
                                else
                                {//채팅방개설(초대된 닉네임을 매칭시켜서 걔네한테 뿌리면됨
                                    int newchatnumber = chattinglist.Count + 1;
                                    //makenickarray.Add(musernickname);

                                    chattinglist.Add(new Chattingdata(newchatnumber, makenickarray));
                                    sendmessage.command = Command.Makechat;
                                    sendmessage.Chatnumber = newchatnumber;
                                    sendmessage.check = 3;
                                    List<Clientdata> tempsend = new List<Clientdata>();
                                    for (int i = 0; i < makenickarray.Count; i++)
                                    {
                                        string makeid = dBhelp.Getid(makenickarray[i]);
                                        Clientdata client = clientlist.Find(x => x.id == makeid);
                                        tempsend.Add(client);
                                    }
                                    for (int i = 0; i < tempsend.Count; i++)
                                    {
                                        sendclient.Add(new SocketData(tempsend[i].socket, sendmessage)); //수신자,(송신자도 포함) 송신
                                    }
                                }
                            }
                        }
                    }
                    break;
                case Command.Sendchat:
                    Dictionary<string, string> sendnick = jsonHelp.getnickinfo(receivemessage.message);
                    string sendchatnickname = sendnick[JsonName.Nickname];
                    Dictionary<string, string> Message = jsonHelp.getmessageinfo(receivemessage.message);
                    string sendMessage = Message[JsonName.Message];
                    int sendchatnumber = receivemessage.Chatnumber;
                    string sendmessageid = dBhelp.Getid(sendchatnickname); //보낸놈 아이디
                    sendmessage.command = Command.Sendchat;
                    sendmessage.Chatnumber = receivemessage.Chatnumber;
                    sendmessage.message = jsonHelp.Sendchatinfo(sendMessage,sendchatnickname);
                    // 채팅방찾기 -> 보낼 정보들 가져오기->clientlist에서 찾기(socket정보, 그리고 sendmessage에 같이 담아서 보냄)
                    for(int i = 0; i < chattinglist.Count; i++)
                    {
                        if(sendchatnumber == chattinglist[i].chatnumber)
                        {
                            List<string> sendchatnickarray = chattinglist[i].chatnickarray;
                            for (int j = 0; j < sendchatnickarray.Count; j++) {
                                string sendid = dBhelp.Getid(sendchatnickarray[j]);
                                Clientdata c = clientlist.Find(x => x.id == sendid);
                                if (c != null && sendid!=sendmessageid) sendclient.Add(new SocketData(c.socket,sendmessage));
                            }
                            break;
                        }
                    }
                    break;
                case Command.Outchat:
                    Dictionary<string, string> Outchatnickname = jsonHelp.getnickinfo(receivemessage.message);
                    string outchatnickname = Outchatnickname[JsonName.Nickname];
                    int chatnumber = receivemessage.Chatnumber;
                    string outchatid = dBhelp.Getid(outchatnickname);
                    int oidx = -1,idx=-1;
                    TCPmessage outsendmessage = new TCPmessage();
                    outsendmessage.check = 1; // 방을 나갈사람
                    outsendmessage.Chatnumber = chatnumber;
                    outsendmessage.command = Command.Outchat;
                    Clientdata clientdata = clientlist.Find(x => x.id == outchatid);
                    sendclient.Add(new SocketData(clientdata.socket,outsendmessage));
                    for (int i = 0; i < chattinglist.Count; i++)
                    {
                        if(chatnumber == chattinglist[i].chatnumber) //방번호찾기
                        {
                            oidx = i;
                            List<string> outchattinglist = chattinglist[i].chatnickarray;
                            for(int j = 0; j < outchattinglist.Count; j++)
                            {
                                if (outchatnickname == outchattinglist[j])
                                {
                                    idx = j; //채팅방을 나가는 인덱스
                                    break;
                                }
                            }
                            chattinglist[oidx].chatnickarray.RemoveAt(idx);
                            break;
                        }
                    }

                    sendmessage.command = Command.Outchat;
                    sendmessage.Chatnumber = chatnumber;
                    sendmessage.check = 0; // 방에 남아있는 사람들
                    sendmessage.message = jsonHelp.nickinfo(outchatnickname);
                    for (int i = 0; i < chattinglist[oidx].chatnickarray.Count; i++)
                    {
                        string outsendid = dBhelp.Getid(chattinglist[oidx].chatnickarray[i]);
                        Clientdata outc = clientlist.Find(x => x.id == outsendid);
                        if (outc != null) sendclient.Add(new SocketData(outc.socket, sendmessage));
                    }

                    break;
                case Command.Joinchat:
                    Dictionary<string, string> Joinchatid = jsonHelp.getidinfo(receivemessage.message);
                    string joinedchatid = Joinchatid[JsonName.ID]; //초대를 당한놈
                    Dictionary<string, string> Joinchatnickname = jsonHelp.getnickinfo(receivemessage.message);
                    string joinchatnickname = Joinchatnickname[JsonName.Nickname];
                    string joinedchatnickname = dBhelp.Getnickname(joinedchatid);
                    int joinchatnumber = receivemessage.Chatnumber;
                    int jidx = -1;
                    for(int i = 0; i < chattinglist.Count; i++)
                    {
                        if(joinchatnumber == chattinglist[i].chatnumber)
                        {
                            jidx = i;
                            break;
                        }
                    }
                    bool no = false;
                    Clientdata joinedc = clientlist.Find(x => x.id == joinedchatid);
                    if (joinedc == null) no = true;
                    if (!no) //로그인이 되어있거나, 친구차단을안했거나(친구차단은 아직 구현안함)
                    {
                        TCPmessage joinsendmessage = new TCPmessage(); //초대받는 친구에게는 따로 command를 보내야함
                        joinsendmessage.Chatnumber = joinchatnumber;
                        joinsendmessage.command = Command.ReceiveJoinchat;
                        joinsendmessage.check = 1;
                        sendclient.Add(new SocketData(joinedc.socket,joinsendmessage));
                        // 초대를 한 당사자는 초대ok된 메시지도 오는거고, 초대가 완료되었다는 메시지도 감
                        sendmessage.command = Command.Joinchat;
                        sendmessage.Chatnumber = joinchatnumber;
                        sendmessage.check = 1; //만약 친구거부를 안했다면, 오케이(아직 자세하게는 예외처리 안함)
                        for (int i = 0; i < chattinglist[jidx].chatnickarray.Count; i++) //기존에 있는방의 멤버들
                        {
                            string joinsendid = chattinglist[jidx].chatnickarray[i];
                            Clientdata joinc = clientlist.Find(x => x.id == joinsendid);
                            if (joinc != null) sendclient.Add(new SocketData(joinc.socket, sendmessage));
                        }
                        chattinglist[jidx].chatnickarray.Add(joinedchatnickname); //초대당한놈
                    }
                    break;

                case Command.Blockfriend: //그냥 DB(test.Blockfriendlist쓰자), 왜냐하면 친구가 구지 아니어도 친구차단을 할수도있으니

                    break;
            }

            return sendclient;
        }
    }
}
