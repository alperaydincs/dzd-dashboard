namespace DZDDashboard.Api.Validators;

internal static class ValidationMessages
{
    internal const string WorkEmailInvalid           = "Work email is not a valid email address.";
    internal const string PersonalEmailInvalid       = "Personal email is not a valid email address.";
    internal const string WorkPhoneInvalid           = "Work phone number is invalid.";
    internal const string PersonalPhoneInvalid       = "Personal phone number is invalid.";
    internal const string EmergencyPhoneInvalid      = "Emergency contact phone number is invalid.";
    internal const string ChildFullNameRequired       = "Child full name is required.";
    internal const string EmergencyFullNameRequired   = "Emergency contact full name is required.";
    internal const string EmergencyPhoneRequired      = "Emergency contact phone number is required.";
    internal const string EmergencyRelationRequired   = "Relationship is required.";
    internal const string EducationLevelRequired      = "Education level is required.";
    internal const string EducationInstitutionRequired = "Institution is required.";

    // Payment screen
    internal const string CurrencyInvalid                = "Currency must be one of the supported codes (TRY, USD, EUR).";
    internal const string SalaryPeriodInvalid            = "Salary period must be one of: Monthly, Weekly, Hourly, Yearly.";
    internal const string SalaryAmountInvalid            = "Salary amount must be greater than zero.";
    internal const string SalaryStartDateRequired        = "Salary start date is required.";
    internal const string BenefitTypeInvalid             = "Benefit type is not recognised.";
    internal const string BenefitPayerInvalid            = "Payer must be either Employer or Employee.";
    internal const string BenefitAmountInvalid           = "Benefit amount must be greater than zero.";
    internal const string BenefitStartDateRequired       = "Benefit start date is required.";
    internal const string DependentTypeRequired          = "Dependent type is required.";
    internal const string DependentAmountInvalid         = "Dependent amount must be greater than zero.";
    internal const string AdditionalPaymentTypeInvalid   = "Additional payment type is not recognised.";
    internal const string AdditionalPaymentAmountInvalid = "Additional payment amount must be greater than zero.";
    internal const string AdditionalPaymentPeriodInvalid = "Period must be one of: OneTime, Monthly, Weekly.";
}
