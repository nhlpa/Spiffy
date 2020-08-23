using Xunit;

namespace Spiffy.Tests
{    
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
            var p = new DbParams();
            p.Add("key", 1);
            Assert.Equal(1, p["key"]);
        }
    }
}