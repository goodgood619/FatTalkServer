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

    }
}
