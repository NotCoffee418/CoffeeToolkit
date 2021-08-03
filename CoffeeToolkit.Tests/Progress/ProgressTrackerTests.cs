using CoffeeToolkit.Progress;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CoffeeToolkit.Tests.Progress
{
    class ProgressTrackerTests
    {
        [Test]
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
            Assert.AreEqual(10, pt.ProcessedItems);
            Assert.AreNotEqual(DateTime.MinValue, pt.StartTime);

            // Validate time estimate
            var estMs = pt.GetEstimatedRemainingTime().TotalMilliseconds;
            Assert.GreaterOrEqual(estMs, 1000);
            Assert.LessOrEqual(estMs, 1500);
        }

        [Test]
        public void ProgressEvent_IncrementTwo_ValidateEventsFired()
        {
            ProgressTracker pt = new ProgressTracker(2);
            int eventFiredCount = 0;
            pt.ProgressChanged += (s, e) =>
            {
                eventFiredCount++;
                if (eventFiredCount == 1)
                {
                    Assert.AreEqual(1, e.ItemsProcessed);
                }
                else if (eventFiredCount == 2)
                {
                    Assert.AreEqual(0, e.TimeRemaining.TotalMilliseconds);
                    Assert.AreEqual(100, e.ProgressPercentage);
                }
            };
            pt.IncrementProgress();
            pt.IncrementProgress();

            Assert.AreEqual(2, eventFiredCount);
        }

        [Test]
        public void ProgressEvent_IncremenOne_ValidatePercentage()
        {
            ProgressTracker pt = new ProgressTracker(200);
            int eventFiredCount = 0;
            pt.ProgressChanged += (s, e) =>
            {
                eventFiredCount++;
                if (eventFiredCount == 1)
                    Assert.AreEqual(0.5, e.ProgressPercentage);
            };
            pt.IncrementProgress();

            Assert.AreEqual(1, eventFiredCount);
        }
    }
}
