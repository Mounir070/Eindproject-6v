using System.Net.Mime;

namespace Eindproject_6v.Models;

public class ImageModel
{
    public int id;
    public string? title;
    public string author;
    public string? description;
    public byte[] bytes;

    public ImageModel(int id, string? title, string author, string? description, byte[] bytes)
    {
        this.id = id;
        this.title = title;
        this.author = author;
        this.description = description;
        this.bytes = bytes;
    }

    public bool HasTitle()
    {
        return this.title is not null;
    }

    public bool HasDescription()
    {
        return this.description is not null;
    }

    public string GetBase64()
    {
        return Convert.ToBase64String(this.bytes);
    }
}
