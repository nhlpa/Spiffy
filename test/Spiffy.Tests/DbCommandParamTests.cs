using Xunit;

namespace Spiffy.Tests
{    
    public class DbCommandParamTests
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
                { "key", 1 }
            };
            p.Add("key2", 2);
            Assert.Equal(1, p["key"]);
            Assert.Equal(2, p["key2"]);
        }
    }
}