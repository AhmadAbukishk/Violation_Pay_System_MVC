using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectTestimonial
{
    public decimal Testimonialid { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Dateadded { get; set; }

    public decimal Userid { get; set; }

    public decimal Stateid { get; set; }

    public virtual ProjectTestimonialState? State { get; set; }

    public virtual ProjectUser? User { get; set; }
}
