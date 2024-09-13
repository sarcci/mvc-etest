using System;
using System.Collections.Generic;

namespace EtestWebApp.Models;

public partial class Odgovor
{
    public int Id { get; set; }

    public string Tekst { get; set; } = null!;

    public int Pid { get; set; }

    public bool True { get; set; }

    public virtual ICollection<Kodg>? Kodgs { get; set; } = new List<Kodg>();

    public virtual Prasanje? PidNavigation { get; set; } = null!;
}
