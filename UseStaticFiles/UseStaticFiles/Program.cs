using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDirectoryBrowser();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
        @"C:\ProgramData\Schneider Electric\Power Operation\v2021\Data\waveformdb"),
    RequestPath = "/StaticFiles",
    EnableDirectoryBrowsing = true
});


// Set up custom content types - associating file extension to MIME type
var provider = new FileExtensionContentTypeProvider();
// Add new mappings
provider.Mappings[".cfg"] = "application/x-msdownload";
provider.Mappings[".dat"] = "application/x-msdownload";


app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();