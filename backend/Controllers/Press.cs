using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace backend.Controllers
{
    [Route("api/game/press")]
    [ApiController]
    public class Press : ControllerBase
    {
        private readonly ILogger<Press> _logger;

        public Press(ILogger<Press> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public string Post([FromForm] string uid, [FromForm] string password, [FromForm] int key)
        {    
            //TODO 邪恶用户尝试修改key，以及不在自己回合进行操作
            var connection = Connection.GetConn();
            var sql = "select status,next,gid from AllView where uid=@uid and password=@password";
            var command = new MySqlCommand(sql, connection);
            command.Parameters.Add(new MySqlParameter("@uid", uid));
            command.Parameters.Add(new MySqlParameter("@password", password));
            connection.Open();
            var result = command.ExecuteReader();
            if (result.Read())
            {
                var gid = result.GetInt32("gid");
                var status = result.GetString("status");
                var next = result.GetInt32("next");
                result.Close();
                status = status.Substring(0, key) + next.ToString()[0] + status.Substring(key + 1);
                sql = "update Game set status=@status,next=@next where gid=@gid";
                command = new MySqlCommand(sql, connection);
                command.Parameters.Add(new MySqlParameter("@status", status));
                command.Parameters.Add(new MySqlParameter("@next", 3 - next));
                command.Parameters.Add(new MySqlParameter("@gid", gid));
                command.ExecuteNonQuery();
                connection.Close();
                return "OK";
            }

            connection.Close();
            return "PE";
        }
    }
}