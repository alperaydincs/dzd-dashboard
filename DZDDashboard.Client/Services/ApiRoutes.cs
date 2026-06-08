namespace DZDDashboard.Client.Services;

/// <summary>API route constants — single source of truth for all client→server URL paths.</summary>
public static class ApiRoutes
{
    public static class Users
    {
        public const string MyProfile      = "api/users/my-profile";
        public const string MyAvatar       = "api/users/my-avatar";
        public const string MyProfileAvatar = "api/users/my-profile/avatar";
        public const string MyContactInfo  = "api/users/my-profile/contact-info";
        public const string MyPaymentSummary = "api/users/my-profile/payment-summary";

        public static string All(int page, int pageSize)         => $"api/users?page={page}&pageSize={pageSize}";
        public static string Card(int userId)                    => $"api/users/{userId}/card";
        public static string SensitiveInfo(int userId)           => $"api/users/{userId}/sensitive-info";
        public static string Avatar(int userId)                  => $"api/users/{userId}/avatar";
        public static string BasicInfo(int userId)               => $"api/users/{userId}/basic-info";
        public static string Contacts(int userId)                => $"api/users/{userId}/contacts";
        public static string CitizenshipInfo(int userId)         => $"api/users/{userId}/citizenship-info";
        public static string AddressInfo(int userId)             => $"api/users/{userId}/address-info";
        public static string EducationInfo(int userId)           => $"api/users/{userId}/education-info";
        public static string Career(int userId)                  => $"api/users/{userId}/career";
        public static string OrganizationPosition(int userId)    => $"api/users/{userId}/organization-position";
        public static string EmergencyContacts(int userId)       => $"api/users/{userId}/emergency-contacts";
        public static string FamilyInfo(int userId)              => $"api/users/{userId}/family-info";

        public static string Payment(int userId)                            => $"api/users/{userId}/payment";
        public static string PaymentSalary(int userId)                      => $"api/users/{userId}/payment/salary";
        public static string PaymentSalaryRecord(int userId, int recordId)  => $"api/users/{userId}/payment/salary/{recordId}";
        public static string PaymentBenefits(int userId)                    => $"api/users/{userId}/payment/benefits";
        public static string PaymentBenefitRecord(int userId, int recordId) => $"api/users/{userId}/payment/benefits/{recordId}";
        public static string PaymentAdditional(int userId)                  => $"api/users/{userId}/payment/additional-payments";
        public static string PaymentAdditionalRecord(int userId, int paymentId) => $"api/users/{userId}/payment/additional-payments/{paymentId}";
    }

    public static class Organization
    {
        public const string Positions      = "api/organization/positions";
        public const string Companies      = "api/organization/companies";
        public const string Departments    = "api/organization/departments";
        public const string Teams          = "api/organization/teams";
        public const string WorkTypes      = "api/organization/worktypes";
        public const string Jobs           = "api/organization/jobs";
        public const string Grades         = "api/organization/grades";
        public const string PayrollLocations = "api/organization/payrolllocations";
        public const string UserGroups     = "api/organization/usergroups";
        public const string CareerPaths    = "api/organization/careerpaths";
        public const string CareerMapRules = "api/organization/careermaprules";

        public static string Position(int id)         => $"{Positions}/{id}";
        public static string Company(int id)          => $"{Companies}/{id}";
        public static string Department(int id)       => $"{Departments}/{id}";
        public static string Team(int id)             => $"{Teams}/{id}";
        public static string WorkType(int id)         => $"{WorkTypes}/{id}";
        public static string Job(int id)              => $"{Jobs}/{id}";
        public static string Grade(int id)            => $"{Grades}/{id}";
        public static string PayrollLocation(int id)  => $"{PayrollLocations}/{id}";
        public static string UserGroup(int id)  => $"{UserGroups}/{id}";
        public static string CareerPath(int id) => $"{CareerPaths}/{id}";
        public static string CareerMapRule(int id)    => $"{CareerMapRules}/{id}";
    }
}
