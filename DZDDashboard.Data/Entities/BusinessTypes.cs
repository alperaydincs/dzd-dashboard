namespace DZDDashboard.Data.Entities;

public abstract class NamedTypeEntity : AuditableEntity
{
    public int    Id   { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class AdditionalPaymentTypeEntity : NamedTypeEntity { }
public class DeductionTypeEntity         : NamedTypeEntity { }
public class ContractTypeEntity          : NamedTypeEntity { }
public class WorkModelEntity             : NamedTypeEntity { }
public class EducationLevelEntity        : NamedTypeEntity { }
public class DependentTypeEntity         : NamedTypeEntity { }
