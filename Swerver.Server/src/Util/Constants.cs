namespace Swerver.Util
{
    /// <summary>This houses all needed Constants.</summary>
    public static class Constants
    {
        /// <summary>The Maximum number of Clients that can connect to the Server.</summary>
        public static int MaxPlayers = 50;

        /// <summary>The Port the Server will run on.</summary>
        public static int Port = 12345;

        /// <summary>How often the Server updates each Second.</summary>
        public static int TicksPerSec = 60;

        /// <summary>How long a Tick is in Milliseconds.</summary>
        public static readonly int MsPerTick = 1000 / TicksPerSec;
    }
}