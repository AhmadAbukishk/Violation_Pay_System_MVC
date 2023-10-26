using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectViolationState
{
    public decimal Stateid { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ProjectViolation> ProjectViolations { get; set; } = new List<ProjectViolation>();
}
