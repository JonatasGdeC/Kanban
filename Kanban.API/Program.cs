using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Kanban.DB.Banco;
using Kanban.Shared.Models.Models;
using Kanban.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<KanbanContext>((options) => {
  options
    .UseSqlServer(builder.Configuration["ConnectionStrings:KanbanDB"])
    .UseLazyLoadingProxies();
});
builder.Services.AddTransient<DAL<Boards>>();
builder.Services.AddTransient<DAL<Columns>>();
builder.Services.AddTransient<DAL<Subtasks>>();
builder.Services.AddTransient<DAL<Tasks>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors();
var app = builder.Build();

app.UseCors(options =>
{
  options.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();

});

app.UseStaticFiles();

app.AddEndPointsBoards();
app.AddEndPointColumns();
app.AddEndPointTasks();
app.AddEndPointSubTasks();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
