using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace backend.Controllers
{
    [Route("api/game/ready")]
    [ApiController]
    public class Ready : ControllerBase
    {
        private readonly ILogger<Ready> _logger;

        public Ready(ILogger<Ready> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public String Post([FromForm] string uid, [FromForm] string password, [FromForm] string attribute)
        {
            if (!(attribute.Equals("ready") || attribute.Equals("unready")))
            {
                return "WA";
            }

            var cmd = attribute.Equals("ready") ? "1" : "0";
            var connection = Connection.GetConn();
            var Sql = "select did from AllView where uid=@uid and password=@password";
            MySqlCommand command = new MySqlCommand(Sql, connection);
            command.Parameters.Add(new MySqlParameter("@uid", uid));
            command.Parameters.Add(new MySqlParameter("@password", password));
            connection.Open();
            var result = command.ExecuteReader();
            if (result.Read())
            {
                var did = result.GetInt32("did").ToString();
                result.Close();
                Sql = "select player1,player2,ready from Desk where did=@did";
                command = new MySqlCommand(Sql, connection);
                command.Parameters.Add(new MySqlParameter("@did", did));
                result = command.ExecuteReader();
                if (result.Read())
                {
                    string player1 = "0", player2 = "0";

                    try
                    {
                        player1 = result.GetString("player1");
                    }
                    catch (System.Data.SqlTypes.SqlNullValueException e)
                    {
                    }

                    try
                    {
                        player2 = result.GetString("player2");
                    }
                    catch (System.Data.SqlTypes.SqlNullValueException e)
                    {
                    }

                    var ready = result.GetString("ready");

                    Sql = "update Desk set ready=@ready where did=@did";
                    command = new MySqlCommand(Sql, connection);
                    command.Parameters.Add(new MySqlParameter("@did", did));
                    if (uid.Equals(player1.ToString()))
                    {
                        command.Parameters.Add(new MySqlParameter("@ready", cmd + ready[1]));
                    }
                    else if (uid.Equals(player2.ToString()))
                    {
                        command.Parameters.Add(new MySqlParameter("@ready", ready[0] + cmd));
                    }

                    result.Close();
                    command.ExecuteNonQuery();
                    return "OK";
                }

                return "ERROR";
            }

            connection.Close();
            return "NO";
        }
    }
}