using CoffeeToolkit.Random;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoffeeToolkit.Tests.Random
{
    public class RngTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(-2, -1)]
        [InlineData(-1, 0)]
        [InlineData(-1000, 0)]
        [InlineData(-1000, 1000)]
        public void Generate_ManyAttempts_InRangeNoException(int min, int max)
        {
            int number = 0;
            for (int i = 0; i < 1000; i++)
            {
                number = Rng.Generate(min, max);
                Assert.True(number >= min);
                Assert.True(number <= max);
            }
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 0)]
        [InlineData(-1000, 0)]
        [InlineData(-1000, 1000)]
        public void Generate_ManyAttempts_NotSpammingSameResult(int min, int max)
        {
            Dictionary<int, int> results = new();
            int number;
            for (int i = 0; i < 1000; i++)
            {
                number = Rng.Generate(min, max);

                // Create key if new or increment value
                if (results.ContainsKey(number))
                    results[number]++;
                else results.Add(number, 1);
            }

            // Assert that we don't have the same number an abnormal amount of times
            int mostHitsCount = results
                .OrderByDescending(x => x.Value)
                .First().Value;
            Assert.True(mostHitsCount < 900);
        }

        [Fact]
        public void Types_TryAll_NoException()
        {
            bool a = Rng.Bool();
            ushort b = Rng.UInt16();
            short c = Rng.Int16();
            uint d = Rng.UInt32();
            int e = Rng.Int32();
            ulong f = Rng.UInt64();
            long g = Rng.Int64();
            float h = Rng.Single();
            double i = Rng.Double();
            byte j = Rng.Byte();
            byte[] k = Rng.ByteArray(16);
        }

        [Fact]
        public void Generate_AllInt32Variants_NotZeroNoException()
        {
            int noArgs = Rng.Generate();
            int oneArg = Rng.Generate(int.MaxValue);
            int TwoArgs = Rng.Generate(1, int.MaxValue);

            Assert.NotEqual(0, noArgs); // Fails 1 in int.MaxValue) times
            Assert.NotEqual(0, oneArg); // Fails 1 in int.MaxValue) times
            Assert.NotEqual(0, TwoArgs);
        }
    }
}
