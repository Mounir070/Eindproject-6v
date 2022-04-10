using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Eindproject_6v.Models;
using MySql.Data.MySqlClient;

namespace Eindproject_6v.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public const string ConnectionString = "Server=informatica.st-maartenscollege.nl;Database=110272;Uid=110272;Pwd=inf2021sql;";

    private static List<ImageModel> GetRecentImages()
    {
        const string query = "select IMG_ID, IMG_TITLE, USER_NAME, IMG_DESCRIPTION, IMG_BLOB from img_info join user_info on img_info.IMG_AUTHOR_ID = user_info.USER_ID order by img_info.IMG_ID desc";
        var images = new List<ImageModel>();
        using var connection = new MySqlConnection(ConnectionString);
        connection.Open();
        var cmd = new MySqlCommand(query, connection);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = reader.GetInt32("IMG_ID");
                
                var title = reader.GetString("IMG_TITLE");
                
                var author = reader.GetString("USER_NAME");
                
                var description = reader.GetString("IMG_DESCRIPTION");

                var size = reader.GetUInt32("IMG_SIZE");
                var bytes = new byte[size];
                reader.GetBytes(reader.GetOrdinal("IMG_BLOB"), 0, bytes, 0, (int) size);
                images.Add(new ImageModel(id, title, author, description, bytes));
            }
        }
        connection.Close();
        return images;
    }

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Route("Explore")]
    public IActionResult Explore()
    {
        var images = GetRecentImages();
        return View(images);
    }

    [Route("NotFound")]
    public IActionResult NotFound()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
