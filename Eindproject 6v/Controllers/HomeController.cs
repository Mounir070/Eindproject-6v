using System.Data.SqlTypes;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Eindproject_6v.Models;
using MySql.Data.MySqlClient;

using System.Web;

namespace Eindproject_6v.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public const string ConnectionString = "Server=informatica.st-maartenscollege.nl;Database=110272;Uid=110272;Pwd=inf2021sql;";

    private static List<ImageModel> GetRecentImages()
    {
        // Voeg USER_NAME toe met een "join" en sorteer van nieuw naar oud
        const string query = "select IMG_ID, IMG_TITLE, USER_ID, USER_NAME, IMG_DESCRIPTION, IMG_SIZE, IMG_BLOB from img_info join user_info on img_info.IMG_AUTHOR_ID = user_info.USER_ID order by img_info.IMG_ID desc";
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

    private static int UploadImage(string imgTitle, int imgAuthorId, string imgDescription, byte[] imgBlob)
    {
        const string query = "insert into img_info (IMG_TITLE, IMG_AUTHOR_ID, IMG_DESCRIPTION, IMG_SIZE, IMG_BLOB) values (@TITLE, @AUTHOR_ID, @DESCRIPTION, @SIZE, @BLOB)";
        using MySqlConnection conn = new MySqlConnection(ConnectionString);
        conn.Open();
        MySqlCommand cmd = new MySqlCommand(query, conn);
        cmd.Parameters.Add("@TITLE", MySqlDbType.VarChar).Value = imgTitle;
        cmd.Parameters.Add("@AUTHOR_ID", MySqlDbType.Int32).Value = imgAuthorId;
        cmd.Parameters.Add("@DESCRIPTION", MySqlDbType.VarChar).Value = imgDescription;
        cmd.Parameters.Add("@SIZE", MySqlDbType.UInt32).Value = imgBlob.Length;
        cmd.Parameters.Add("@BLOB", MySqlDbType.Blob).Value = imgBlob;
        return cmd.ExecuteNonQuery();
    }

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SubmitImage(string imgTitle, string imgDescription, IFormFile imgFile)
    {
        int? id = HttpContext.Session.GetInt32(LoginController.SessionKeyId);
        if (id is null)
        {
            return RedirectToAction("Index", "Home");
        }
        if (IsImage(imgFile))
        {
            try
            {
                Stream stream = imgFile.OpenReadStream();
                if (!stream.CanRead)
                {
                    return RedirectToAction("Index", "Home");
                }
                // https://stackoverflow.com/questions/36432028/how-to-convert-a-file-into-byte-array-in-memory
                using (var memoryStream = new MemoryStream())
                {
                    imgFile.CopyToAsync(memoryStream);
                    byte[] blob = memoryStream.ToArray();
                    int upload = UploadImage(imgTitle, id.Value, imgDescription, blob);
                }
            }
            catch (MySqlException e)
            {
                // dit wordt gerund als er iets fout gaat
                // we moeten hier eigenlijk checken wat fout gaat
                return RedirectToAction("Index", "Home");
            }
        }
        return RedirectToAction("Index", "Home");
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

    // https://stackoverflow.com/questions/11063900/determine-if-uploaded-file-is-image-any-format-on-mvc
    private static bool IsImage(IFormFile file)
    {
        if (file is null)
        {
            return false;
        }

        var contentType = file.ContentType.ToLower();
        // https://www.tutorialsteacher.com/csharp/csharp-switch
        // want bijvoorbeeld als de content Type iets anders is dan die van de bovenste link dan return je false
        switch (contentType)
        {
            case "image/jpg":
            case "image/jpeg":
            case "image/pjpeg":
            case "image/gif":
            case "image/x-png":
            case "image/png":
                break;
            default:
                return false;
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        switch (fileExtension)
        {
            case ".jpg":
            case ".jpeg":
            case ".gif":
            case ".png":
                break;
            default:
                return false;
        }

        return true;
    }
}
