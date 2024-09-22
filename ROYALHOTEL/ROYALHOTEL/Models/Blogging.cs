using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROYALHOTEL.Models;

public partial class Blogging
{
    public decimal Blogid { get; set; }

    public string? Imageurl { get; set; }

    public string? Heading { get; set; }

    public DateTime? Datecreated { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public string? Content { get; set; }
}
