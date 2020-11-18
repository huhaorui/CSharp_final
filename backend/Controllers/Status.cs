using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace backend.Controllers
{
    [Route("api/game/status")]
    [ApiController]
    public class Status : ControllerBase
    {
        private readonly ILogger<Status> _logger;

        public Status(ILogger<Status> logger)
        {
            _logger = logger;
        }

        private bool WinForSomeOne(string status, char number)
        {
            for (var i = 0; i < 3; i++)
            {
                if (status[i] == number && status[i + 3] == number && status[i + 6] == number)
                {
                    return true;
                }
            }

            for (var i = 0; i < 3; i++)
            {
                if (status[3 * i] == number && status[3 * i + 1] == number && status[3 * i + 2] == number)
                {
                    return true;
                }
            }

            if (status[0] == number && status[4] == number && status[8] == number)
            {
                return true;
            }

            if (status[2] == number && status[4] == number && status[6] == number)
            {
                return true;
            }

            return false;
        }

        private int Win(string status)
        {
            if (WinForSomeOne(status, '1'))
            {
                return 1;
            }
            else if (WinForSomeOne(status, '2'))
            {
                return 2;
            }
            else if (status.IndexOf("0", StringComparison.Ordinal) == -1)
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }



        [HttpPost]
        public string Post([FromForm] string uid, [FromForm] string password, [FromForm] int seat)
        {
            var connection = Connection.GetConn();
            var sql = "select status,did,next,ready,gid from AllView where uid=@uid and password=@password";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.Add(new MySqlParameter("@uid", uid));
            command.Parameters.Add(new MySqlParameter("@password", password));
            connection.Open();
            var result = command.ExecuteReader();
            var response = "";
            if (result.Read())
            {
                var did = result.GetString("did");
                var gid = result.GetString("gid");
                var s = result.GetString("status");
                response += result.GetInt32("did"); //0
                response += ":";
                response += result.GetString("status"); //1
                response += ":";
                response += result.GetInt32("next"); //2
                response += ":";
                response += result.GetString("ready"); //3
                result.Close();
                sql = "select player1,player2 from Desk where did=@did";
                command = new MySqlCommand(sql, connection);
                command.Parameters.Add(new MySqlParameter("@did", did));
                result = command.ExecuteReader();
                result.Read();
                string player = result.GetString("player1"),
                    player2 = result.GetString("player2");
                result.Close();
                int score1 = 0, score2 = 0;
                if (!player.Equals(""))
                {
                    sql = "select score from User where uid=@uid";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.Add(new MySqlParameter("@uid", player));
                    result = command.ExecuteReader();
                    result.Read();
                    score1 = result.GetInt32("score");
                    result.Close();
                }

                if (!player2.Equals(""))
                {
                    sql = "select score from User where uid=@uid";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.Add(new MySqlParameter("@uid", player2));
                    result = command.ExecuteReader();
                    result.Read();
                    score2 = result.GetInt32("score");
                    result.Close();
                }

                response += ":" + player + ":" + player2 + ":" + score1 + ":" + score2;
                if (Win(s) != 0)
                {
                    result.Close();
                    sql = "update Desk set ready='00' where did=@did";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.Add(new MySqlParameter("@did", did));
                    command.ExecuteNonQuery();
                    Thread.Sleep(3000);
                    sql = "update Game set status='000000000',next=@next where gid=@gid";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.Add(new MySqlParameter("@next", (new Random().Next(1, 1000) % 2 + 1).ToString()));
                    command.Parameters.Add(new MySqlParameter("@gid", gid));
                    command.ExecuteNonQuery();
                    sql = "update User set score=score+@score where uid=@uid";
                    command = new MySqlCommand(sql, connection);
                    if (Win(s) == 3)
                    {
                        command.Parameters.Add(new MySqlParameter("@score", 1));
                    }
                    else if (Win(s) == seat)
                    {
                        command.Parameters.Add(new MySqlParameter("@score", 3));
                    }
                    else
                    {
                        command.Parameters.Add(new MySqlParameter("@score", -3));
                    }

                    command.Parameters.Add(new MySqlParameter("@uid", uid));
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                response += "NG"; //No Game
            }

            result.Close();
            return response;
        }
    }
}