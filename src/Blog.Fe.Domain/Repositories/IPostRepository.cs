using System.Threading;
using System.Threading.Tasks;
using Blog.Fe.Domain.Entities;

namespace Blog.Fe.Domain.Repositories;

public interface IPostRepository
{
	Task<Post[]> Get(CancellationToken cancellationToken = default);
	Task<Post> Get(int id, CancellationToken cancellationToken = default);
	Task<Post?> Find(string url, CancellationToken cancellationToken = default);
}
