using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using RentalCars.Data;
using RentalCars.Models;

public class RentalServiceTests
{
    [Fact]
    public async Task GetRentalsWithStatus_ReturnsCorrectRentals()
    {
        // Arrange
        var data = new List<Rentals>
        {
            new() { RentalsId = 1, Status = Status.Active },
            new() { RentalsId = 2, Status = Status.Active },
            new() { RentalsId = 3, Status = Status.Returned }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Rentals>>();
        mockSet.As<IAsyncEnumerable<Rentals>>().Setup(m => m.GetAsyncEnumerator(new CancellationToken()))
            .Returns(new TestAsyncEnumerator<Rentals>(data.GetEnumerator()));
        mockSet.As<IQueryable<Rentals>>().Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<Rentals>(data.Provider));
        mockSet.As<IQueryable<Rentals>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Rentals>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Rentals>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        var mockContext = new Mock<RentalContext>();
        mockContext.Setup(c => c.Rentals).Returns(mockSet.Object);

        var service = new RentalService(mockContext.Object);

        // Act
        var rentals = await service.GetRentalsWithStatus(Status.Active);

        // Assert
        Assert.Equal(2, rentals.Count);
        Assert.All(rentals, r => Assert.Equal(Status.Active, r.Status));
    }
}

public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_inner.MoveNext());
    }

    public T Current => _inner.Current;

    public ValueTask DisposeAsync()
    {
        return new ValueTask(Task.CompletedTask);
    }

    public void Dispose()
    {
        _inner.Dispose();
    }

    public Task<bool> MoveNext(CancellationToken cancellationToken)
    {
        return Task.FromResult(_inner.MoveNext());
    }
}

public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
    {
        return new TestAsyncEnumerable<TResult>(expression);
    }
}

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    {
    }

    public TestAsyncEnumerable(Expression expression)
        : base(expression)
    {
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new())
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}