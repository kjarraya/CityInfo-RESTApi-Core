using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Services
{
    public interface IMailServiceInterface
    {
        void send(string subject, string message);
    }
}
