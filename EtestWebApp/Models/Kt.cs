using System;
using System.Collections.Generic;

namespace EtestWebApp.Models;

public partial class Kt
{
    public int Id { get; set; }

    public int Kid { get; set; }

    public int? Tid { get; set; }

    public DateTime? Datum { get; set; }

    public int? Score { get; set; }

    public virtual Korisnik KidNavigation { get; set; } = null!;

    public virtual ICollection<Kodg> Kodgs { get; set; } = new List<Kodg>();

    public virtual Test? TidNavigation { get; set; }
}
