using CoffeeToolkit.Progress;
using Xunit;
using System;
using System.Threading;

namespace CoffeeToolkit.Tests.Progress
{
    public class ProgressTrackerTests
    {
        [Fact]
        public void IncrementProgress_IncrementFirst_ValidateIncrementAndTimeEstimate()
        {
            // Increment pt 10 times with delay
            ProgressTracker pt = new ProgressTracker(20);
            for (int i = 0; i < 10; i++)
            {
                pt.IncrementProgress();
                Thread.Sleep(100);
            }

            // Validate increment
            Assert.Equal(10, pt.ProcessedItems);
            Assert.NotEqual(DateTime.MinValue, pt.StartTime);

            // Validate time estimate
            var estMs = pt.GetEstimatedRemainingTime().TotalMilliseconds;
            Assert.True(estMs >= 1000);
            Assert.True(estMs <= 1500);
        }

        [Fact]
        public void ProgressEvent_IncrementTwo_ValidateEventsFired()
        {
            ProgressTracker pt = new ProgressTracker(2);
            int eventFiredCount = 0;
            pt.ProgressChanged += (s, e) =>
            {
                eventFiredCount++;
                if (eventFiredCount == 1)
                {
                    Assert.Equal(1, e.ItemsProcessed);
                }
                else if (eventFiredCount == 2)
                {
                    Assert.Equal(0, e.TimeRemaining.TotalMilliseconds);
                    Assert.Equal(100, e.ProgressPercentage);
                }
            };
            pt.IncrementProgress();
            pt.IncrementProgress();

            Assert.Equal(2, eventFiredCount);
        }

        [Fact]
        public void ProgressEvent_IncremenOne_ValidatePercentage()
        {
            ProgressTracker pt = new ProgressTracker(200);
            int eventFiredCount = 0;
            pt.ProgressChanged += (s, e) =>
            {
                eventFiredCount++;
                if (eventFiredCount == 1)
                    Assert.Equal(0.5, e.ProgressPercentage);
            };
            pt.IncrementProgress();

            Assert.Equal(1, eventFiredCount);
        }
    }
}
