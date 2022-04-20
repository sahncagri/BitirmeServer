using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Tool
{
    class ConnHelper
    {
        private const string CONNECTIONSTRING = "datasource=127.0.0.1;port=3306;database=game01;user=root;pwd=147896325aa.;";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING);
            conn.Open();
            return conn;
        }
        public static void CloseConnection(MySqlConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
            }
            else
            {
                Console.WriteLine("MysqlConnection can't be closed");
            }
        }
    }
}
