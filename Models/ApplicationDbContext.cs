using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext:DbContext{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){
    
    }
    public DbSet<Employee> Employes { get; set; }
    public DbSet<Users> Users { get; set; }

    internal object Find(Guid id)
    {
        throw new NotImplementedException();
    }
}
