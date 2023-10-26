using System;
using System.Collections.Generic;

namespace Traffic_Violation.Models;

public partial class ProjectTestimonialState
{
    public decimal Stateid { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ProjectTestimonial> ProjectTestimonials { get; set; } = new List<ProjectTestimonial>();
}
