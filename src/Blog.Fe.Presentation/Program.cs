using Blog.Fe.Infrastructure.Extensions;
using Blog.Fe.Presentation.Authentication;
using Blog.Fe.Presentation.Authorization;
using Blog.Fe.Presentation.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services
	.AddBasicAuthentication(builder.Configuration)
	.AddLocalOriginPolicy();

builder.Services.AddLogging(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles();
app.UseSerilogRequestLogging();

if (!app.Environment.IsDevelopment())
{
	app.UseForwardedHeaders(new ForwardedHeadersOptions
	{
		ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
	});
	app.UseExceptionHandler("/shared/error");
	app.UseHsts();
}

app.UseRouting();
app.UseHttpMetrics();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app
	.MapMetrics()
	.RequireAuthorization(LocalOriginRequirement.Name);

app.MigrateUp();
app.Run();
