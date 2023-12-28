
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"))
            .AddNpgSql(builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnectionPostgre"));
        builder.Services.AddHealthChecksUI().AddInMemoryStorage();
        // Add services to the container.
        builder.Services.AddRazorPages();

        var app = builder.Build();
        app.UseHealthChecks("/healthcheck", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse //nuget: AspNetCore.HealthChecks.UI.Client
        });

        //nuget: AspNetCore.HealthChecks.UI
        app.UseHealthChecksUI(options =>
        {
            options.UIPath = "/hc-ui";
            options.ApiPath = "/health-ui-api";
        });
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();

    }
}
