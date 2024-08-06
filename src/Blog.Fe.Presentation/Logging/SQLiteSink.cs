using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Fe.Domain.Repositories;
using Blog.Fe.Presentation.Extensions;
using Serilog.Core;
using Serilog.Events;

namespace Blog.Fe.Presentation.Logging;

internal sealed class SQLiteSink : IBatchedLogEventSink
{
    private readonly ILogItemRepository _repository;
    public SQLiteSink(ILogItemRepository repository) 
        => _repository = repository;

    public Task EmitBatchAsync(IReadOnlyCollection<LogEvent> batch)
        => _repository.Create(batch.Map());
}
