using System;
using System.Collections.Generic;

namespace ROYALHOTEL.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public string? Imageurl { get; set; }

    public string? Testimonialtext { get; set; }
}
