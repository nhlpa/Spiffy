namespace Spiffy.Tests;

using Xunit;

public class DbParamTests
{
    [Fact]
    public void ShouldCreateEmptyParams()
    {
        var p = new DbParams();
        Assert.Empty(p.Keys);
    }

    [Fact]
    public void ShouldCreateParamsWithOneEntry()
    {
        var p = new DbParams("key", 1);
        Assert.Equal(1, p["key"]);
    }

    [Fact]
    public void ShouldAddParams()
    {
        var p = new DbParams
    {
        { "key", 1 },
        { "key2", 2 }
    };
        Assert.Equal(1, p["key"]);
        Assert.Equal(2, p["key2"]);
    }

    [Fact]
    public void ShouldCombineParams()
    {
        var p1 = new DbParams("key", 1)
    {
        { "key2", 2 }
    };

        var p2 = new DbParams("key1", 3)
    {
        { "key2", "WRONG" }
    };

        p1.Add(p2);

        Assert.Equal(1, p1["key"]);
        Assert.Equal(2, p1["key2"]);
        Assert.Equal(3, p1["key1"]);
    }
}
