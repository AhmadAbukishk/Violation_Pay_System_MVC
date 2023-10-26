using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traffic_Violation.Models;

public partial class ProjectServiceSection
{
    public decimal Serviceid { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Imgpath { get; set; } = null!;

    [NotMapped]
    public virtual IFormFile? Imgfile { get; set; }
}
