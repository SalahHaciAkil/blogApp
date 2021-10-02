using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API._Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string fromAddress, string toAdress, string subject, string message); 
    }
}