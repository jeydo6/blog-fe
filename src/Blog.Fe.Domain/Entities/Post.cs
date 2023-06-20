namespace Blog.Fe.Domain.Entities;

public sealed class Post : BaseEntity
{
	public int Id { get; set; }
	public string Url { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int State { get; set; }
}
