using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectViolation
{
    public decimal Violationid { get; set; }

    public DateTime Violationdate { get; set; }

    public string Location { get; set; } = null!;

    public decimal Violationtypeid { get; set; }

    public decimal Carid { get; set; }

    public decimal Stateid { get; set; }

    public virtual ProjectCar? Car { get; set; }

    public virtual ProjectViolationState? State { get; set; }

    public virtual ProjectViolationType? Violationtype { get; set; }
}
