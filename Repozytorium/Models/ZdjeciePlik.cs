using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Repozytorium.Models
{
  public class ZdjeciePlik
  {
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string UzytkownikId { get; set; }
    public Uzytkownik Uzytkownik { get; set; }

    //---  ContainerName/SizeName/ImageName
    //---  zdjecia/small/56.jpg
    //---  zdjecia/large/56.jpg
    public string ImageName { get; set; }
    public string SizeName  { get; set; }

    [NotMapped]
    [Required]
    [DataType(DataType.Upload)]
    [Display(Name = "Wybierz plik")]
    public HttpPostedFileBase ImgBytes  { get; set; }

  }
}