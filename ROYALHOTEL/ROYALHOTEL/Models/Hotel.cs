using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROYALHOTEL.Models;

public partial class Hotel
{
    public decimal Hotelid { get; set; }

    public string Hotelname { get; set; } = null!;

    public string Imagepath { get; set; } = null!;

    [NotMapped]
    public IFormFile ImageFile { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Testimonials1> Testimonials1s { get; set; } = new List<Testimonials1>();
}
