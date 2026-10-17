using Microsoft.EntityFrameworkCore;
using PawPaw.Core.Appointments;
using PawPaw.Core.Clinics;
using PawPaw.Core.Common;
using PawPaw.Core.Files;
using PawPaw.Core.Notifications;
using PawPaw.Core.Pets;
using PawPaw.Core.Users;
using PawPaw.Core.Vaccinations;
using PawPaw.Core.Veterinarians;

namespace PawPaw.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<UserAccount> Users => Set<UserAccount>();
  public DbSet<Pet> Pets => Set<Pet>();
  public DbSet<PetProfile> PetProfiles => Set<PetProfile>();
  public DbSet<Vaccination> Vaccinations => Set<Vaccination>();
  public DbSet<Clinic> Clinics => Set<Clinic>();
  public DbSet<Veterinarian> Veterinarians => Set<Veterinarian>();
  public DbSet<Appointment> Appointments => Set<Appointment>();
  public DbSet<Notification> Notifications => Set<Notification>();
  public DbSet<FileAsset> Files => Set<FileAsset>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<UserAccount>(entity =>
    {
      entity.ToTable("users");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.Email).HasMaxLength(320).IsRequired();
      entity.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
      entity.HasIndex(x => x.Email).IsUnique();
    });

    modelBuilder.Entity<Pet>(entity =>
    {
      entity.ToTable("pets");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
      entity.Property(x => x.Species).HasMaxLength(80).IsRequired();
      entity.Property(x => x.Breed).HasMaxLength(120);
      entity.HasOne(x => x.Owner).WithMany(x => x.Pets).HasForeignKey(x => x.UserAccountId).OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<PetProfile>(entity =>
    {
      entity.ToTable("pet_profiles");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.WeightKg).HasPrecision(8, 2);
      entity.Property(x => x.Biography).HasMaxLength(2000);
      entity.Property(x => x.MedicalNotes).HasMaxLength(4000);
      entity.HasIndex(x => x.PetId).IsUnique();
      entity.HasOne(x => x.Pet).WithOne(x => x.Profile).HasForeignKey<PetProfile>(x => x.PetId).OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<Vaccination>(entity =>
    {
      entity.ToTable("vaccinations");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.VaccineName).HasMaxLength(200).IsRequired();
      entity.Property(x => x.Notes).HasMaxLength(2000);
      entity.HasOne(x => x.Pet).WithMany(x => x.Vaccinations).HasForeignKey(x => x.PetId).OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<Clinic>(entity =>
    {
      entity.ToTable("clinics");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
      entity.Property(x => x.Address).HasMaxLength(500);
      entity.Property(x => x.PhoneNumber).HasMaxLength(50);
    });

    modelBuilder.Entity<Veterinarian>(entity =>
    {
      entity.ToTable("veterinarians");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.FullName).HasMaxLength(200).IsRequired();
      entity.Property(x => x.LicenseNumber).HasMaxLength(100);
      entity.HasOne(x => x.Clinic).WithMany(x => x.Veterinarians).HasForeignKey(x => x.ClinicId).OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<Appointment>(entity =>
    {
      entity.ToTable("appointments");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.Reason).HasMaxLength(1000).IsRequired();
      entity.HasOne(x => x.Pet).WithMany(x => x.Appointments).HasForeignKey(x => x.PetId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(x => x.Clinic).WithMany().HasForeignKey(x => x.ClinicId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(x => x.Veterinarian).WithMany(x => x.Appointments).HasForeignKey(x => x.VeterinarianId).OnDelete(DeleteBehavior.SetNull);
      entity.HasIndex(x => x.ScheduledAtUtc);
    });

    modelBuilder.Entity<Notification>(entity =>
    {
      entity.ToTable("notifications");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.Title).HasMaxLength(250).IsRequired();
      entity.Property(x => x.Body).HasMaxLength(4000).IsRequired();
      entity.HasOne(x => x.UserAccount).WithMany().HasForeignKey(x => x.UserAccountId).OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<FileAsset>(entity =>
    {
      entity.ToTable("files");
      entity.HasKey(x => x.Id);
      entity.Property(x => x.StorageKey).HasMaxLength(500).IsRequired();
      entity.Property(x => x.OriginalFileName).HasMaxLength(255).IsRequired();
      entity.Property(x => x.ContentType).HasMaxLength(150).IsRequired();
      entity.HasIndex(x => x.StorageKey).IsUnique();
      entity.HasOne(x => x.UserAccount).WithMany().HasForeignKey(x => x.UserAccountId).OnDelete(DeleteBehavior.SetNull);
      entity.HasOne(x => x.Pet).WithMany(x => x.Files).HasForeignKey(x => x.PetId).OnDelete(DeleteBehavior.SetNull);
    });
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    var now = DateTimeOffset.UtcNow;

    foreach (var entry in ChangeTracker.Entries<EntityBase>())
    {
      if (entry.State == EntityState.Added)
      {
        entry.Entity.CreatedAtUtc = now;
        entry.Entity.UpdatedAtUtc = now;
      }
      else if (entry.State == EntityState.Modified)
      {
        entry.Entity.UpdatedAtUtc = now;
      }
    }

    return await base.SaveChangesAsync(cancellationToken);
  }
}
