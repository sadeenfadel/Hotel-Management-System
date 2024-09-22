using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROYALHOTEL.Models;

public partial class Gallery
{
    public decimal Galleryid { get; set; }

    public string? Imagepath { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }
}
