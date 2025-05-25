using Microsoft.EntityFrameworkCore;

namespace DragoAnt.Extensions.EntityFrameworkCore.Tests;

public class FixedQueryTest
{
    [Fact]
    public async Task Empty()
    {
        var list = await FixedQuery<object>.Empty.ToListAsync();
        list.Should().NotBeNull();
        list.Should().BeEmpty();
    }

    [Fact]
    public async Task Null()
    {
        var list = await FixedQuery<object>.Create((IEnumerable<object>?)null).ToListAsync();
        list.Should().NotBeNull();
        list.Should().BeEmpty();
    }

    [Fact]
    public async Task WithItems()
    {
        var list = await FixedQuery<int?>.Create(null, 1).ToListAsync();
        list.Should().NotBeNull();
        list.Should().HaveCount(2);
    }
        
    [Fact]
    public async Task Where()
    {
        var list = await FixedQuery<int?>.Create(null, 1).Where(i => i == 1).ToListAsync();
        list.Should().NotBeNull();
        list.Should().HaveCount(1);
        list[0].Should().Be(1);
    }
        
    [Fact]
    public async Task EmptyWhere()
    {
        var list = await FixedQuery<object>.Empty.Where(o => o != null).ToListAsync();
            
        list.Should().NotBeNull();
        list.Should().HaveCount(0);
    }
        
    [Fact]
    public async Task OrderBy()
    {
        var list = await FixedQuery<int?>.Create(null, 1).OrderBy(i => i).ToListAsync();
        list.Should().NotBeNull();
        list.Should().HaveCount(2);
            
        list[0].Should().Be(null);
        list[1].Should().Be(1);
    }
        
    [Fact]
    public async Task EmptyOrderBy()
    {
        var list = await FixedQuery<int>.Empty.OrderBy(i => i).ToListAsync();
            
        list.Should().NotBeNull();
        list.Should().HaveCount(0);
    }
        
    [Fact]
    public async Task Skip()
    {
        var list = await FixedQuery<int?>.Create(null, 1).Skip(1).ToListAsync();
        list.Should().NotBeNull();
        list.Should().HaveCount(1);
            
        list[0].Should().Be(1);
    }
        
    [Fact]
    public async Task EmptySkip()
    {
        var list = await FixedQuery<int>.Empty.Skip(1).ToListAsync();
            
        list.Should().NotBeNull();
        list.Should().HaveCount(0);
    }
        
    [Fact]
    public async Task Take()
    {
        var list = await FixedQuery<int?>.Create(null, 1).Skip(1).Take(1).ToListAsync();
        list.Should().NotBeNull();
        list.Should().HaveCount(1);
            
        list[0].Should().Be(1);
    }
        
    [Fact]
    public async Task EmptyTake()
    {
        var list = await FixedQuery<int>.Empty.Take(1).ToListAsync();
            
        list.Should().NotBeNull();
        list.Should().HaveCount(0);
    }
}