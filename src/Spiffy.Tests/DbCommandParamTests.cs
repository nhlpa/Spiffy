using Xunit;

namespace Spiffy.Tests
{    
    public class DbCommandParamTests
    {
        [Fact]
        public void ShouldCreateEmptyParams()
        {
            var p = new DbCommandParams();
            Assert.Empty(p.Keys);
        }

        [Fact]
        public void ShouldCreateParamsWithOneEntry()
        {
            var p = new DbCommandParams("key", 1);
            Assert.Equal(1, p["key"]);
        }

        [Fact]
        public void ShouldAddParams()
        {
            var p = new DbCommandParams();
            p.Add("key", 1);
            Assert.Equal(1, p["key"]);
        }
    }
}