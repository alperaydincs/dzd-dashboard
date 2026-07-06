namespace DZDDashboard.Client.Services;

public static class ApiRoutes
{
    public static class Users
    {
        public const string MyProfile      = "api/users/my-profile";
        public const string MyAvatar       = "api/users/my-avatar";
        public const string MyProfileAvatar = "api/users/my-profile/avatar";
        public const string MyProfileAvatarColor = "api/users/my-profile/avatar-color";
        public const string MyContactInfo  = "api/users/my-profile/contact-info";
        public const string MyPayment        = "api/users/my-profile/payment";
        public const string MyDocuments      = "api/users/my-profile/documents";
        public const string MyCard               = "api/users/my-profile/card";
        public const string MySensitiveInfo      = "api/users/my-profile/sensitive-info";
        public const string MyEmergencyContacts  = "api/users/my-profile/emergency-contacts";
        public const string MyFamilyInfo         = "api/users/my-profile/family-info";
        public const string MyAddressInfo        = "api/users/my-profile/address-info";
        public const string MyEducationInfo      = "api/users/my-profile/education-info";

        public static string All(int page, int pageSize)         => $"api/users?page={page}&pageSize={pageSize}";
        public static string Search(string? query, int take = 20) => $"api/users/search?query={Uri.EscapeDataString(query ?? string.Empty)}&take={take}";
        public static string Card(int userId)                    => $"api/users/{userId}/card";
        public static string CardBySlug(string slug)             => $"api/users/by-slug/{Uri.EscapeDataString(slug)}/card";
        public static string SensitiveInfo(int userId)           => $"api/users/{userId}/sensitive-info";
        public static string Avatar(int userId)                  => $"api/users/{userId}/avatar";
        public static string BasicInfo(int userId)               => $"api/users/{userId}/basic-info";
        public static string Contacts(int userId)                => $"api/users/{userId}/contacts";
        public static string CitizenshipInfo(int userId)         => $"api/users/{userId}/citizenship-info";
        public static string AddressInfo(int userId)             => $"api/users/{userId}/address-info";
        public static string EducationInfo(int userId)           => $"api/users/{userId}/education-info";
        public static string CurrentPosition(int userId)         => $"api/users/{userId}/position-history/current";
        public static string Documents(int userId)               => $"api/users/{userId}/documents";
        public static string Document(int userId, int docId)     => $"api/users/{userId}/documents/{docId}";
        public static string DocumentContent(int userId, int docId) => $"api/users/{userId}/documents/{docId}/content";
        public static string MyDocumentContent(int docId)            => $"api/users/my-profile/documents/{docId}/content";
        public static string DocumentReview(int userId, int docId)  => $"api/users/{userId}/documents/{docId}/review";
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
        public static string PaymentDeductions(int userId)                  => $"api/users/{userId}/payment/deductions";
        public static string PaymentDeductionRecord(int userId, int deductionId) => $"api/users/{userId}/payment/deductions/{deductionId}";
    }

    public static class Organization
    {
        public const string Positions      = "api/organization/positions";
        public const string Companies      = "api/organization/companies";
        public const string Departments    = "api/organization/departments";
        public const string Teams          = "api/organization/teams";
        public const string Jobs           = "api/organization/jobs";
        public const string PayrollLocations = "api/organization/payrolllocations";
        public const string CareerPaths     = "api/organization/careerpaths";
        public const string CareerPathRules = "api/organization/careerpathrules";

        public static string Position(int id)         => $"{Positions}/{id}";
        public static string Company(int id)          => $"{Companies}/{id}";
        public static string Department(int id)       => $"{Departments}/{id}";
        public static string Team(int id)             => $"{Teams}/{id}";
        public static string Job(int id)              => $"{Jobs}/{id}";
        public static string PayrollLocation(int id)  => $"{PayrollLocations}/{id}";
        public static string CareerPath(int id)     => $"{CareerPaths}/{id}";
        public static string CareerPathRule(int id) => $"{CareerPathRules}/{id}";
    }

    public static class Onboarding
    {
        public const string Base = "api/onboarding";
        public const string DueSoonDocuments = "api/onboarding/due-soon-documents";

        public static string Process(int id)                    => $"{Base}/{id}";
        public static string Complete(int id)                   => $"{Base}/{id}/complete";
        public static string Cancel(int id)                     => $"{Base}/{id}/cancel";
        public static string ItemComplete(int id, int itemId)   => $"{Base}/{id}/items/{itemId}/complete";
        public static string ItemReopen(int id, int itemId)     => $"{Base}/{id}/items/{itemId}/reopen";
        public static string Documents(int id)                  => $"{Base}/{id}/documents";
        public static string DocumentUpload(int id, int docId)  => $"{Base}/{id}/documents/{docId}/upload";
        public static string Document(int id, int docId)        => $"{Base}/{id}/documents/{docId}";
        public static string DocumentApprove(int id, int docId) => $"{Base}/{id}/documents/{docId}/approve";
        public static string DocumentRequestCorrection(int id, int docId) => $"{Base}/{id}/documents/{docId}/request-correction";
        public static string DocumentReopen(int id, int docId)  => $"{Base}/{id}/documents/{docId}/reopen";
    }

    public static class ProcessTemplates
    {
        public const string Base = "api/process-templates";
        public static string List(string kind) => $"{Base}?kind={Uri.EscapeDataString(kind)}";
        public static string Item(int id) => $"{Base}/{id}";
    }

    public static class ChecklistTemplates
    {
        public const string Base = "api/checklist-templates";
        public static string List(int processTemplateId) => $"{Base}?processTemplateId={processTemplateId}";
        public static string Item(int id) => $"{Base}/{id}";
    }

    public static class DocumentTemplates
    {
        public const string Base = "api/document-templates";
        public static string List(int processTemplateId) => $"{Base}?processTemplateId={processTemplateId}";
        public static string Item(int id) => $"{Base}/{id}";
    }

    public static class MyOnboarding
    {
        public const string State          = "api/my-onboarding/state";
        public const string Documents      = "api/my-onboarding/documents";
        public static string Document(int docId)        => $"{Documents}/{docId}";
        public static string DocumentContent(int docId) => $"{Documents}/{docId}/content";
    }

    public static class Trainings
    {
        public const string MyProgress = "api/trainings/my-progress";
        public static string UserProgress(int userId) => $"api/trainings/users/{userId}/progress";
    }

    public static class Offboarding
    {
        public const string Base = "api/offboarding";
        public const string DueSoonDocuments = "api/offboarding/due-soon-documents";

        public static string Process(int id)                    => $"{Base}/{id}";
        public static string ItemComplete(int id, int itemId)   => $"{Base}/{id}/items/{itemId}/complete";
        public static string ItemReopen(int id, int itemId)     => $"{Base}/{id}/items/{itemId}/reopen";
        public static string Documents(int id)                  => $"{Base}/{id}/documents";
        public static string DocumentUpload(int id, int docId)  => $"{Base}/{id}/documents/{docId}/upload";
        public static string Document(int id, int docId)        => $"{Base}/{id}/documents/{docId}";
        public static string DocumentApprove(int id, int docId) => $"{Base}/{id}/documents/{docId}/approve";
        public static string DocumentRequestCorrection(int id, int docId) => $"{Base}/{id}/documents/{docId}/request-correction";
        public static string DocumentReopen(int id, int docId)  => $"{Base}/{id}/documents/{docId}/reopen";
    }
}
