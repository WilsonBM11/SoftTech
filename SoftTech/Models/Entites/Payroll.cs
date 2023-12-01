using System;
using System.Collections.Generic;

namespace SoftTech.Models.Entites;

public partial class Payroll
{
    public string id { get; set; } = Guid.NewGuid().ToString();

    public DateTime date_payroll { get; set; }

    public double income_tax { get; set; }

    public double employer_ccss { get; set; }

    public double employee_ccss { get; set; }

    public double total_salary { get; set; }

    public double total_payment { get; set; }

    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();
}
