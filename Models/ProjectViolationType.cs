using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectViolationType
{
    public decimal Violationtypeid { get; set; }

    public string? Name { get; set; }

    public decimal Fine { get; set; }

    public virtual ICollection<ProjectViolation> ProjectViolations { get; set; } = new List<ProjectViolation>();
}
