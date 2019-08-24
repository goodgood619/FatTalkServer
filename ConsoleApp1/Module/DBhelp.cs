using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;
namespace ConsoleApp1.Module
{
    public class DBhelp
    {
        DBConnect dbconnect = new DBConnect();
        private readonly object dbquerylock;
        public DBhelp()
        {
            dbquerylock = new object();
        }
        public bool IsexistID(string id)
        {
            bool result = false;
            lock (dbquerylock)
            {
                string query = $"select * from test.member where ID='{id}'";
                result = dbconnect.IsexistRow(query);
            }
            return result;
        }
        public bool IsExistPassword(string password)
        {
            bool result = false;
            lock (dbquerylock)
            {
                string query = $"select * from test.member where Password='{password}'";
                result = dbconnect.IsexistRow(query);
            }
            return result;
        }
        public bool Isexistnickname(string nickname)
        {
            bool result = false;
            lock (dbquerylock)
            {
                string query = $"select * from test.member where Nickname='{nickname}'";
                result = dbconnect.IsexistRow(query);
            }
            return result;
        }
        public bool Isexistblock(string blockingnickname,string blockednickname)
        {
            bool result = false;
            lock (dbquerylock)
            {
                string query = $"select * from test.Blockfriendlist where Nickname='{blockingnickname}' and BlockedNickname='{blockednickname}'";
                result = dbconnect.IsexistRow(query);
            }
            return result;
        }
        public bool Isexistblockfriend(string blockingnickname,string blockednickname)
        {
            bool result = false;
            lock (dbquerylock)
            {
                string query = $"select * from test.friendlist where Nickname='{blockingnickname}' and Friendnickname='{blockednickname}' and Block=1";
                result = dbconnect.IsexistRow(query);
            }
            return result;
        }
        public bool Isexistfriend(string usernickname,string friendnickname)
        {
            bool result = false;
            lock (dbquerylock)
            {
                string query = $"select * from test.friendlist where Nickname='{usernickname}' and Friendnickname = '{friendnickname}'";
                result = dbconnect.IsexistRow(query);
            }
            return result;
        }
        public bool validLogin(string id, string password)
        {
            bool result = false;
            lock (dbquerylock)
            {
                string query = $"select * from test.member where ID='{id}' and Password='{password}'";
                result = dbconnect.IsexistRow(query);
            }
            return result;
        }
        public void join(string id, string password, string nickname, string phone, int usernumber)
        {
            lock (dbquerylock)
            {
                string query = $"insert into test.member values ('{id}','{password}','{nickname}','{phone}','{usernumber}')";
                dbconnect.sendquery(query);
            }
        }
        public void plusfriend(string id, string usernickname, string friendid, string friendnickname,int block)
        {
            lock (dbquerylock)
            {
                string query = $"insert into test.friendlist values ('{id}','{usernickname}','{friendid}','{friendnickname}','{block}')";
                dbconnect.sendquery(query);
            }
        }
        public void Blockfriend(string blockingnickname,string blockednickname)
        {
            lock (dbquerylock)
            {
                string query = $"insert into test.Blockfriendlist values ('{blockingnickname}','{blockednickname}')";
                dbconnect.sendquery(query);
            }
        }
        public void Updatenotblock(string blockingnickname,string blockednickname)
        {
            lock (dbquerylock)
            {
                string query = $"update test.friendlist set Block = 0 where Nickname='{blockingnickname}' and Friendnickname='{blockednickname}'";
                dbconnect.sendquery(query);
            }
        }
        public void Updateblock(string blockingnickname,string blockednickname)
        {
            lock (dbquerylock)
            {
                string query = $"update test.friendlist set Block = 1 where Nickname='{blockingnickname}' and Friendnickname = '{blockednickname}'";
                dbconnect.sendquery(query);
            }
        }
        public string Findpass(string id)
        {
            string password = string.Empty;
            lock (dbquerylock)
            {
                string query = $"select Password from test.member where ID='{id}'";
                DataSet ret = dbconnect.selectquery(query);
                password = Convert.ToString(ret.Tables[0].Rows[0]["Password"]);
            }
            return password;

        }
        public int Getusernumber(string id)
        {
            int usernumber = 0;
            lock (dbquerylock)
            {
                string query = $"select Usernumber from test.member where ID='{id}'";
                DataSet ret = dbconnect.selectquery(query);
                usernumber = Convert.ToInt32(ret.Tables[0].Rows[0]["Usernumber"]);
            }
            return usernumber;
        }
        public string Getid(string usernickname)
        {
            string id = string.Empty;
            lock (dbquerylock)
            {
                string query = $"select Id from test.member where Nickname='{usernickname}'";
                DataSet ret = dbconnect.selectquery(query);
                id = Convert.ToString(ret.Tables[0].Rows[0]["Id"]);
            }
            return id;
        }
        public string Getnickname(int usernumber)
        {
            string nickname = string.Empty;
            lock (dbquerylock)
            {
                string query = $"select Nickname from test.member where Usernumber='{usernumber}'";
                DataSet ret = dbconnect.selectquery(query);
                nickname = Convert.ToString(ret.Tables[0].Rows[0]["Nickname"]);
            }
            return nickname;
        }
        public string Getnickname(string id)
        {
            string nickname = string.Empty;
            lock (dbquerylock)
            {
                string query = $"select Nickname from test.member where ID='{id}'";
                DataSet ret = dbconnect.selectquery(query);
                nickname = Convert.ToString(ret.Tables[0].Rows[0]["Nickname"]);
            }
            return nickname;

        }
        public string Getphone(string id)
        {
            string phone = string.Empty;
            lock (dbquerylock)
            {
                string query = $"select Phone from test.member where ID='{id}'";
                DataSet ret = dbconnect.selectquery(query);
                phone = Convert.ToString(ret.Tables[0].Rows[0]["Phone"]);
            }
            return phone;
        }
        public int Getjoinusernumber()
        {
            string query = $"select * from test.member order by Usernumber";
            DataSet ret = dbconnect.selectquery(query);
            int usernumber = 1;
            if (ret.Tables.Count == 0) return usernumber;
            foreach (DataRow data in ret.Tables[0].Rows)
            {
                int num = Convert.ToInt32(data["Usernumber"]);
                if (usernumber != num)
                {
                    usernumber = num;
                    break;
                }
                else usernumber++;
            }
            return usernumber;
        }
        public bool Plusid(string plusid, string userid)
        {
            DataSet ret = null;
            lock (dbquerylock)
            {
                string query = $"select FriendId from test.friendlist where Id='{userid}'";
                ret = dbconnect.selectquery(query);
            }
            bool no = false;
            if (ret.Tables.Count == 0) return true;
            foreach (DataRow data in ret.Tables[0].Rows)
            {
                string checkplusid = Convert.ToString(data["Friendid"]);
                if (checkplusid == plusid)
                {
                    no = true;
                    break;
                }
            }
            if (no) return false;
            else return true;
        }
        public bool Blockplusfriend(string userid,string plusid) // 수정다시
        {
            DataSet ret = null;
            lock (dbquerylock)
            {
                string query = $"select Block from test.friendlist where Id='{plusid}' and Friendid='{userid}'";
                ret = dbconnect.selectquery(query);
            }
            bool no = false;
            if (ret.Tables.Count == 0) return no;
            foreach(DataRow data in ret.Tables[0].Rows)
            {
                int block = Convert.ToInt32(data["Block"]);
                if (block == 1)
                {
                    no = true;
                    break;
                }
            }
            if (no) return true;
            else return false;
        }
        public bool Blockmakechat(string usernickname, string joinchatnickname) //수정다시
        {
            DataSet ret = null;
            lock (dbquerylock)
            {
                string query = $"select Block from test.friendlist where Id='{joinchatnickname}' and Friendid='{usernickname}'";
                ret = dbconnect.selectquery(query);
            }
            bool no = false;
            if (ret.Tables.Count == 0) return no;
            foreach (DataRow data in ret.Tables[0].Rows)
            {
                int block = Convert.ToInt32(data["Block"]);
                if (block == 1)
                {
                    no = true;
                    break;
                }
            }
            if (no) return true;
            else return false;
        }
        public int Refreshfriendcount(string usernickname)
        {
            int cnt = 0;
            lock (dbquerylock)
            {
                string query = $"select * from test.friendlist where Nickname='{usernickname}' and Block= 0";
                DataSet ret = dbconnect.selectquery(query);
                cnt = ret.Tables[0].Rows.Count;
            }
            return cnt;
        }
        public string[] Refreshnickarray(string usernickname)
        {
            DataSet ret = null;
            lock (dbquerylock)
            {
                string query = $"select Friendnickname,Block from test.friendlist where Nickname='{usernickname}'";
                ret = dbconnect.selectquery(query);
            }
            string[] s = new string[ret.Tables[0].Rows.Count];
            int idx = 0;
            foreach(DataRow data in ret.Tables[0].Rows)
            {
                string temp = Convert.ToString(data["Friendnickname"]);
                int block = Convert.ToInt32(data["Block"]);
                if (block == 0)
                {
                    s[idx] = temp;
                    idx++;
                }
            }
            return s;
        }
        public void deletenickarray(string usernickname,string deletenickname)
        {
            lock (dbquerylock)
            {
                string query = $"delete from test.friendlist where Friendnickname='{deletenickname}' and Nickname='{usernickname}'";
                dbconnect.sendquery(query);
            }
        }
        public void deleteBlockfriend(string blockingnickname,string blockednickname)
        {
            lock (dbquerylock)
            {
                string query = $"delete from test.Blockfriendlist where Nickname='{blockingnickname}' and BlockedNickname='{blockednickname}'";
                dbconnect.sendquery(query);
            }
        }
    }
}
