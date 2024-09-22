using System;
using System.Collections.Generic;

namespace ROYALHOTEL.Models;

public partial class Reporting
{
    public decimal Reportid { get; set; }

    public byte Year { get; set; }

    public decimal? Totalroomsbooked { get; set; }

    public decimal? Pricepernight { get; set; }

    public decimal? Totalexpenses { get; set; }

    public decimal? Revenue { get; set; }

    public decimal? Netprofit { get; set; }

    public string? Profitorloss { get; set; }
}
