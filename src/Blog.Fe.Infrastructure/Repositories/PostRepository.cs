using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Fe.Domain.Entities;
using Blog.Fe.Domain.Repositories;
using Blog.Fe.Infrastructure.DataContexts;
using LinqToDB;

namespace Blog.Fe.Infrastructure.Repositories;

internal sealed class PostRepository : IPostRepository
{
	private readonly BlogFeDataContext _db;
	
	public PostRepository(BlogFeDataContext db) => _db = db;

	public Task<Post[]> Get(CancellationToken cancellationToken = default)
		=> _db.Posts.ToArrayAsync(cancellationToken);

	public async Task<Post> Get(int id, CancellationToken cancellationToken = default)
	{
		var query =
			from p in _db.Posts
			where p.Id == id
			select p;
		var post = await query.FirstOrDefaultAsync(cancellationToken);

		ArgumentNullException.ThrowIfNull(post);
		return post;
	}

	public Task<Post?> Find(string url, CancellationToken cancellationToken = default)
	{
		var query =
			from p in _db.Posts
			where p.Url == url
			select p;
		return query.FirstOrDefaultAsync(cancellationToken);
	}
}
