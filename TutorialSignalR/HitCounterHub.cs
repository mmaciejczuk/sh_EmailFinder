using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace TutorialSignalR
{
    [HubName("hitCounter")]
    public class HitCounterHub : Hub
    {
        public static int _hitCount { get; set; }

        public HitCounterHub()
        {
            _hitCount = 0;
        }
        
        public void RecordHit()
        {
             this.Clients.All.onHitRecorded(Variable.variable);
        }

        public void Up()
        {
            _hitCount  += 1;
        }
    }
}