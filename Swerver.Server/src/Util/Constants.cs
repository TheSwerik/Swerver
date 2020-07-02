namespace ServerLibrary.Util
{
    /// <summary>This houses all needed Constants.</summary>
    public static class Constants
    {
        /// <summary>How often the Server updates each Second.</summary>
        public static int TicksPerSec = 60;

        /// <summary>How long a Tick is in Milliseconds.</summary>
        public static readonly int MsPerTick = 1000 / TicksPerSec;
    }
}