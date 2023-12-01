using System;
using System.Collections.Generic;

namespace SoftTech.Models.Entites;

public partial class Salary
{
    public string id { get; set; } = Guid.NewGuid().ToString();

    public int horas_e { get; set; }

    public double gross_salary { get; set; }

    public double income_tax { get; set; }

    public double ccss_amount { get; set; }

    public double net_salary { get; set; }

    public string id_payroll { get; set; } = null!;

    public string id_employee { get; set; } = null!;

    public virtual Employee id_employeeNavigation { get; set; } = null!;

    public virtual Payroll id_payrollNavigation { get; set; } = null!;
}
