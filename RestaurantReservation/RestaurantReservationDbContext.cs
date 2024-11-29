using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation
{
    public class RestaurantReservationDbContext : DbContext
    {
       

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Table> Tables { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            //var config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
            //    .Build();

            //var connectionString = config.GetSection("constr").Value;
            optionsBuilder.ConfigureWarnings(warnings =>
        warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            optionsBuilder.UseSqlServer("Server=DESKTOP-SF7GKJ8\\SQLEXPRESS;Database=RestaurantReservationCore;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;"
                
               , sqlOptions=>sqlOptions.CommandTimeout(200)
                );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Order>()
       .HasOne(o => o.Reservation)
       .WithMany(r => r.Orders)
       .HasForeignKey(o => o.ReservationId)
       .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany(mi => mi.OrderItems)
                .HasForeignKey(oi => oi.ItemId)
                .OnDelete(DeleteBehavior.Restrict); 
        
        modelBuilder.Entity<Reservation>(b =>
            {
                b.Property<int>("ReservationId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationId"));

                b.Property<int?>("CustomerId").HasColumnType("int");
                b.Property<int>("PartySize").HasColumnType("int");
                b.Property<DateTime>("ReservationDate").HasColumnType("datetime2");
                b.Property<int?>("RestaurantId").HasColumnType("int");
                b.Property<int?>("TableId").HasColumnType("int");

                b.HasKey("ReservationId");

                b.HasIndex("CustomerId");
                b.HasIndex("RestaurantId");
                b.HasIndex("TableId");

                b.HasOne<Customer>()
                    .WithMany(c => c.Reservations)
                    .HasForeignKey("CustomerId")
                    .OnDelete(DeleteBehavior.SetNull); 

                b.HasOne<Restaurant>()
                    .WithMany(r => r.Reservations)
                    .HasForeignKey("RestaurantId")
                    .OnDelete(DeleteBehavior.SetNull);  

                b.HasOne<Table>()
                    .WithMany(t => t.Reservations)
                    .HasForeignKey("TableId")
                    .OnDelete(DeleteBehavior.SetNull);  

                b.ToTable("Reservations");

               
            });
        }
      







    }
}
