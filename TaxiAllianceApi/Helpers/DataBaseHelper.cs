using Microsoft.EntityFrameworkCore;

namespace TaxiAllianceApi
{
    public class DataBaseHelper : DbContext
    {
        public DbSet<Role> Role { get; set; }
        public DbSet<HR_Department> HR_Department { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<Cabbie> Cabbie { get; set; }
        public DbSet<Lectures> Lectures { get; set; }
        public DbSet<Questions> Questions {get; set;}
        public DbSet<Test> Test { get; set; }
        public DbSet<Score> Score { get; set; }
        public DbSet<ResultModel> ResultModels { get; set; }


        public DataBaseHelper(DbContextOptions<DataBaseHelper> options)
            : base(options)
        {

        }

        public DataBaseHelper()
        {

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(@"Data Source=WIN-VINA1KDM9TU\QQQQEDR;Initial Catalog=Taxi_Alliance;Integrated Security=True");
            //optionsBuilder.UseSqlServer(@"Data Source=WIN-VINA1KDM9TU\QQQQEDR;Initial Catalog=Taxi_Alliance;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResultModel>(entity =>
            {
                entity.HasNoKey();
            });            
        }
    }
}

