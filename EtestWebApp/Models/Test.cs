using System;
using System.Collections.Generic;

namespace EtestWebApp.Models;

public partial class Test
{
    public int Id { get; set; }

    public string Ime { get; set; } = null!;

    public bool Tip { get; set; } 

    public virtual ICollection<Kt>? Kts { get; set; } = new List<Kt>();

    public virtual ICollection<Prasanje>? Prasanjes { get; set; } = new List<Prasanje>();
}
