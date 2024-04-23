namespace RentalCars.Models;

public static class DbInitializer
{
    public static void Initialize(RentalContext context)
    {
        context.Database.EnsureCreated();

        if (context.Customers.Any()) return;

        var customers = new Customer[]
        {
            new()
            {
                FirstName = "Carson", LastName = "Alexander", Email = "carson@example.com",
                RegistrationDate = DateTime.Parse("2022-09-01")
            },
            new()
            {
                FirstName = "Meredith", LastName = "Alonso", Email = "meredith@example.com",
                RegistrationDate = DateTime.Parse("2022-09-01")
            }
        };
        foreach (var c in customers) context.Customers.Add(c);
        context.SaveChanges();

        var cars = new Car[]
        {
            new() { Model = "Model S", Price = 80000, Year = 2020, Mileage = 10000 },
            new() { Model = "Model 3", Price = 50000, Year = 2021, Mileage = 5000 }
            // Add  cars here...
        };
        foreach (var c in cars) context.Courses.Add(c);
        context.SaveChanges();

        var rentals = new Rentals[]
        {
            new()
            {
                CustomerId = 1, CarId = 1, RentalDate = DateTime.Parse("2022-07-01"), ReturnDate = null,
                Status = Status.Active, Price = 1000, Mileage = 100
            },
            new()
            {
                CustomerId = 2, CarId = 2, RentalDate = DateTime.Parse("2022-06-01"),
                ReturnDate = DateTime.Parse("2022-06-05"), Status = Status.Returned, Price = 800, Mileage = 80
            }
            // Add more  here...
        };
        foreach (var r in rentals) context.Rentals.Add(r);
        context.SaveChanges();
    }
}