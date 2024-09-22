using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROYALHOTEL.Models;

public partial class Contactu
{
    public decimal Contactusid { get; set; }

    public string? Address { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    [NotMapped]
    public IFormFile ImageFile { get; set; }

    public string? Mapurl { get; set; }
}
