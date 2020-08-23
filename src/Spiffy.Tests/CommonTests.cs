using Xunit;

namespace Spiffy.Tests
{
    public class CommonTests 
    {
        [Fact]
        public void ChangeTypeForValue()
        {
            object val = 1;
            var changed = Common.ChangeType<int>(val);
            Assert.Equal(1, changed);
        }

        [Fact]
        public void ChangeTypeForNullShouldReturnDefault()
        {
            var changed = Common.ChangeType<int>(null);
            Assert.Equal(default, changed);
        }

        [Fact]
        public void ChangeTypeForNullableValue()
        {            
            var changed = Common.ChangeType<int?>(null);
            Assert.Null(changed);
        }

        [Fact]
        public void ChangeTypeForNullableValueWithValue()
        {
            object val = 1;
            var changed = Common.ChangeType<int?>(val);
            Assert.Equal(1, changed);
        }
    }
}