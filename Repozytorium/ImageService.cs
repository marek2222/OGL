using ImageResizer;
using Repozytorium.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

//using Microsoft.WindowsAzure;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Blob;

namespace Repozytorium
{
  public struct ImageDimensions
  {
    private readonly string _sizeName;
    private readonly int _width;
    private readonly int _height;

    public ImageDimensions(string sizeName, int width, int height)
    {
      this._sizeName = sizeName;
      this._width = width;
      this._height = height;
    }
    public string SizeName { get { return _sizeName; } }
    public int Width { get { return _width; } }
    public int Height { get { return _height; } }
  }

  public static class GalleryImages
  {
    public static readonly ReadOnlyCollection<ImageDimensions> GalleryDimensionsList = new ReadOnlyCollection<ImageDimensions>
     (new[] {
			new ImageDimensions("large",900,600),
			new ImageDimensions("small",200,200)
		});
  }

  //public class Zdjecie
  //{
  //  //---  ContainerName/SizeName/ImageName
  //  //---  zdjecia/small/56.jpg
  //  public byte[] ImgBytes;
  //  public string SizeName;
  //  public string ImageName;
  //}

  public class ImageUpload
  {

    public string UploadImageAndReturnImageName(HttpPostedFileBase fileBase)
    {
      byte[] image = fileBase.InputStream.ReadFully();
      if (!ImageOptimization.ValidateImage(image))
        return null;

      List<Zdjecie> imagesToUpload = GenerateImageMiniatures(image);
      try
      {
        UploadMultipleImagesToBlob(imagesToUpload);
      }
      catch
      {
        return null;
      }
      //the same image name for all 
      return imagesToUpload.First().ImageName;
    }

    List<Zdjecie> GenerateImageMiniatures(byte[] image)
    {
      List<Zdjecie> imagesToUpload = new List<Zdjecie>();

      string blobName = CreateBlobName();

      foreach (var img in GalleryImages.GalleryDimensionsList)
      {
        byte[] imgBytes = ImageOptimization.OptimizeImageFromBytes(img.Width, img.Height, image);
        Zdjecie Zdjecie = new Zdjecie()
        {
          ImgBytes = imgBytes,
          SizeName = img.SizeName,
          ImageName = blobName + "."
              + ImageOptimization.GetImageExtension(imgBytes).ToString()
        };
        imagesToUpload.Add(Zdjecie);
      }
      return imagesToUpload;
    }

    void UploadMultipleImagesToBlob(List<Zdjecie> images)
    {

      //foreach (var img in images)
      //{
      //  string blobName = GetFullBlobName(img.SizeName, img.ImageName);
      //  var blob = new Zdjecie();
      //  blob.ImageName = img.ImageName;
      //  blob.SizeName = img.SizeName;
      //  blob.ImgBytes = img.ImgBytes;
      //  //using (var binaryReader = new BinaryReader(model.ImageUpload.InputStream))
      //  //  product.Image = binaryReader.ReadBytes(img.ImgBytes.ContentLength);

      //  _repo.AddImage(blob);
      //  _repo.SaveChanges();
      //}
    }


    public void DeleteImageByNameWithMiniatures(string imageNameWithExtension)
    {
      //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
      //    CloudConfigurationManager.GetSetting("StorageConnectionString"));

      //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

      //CloudBlobContainer container = blobClient.GetContainerReference("zdjecia");
      //foreach (var img in GalleryImages.GalleryDimensionsList)
      //{
      //  DeleteImageByName(container, GetFullBlobName(img.SizeName, imageNameWithExtension));
      //}
    }

    //void DeleteImageByName(CloudBlobContainer container, string blobName)
    //{
    //  CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
    //  blockBlob.DeleteIfExists();
    //}

    static string CreateBlobName()
    {
      return System.Guid.NewGuid().ToString();
    }
    public static string GetFullBlobName(string sizeName, string imageNameWithExtension)
    {
      return sizeName + "/" + imageNameWithExtension;
    }
  }

  enum ImageExtension
  {
    bmp,
    jpeg,
    gif,
    png,
    unknown
  }
  static class ImageOptimization
  {
    public static ImageExtension GetImageExtension(byte[] bytes)
    {
      var bmp = Encoding.ASCII.GetBytes("BM");      // BMP
      var gif = Encoding.ASCII.GetBytes("GIF");     // GIF
      var png = new byte[] { 137, 80, 78, 71 };     // PNG
      var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
      var jpeg2 = new byte[] { 255, 216, 255, 225 };// jpeg canon

      if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
        return ImageExtension.bmp;

      if (gif.SequenceEqual(bytes.Take(gif.Length)))
        return ImageExtension.gif;

      if (png.SequenceEqual(bytes.Take(png.Length)))
        return ImageExtension.png;

      if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
        return ImageExtension.jpeg;

      if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
        return ImageExtension.jpeg;

      return ImageExtension.unknown;
    }
    public static bool ValidateImage(byte[] image)
    {
      if (GetImageExtension(image) != ImageExtension.unknown)
        return true;

      return false;
    }
    public static byte[] OptimizeImageFromBytes(int imgWidth, int imgHeight, byte[] imgBytes)
    {
      var settings = new ResizeSettings
      {
        MaxWidth = imgWidth,
        MaxHeight = imgHeight
      };

      MemoryStream ms = new MemoryStream();
      ImageBuilder.Current.Build(imgBytes, ms, settings);
      return ms.ToArray();
    }
  }

  public static class StreamHelpers
  {
    public static byte[] ReadFully(this Stream input)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        input.CopyTo(ms);
        return ms.ToArray();
      }
    }
  }
}