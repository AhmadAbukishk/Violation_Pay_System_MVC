using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Traffic_Violation.Models;

public partial class ProjectUser
{
    public decimal Userid { get; set; }

    public string Fname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public DateTime? Birthdate { get; set; }

    public int? Phonenumber { get; set; }

    public string Email { get; set; } = null!;

    public string? Imgpath { get; set; }
    [NotMapped]
    public virtual IFormFile? ImgFile { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<ProjectCar> ProjectCars { get; set; } = new List<ProjectCar>();

    public virtual ICollection<ProjectLicense> ProjectLicenses { get; set; } = new List<ProjectLicense>();

    public virtual ICollection<ProjectTestimonial> ProjectTestimonials { get; set; } = new List<ProjectTestimonial>();

    public virtual ICollection<ProjectUserLogin> ProjectUserLogins { get; set; } = new List<ProjectUserLogin>();
}
