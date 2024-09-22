using System;
using System.Collections.Generic;

namespace ROYALHOTEL.Models;

public partial class Reservation
{
    public decimal Reservationid { get; set; }

    public decimal? Userid { get; set; }

    public decimal? Roomid { get; set; }

    public DateTime? Reservationdate { get; set; }

    public DateTime? Checkindate { get; set; }

    public DateTime? Checkoutdate { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Room? Room { get; set; }

    public virtual User? User { get; set; }
}
