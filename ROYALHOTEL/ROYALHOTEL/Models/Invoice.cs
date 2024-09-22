using System;
using System.Collections.Generic;

namespace ROYALHOTEL.Models;

public partial class Invoice
{
    public decimal Invoiceid { get; set; }

    public decimal? Reservationid { get; set; }

    public decimal? Userid { get; set; }

    public DateTime? Invoicedate { get; set; }

    public string? Invoicecontent { get; set; }

    public virtual Reservation? Reservation { get; set; }

    public virtual User? User { get; set; }
}
