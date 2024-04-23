using Microsoft.EntityFrameworkCore;
using RentalCars.Models;

namespace RentalCars.Data;

public class RentalService
{
    private readonly RentalContext _context;

    public RentalService(RentalContext context)
    {
        _context = context;
    }

    public async Task CreateCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Customer>> GetCustomers()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task DeleteCustomer(int customerId)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    // Car methods
    public async Task CreateCar(Car car)
    {
        _context.Courses.Add(car);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCar(int carId)
    {
        var car = await _context.Courses.FindAsync(carId);
        if (car != null)
        {
            _context.Courses.Remove(car);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Car>> GetCars()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task EditCar(Car car)
    {
        _context.Courses.Update(car);
        await _context.SaveChangesAsync();
    }

    // Rental methods
    public async Task CreateRental(Rentals rental)
    {
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRental(int rentalId)
    {
        var rental = await _context.Rentals.FindAsync(rentalId);
        if (rental != null)
        {
            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditRental(Rentals rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Rentals>> GetRentalsForUser(int customerId)
    {
        return await _context.Rentals
            .Where(r => r.CustomerId == customerId)
            .ToListAsync();
    }

    // get all rentals sorted by rental date

    public async Task<List<Rentals>> GetRentalsSortedByDate()
    {
        return await _context.Rentals
            .OrderBy(r => r.RentalDate)
            .ToListAsync();
    }


    public async Task<List<Rentals>> GetRentalsWithStatus(Status status)
    {
        return await _context.Rentals
            .Where(r => r.Status == status)
            .ToListAsync();
    }
}