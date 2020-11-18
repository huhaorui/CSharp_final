using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace backend.Controllers
{
    [Route("api/desk/list")]
    [ApiController]
    public class Desk : ControllerBase
    {
        private readonly ILogger<Desk> _logger;

        public Desk(ILogger<Desk> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public String Post()
        {
            var connection = Connection.GetConn();
            var Sql = "select * from Desk";
            MySqlCommand command = new MySqlCommand(Sql, connection);
            connection.Open();
            var result = command.ExecuteReader();
            var response = "";
            while (result.Read())
            {
                int did = 0, gid = 0;
                string player1 = "0", player2 = "0";
                if (!result.IsDBNull(0))
                    did = result.GetInt32("did");
                if (!result.IsDBNull(1))
                    player1 = result.GetString("player1");
                if (!result.IsDBNull(2))
                    player2 = result.GetString("player2");
                if (!result.IsDBNull(3))
                    gid = result.GetInt32("gid");
                if (player1 == "")
                    player1 = "0";
                if (player2 == "")
                    player2 = "0";
                response += $"{did} {player1} {player2} {gid}\n";
            }

            connection.Close();
            Console.WriteLine(response);
            return response;
        }
    }
}