using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROYALHOTEL.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string Userfname { get; set; } = null!;

    public string Userlname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Imagepath { get; set; } = null!;

    [NotMapped]
    public IFormFile ImageFile { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Testimonials1> Testimonials1s { get; set; } = new List<Testimonials1>();
}
