using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Eindproject_6v.Controllers;

public class LoginController : Controller
{
    public const string SessionKeyName = "_Name";
    public const string SessionKeyId = "_Id";
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    // Maak een gehashte versie van het wachtwoord
    private static string HashPassword(string password)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return BytesToHex(hash);
    }

    // Converteer de hash naar hexadecimaal getal
    private static string BytesToHex(byte[] bytes)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);
        for (int i = 0; i < bytes.Length; i++)
        {
            result.Append(bytes[i].ToString("X2"));
        }
        return result.ToString();
    }

    private static int RegisterAccount(string user, string password)
    {
        const string query = "insert into user_info (USER_NAME, HASHED_PW) values (@USER, @PASSWORD)";
        
        using (MySqlConnection conn = new MySqlConnection(HomeController.ConnectionString))
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.Add("@USER", MySqlDbType.VarChar).Value = user;
                cmd.Parameters.Add("@PASSWORD", MySqlDbType.VarChar).Value = HashPassword(password);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                // gebruik switch voor https://dev.mysql.com/doc/mysql-errors/5.7/en/server-error-reference.html
                uint code = e.Code;
                switch (code)
                {
                    case 1062:
                    case 1586:    
                        Console.WriteLine("Duplicate key");
                        // de username bestaat al
                        break;
                    default:
                        Console.WriteLine("error code = " + e.Code + ", error number = " + e.Number);
                        Console.WriteLine(e.Message);
                        break;
                }
            }
        }

        return -1;
    }
    
    private static bool LoginAccount(string user, string password)
    {
        const string query = "select HASHED_PW from user_info where USER_NAME = @USER";
        
        using (MySqlConnection conn = new MySqlConnection(HomeController.ConnectionString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@USER", MySqlDbType.VarChar).Value = user;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return reader.GetString("HASHED_PW") == HashPassword(password);
                }
            }
        }

        return false;
    }

    private static int? GetUserId(string user)
    {
        const string query = "select USER_ID from user_info where USER_NAME = @USER";
        
        using (MySqlConnection conn = new MySqlConnection(HomeController.ConnectionString))
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.Add("@USER", MySqlDbType.VarChar).Value = user;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return reader.GetInt32("USER_ID");
                }
            }
        }

        return null;
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        bool logged = LoginAccount(username, password);
        int? id = GetUserId(username);
        if (logged && id.HasValue)
        {
            HttpContext.Session.SetString(SessionKeyName, username);
            HttpContext.Session.SetInt32(SessionKeyId, id.Value);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }
    
    [HttpPost]
    public IActionResult Register(string username, string password, string confirm_password)
    {
        if (confirm_password != password)
        {
            return RedirectToAction("Index", "Login");
        }

        int signup = RegisterAccount(username, password);
        int? id = GetUserId(username);
        if (id.HasValue)
        {
            HttpContext.Session.SetString(SessionKeyName, username);
            HttpContext.Session.SetInt32(SessionKeyId, id.Value);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
