using Microsoft.EntityFrameworkCore;

namespace HC.DataAccess.Models;

public partial class HealthcareAppointmentContext : DbContext
{
    public HealthcareAppointmentContext()
    {
    }

    public HealthcareAppointmentContext(DbContextOptions<HealthcareAppointmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentStatus> AppointmentStatuses { get; set; }

    public virtual DbSet<HealthcareProfessional> HealthcareProfessionals { get; set; }

    public virtual DbSet<ProfessionalsSpeciality> ProfessionalsSpecialities { get; set; }

    public virtual DbSet<Speciality> Specialities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;database=healthcare_appointment;user=root;password=LenovoG@5100626", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PRIMARY");

            entity.ToTable("appointment", tb => tb.HasComment("Main table which has data of the appointments of the user with the professionals"));

            entity.HasIndex(e => e.StatusId, "FK_appointment_appointment_status_idx");

            entity.HasIndex(e => e.HealthcareProfessionalId, "FK_appointment_healthcare_professionals_idx");

            entity.HasIndex(e => e.UserId, "FK_appointment_users_idx");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.HealthcareProfessionalId).HasColumnName("healthcare_professional_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.HealthcareProfessional).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.HealthcareProfessionalId)
                .HasConstraintName("FK_appointment_healthcare_professionals");

            entity.HasOne(d => d.Status).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_appointment_appointment_status");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_appointment_users");
        });

        modelBuilder.Entity<AppointmentStatus>(entity =>
        {
            entity.HasKey(e => e.AppointmentStatusId).HasName("PRIMARY");

            entity.ToTable("appointment_status", tb => tb.HasComment("Represents status of the appointment"));

            entity.Property(e => e.AppointmentStatusId).HasColumnName("appointment_status_id");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
        });

        modelBuilder.Entity<HealthcareProfessional>(entity =>
        {
            entity.HasKey(e => e.ProfessionalId).HasName("PRIMARY");

            entity.ToTable("healthcare_professionals", tb => tb.HasComment("Doctors who will provide the treatment to the users"));

            entity.Property(e => e.ProfessionalId).HasColumnName("professional_id");
            entity.Property(e => e.ProfessionalName)
                .HasMaxLength(45)
                .HasColumnName("professional_name");
        });

        modelBuilder.Entity<ProfessionalsSpeciality>(entity =>
        {
            entity.HasKey(e => e.ProfessionalsSpecialitiesId).HasName("PRIMARY");

            entity.ToTable("professionals_specialities", tb => tb.HasComment("Connector table for the healthcare professional and his specialities"));

            entity.HasIndex(e => e.HealthcareProfessionalId, "FK_healthcare_professional_specialities_idx");

            entity.HasIndex(e => e.SpecialityId, "FK_professionals_specialities_specialities_idx");

            entity.Property(e => e.ProfessionalsSpecialitiesId).HasColumnName("professionals_specialities_id");
            entity.Property(e => e.HealthcareProfessionalId).HasColumnName("healthcare_professional_id");
            entity.Property(e => e.SpecialityId).HasColumnName("speciality_id");

            entity.HasOne(d => d.HealthcareProfessional).WithMany(p => p.ProfessionalsSpecialities)
                .HasForeignKey(d => d.HealthcareProfessionalId)
                .HasConstraintName("FK_professionals_specialities_healthcare_professionals");

            entity.HasOne(d => d.Speciality).WithMany(p => p.ProfessionalsSpecialities)
                .HasForeignKey(d => d.SpecialityId)
                .HasConstraintName("FK_professionals_specialities_specialities");
        });

        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.HasKey(e => e.SpecialityId).HasName("PRIMARY");

            entity.ToTable("specialities", tb => tb.HasComment("Specialities of the healthcare practisners"));

            entity.Property(e => e.SpecialityId).HasColumnName("speciality_id");
            entity.Property(e => e.SpecialityName)
                .HasMaxLength(45)
                .HasColumnName("speciality_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users", tb => tb.HasComment("Users who can book appointment"));

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
