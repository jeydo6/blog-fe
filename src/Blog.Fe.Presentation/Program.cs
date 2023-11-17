using System;
using Blog.Fe.Infrastructure.Extensions;
using Blog.Fe.Presentation.Authentication;
using Blog.Fe.Presentation.Extensions;
using Blog.Fe.Presentation.Policies;
using Microsoft.AspNetCore.Authorization;
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

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services
	.AddAuthentication()
	.AddBasicAuthentication(o =>
	{
		var settings = builder.Configuration.Get<BasicAuthenticationSettings>();
        ArgumentException.ThrowIfNullOrEmpty(settings.UserName);
        ArgumentException.ThrowIfNullOrEmpty(settings.Password);
		o.UserName = settings.UserName;
		o.Password = settings.Password;
	});

builder.Services.AddSerilog(cfg => cfg
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.WriteTo.Console()
);

builder.Services
	.AddSingleton<IAuthorizationHandler, LocalOriginHandler>()
	.AddAuthorization(o => o
		.AddPolicy(LocalOriginRequirement.Name, p =>
		{
			p.AddRequirements(new LocalOriginRequirement());
			p.AddAuthenticationSchemes(BasicAuthenticationDefaults.AuthenticationScheme);
		}));

var app = builder.Build();

app.UseSerilogRequestLogging();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/shared/error");
	app.UseHsts();
}

if (app.Environment.IsStaging())
{
	app.UseForwardedHeaders(new ForwardedHeadersOptions
	{
		ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
	});
}
else if (app.Environment.IsProduction())
{
	app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpMetrics();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app
	.MapMetrics()
	.RequireAuthorization(LocalOriginRequirement.Name);

app.MigrateUp();
app.Run();
