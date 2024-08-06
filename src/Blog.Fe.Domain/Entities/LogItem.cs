using Blog.Fe.Domain.Enums;

namespace Blog.Fe.Domain.Entities;

public sealed class LogItem : BaseEntity
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Properties { get; set; } = string.Empty;
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}
