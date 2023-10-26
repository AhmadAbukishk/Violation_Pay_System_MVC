using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectCar
{
    public decimal Carid { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public string? Color { get; set; }

    public decimal Platenumber { get; set; }

    public decimal Userid { get; set; }

    public virtual ICollection<ProjectViolation> ProjectViolations { get; set; } = new List<ProjectViolation>();

    public virtual ProjectUser? User { get; set; }
}
