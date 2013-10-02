using System;
using Sitecore.Web.Authentication;
using System.Collections.Generic;

namespace Sitecore.KickUserSessionAgent
{
    public class KickUserSession
    {
        private TimeSpan maximumIdleTime;

        public KickUserSession(string maximumIdleTime)
        {
            this.maximumIdleTime = TimeSpan.Parse(maximumIdleTime);
        }

        public void Run()
        {
            List<DomainAccessGuard.Session> userSessionList = DomainAccessGuard.Sessions;

            if (userSessionList != null && userSessionList.Count > 0)
            {
                foreach (DomainAccessGuard.Session userSession in userSessionList.ToArray())
                {
                    TimeSpan span = (TimeSpan)(DateTime.Now - userSession.LastRequest);
                    if (span > this.maximumIdleTime)
                    {
                        DomainAccessGuard.Kick(userSession.SessionID);
                        Sitecore.Diagnostics.Log.Audit("Kicked out user is : " + userSession.UserName, this);
                    }
                }
            }
        }
    }
}
