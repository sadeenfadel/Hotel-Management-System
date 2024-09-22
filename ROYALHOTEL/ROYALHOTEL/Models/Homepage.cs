using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROYALHOTEL.Models;

public partial class Homepage
{
    public decimal Homepageid { get; set; }

    public string? Paragraph { get; set; }

    public string? Imagepath { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public string? Greeting { get; set; }
}
