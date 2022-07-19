using proyectef;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using proyectef.Models;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<TareasContext>(p => p.UseInMemoryDatabase("TareasDB"));
builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas"));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) => 
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Database in memory: " + dbContext.Database.IsInMemory());
});

app.MapGet("/api/tareas", async([FromServices] TareasContext dbContext)=>
{
    return Results.Ok(dbContext.Tareas.Include(p => p.Categoria));
});

app.MapPost("/api/tareas", async([FromServices] TareasContext dbContext, [FromBody] Tarea tarea)=>
{
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;
    await dbContext.AddAsync(tarea);
    //await dbContext.Tareas.AddAsync(tarea);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/api/tareas/{id}", async([FromServices] TareasContext dbContext, [FromBody] Tarea tarea, [FromRoute] Guid id)=>
{
    var tareaActual = dbContext.Tareas.Find(id);
    if(tareaActual != null){
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.Prioridad = tarea.Prioridad;
        tareaActual.Descripcion = tarea.Descripcion;

        await dbContext.SaveChangesAsync();
            return Results.Ok();
    }
    return Results.NotFound();
});

app.MapDelete("/api/task/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id) => {
    var task = dbContext.Tareas.Find(id);

    if(task == null)
        return Results.NotFound("Task not found.");
        
    dbContext.Remove(task);
    await dbContext.SaveChangesAsync();

    return Results.Ok("Removed!");
});
app.Run();
