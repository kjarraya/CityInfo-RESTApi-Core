using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Services
{
    public class LocalMailService: IMailServiceInterface
    {
        private string _mailTO = Startup.configuration["mailSettings:mailToAddress"];
        private string _mailFrom = Startup.configuration["mailSettings:mailFromAddress"];
        public void  send(string subject, string message)
        {
            Debug.WriteLine($"mail from {_mailFrom} to {_mailTO} with LocalMailService.");
            Debug.WriteLine($"subject: {subject}");
            Debug.WriteLine($"message {message}");
        }
    }
}
