using CoffeeToolkit.Linq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoffeeToolkit.Tests.Linq
{
    public class LinqExtensionTests
    {
        [Fact]
        public void DefaultOrNull_Defaultable_ExpectNull()
        {
            // List of value type, which returns not null as default()
            List<KeyValuePair<string, string>> emptyDefaultableList = new();
            Assert.Null(emptyDefaultableList.FirstOrNull());
        }

        [Fact]
        public void DefaultOrNull_FilteredValueType_ExpectFirstValue()
        {
            // List of value type, which returns not null as default()
            List<KeyValuePair<string, string>> emptyDefaultableList = new()
            {
                new("firstkey", "firstvalue"),
                new("secondkey", "secondvalue"),
            };

            // Run through linq query, get nullable value as result
            KeyValuePair<string, string>? result = emptyDefaultableList
                .Where(x => true == true) // select all
                .FirstOrNull();

            Assert.Equal("firstkey", result.Value.Key);
        }

        [Fact]
        public void UniqueBy_LibraryVersion_ExpectFilteredEntries()
        {
            // List of value type, which returns not null as default()
            List<KeyValuePair<string, string>> kvList = new()
            {
                new("firstkey", "firstvalue"),
                new("secondkey", "sharedvalue"),
                new("thirdkey", "sharedvalue")
            };

            // Explicitly call the library version
            var explicitValues = kvList.UniqueBy(x => x.Value);
            var implicitValues = kvList.DistinctBy(x => x.Value);

            Assert.Equal(2, explicitValues.Count());
            Assert.Equal(2, implicitValues.Count());
            for (int i = 0; i < 2; i++)
            {
                Assert.Equal(explicitValues.ElementAt(i).Key, implicitValues.ElementAt(i).Key);
                Assert.Equal(explicitValues.ElementAt(i).Value, implicitValues.ElementAt(i).Value);
            }
        }
    }
}
