using RecipeBookAPI.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Diagnostics.Eventing.Reader;



//var MyAllowSpecificOrigins = "CORSPolicy";

var builder = WebApplication.CreateBuilder(args);

/*builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader().AllowAnyMethod()
                          .WithOrigins("http://localhost:3000",
                                              "https//appname.azurestaticapps.net");
                      });
}); */




builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder =>
    {
        builder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000", "https//appname.azurestaticapps.net");
    });
}); 

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerGeneratorOptions =>
{
    SwaggerGeneratorOptions.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ASP.NET REACT", Version = "v1"});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseSwagger();
app.UseSwaggerUI(SwaggerUIOptions =>
{
    SwaggerUIOptions.DocumentTitle = "ASP.NET REACT";
    SwaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API serving a post model.");
    SwaggerUIOptions.RoutePrefix=string.Empty;
});

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

app.MapGet("/get-all-posts", async () => await PostsRepository.GetPostsAsync()).WithTags("Posts endpoints");

app.MapGet("/get-post-by-id/{postId}", async (int postId)=> 
{
    Post onePost = await PostsRepository.GetPostByIdAsync(postId);
    if (onePost is not null)
        return Results.Ok(onePost);
    else
        return Results.BadRequest();
}).WithTags("Posts endpoints");

app.MapPost("/create-post", async (Post postToCreate) =>
{
    bool createSuccessful = await PostsRepository.CreatePostAsync(postToCreate);
    if (createSuccessful)
        return Results.Ok("Post create successfully");
    else
        return Results.BadRequest();
}).WithTags("Posts endpoints");

app.MapPut("/update-post", async (Post postToUpdate) =>
{
    bool updateSuccessful = await PostsRepository.UpdatePostAsync(postToUpdate);
    if (updateSuccessful)
        return Results.Ok("Post update successfully");
    else 
        return Results.BadRequest();
}).WithTags("Posts endpoints");

app.MapDelete("/delete-post-by-id/{postId}", async (int postId) =>
{
    bool deleteSuccessful = await PostsRepository.DeletePostAsync(postId);
    if (deleteSuccessful)
        return Results.Ok("Post delete successfully");
    else
        return Results.BadRequest();
}).WithTags("Posts endpoints");




app.UseAuthorization();

app.MapControllers();

app.Run();
