using Microsoft.EntityFrameworkCore;

namespace RentalCars.Models;

public class RentalContext : DbContext
{
    public RentalContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "rental.db");
    }

    public DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Rentals> Rentals { get; set; }
    public DbSet<Car> Courses { get; set; }

    public string DbPath { get; }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}

public enum Status
{
    Active,
    Cancelled,
    Returned,
    Reserved
}

public class Rentals
{
    public int RentalsId { get; set; }
    public int CustomerId { get; set; }
    public int CarId { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public Status Status { get; set; }
    public int Price { get; set; }
    public int Mileage { get; set; }

    public Car Car { get; set; }
    public Customer Customer { get; set; }
}

public class Customer
{
    public int CustomerId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
}

public class Car
{
    public int CarId { get; set; }
    public string Model { get; set; }
    public int Price { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
}