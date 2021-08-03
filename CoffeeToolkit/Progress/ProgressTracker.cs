using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeToolkit.Progress
{
    /// <summary>
    /// Easy access to thread-safe progress tracking and completion estimator.
    /// </summary>
    public class ProgressTracker
    {
        public ProgressTracker(int totalItems)
        {
            _totalItems = totalItems;
        }

        /* Fields */
        private int _processedItems = 0;
        private int _totalItems = 0;

        /* Events */
        public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
        public event ProgressChangedEventHandler ProgressChanged;


        /* Properties */

        /// <summary>
        /// The amount of items that have been processed.
        /// This should be incremented after each item or group of items.
        /// </summary>
        public int ProcessedItems
        {
            get { return _processedItems; }
            set {
                // Safety check for exceeding TotalItems
                if (value > TotalItems)
                    throw new ArgumentOutOfRangeException(
                        "ProgressTracker reported that ProcessedItems was set to " +
                        $"{value} while the maximum value is {TotalItems}.");

                // Keep it positive
                else if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        "ProcessedItems must be a number between 0 and TotalItems");

                // Automatically start tracking
                if (_processedItems == 0 && StartTime == DateTime.MinValue)
                    StartTime = DateTime.Now;

                // Define field
                _processedItems = value; 

                // Fire event
                
                if (ProgressChanged != null)
                    ProgressChanged(this, new ProgressChangedEventArgs() 
                    { 
                        ItemsProcessed = value,
                        TotalItems = this.TotalItems,
                        ProgressPercentage = GetProgressPercentage(),
                        TimeRemaining = GetEstimatedRemainingTime(),
                    });
            }
        }

        /// <summary>
        /// The amount of items that must be processed for the ProgressTracker to complete
        /// </summary>
        public int TotalItems
        {
            get { return _totalItems; }
            set 
            {
                // Positive value check
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        "TotalItems must be greater than 0");

                // TotalItems must always be greater than ProcessedItems
                if (value < ProcessedItems)
                    throw new ArgumentOutOfRangeException(
                        "TotalItems must always be greater than ProcessedItems");

                _totalItems = value; 
            }
        }

        public DateTime StartTime { get; private set; } 
            = DateTime.MinValue;

        /// <summary>
        /// Call this function to increase and recalculate progress.
        /// </summary>
        /// <param name="incrementAmount">Amount to increment</param>
        public void IncrementProgress(int incrementAmount = 1)
            => ProcessedItems++;

        /// <summary>
        /// Resets this ProgressTracker's progress count and start time.
        /// This will be called automatically when adding your first item
        /// </summary>
        public void RestartTracker()
        {
            ProcessedItems = 0;
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// Gets a TimeSpan of the amount of time left before progress is complete.
        /// Assumes that all items the same amount of time to process. Uses average.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetEstimatedRemainingTime()
        {
            if (StartTime == DateTime.MinValue)
                throw new InvalidOperationException(
                    "You must call RestartTracker() before you can request a time estimate.");

            // Calculate time needed
            long elapsedMs = (long)Math.Round(DateTime.Now.Subtract(StartTime).TotalMilliseconds);
            double millisecondsPerItem = elapsedMs / ProcessedItems;
            int itemsRemaining = TotalItems - ProcessedItems;

            var result = TimeSpan.FromMilliseconds(millisecondsPerItem * itemsRemaining);
            return result;
        }

        /// <summary>
        /// Returns the percentage of completed items
        /// </summary>
        /// <returns></returns>
        public float GetProgressPercentage()
            => ((float)ProcessedItems / (float)TotalItems) * 100;


        public class ProgressChangedEventArgs
        {
            public int ItemsProcessed { get; internal set; }
            public int TotalItems { get; internal set; }
            public float ProgressPercentage { get; internal set; }
            public TimeSpan TimeRemaining { get; internal set; }
        }
    }
}
