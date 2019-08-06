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
        public DBhelp()
        {

        }
        public bool IsexistID(string id)
        {
            string query = $"select * from test.member where ID='{id}'";
            bool result = dbconnect.IsexistRow(query);
            return result;
        }
        public bool IsExistPassword(string password)
        {
            string query = $"select * from test.member where Password='{password}'";
            bool result = dbconnect.IsexistRow(query);
            return result;
        }
        public bool Isexistnickname(string nickname)
        {
            string query = $"select * from test.member where Nickname='{nickname}'";
            bool result = dbconnect.IsexistRow(query);
            return result;
        }
        public bool validLogin(string id, string password)
        {
            string query = $"select * from test.member where ID='{id}' and Password='{password}'";
            bool result = dbconnect.IsexistRow(query);
            return result;
        }
        public void join(string id, string password, string nickname, string phone, int usernumber)
        {

            string query = $"insert into test.member values ('{id}','{password}','{nickname}','{phone}','{usernumber}')";
            dbconnect.sendquery(query);
        }
        public void plusfriend(string id, string usernickname, string friendid, string friendnickname)
        {
            string query = $"insert into test.friendlist values ('{id}','{usernickname}','{friendid}','{friendnickname}')";
            dbconnect.sendquery(query);
        }
        public string Findpass(string id)
        {
            string query = $"select Password from test.member where ID='{id}'";
            DataSet ret = dbconnect.selectquery(query);
            string password = Convert.ToString(ret.Tables[0].Rows[0]["Password"]);
            return password;

        }
        public int Getusernumber(string id)
        {
            string query = $"select Usernumber from test.member where ID='{id}'";
            DataSet ret = dbconnect.selectquery(query);
            int usernumber = Convert.ToInt32(ret.Tables[0].Rows[0]["Usernumber"]);
            return usernumber;
        }
        public string Getid(string usernickname)
        {
            string query = $"select Id from test.member where Nickname='{usernickname}'";
            DataSet ret = dbconnect.selectquery(query);
            string id = Convert.ToString(ret.Tables[0].Rows[0]["Id"]);
            return id;
        }
        public string Getnickname(int usernumber)
        {
            string query = $"select Nickname from test.member where Usernumber='{usernumber}'";
            DataSet ret = dbconnect.selectquery(query);
            string nickname = Convert.ToString(ret.Tables[0].Rows[0]["Nickname"]);
            return nickname;
        }
        public string Getnickname(string id)
        {
            string query = $"select Nickname from test.member where ID='{id}'";
            DataSet ret = dbconnect.selectquery(query);
            string nickname = Convert.ToString(ret.Tables[0].Rows[0]["Nickname"]);
            return nickname;

        }
        public string Getphone(string id)
        {
            string query = $"select Phone from test.member where ID='{id}'";
            DataSet ret = dbconnect.selectquery(query);
            string phone = Convert.ToString(ret.Tables[0].Rows[0]["Phone"]);
            return phone;
        }
        public int Getjoinusernumber()
        {
            string query = $"select * from test.member order by Usernumber";
            DataSet ret = dbconnect.selectquery(query);
            int usernumber = 1;
            if (ret == null) return usernumber;
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
            string query = $"select FriendId from test.friendlist where Id='{userid}'";
            DataSet ret = dbconnect.selectquery(query);
            bool no = false;
            if (ret == null) return no;
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

        public int Refreshfriendcount(string usernickname)
        {
            string query = $"select * from test.friendlist where Nickname='{usernickname}'";
            DataSet ret = dbconnect.selectquery(query);
            int cnt = ret.Tables[0].Rows.Count;
            return cnt;
        }
        public string[] Refreshnickarray(string usernickname)
        {
            string query = $"select Friendnickname from test.friendlist where Nickname='{usernickname}'";
            DataSet ret = dbconnect.selectquery(query);
            string[] s = new string[ret.Tables[0].Rows.Count];
            int idx = 0;
            foreach(DataRow data in ret.Tables[0].Rows)
            {
                string temp = Convert.ToString(data["Friendnickname"]);
                s[idx] = temp;
                idx++;
            }
            return s;
        }
        public void deletenickarray(string usernickname,string deletenickname)
        {
            string query = $"delete from test.friendlist where Friendnickname='{deletenickname}' and Nickname='{usernickname}'";
            dbconnect.sendquery(query);
        }
    }
}
