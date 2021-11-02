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
    }
}
