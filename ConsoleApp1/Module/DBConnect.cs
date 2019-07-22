using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;
namespace ConsoleApp1.Module
{
    public class DBConnect
    {
        private MySqlConnection dbconnect;
        public DBConnect()
        {
            try
            {
                string s = $"Server=localhost;Database=test;Uid=root;Pwd=1234;charset=utf8";
                this.dbconnect = new MySqlConnection(s);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void sendquery(string query)
        {
            try
            {
                dbconnect.Open();
                MySqlCommand command = new MySqlCommand(query, dbconnect);
                command.ExecuteNonQuery(); //query가 눈에 호출되지않게 실행
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                dbconnect.Close();
            }
        }
        public DataSet selectquery(string s)
        {
            DataSet dataSet = new DataSet();
            try
            {
                dbconnect.Open();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(s, dbconnect);
                dataAdapter.Fill(dataSet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
            dbconnect.Close();
            }
            return dataSet;
        }
        public int GetCountRow(string s)
        {
            DataSet dataSet = selectquery(s);
            return dataSet.Tables[0].Rows.Count;
        }
        public bool IsexistRow(string s)
        {
            int count = GetCountRow(s);
            if (count > 0) return true;
            else return false;
        }
    }
}
