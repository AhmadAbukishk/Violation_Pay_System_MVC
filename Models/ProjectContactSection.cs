using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectContactSection
{
    public decimal Contactid { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;
}
