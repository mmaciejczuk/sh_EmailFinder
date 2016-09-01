using System;
using System.Collections.Generic;

namespace EmailFinder.Models
{
    public class EmailFinderViewModel
    {
        public List<Tuple<string, EmailDiscoveryService>> EmailDiscoveryServices { get; private set; }

        public EmailFinderViewModel()
        {
            EmailDiscoveryServices = new List<Tuple<string, EmailDiscoveryService>>()
            {
                new Tuple<string, EmailDiscoveryService>("mailboxlayer.com", EmailDiscoveryService.MailboxLayer),
                new Tuple<string, EmailDiscoveryService>("verify-email.org", EmailDiscoveryService.VerifyEmail)
            };
        }
    }
}