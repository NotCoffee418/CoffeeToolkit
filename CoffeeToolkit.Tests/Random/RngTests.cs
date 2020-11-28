using CoffeeToolkit.Random;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeToolkit.Tests.Random
{
    class RngTests
    {
        [Test]
        [TestCase(0,1)]
        [TestCase(-2, -1)]
        [TestCase(-1, 0)]
        [TestCase(-1000, 0)]
        [TestCase(-1000, 1000)]
        public void Generate_ManyAttempts_InRangeNoException(int min, int max)
        {
            int number = 0;
            for (int i = 0; i < 1000; i++)
            {
                number = Rng.Generate(min, max);
                Assert.GreaterOrEqual(number, min);
                Assert.LessOrEqual(number, max);
            }
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(-1, 0)]
        [TestCase(-1000, 0)]
        [TestCase(-1000, 1000)]
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
            Assert.Less(mostHitsCount, 900);
        }

        [Test]
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

        [Test]
        public void Generate_AllInt32Variants_NotZeroNoException()
        {
            int noArgs = Rng.Generate();
            int oneArg = Rng.Generate(int.MaxValue);
            int TwoArgs = Rng.Generate(1, int.MaxValue);

            Assert.NotZero(noArgs); // Fails 1 in int.MaxValue) times
            Assert.NotZero(oneArg); // Fails 1 in int.MaxValue) times
            Assert.NotZero(TwoArgs);
        }
    }
}
