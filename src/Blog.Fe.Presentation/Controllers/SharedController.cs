using System.Diagnostics;
using Blog.Fe.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Fe.Presentation.Controllers;

public sealed class SharedController : Controller
{
	[Route("error")]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new Error
		{
			RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
		});
	}
}
