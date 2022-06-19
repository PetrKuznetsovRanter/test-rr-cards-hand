using System;
using System.Diagnostics;

namespace Cysharp.Threading.Tasks.Internal
{
    internal readonly struct ValueStopwatch
    {
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double) Stopwatch.Frequency;

        private readonly long startTimestamp;

        private ValueStopwatch(long startTimestamp)
        {
            this.startTimestamp = startTimestamp;
        }

        public TimeSpan Elapsed => TimeSpan.FromTicks(ElapsedTicks);

        public bool IsInvalid => startTimestamp == 0;

        public long ElapsedTicks
        {
            get
            {
                if (startTimestamp == 0)
                    throw new InvalidOperationException("Detected invalid initialization(use 'default'), only to create from StartNew().");

                var delta = Stopwatch.GetTimestamp() - startTimestamp;
                return (long) (delta * TimestampToTicks);
            }
        }

        public static ValueStopwatch StartNew()
        {
            return new(Stopwatch.GetTimestamp());
        }
    }
}