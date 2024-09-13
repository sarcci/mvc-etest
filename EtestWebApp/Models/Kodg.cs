using System;
using System.Collections.Generic;

namespace EtestWebApp.Models;

public partial class Kodg
{
    public int Id { get; set; }

    public int Ktid { get; set; }

    public int Pid { get; set; }

    public int Oid { get; set; }

    public virtual Kt? Kt { get; set; } = null!;

    public virtual Odgovor? OidNavigation { get; set; } = null!;

    public virtual Prasanje? PidNavigation { get; set; } = null!;
}
