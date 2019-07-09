using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Services.Messaging
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendContactEmailAsync(string from,
                                                string subject,
                                                string message);

        Task SendProblemRegisteringUserForCourseAsync(string message);
        Task SendFailedPaymentMessageAsync(string customerEmail,string message);

        Task SendRegisterUserForCourseAsync(string customerEmail, string message);
      
    }
}
