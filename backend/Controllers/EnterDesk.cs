using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace backend.Controllers
{
    [Route("api/desk/enter")]
    [ApiController]
    public class EnterDesk : ControllerBase
    {
        private readonly ILogger<EnterDesk> _logger;

        public EnterDesk(ILogger<EnterDesk> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public string Post([FromForm] string uid, [FromForm] string password, [FromForm] int desk, [FromForm] int seat,
            [FromForm] string attribute)
        {
            var connection = Connection.GetConn();
            var sql = "select * from User where uid=@uid and password=@password";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.Add(new MySqlParameter("@uid", uid));
            command.Parameters.Add(new MySqlParameter("@password", password));
            connection.Open();
            var result = command.ExecuteReader();
            if (result.HasRows)
            {
                result.Close();
                sql = "select * from Desk where player1=@player1 or player2=@player2";
                command = new MySqlCommand(sql, connection);
                command.Parameters.Add(new MySqlParameter("@player1", uid));
                command.Parameters.Add(new MySqlParameter("@player2", uid));
                result = command.ExecuteReader();
                if (result.HasRows && attribute.Equals("sitdown")){
                    return "AS";//Already sit
                }
                result.Close();
                if (attribute.Equals("sitdown"))
                {
                    sql = seat == 1
                        ? "update Desk set player1=@uid where did=@desk"
                        : "update Desk set player2=@uid where did=@desk";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.Add(new MySqlParameter("@uid", uid));
                    command.Parameters.Add(new MySqlParameter("@desk", desk));
                }
                else
                {
                    sql = seat == 1
                        ? "update Desk set player1='' where player1=@uid"
                        : "update Desk set player2='' where player2=@uid";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.Add(new MySqlParameter("@uid", uid));
                }

                if (command.ExecuteNonQuery() == 1)
                {
                    
                    return "OK";
                }
                else
                {
                    return "SH"; //somebody here
                }
            }
            else
            {
                return "PE"; //password error
            }
        }
    }
}