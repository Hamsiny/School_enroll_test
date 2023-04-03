using Microsoft.EntityFrameworkCore;
using UxtrataTask;
using UxtrataTask.Context;
using UxtrataTask.Filter;
using UxtrataTask.Middleware;
using UxtrataTask.Models;
using UxtrataTask.Repository;

var builder = WebApplication.CreateBuilder(args);

// add dbcontext
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"), 
        new MySqlServerVersion(new Version(8, 0, 26))));

// add repository
builder.Services.AddScoped<IGenericMySqlAccessRepository<Student>, GenericMySqlAccessRepository<Student>>();
builder.Services.AddScoped<IGenericMySqlAccessRepository<Course>, GenericMySqlAccessRepository<Course>>();
builder.Services.AddScoped<IGenericMySqlAccessRepository<CourseSelection>, GenericMySqlAccessRepository<CourseSelection>>();

// add auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// add filter
builder.Services.AddMvc(options =>
{
    options.Filters.Add(new ValidateModelAttribute());
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDeveloperExceptionPage();
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// add middleware
app.UseMiddleware<HttpExceptionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
