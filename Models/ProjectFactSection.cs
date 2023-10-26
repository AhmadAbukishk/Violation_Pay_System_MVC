using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectFactSection
{
    public decimal Factid { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;
}
