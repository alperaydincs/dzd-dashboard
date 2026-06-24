using DZDDashboard.Common.Validation;
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

        builder.Property(u => u.Email).HasMaxLength(ValidationConstants.MaxEmailLength);
        builder.Property(u => u.NormalizedEmail).HasMaxLength(ValidationConstants.MaxEmailLength);
        builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("IX_Users_NormalizedEmail");

        builder.Property(u => u.EntraObjectId).HasMaxLength(36);        builder.HasIndex(u => u.EntraObjectId)
               .IsUnique()
               .HasDatabaseName("IX_Users_EntraObjectId");

        builder.Property(u => u.FirstName).HasMaxLength(ValidationConstants.MaxNameLength);
        builder.Property(u => u.LastName).HasMaxLength(ValidationConstants.MaxNameLength);
        builder.HasIndex(u => new { u.LastName, u.FirstName }).HasDatabaseName("IX_Users_Name");

        builder.Property(u => u.Slug).IsRequired().HasMaxLength(ValidationConstants.MaxStandardLength);
        builder.HasIndex(u => u.Slug).IsUnique().HasDatabaseName("IX_Users_Slug");
        builder.Property(u => u.CompanyName).HasMaxLength(ValidationConstants.MaxStandardLength);

        builder.HasOne(u => u.ContractTypeRef)
               .WithMany()
               .HasForeignKey(u => u.ContractTypeId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(u => u.WorkModelRef)
               .WithMany()
               .HasForeignKey(u => u.WorkModelId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.Property(u => u.ApprovalProcessUnit).HasMaxLength(ValidationConstants.MaxEntityNameLength);

        builder.Property(u => u.PhoneNumber).HasMaxLength(ValidationConstants.MaxPhoneLength);
        builder.Property(u => u.PersonalEmail).HasMaxLength(ValidationConstants.MaxEmailLength);
        builder.Property(u => u.PersonalPhoneNumber).HasMaxLength(ValidationConstants.MaxPhoneLength);

        builder.Property(u => u.Gender).HasMaxLength(30);        builder.Property(u => u.DisabilityStatus).HasDefaultValue(false);
        builder.Property(u => u.DisabilityDegree).HasMaxLength(ValidationConstants.MaxNameLength);
        builder.Property(u => u.Nationality).HasMaxLength(ValidationConstants.MaxNameLength);
        builder.Property(u => u.CitizenshipNumber).HasMaxLength(ValidationConstants.MaxNumericIdentifierLength);

        builder.Property(u => u.MaritalStatus).HasMaxLength(ValidationConstants.MaxShortNameLength);
        builder.Property(u => u.SpouseFullName).HasMaxLength(ValidationConstants.MaxFullNameLength);

        builder.Property(u => u.LegalAddress).HasMaxLength(ValidationConstants.MaxAddressLength);
        builder.Property(u => u.CurrentAddress).HasMaxLength(ValidationConstants.MaxAddressLength);

        builder.Property(u => u.City).HasMaxLength(ValidationConstants.MaxNameLength);
        builder.Property(u => u.Country).HasMaxLength(ValidationConstants.MaxNameLength);

        builder.Property(u => u.BankName).HasMaxLength(ValidationConstants.MaxEntityNameLength);
        builder.Property(u => u.Iban).HasMaxLength(34);
        builder.Property(u => u.RegistrationNumber).HasMaxLength(ValidationConstants.MaxShortNameLength);

        builder.Property(u => u.CvFilePath).HasMaxLength(ValidationConstants.MaxAddressLength);
        builder.Property(u => u.EmployeeGroup).HasMaxLength(ValidationConstants.MaxNameLength);
        builder.Property(u => u.AutoEnrollmentPensionStatus).HasMaxLength(ValidationConstants.MaxShortNameLength);
        
        builder.Property(u => u.HasEmployerPension).HasDefaultValue(false);
        builder.Property(u => u.EmployerPensionEmployeeContribution).HasPrecision(18, 2);
        builder.Property(u => u.EmployerPensionEmployerContribution).HasPrecision(18, 2);
        builder.Property(u => u.HasPrivateHealthInsurance).HasDefaultValue(false);
        builder.Property(u => u.PrivateHealthInsuranceEmployeeCost).HasPrecision(18, 2);
        builder.Property(u => u.PrivateHealthInsuranceDependentCost).HasPrecision(18, 2);
        builder.Property(u => u.MealBenefitAmount).HasPrecision(18, 2);

        builder.Property(u => u.IsActive).HasDefaultValue(true);

        builder.Property(u => u.LifecycleStatus)
               .IsRequired()
               .HasMaxLength(ValidationConstants.MaxShortNameLength)
               .HasDefaultValue(DZDDashboard.Common.Constants.UserLifecycleStatuses.Active);

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

        builder.HasIndex(u => u.OrganizationPositionId)
               .IsUnique()
               .HasFilter("[OrganizationPositionId] IS NOT NULL")
               .HasDatabaseName("IX_Users_OrganizationPositionId");

        builder.HasOne(u => u.CareerPath)
               .WithMany()
               .HasForeignKey(u => u.CareerPathId)
               .OnDelete(DeleteBehavior.SetNull);

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
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.GradeHistories)
               .WithOne(g => g.User)
               .HasForeignKey(g => g.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}