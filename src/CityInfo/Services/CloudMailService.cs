using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Services
{
    public class CloudMailService : IMailServiceInterface
    {
        private string _mailTO = "khalil.jarraya@zags.com";
        private string _mailFrom = "jarrayakhalil@gmail.com";
        public void send(string subject, string message)
        {
            Debug.WriteLine($"mail from {_mailFrom} to {_mailTO} with CloudMailService.");
            Debug.WriteLine($"subject: {subject}");
            Debug.WriteLine($"message {message}");
        }
    }
}
