using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackedBrain.ServiceBus.Core.Messaging
{
    public interface IMessageSessionIdProvider
    {
        string GenerateSessionId<T>(T body);
    }
}
