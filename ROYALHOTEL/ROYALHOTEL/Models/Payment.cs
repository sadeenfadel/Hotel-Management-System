using System;
using System.Collections.Generic;

namespace ROYALHOTEL.Models;

public partial class Payment
{
    public decimal Paymentid { get; set; }

    public decimal? Reservationid { get; set; }

    public string? Paymentmethod { get; set; }

    public string Cardholdername { get; set; } = null!;

    public string Creditcardnumber { get; set; } = null!;

    public string Expirydate { get; set; } = null!;

    public DateTime? Paymentdate { get; set; }

    public decimal Amount { get; set; }

    public virtual Reservation? Reservation { get; set; }
}
