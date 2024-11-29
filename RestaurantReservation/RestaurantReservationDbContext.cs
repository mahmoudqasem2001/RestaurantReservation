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

            modelBuilder.HasDbFunction(() => CalculateRestaurantRevenue(default))
        .HasName("CalculateRestaurantRevenue");

            modelBuilder.Entity<ReservationDetail>().HasNoKey().ToView("ReservationDetails");
            modelBuilder.Entity<EmployeeDetail>().HasNoKey().ToView("EmployeeDetails");



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

                // Seed Customers
                modelBuilder.Entity<Customer>().HasData(
                    new Customer { CustomerId = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", PhoneNumber = "123-456-7890" },
                    new Customer { CustomerId = 2, FirstName = "Bob", LastName = "Johnson", Email = "bob@example.com", PhoneNumber = "234-567-8901" },
                    new Customer { CustomerId = 3, FirstName = "Charlie", LastName = "Brown", Email = "charlie@example.com", PhoneNumber = "345-678-9012" },
                    new Customer { CustomerId = 4, FirstName = "Diana", LastName = "White", Email = "diana@example.com", PhoneNumber = "456-789-0123" },
                    new Customer { CustomerId = 5, FirstName = "Eve", LastName = "Black", Email = "eve@example.com", PhoneNumber = "567-890-1234" }
                );

                // Seed Restaurants
                modelBuilder.Entity<Restaurant>().HasData(
                    new Restaurant { RestaurantId = 1, Name = "The Gourmet Spot", Address = "123 Main St", PhoneNumber = "111-222-3333", OpeningHours = "9 AM - 10 PM" },
                    new Restaurant { RestaurantId = 2, Name = "Bistro Delight", Address = "456 Maple Ave", PhoneNumber = "222-333-4444", OpeningHours = "8 AM - 9 PM" },
                    new Restaurant { RestaurantId = 3, Name = "Pizza Paradise", Address = "789 Elm Rd", PhoneNumber = "333-444-5555", OpeningHours = "11 AM - 11 PM" },
                    new Restaurant { RestaurantId = 4, Name = "Burger Haven", Address = "101 Pine Blvd", PhoneNumber = "444-555-6666", OpeningHours = "10 AM - 8 PM" },
                    new Restaurant { RestaurantId = 5, Name = "Salad Bar", Address = "202 Oak Ln", PhoneNumber = "555-666-7777", OpeningHours = "10 AM - 7 PM" }
                );

                // Seed Tables
                modelBuilder.Entity<Table>().HasData(
                    new Table { TableId = 1, RestaurantId = 1, Capacity = 4 },
                    new Table { TableId = 2, RestaurantId = 1, Capacity = 6 },
                    new Table { TableId = 3, RestaurantId = 2, Capacity = 4 },
                    new Table { TableId = 4, RestaurantId = 2, Capacity = 8 },
                    new Table { TableId = 5, RestaurantId = 3, Capacity = 2 }
                );

                // Seed Employees
                modelBuilder.Entity<Employee>().HasData(
                    new Employee { EmployeeId = 1, RestaurantId = 1, FirstName = "John", LastName = "Doe", Position = "Manager" },
                    new Employee { EmployeeId = 2, RestaurantId = 2, FirstName = "Sara", LastName = "Lee", Position = "Chef" },
                    new Employee { EmployeeId = 3, RestaurantId = 3, FirstName = "Mike", LastName = "Smith", Position = "Waiter" },
                    new Employee { EmployeeId = 4, RestaurantId = 4, FirstName = "Anna", LastName = "Brown", Position = "Waiter" },
                    new Employee { EmployeeId = 5, RestaurantId = 5, FirstName = "Tom", LastName = "White", Position = "Cleaner" }
                );

                // Seed MenuItems
                modelBuilder.Entity<MenuItem>().HasData(
                    new MenuItem { ItemId = 1, RestaurantId = 1, Name = "Burger", Description = "Juicy beef burger", Price = 9.99m },
                    new MenuItem { ItemId = 2, RestaurantId = 2, Name = "Pizza", Description = "Cheese and pepperoni pizza", Price = 12.99m },
                    new MenuItem { ItemId = 3, RestaurantId = 3, Name = "Pasta", Description = "Creamy Alfredo pasta", Price = 10.99m },
                    new MenuItem { ItemId = 4, RestaurantId = 4, Name = "Salad", Description = "Fresh garden salad", Price = 7.99m },
                    new MenuItem { ItemId = 5, RestaurantId = 5, Name = "Soup", Description = "Hot tomato soup", Price = 6.99m }
                );

                // Seed Reservations
                modelBuilder.Entity<Reservation>().HasData(
                    new Reservation { ReservationId = 1, CustomerId = 1, RestaurantId = 1, TableId = 1, ReservationDate = new DateTime(2024, 12, 1), PartySize = 4 },
                    new Reservation { ReservationId = 2, CustomerId = 2, RestaurantId = 2, TableId = 3, ReservationDate = new DateTime(2024, 12, 2), PartySize = 2 },
                    new Reservation { ReservationId = 3, CustomerId = 3, RestaurantId = 3, TableId = 5, ReservationDate = new DateTime(2024, 12, 3), PartySize = 2 },
                    new Reservation { ReservationId = 4, CustomerId = 4, RestaurantId = 4, TableId = 2, ReservationDate = new DateTime(2024, 12, 4), PartySize = 6 },
                    new Reservation { ReservationId = 5, CustomerId = 5, RestaurantId = 5, TableId = 4, ReservationDate = new DateTime(2024, 12, 5), PartySize = 8 }
                );

                // Seed Orders
                modelBuilder.Entity<Order>().HasData(
                    new Order { OrderId = 1, ReservationId = 1, EmployeeId = 1, OrderDate = new DateTime(2024, 12, 1), TotalAmount = 39.96m },
                    new Order { OrderId = 2, ReservationId = 2, EmployeeId = 2, OrderDate = new DateTime(2024, 12, 2), TotalAmount = 25.98m },
                    new Order { OrderId = 3, ReservationId = 3, EmployeeId = 3, OrderDate = new DateTime(2024, 12, 3), TotalAmount = 21.98m },
                    new Order { OrderId = 4, ReservationId = 4, EmployeeId = 4, OrderDate = new DateTime(2024, 12, 4), TotalAmount = 63.94m },
                    new Order { OrderId = 5, ReservationId = 5, EmployeeId = 5, OrderDate = new DateTime(2024, 12, 5), TotalAmount = 111.92m }
                );

                // Seed OrderItems
                modelBuilder.Entity<OrderItem>().HasData(
                    new OrderItem { OrderItemId = 1, OrderId = 1, ItemId = 1, Quantity = 4 },
                    new OrderItem { OrderItemId = 2, OrderId = 2, ItemId = 2, Quantity = 2 },
                    new OrderItem { OrderItemId = 3, OrderId = 3, ItemId = 3, Quantity = 2 },
                    new OrderItem { OrderItemId = 4, OrderId = 4, ItemId = 4, Quantity = 6 },
                    new OrderItem { OrderItemId = 5, OrderId = 5, ItemId = 5, Quantity = 8 }
                );

            });
        }

        public static decimal CalculateRestaurantRevenue(int restaurantId)
        {
            throw new NotSupportedException("This method is intended for use with database queries only.");
        }

     

    }
}
