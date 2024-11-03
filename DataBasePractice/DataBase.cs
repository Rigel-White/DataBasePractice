using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DataBasePractice
{
    class DataBase
    {
        SqlConnection sqlConnection =new SqlConnection(@"Data Source = DESKTOP-M9E9S8K;Initial Catalog = demoPractice;Integrated Security = True");
        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed) { sqlConnection.Open(); }
        }
        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open) { sqlConnection.Close(); }
        }
        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
