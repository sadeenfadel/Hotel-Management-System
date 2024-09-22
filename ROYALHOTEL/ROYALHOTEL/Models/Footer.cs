using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROYALHOTEL.Models;

public partial class Footer
{
    public decimal Footerid { get; set; }

    public string? About { get; set; }

    public string Links { get; set; } = null!;

    public string Newsletter { get; set; } = null!;

    public string? Imagepath { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }
}
