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
        public bool validLogin(string id, string password)
        {
            string query = $"select * from test.member where ID='{id}' and Password='{password}'";
            bool result = dbconnect.IsexistRow(query);
            return result;
        }
        public void join(string id, string password, string nickname, string phone) {

            string query = $"insert into test.member values ('{id}','{password}','{nickname}','{phone}')";
            dbconnect.sendquery(query);
        }
        public string Findpass(string id)
        {
            string query = $"select Password from test.member where ID='{id}'";
            DataSet ret = dbconnect.selectquery(query);
            string password = Convert.ToString(ret.Tables[0].Rows[0]["Password"]);
            return password;
        }

    }
}
