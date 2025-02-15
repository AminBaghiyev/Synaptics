namespace Synaptics.Domain.Entities.BaseEntity;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}
