using System;

namespace Blog.Fe.Presentation.ViewModels;

public sealed class Post
{
	public string Url { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset ChangedAt { get; set; }
}
