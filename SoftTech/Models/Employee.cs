using System;
using System.Collections.Generic;

namespace SoftTech.Models;

public partial class Employee
{
    public string id { get; set; } = null!;

    public string identification { get; set; } = null!;

    public string name_emp { get; set; } = null!;

    public string email { get; set; } = null!;

    public string phone_number { get; set; } = null!;

    public string address_emp { get; set; } = null!;

    public DateTime birthdate { get; set; }

    public string id_user { get; set; } = null!;

    public string id_depto { get; set; } = null!;

    public DateTime contract_date { get; set; }

    public double salary { get; set; }

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    public virtual ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();

    public virtual Department id_deptoNavigation { get; set; } = null!;
}
