using MySql.Data.MySqlClient;

namespace backend
{
    public class Connection
    {
        static string connstr = "data source=router.huhaorui.com;database=Gomoku;user id=Gomoku;password=xGGt3ePGZTyBCXbG;pooling=false;charset=utf8";//pooling代表是否使用连接池
        public static MySqlConnection GetConn()
        {
            var conn = new MySqlConnection(connstr);
            return conn;
        }
        
    }
}