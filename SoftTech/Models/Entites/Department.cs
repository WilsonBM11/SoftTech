using System;
using System.Collections.Generic;

namespace SoftTech.Models.Entites;

public partial class Department
{
    public string id { get; set; } = Guid.NewGuid().ToString();

    public string name_depto { get; set; } = null!;

    public string description_depto { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
