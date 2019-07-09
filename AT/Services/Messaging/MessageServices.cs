using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace AT.Services.Messaging
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly EmailConfig _emailConfiguration;
       

        public AuthMessageSender(IOptions<EmailConfig> emailSettings)
        {
            _emailConfiguration = emailSettings.Value;
           
        }
        public async Task SendEmailAsync(string email,
                                    string subject,
                                    string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.FromWho,
                                                        _emailConfiguration.FromEmailAddress));

            emailMessage.To.Add(new MailboxAddress("", email));
            //emailMessage.Bcc.Add(new MailboxAddress("", "enquiries@amatistraining.com"));
            emailMessage.Subject = subject;


            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput())))
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_emailConfiguration.SMTPServer,
                                              _emailConfiguration.Port, true);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.FromEmailAddress,
                                    _emailConfiguration.Password);


                await client.SendAsync(emailMessage);
                client.Disconnect(true);
            }

          

            return;
        }

        public async Task SendContactEmailAsync(string from,
                                                string subject,
                                                string message)
        {
            //this is different from the SendEmail Async method in that it is called from the 
            //contact form and so the email is sent to the Secretariat email inbox
            var emailMessage = new MimeMessage();
            //although the email is from the person who filled out the contact form it will be sent by the
            //email account setup in the configuration settings
            emailMessage.From.Add(new MailboxAddress(from, _emailConfiguration.FromEmailAddress));

            //The email will be sent to the inbox of the account setup in account settings
            //so the same physical account that sends the email
            emailMessage.To.Add(new MailboxAddress("", _emailConfiguration.FromEmailAddress));

            emailMessage.Subject = subject;

            //the message is the one typed into the contact form
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_emailConfiguration.SMTPServer,
                                          _emailConfiguration.Port);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.FromEmailAddress,
                                    _emailConfiguration.Password);


                await client.SendAsync(emailMessage);
                client.Disconnect(true);
            }

            return;


        }

        public async Task SendFailedPaymentMessageAsync(string customerEmail, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.FromWho,
                                                        _emailConfiguration.FromEmailAddress));

            emailMessage.To.Add(new MailboxAddress("", customerEmail));
            emailMessage.Bcc.Add(new MailboxAddress("", _emailConfiguration.FromEmailAddress));
            emailMessage.Subject = "Payment to Amatis Training Unsuccessful";


            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput())))
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_emailConfiguration.SMTPServer,
                                              _emailConfiguration.Port, true);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.FromEmailAddress,
                                    _emailConfiguration.Password);


                await client.SendAsync(emailMessage);
                client.Disconnect(true);
            }



            return;
        }

        public async Task SendRegisterUserForCourseAsync(string customerEmail, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.FromWho,
                                                        _emailConfiguration.FromEmailAddress));

            emailMessage.To.Add(new MailboxAddress("", customerEmail));
            emailMessage.Bcc.Add(new MailboxAddress("", _emailConfiguration.FromEmailAddress));
            emailMessage.Subject = "Payment Received for New Course";


            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput())))
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_emailConfiguration.SMTPServer,
                                              _emailConfiguration.Port, true);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.FromEmailAddress,
                                    _emailConfiguration.Password);


                await client.SendAsync(emailMessage);
                client.Disconnect(true);
            }



            return;

        }

        public async Task SendProblemRegisteringUserForCourseAsync(string message)
        {
            //this is different from the SendEmail Async method in that it is called from the 
            //contact form and so the email is sent to the Secretariat email inbox
            var emailMessage = new MimeMessage();
            //although the email is from the person who filled out the contact form it will be sent by the
            //email account setup in the configuration settings
            emailMessage.From.Add(new MailboxAddress("Amatis Traing Website", _emailConfiguration.FromEmailAddress));

            //The email will be sent to the inbox of the account setup in account settings
            //so the same physical account that sends the email
            emailMessage.To.Add(new MailboxAddress("", _emailConfiguration.FromEmailAddress));

            emailMessage.Subject = "Attention: Problem registering a user for a course after successful payment";

            //the message is the one typed into the contact form
            BodyBuilder bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = message;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_emailConfiguration.SMTPServer,
                                          _emailConfiguration.Port);

                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.FromEmailAddress,
                                    _emailConfiguration.Password);


                await client.SendAsync(emailMessage);
                client.Disconnect(true);
            }

            return;
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
