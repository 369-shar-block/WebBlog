using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WebBlog.Data;
using WebBlog.Models;
using WebBlog.Services;
using WebBlog.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = DataUtility.GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddDefaultUI().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//custom services
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IBlogPostService, BlogPostService>();
builder.Services.AddScoped<IEmailSender, EmailService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));



//google auth
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]?? Environment.GetEnvironmentVariable("ClientId")!;
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]?? Environment.GetEnvironmentVariable("ClientSecret")!;
});

builder.Services.AddMvc();

//add swagger
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StarLink Blog API",
        Version = "v1",
        Description = "Serve up Blog Data using .Net6 Apis",
        Contact = new OpenApiContact
        {
            Name = "Tushar",
            Email = "sharcoder@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/tushar-saini-948402223/")
        }
    });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));

});


var app = builder.Build();

//callong managedata
var scope = app.Services.CreateScope();
await DataUtility.ManageDataAsync(scope.ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PublicAPI v1");
    c.InjectStylesheet("/css/swagger.css");
    c.InjectJavascript("/js/swagger.js");

    c.DocumentTitle = "StarLink Blog Public API";
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "custom",
    pattern: "Content/{slug}",
    defaults: new { controller = "BlogPosts", action = "Details" });



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
