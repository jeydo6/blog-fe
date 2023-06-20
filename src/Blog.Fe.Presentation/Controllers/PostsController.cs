using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blog.Fe.Domain.Repositories;
using Blog.Fe.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Fe.Presentation.Controllers;

[Route("posts")]
public sealed class PostsController : Controller
{
	private readonly IPostRepository _repository;
	
	public PostsController(IPostRepository repository)
		=> _repository = repository;

	public async Task<IActionResult> Index()
	{
		var posts = await _repository.Get();
		return View(posts
			.Where(p => p.State is 2)
			.Select(p => new Post
			{
				Url = p.Url,
				Title = p.Title,
				Description = p.Description,
				CreatedAt = p.CreatedAt.ToLocalTime(),
				ChangedAt = p.ChangedAt.ToLocalTime()
			})
			.ToArray());
	}

	[Route("{url}")]
	public async Task<IActionResult> Details(string url)
	{
		var post = await _repository.Find(url);
		return post is not null ?
			View(new Post
			{
				Url = post.Url,
				Title = post.Title,
				Description = post.Description,
				CreatedAt = post.CreatedAt.ToLocalTime(),
				ChangedAt = post.ChangedAt.ToLocalTime()
			}) :
			View("Error", new Error
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				Description = $"Post with {nameof(url)}='{url}' was not found."
			});
	}
}
