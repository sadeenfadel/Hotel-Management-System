using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROYALHOTEL.Models;

public partial class Testimonials1
{
    public decimal Testimonialid { get; set; }

    public decimal Userid { get; set; }

    public decimal Hotelid { get; set; }

    public string? Imagepath { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }
    public string? Testimonialtext { get; set; }

    public bool? Rating { get; set; }

    public DateTime? Datecreated { get; set; }

    public virtual Hotel Hotel { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
