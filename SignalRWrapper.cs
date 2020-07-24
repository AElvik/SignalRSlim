using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO.SignalR.Slim
{
    public class SignalRWrapper
    {
        private readonly Lazy<IHubContext> eventsHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<EventsHub>());

        private IHubContext EventsHub => this.eventsHub.Value;

        public void PushMessage(string pushMessageEventName, string receiverGroup, object message, Guid? clientId)
        {
            var groupName = receiverGroup;
            var eventName = pushMessageEventName;

            this.EventsHub.Clients.Group(groupName).newEvent(eventName, message, clientId);   
        }
    }
}
