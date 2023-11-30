using System;
using System.Collections.Generic;

namespace SoftTech.Models;

public partial class Vacation
{
    public string id { get; set; } = null!;

    public DateTime date_vac { get; set; }

    public int vacation_days { get; set; }

    public int remaining_days { get; set; }

    public string status_vac { get; set; } = null!;

    public string id_emp { get; set; } = null!;

    public virtual Employee id_empNavigation { get; set; } = null!;
}
