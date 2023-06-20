using System;

namespace Blog.Fe.Domain.Entities;

public abstract class BaseEntity
{
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset ChangedAt { get; set; }
}
