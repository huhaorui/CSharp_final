using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace backend.Controllers
{
    [Route("api/user/register")]
    [ApiController]
    public class Register : ControllerBase
    {
        private readonly ILogger<Register> _logger;

        public Register(ILogger<Register> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public String Post([FromForm] string uid, [FromForm] string password)
        {
            var connection = Connection.GetConn();
            var Sql = "insert into User (uid,password) values (@uid,@password)";
            MySqlCommand command = new MySqlCommand(Sql, connection);
            command.Parameters.Add(new MySqlParameter("@uid", uid));
            command.Parameters.Add(new MySqlParameter("@password", password));
            connection.Open();
            if (command.ExecuteNonQuery()==1)
            {
                connection.Close();
                return "OK";
            }
            else
            {
                connection.Close();
                return "NOK";
            }
            
        }
    }
}