using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

public class MentorDb : DbContext
{
    public MentorDb()
    {
        Database.SetInitializer(new MigrateDatabaseToLatestVersion<MentorDb, MentorDbMigrationConfiguration>());
        Configuration.LazyLoadingEnabled = false;
        Configuration.ProxyCreationEnabled = false;
        Configuration.AutoDetectChangesEnabled = true;
        Configuration.ValidateOnSaveEnabled = true;
    }

    //REF: http://msdn.microsoft.com/en-us/data/jj819164.aspx
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Types().Configure(c => c.ToTable(c.ClrType.Name));
        //modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        modelBuilder.Properties<string>().Configure(c => c.IsUnicode(false));
    }

    public DbSet<Article> Articles { get; set; }

    public void Save<T>(T entity) where T : class
    {
        Entry(entity).State = EntityState.Modified;
    }

    public T Add<T>(T entity) where T : class
    {
        Entry(entity).State = EntityState.Added;
        return entity;
    }

    public void Delete<T>(T entity) where T : class
    {
        Entry(entity).State = EntityState.Deleted;
    }

    public void Commit()
    {
        SaveChanges();
    }
};

internal sealed class MentorDbMigrationConfiguration : DbMigrationsConfiguration<MentorDb>
{
    public MentorDbMigrationConfiguration()
    {
        AutomaticMigrationsEnabled = true;
        AutomaticMigrationDataLossAllowed = true;
    }

    protected override void Seed(MentorDb context)
    {
        //context.Logins.AddOrUpdate(
        //    x => x.Email,
        //    new[]
        //    {
        //        new Login {Email = "paul@tagovi.com", Password = "pw", IsActive = true},
        //    });
    }
};
