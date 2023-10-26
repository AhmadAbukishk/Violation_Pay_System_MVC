using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Traffic_Violation.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ProjectAboutSection> ProjectAboutSections { get; set; }

    public virtual DbSet<ProjectCar> ProjectCars { get; set; }

    public virtual DbSet<ProjectContactSection> ProjectContactSections { get; set; }

    public virtual DbSet<ProjectFactSection> ProjectFactSections { get; set; }

    public virtual DbSet<ProjectHeaderSection> ProjectHeaderSections { get; set; }

    public virtual DbSet<ProjectLicense> ProjectLicenses { get; set; }

    public virtual DbSet<ProjectRole> ProjectRoles { get; set; }

    public virtual DbSet<ProjectServiceSection> ProjectServiceSections { get; set; }

    public virtual DbSet<ProjectTestimonial> ProjectTestimonials { get; set; }

    public virtual DbSet<ProjectTestimonialState> ProjectTestimonialStates { get; set; }

    public virtual DbSet<ProjectUser> ProjectUsers { get; set; }

    public virtual DbSet<ProjectUserLogin> ProjectUserLogins { get; set; }

    public virtual DbSet<ProjectViolation> ProjectViolations { get; set; }

    public virtual DbSet<ProjectViolationState> ProjectViolationStates { get; set; }

    public virtual DbSet<ProjectViolationType> ProjectViolationTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=xe)));User Id=C##MVCAHMAD;Password=Test123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##MVCAHMAD")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<ProjectAboutSection>(entity =>
        {
            entity.HasKey(e => e.Aboutid).HasName("PROJECT_ABOUT_SECTION_PK");

            entity.ToTable("PROJECT_ABOUT_SECTION");

            entity.Property(e => e.Aboutid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ABOUTID");
            entity.Property(e => e.Header)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("HEADER");
            entity.Property(e => e.Imgpath)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("IMGPATH");
            entity.Property(e => e.Subheader)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("SUBHEADER");
        });

        modelBuilder.Entity<ProjectCar>(entity =>
        {
            entity.HasKey(e => e.Carid).HasName("PROJECT_CAR_PK");

            entity.ToTable("PROJECT_CAR");

            entity.HasIndex(e => e.Platenumber, "PROJECT_CAR_UK1").IsUnique();

            entity.Property(e => e.Carid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARID");
            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BRAND");
            entity.Property(e => e.Color)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("COLOR");
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MODEL");
            entity.Property(e => e.Platenumber)
                .HasColumnType("NUMBER")
                .HasColumnName("PLATENUMBER");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectCars)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("PROJECT_CAR_FK1");
        });

        modelBuilder.Entity<ProjectContactSection>(entity =>
        {
            entity.HasKey(e => e.Contactid).HasName("PROJECT_CONTACT_SECTION_PK");

            entity.ToTable("PROJECT_CONTACT_SECTION");

            entity.Property(e => e.Contactid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTID");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Content)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PHONE");
            entity.Property(e => e.Title)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<ProjectFactSection>(entity =>
        {
            entity.HasKey(e => e.Factid).HasName("PROJECT_FACT_SECTION_PK");

            entity.ToTable("PROJECT_FACT_SECTION");

            entity.Property(e => e.Factid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("FACTID");
            entity.Property(e => e.Content)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Title)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<ProjectHeaderSection>(entity =>
        {
            entity.HasKey(e => e.Headerid).HasName("PROJECT_HEADER_SECTION_PK");

            entity.ToTable("PROJECT_HEADER_SECTION");

            entity.Property(e => e.Headerid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("HEADERID");
            entity.Property(e => e.Imgpath)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("IMGPATH");
            entity.Property(e => e.Subtitle)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("SUBTITLE");
            entity.Property(e => e.Title)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<ProjectLicense>(entity =>
        {
            entity.HasKey(e => e.Licenseid).HasName("PROJECT_LICENSE_PK");

            entity.ToTable("PROJECT_LICENSE");

            entity.Property(e => e.Licenseid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("LICENSEID");
            entity.Property(e => e.Expdate)
                .HasColumnType("DATE")
                .HasColumnName("EXPDATE");
            entity.Property(e => e.Issdate)
                .HasColumnType("DATE")
                .HasColumnName("ISSDATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectLicenses)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("PROJECT_LICENSE_FK1");
        });

        modelBuilder.Entity<ProjectRole>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("PROJECT_ROLE_PK");

            entity.ToTable("PROJECT_ROLE");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<ProjectServiceSection>(entity =>
        {
            entity.HasKey(e => e.Serviceid).HasName("PROJECT_SERVICE_SECTION_PK");

            entity.ToTable("PROJECT_SERVICE_SECTION");

            entity.Property(e => e.Serviceid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("SERVICEID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imgpath)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("IMGPATH");
            entity.Property(e => e.Title)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<ProjectTestimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("PROJECT_TESTIMONIAL_PK");

            entity.ToTable("PROJECT_TESTIMONIAL");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Content)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Dateadded)
                .HasColumnType("DATE")
                .HasColumnName("DATEADDED");
            entity.Property(e => e.Stateid)
                .HasColumnType("NUMBER")
                .HasColumnName("STATEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.State).WithMany(p => p.ProjectTestimonials)
                .HasForeignKey(d => d.Stateid)
                .HasConstraintName("PROJECT_TESTIMONIAL_FK2");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectTestimonials)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("PROJECT_TESTIMONIAL_FK1");
        });

        modelBuilder.Entity<ProjectTestimonialState>(entity =>
        {
            entity.HasKey(e => e.Stateid).HasName("PROJECT_TESTIMONIAL_STATE_PK");

            entity.ToTable("PROJECT_TESTIMONIAL_STATE");

            entity.Property(e => e.Stateid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("STATEID");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<ProjectUser>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("PROJECT_USER_PK");

            entity.ToTable("PROJECT_USER");

            entity.HasIndex(e => e.Email, "PROJECT_USER_UK1").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Birthdate)
                .HasColumnType("DATE")
                .HasColumnName("BIRTHDATE");
            entity.Property(e => e.Email)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fname)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Imgpath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMGPATH");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Phonenumber)
                .HasPrecision(9)
                .HasColumnName("PHONENUMBER");
        });

        modelBuilder.Entity<ProjectUserLogin>(entity =>
        {
            entity.HasKey(e => e.Loginid).HasName("PROJECT_USER_LOGIN_PK");

            entity.ToTable("PROJECT_USER_LOGIN");

            entity.HasIndex(e => e.Username, "PROJECT_USER_LOGIN_UK1").IsUnique();

            entity.Property(e => e.Loginid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("LOGINID");
            entity.Property(e => e.Password)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Role).WithMany(p => p.ProjectUserLogins)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("PROJECT_USER_LOGIN_FK2");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectUserLogins)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("PROJECT_USER_LOGIN_FK1");
        });

        modelBuilder.Entity<ProjectViolation>(entity =>
        {
            entity.HasKey(e => e.Violationid).HasName("PROJECT_VIOLATION_PK");

            entity.ToTable("PROJECT_VIOLATION");

            entity.Property(e => e.Violationid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("VIOLATIONID");
            entity.Property(e => e.Carid)
                .HasColumnType("NUMBER")
                .HasColumnName("CARID");
            entity.Property(e => e.Location)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("LOCATION");
            entity.Property(e => e.Stateid)
                .HasColumnType("NUMBER")
                .HasColumnName("STATEID");
            entity.Property(e => e.Violationdate)
                .HasColumnType("DATE")
                .HasColumnName("VIOLATIONDATE");
            entity.Property(e => e.Violationtypeid)
                .HasColumnType("NUMBER")
                .HasColumnName("VIOLATIONTYPEID");

            entity.HasOne(d => d.Car).WithMany(p => p.ProjectViolations)
                .HasForeignKey(d => d.Carid)
                .HasConstraintName("PROJECT_VIOLATION_FK1");

            entity.HasOne(d => d.State).WithMany(p => p.ProjectViolations)
                .HasForeignKey(d => d.Stateid)
                .HasConstraintName("PROJECT_VIOLATION_FK2");

            entity.HasOne(d => d.Violationtype).WithMany(p => p.ProjectViolations)
                .HasForeignKey(d => d.Violationtypeid)
                .HasConstraintName("PROJECT_VIOLATION_FK3");
        });

        modelBuilder.Entity<ProjectViolationState>(entity =>
        {
            entity.HasKey(e => e.Stateid).HasName("PROJECT_VIOLATION_STATE_PK");

            entity.ToTable("PROJECT_VIOLATION_STATE");

            entity.Property(e => e.Stateid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("STATEID");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<ProjectViolationType>(entity =>
        {
            entity.HasKey(e => e.Violationtypeid).HasName("PROEJCT_VIOLATION_TYPE_PK");

            entity.ToTable("PROJECT_VIOLATION_TYPE");

            entity.Property(e => e.Violationtypeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("VIOLATIONTYPEID");
            entity.Property(e => e.Fine)
                .HasColumnType("FLOAT")
                .HasColumnName("FINE");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });
        modelBuilder.HasSequence("CATEGORY_SEQ");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
