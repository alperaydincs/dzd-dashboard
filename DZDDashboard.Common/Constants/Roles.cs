namespace DZDDashboard.Common.Constants;

public static class Roles
{
    public const string Admin     = "Admin";
    public const string Hr        = "HR";
    public const string AdminOrHr = $"{Admin},{Hr}";
    public const string AdminOrHrPolicy = "AdminOrHr";
    public const string SensitiveDataPolicy = "SensitiveEmployeeData";
}
