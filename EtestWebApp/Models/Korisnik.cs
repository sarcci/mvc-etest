using System;
using System.Collections.Generic;

namespace EtestWebApp.Models;

public partial class Korisnik
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public virtual ICollection<Kt> Kts { get; set; } = new List<Kt>();
}
