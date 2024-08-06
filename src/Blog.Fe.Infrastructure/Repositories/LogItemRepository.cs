using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Fe.Domain.Entities;
using Blog.Fe.Domain.Repositories;
using Blog.Fe.Infrastructure.DbContexts;
using LinqToDB.Data;

namespace Blog.Fe.Infrastructure.Repositories;

internal sealed class LogItemRepository : ILogItemRepository
{
    private static readonly BulkCopyOptions BulkCopyOptions = new BulkCopyOptions
    {
        BulkCopyType = BulkCopyType.ProviderSpecific
    };

    private readonly DbContext _db;
	
    public LogItemRepository(DbContext db)
        => _db = db;

    public Task Create(IEnumerable<LogItem> logItems, CancellationToken cancellationToken = default)
        => _db.LogItems.BulkCopyAsync(BulkCopyOptions, logItems, cancellationToken);
}
