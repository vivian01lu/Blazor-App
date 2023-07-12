//This statement allows the app to use the new service.
using BlazingPizza.Data;
using BlazingPizza.Services;


var builder = WebApplication.CreateBuilder(args);

//initialized the PizzaStoreContext
// The first AddHttpClient statement allows the app to access HTTP commands. The app uses an HttpClient to get the JSON for pizza specials.
builder.Services.AddHttpClient();
// The second statement registers the new PizzaStoreContext and provides the filename for the SQLite database.
builder.Services.AddSqlite<PizzaStoreContext>("Data Source=pizza.db");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<OrderState>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");



// Initialize the database
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    if (db.Database.EnsureCreated())
    {
        SeedData.Initialize(db);
    }
}
//This change creates a database scope with the PizzaStoreContext. If there isn't a database already created,
//it calls the SeedData static class to create one.
app.Run();
