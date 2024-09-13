using System;
using System.Collections.Generic;

namespace EtestWebApp.Models;

public partial class Prasanje
{
    public int Id { get; set; }

    public int Tid { get; set; }

    public string Tekst { get; set; } = null!;

    public bool Tip { get; set; }

    public virtual ICollection<Kodg>? Kodgs { get; set; } = new List<Kodg>();

    public virtual ICollection<Odgovor>? Odgovors { get; set; } = new List<Odgovor>();

    public virtual Test? TidNavigation { get; set; } = null!;
}
