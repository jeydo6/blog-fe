using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Fe.Domain.Entities;

namespace Blog.Fe.Domain.Repositories;

public interface ILogItemRepository
{
    Task Create(IEnumerable<LogItem> logItems, CancellationToken cancellationToken = default);
}
