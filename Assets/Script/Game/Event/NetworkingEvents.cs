using UnityEngine.Networking;

namespace ThreeK.Game.Event
{
    public class ClientConnectEvent : FEvent
    {
        public ClientConnectEvent(NetworkClient client) : base(client)
        {
        }
    }
}
