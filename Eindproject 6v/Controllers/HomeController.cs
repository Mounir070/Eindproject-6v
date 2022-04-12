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
        // Voeg USER_NAME toe met een "join" en sorteer van nieuw naar oud
        const string query = "select IMG_ID, IMG_TITLE, USER_ID, USER_NAME, IMG_DESCRIPTION, IMG_BLOB from img_info join user_info on img_info.IMG_AUTHOR_ID = user_info.USER_ID order by img_info.IMG_ID desc";
        var images = new List<ImageModel>();
        using var connection = new MySqlConnection(ConnectionString);
        connection.Open();
        var cmd = new MySqlCommand(query, connection);
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var imgId = reader.GetInt32("IMG_ID");

                var imgTitle = reader.GetString("IMG_TITLE");

                var imgAuthorId = reader.GetInt32("USER_ID");

                var imgAuthor = reader.GetString("USER_NAME");

                var imgDescription = reader.GetString("IMG_DESCRIPTION");

                var imgSize = reader.GetUInt32("IMG_SIZE");
                var imgBlob = new byte[imgSize];
                reader.GetBytes(reader.GetOrdinal("IMG_BLOB"), 0, imgBlob, 0, (int) imgSize);
                images.Add(new ImageModel(imgId, imgTitle, imgAuthorId, imgAuthor, imgDescription, imgBlob));
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
