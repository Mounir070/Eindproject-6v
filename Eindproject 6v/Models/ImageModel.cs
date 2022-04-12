using System.Net.Mime;

namespace Eindproject_6v.Models;

// struct omdat de waardes niet null kunnen zijn
public struct ImageModel
{
    public int ImgId;
    public string ImgTitle;
    public int ImgAuthorId;
    public string ImgAuthor;
    public string ImgDescription;
    public byte[] ImgBlob;

    public ImageModel(int imgId, string imgTitle, int imgAuthorId, string imgAuthor, string imgDescription, byte[] imgBlob)
    {
        this.ImgId = imgId;
        this.ImgTitle = imgTitle;
        this.ImgAuthorId = imgAuthorId;
        this.ImgAuthor = imgAuthor;
        this.ImgDescription = imgDescription;
        this.ImgBlob = imgBlob;
    }

    public string GetBase64()
    {
        return Convert.ToBase64String(this.ImgBlob);
    }
}
