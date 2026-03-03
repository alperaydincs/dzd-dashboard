using DZDDashboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DZDDashboard.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username).IsRequired().HasMaxLength(256);
        builder.Property(u => u.NormalizedUsername).IsRequired().HasMaxLength(256);
        builder.Property(u => u.Email).HasMaxLength(256); 
        builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
        builder.HasIndex(u => u.NormalizedUsername).IsUnique().HasDatabaseName("IX_Users_NormalizedUsername");
        builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("IX_Users_NormalizedEmail");

        builder.Property(u => u.FirstName).HasMaxLength(100);
        builder.Property(u => u.LastName).HasMaxLength(100);
        builder.Property(u => u.CompanyName).HasMaxLength(200);

        builder.Property(u => u.ContractType).HasMaxLength(50);
        
        builder.Property(u => u.WorkModel).HasMaxLength(50);
        builder.Property(u => u.UnitName).HasMaxLength(200); 

        builder.Property(u => u.ApprovalProcessUnit).HasMaxLength(150);

        builder.Property(u => u.PhoneNumber).HasMaxLength(20); 
        builder.Property(u => u.PersonalEmail).HasMaxLength(256);
        builder.Property(u => u.PersonalPhoneNumber).HasMaxLength(20);

        builder.Property(u => u.Gender).HasMaxLength(30);
        builder.Property(u => u.DisabilityStatus).HasDefaultValue(false);
        builder.Property(u => u.DisabilityDegree).HasMaxLength(100);
        builder.Property(u => u.Nationality).HasMaxLength(100);
        builder.Property(u => u.CitizenshipNumber).HasMaxLength(20); 

        builder.Property(u => u.EducationStatus).HasMaxLength(50);
        builder.Property(u => u.HighestEducationLevel).HasMaxLength(50);
        builder.Property(u => u.HighSchoolName).HasMaxLength(200);
        builder.Property(u => u.AssociateDegreeUniversityName).HasMaxLength(200);
        builder.Property(u => u.AssociateDegreeProgramName).HasMaxLength(200);
        builder.Property(u => u.BachelorsUniversityName).HasMaxLength(200);
        builder.Property(u => u.BachelorsProgramName).HasMaxLength(200); 
        builder.Property(u => u.MastersUniversityName).HasMaxLength(200); 
        builder.Property(u => u.MastersProgramName).HasMaxLength(200); 
        builder.Property(u => u.DoctorateUniversityName).HasMaxLength(200);
        builder.Property(u => u.DoctorateProgramName).HasMaxLength(200);
        builder.Property(u => u.DoctoratePhdStatus).HasMaxLength(100); 

        builder.Property(u => u.EmergencyContactFullName).HasMaxLength(200);
        builder.Property(u => u.EmergencyContactRelationship).HasMaxLength(100);
        builder.Property(u => u.EmergencyContactPhoneNumber).HasMaxLength(20);

        builder.Property(u => u.MaritalStatus).HasMaxLength(50);
        builder.Property(u => u.SpouseFullName).HasMaxLength(200);

        builder.Property(u => u.LegalAddress).HasMaxLength(500);
        builder.Property(u => u.CurrentAddress).HasMaxLength(500);
        
        builder.Property(u => u.City).HasMaxLength(100);
        builder.Property(u => u.Country).HasMaxLength(100);

        builder.Property(u => u.BankName).HasMaxLength(150);
        builder.Property(u => u.Iban).HasMaxLength(34); 

        builder.Property(u => u.RegistrationNumber).HasMaxLength(50);
        
        builder.Property(u => u.CvFilePath).HasMaxLength(500);
        builder.Property(u => u.EmployeeGroup).HasMaxLength(100); 

        builder.Property(u => u.AutoEnrollmentPensionStatus).HasMaxLength(50);
        
        builder.Property(u => u.HasEmployerPension).HasDefaultValue(false);
        builder.Property(u => u.EmployerPensionEmployeeContribution).HasPrecision(18, 2);
        builder.Property(u => u.EmployerPensionEmployerContribution).HasPrecision(18, 2);
        builder.Property(u => u.HasPrivateHealthInsurance).HasDefaultValue(false);
        builder.Property(u => u.PrivateHealthInsuranceEmployeeCost).HasPrecision(18, 2);
        builder.Property(u => u.PrivateHealthInsuranceDependentCost).HasPrecision(18, 2);
        builder.Property(u => u.MealBenefitAmount).HasPrecision(18, 2);

        builder.Property(u => u.IsActive).HasDefaultValue(true);

        builder.HasOne(u => u.Team)
               .WithMany(t => t.Users)
               .HasForeignKey(u => u.TeamId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(u => u.UserGroup)
               .WithMany()
               .HasForeignKey(u => u.UserGroupId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(u => u.Job)
               .WithMany(j => j.Users)
               .HasForeignKey(u => u.JobId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(u => u.Department)
               .WithMany(d => d.Users)
               .HasForeignKey(u => u.DepartmentId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(u => u.PayrollLocation)
               .WithMany()
               .HasForeignKey(u => u.PayrollLocationId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.TargetEfforts)
               .WithOne(te => te.User)
               .HasForeignKey(te => te.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.UserTrainings)
               .WithOne(ut => ut.User)
               .HasForeignKey(ut => ut.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.ExCompanyHistories)
               .WithOne(ech => ech.User)
               .HasForeignKey(ech => ech.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.ReportsTo)
               .WithMany(u => u.Subordinates)
               .HasForeignKey(u => u.ReportsToId)
               .OnDelete(DeleteBehavior.Restrict); 

        builder.HasOne(u => u.ModifiedBy)
               .WithMany()
               .HasForeignKey(u => u.ModifiedById)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Avatar)
               .WithOne(a => a.User)
               .HasForeignKey<UserAvatar>(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Children)
               .WithOne(c => c.User)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade); 

        builder.HasMany(u => u.SalaryHistories)
               .WithOne(s => s.User)
               .HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Cascade); 

        builder.HasMany(u => u.GradeHistories)
               .WithOne(g => g.User)
               .HasForeignKey(g => g.UserId)
               .OnDelete(DeleteBehavior.Cascade); 
    }
}