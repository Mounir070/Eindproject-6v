using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Eindproject_6v.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    private static string HashPassword(string password)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }

    private static int RegisterAccount(string user, string password)
    {
        const string query = "insert into user_info (user_name, hashed_pw) values (?user, ?password)";
        
        using (MySqlConnection conn = new MySqlConnection(HomeController.ConnectionString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.Add("?user", MySqlDbType.VarChar).Value = user;
            cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = HashPassword(password);
            return cmd.ExecuteNonQuery();
        }
    }
    
    private static bool Login(string user, string password)
    {
        const string query = "select hashed_pw from user_info where user_name = ?user";
        
        using (MySqlConnection conn = new MySqlConnection(HomeController.ConnectionString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.Add("?user", MySqlDbType.VarChar).Value = user;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return reader["hashed_pw"].ToString() == HashPassword(password);
                }
            }
        }

        return false;
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
