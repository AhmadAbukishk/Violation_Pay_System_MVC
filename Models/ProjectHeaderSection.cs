using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traffic_Violation.Models;

public partial class ProjectHeaderSection
{
    public decimal Headerid { get; set; }

    public string Title { get; set; } = null!;

    public string Subtitle { get; set; } = null!;

    public string Imgpath { get; set; } = null!;

    [NotMapped]
    public virtual IFormFile? Imgfile { get; set; }
}
