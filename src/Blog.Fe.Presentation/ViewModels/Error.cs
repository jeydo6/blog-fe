namespace Blog.Fe.Presentation.ViewModels;

public sealed class Error
{
	public string? RequestId { get; set; }

	public string Description { get; set; } = string.Empty;

	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
