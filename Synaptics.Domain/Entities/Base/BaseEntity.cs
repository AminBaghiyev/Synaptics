﻿namespace Synaptics.Domain.Entities.Base;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsUpdated { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
