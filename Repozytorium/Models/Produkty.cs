using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
  public class Produkty
  {
    [Key, Display(Name = "Id produktu:")]
    public Int32 ProductId { get; set; }

    [Required]
    [Display(Name = "Nazwa produktu")]
    public String ProductName { get; set; }

    [Required]
    [Display(Name = "Price")]
    public Decimal Price { get; set; }

    [Required]
    [Display(Name = "Category")]
    public Int32 CategoryId { get; set; }

    [Required]
    [DataType(DataType.Upload)]
    [Display(Name = "Wybierz plik")]
    public HttpPostedFileBase ImageUpload { get; set; }
  }
}