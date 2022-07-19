using Microsoft.EntityFrameworkCore;
using proyectef.Models;

namespace proyectef;
public class TareasContext: DbContext
{
    public DbSet<Categoria> Categorias {get;set;}
    public DbSet<Tarea> Tareas {get;set;}
    public TareasContext(DbContextOptions<TareasContext> options) :base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        List<Categoria> categoriasInit = new List<Categoria>();
        categoriasInit.Add(new Categoria(){
            CategoriaId = Guid.Parse("fad9335a-270f-4340-b0f9-b156d91f0a43"), 
            Nombre = "Actividades Pendientes",
            Peso = 20
        });
        categoriasInit.Add(new Categoria(){
            CategoriaId = Guid.Parse("fad9335a-270f-4340-b0f9-b156d91f0a02"), 
            Nombre = "Actividades Personales",
            Peso = 50
        });        

        modelBuilder.Entity<Categoria>(categoria =>
        {
            categoria.ToTable("Categoria");
            categoria.HasKey(p => p.CategoriaId);
            categoria.Property(p=> p.Nombre).IsRequired().HasMaxLength(150);
            categoria.Property(p=> p.Descripcion).IsRequired(false);
            categoria.Property(p => p.Peso);
            categoria.HasData(categoriasInit);
        });

        List<Tarea> tareasInit = new List<Tarea>();
        tareasInit.Add(new Tarea(){
            TareaId = Guid.Parse("fad9335a-270f-4340-b0f9-b156d91f0a11"), CategoriaId = Guid.Parse("fad9335a-270f-4340-b0f9-b156d91f0a43"), Prioridad = Priority.Medium, Titulo = "Pago de servicios publicos", FechaCreacion = DateTime.Now
        });
        tareasInit.Add(new Tarea(){
            TareaId = Guid.Parse("fad9335a-270f-4340-b0f9-b156d91f0a12"), CategoriaId = Guid.Parse("fad9335a-270f-4340-b0f9-b156d91f0a02"), Prioridad = Priority.Low, Titulo = "Terminar de ver película en Netflix", FechaCreacion = DateTime.Now
        });        

        modelBuilder.Entity<Tarea>(tarea => 
        {
            tarea.ToTable("Tarea");
            tarea.HasKey(p => p.TareaId);
            tarea.HasOne(p => p.Categoria).WithMany(p => p.Tareas).HasForeignKey(p=> p.CategoriaId);
            tarea.Property(p => p.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(p => p.Descripcion).IsRequired(false);
            tarea.Property(p => p.Prioridad);
            tarea.Property(p => p.FechaCreacion);
            tarea.Ignore(p => p.Resumen);
            tarea.HasData(tareasInit);
        });       
        
    }
}