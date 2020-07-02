using Swerver.Util;

namespace Swerver.Server
{
    public class Client
    {
        public readonly int Id;
        internal readonly Tcp Tcp;
        internal readonly Udp Udp;

        internal Client(int id)
        {
            Id = id;
            Tcp = new Tcp(Id);
            Udp = new Udp(Id);
        }
    }
}