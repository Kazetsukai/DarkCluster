using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Network
{
    public interface IEvent
    {
        int EventId { get; set; }
    }
}
