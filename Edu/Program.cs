var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();
builder.Services.AddSomething();
builder.Services.Configure<RouteOptions>(options => options.ConstraintMap.Add("dickstring", typeof(DickConstraint)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/dick/{dickLen:dickstring(12345)=Smol}", async (HttpContext context, string dickLen) =>
{
    DickService service = app.Services.GetRequiredService<DickService>();
    await context.Response.WriteAsync(service.SendDick(dickLen + "cm"));
})
.WithName("GetDick");

app.Run();

class DickService
{
    public string SendDick(string? len)
    {
        return "BIG DICK " + len;
    }
}

public static class ServiceExtensions
{
    public static void AddSomething(this IServiceCollection service)
    {
        service.AddTransient<DickService>();
    }
}

class DickConstraint : IRouteConstraint
{

    string dickString;

    public DickConstraint(string dickString)
    {
        this.dickString = dickString;
    }

    public bool Match(HttpContext? httpContext,
         IRouter? route,
         string routeKey,
         RouteValueDictionary values,
         RouteDirection routeDirection)
    {
        return values[routeKey]?.ToString() == dickString;
    }
}