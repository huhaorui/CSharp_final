using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace backend.Controllers
{
    [Route("api/user/login")]
    [ApiController]
    public class Login : ControllerBase
    {
        private readonly ILogger<Login> _logger;

        public Login(ILogger<Login> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public string Post([FromForm] string uid ,[FromForm] string password)
        {
            var connection = Connection.GetConn();
            const string sql = "select * from User where uid=@uid and password=@password";
            var command = new MySqlCommand(sql, connection);
            command.Parameters.Add(new MySqlParameter("@uid", uid));
            command.Parameters.Add(new MySqlParameter("@password", password));
            connection.Open();
            var result = command.ExecuteReader();
            if (result.HasRows)
            {
                connection.Close();
                return "OK";
            }
            connection.Close();
            return "NO";
        }
    }
}