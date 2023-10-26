using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traffic_Violation.Models;

public partial class ProjectAboutSection
{
    public decimal Aboutid { get; set; }

    public string Header { get; set; } = null!;

    public string Subheader { get; set; } = null!;

    public string Imgpath { get; set; } = null!;

    [NotMapped]
    public virtual IFormFile? Imgfile { get; set;}
}
