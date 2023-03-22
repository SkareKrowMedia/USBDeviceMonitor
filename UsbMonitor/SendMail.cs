using System;

using System.Collections.Generic;

using System.Linq;

using System.Net.Mail;

using System.Net;

using System.Text;

using System.Threading.Tasks;

using MailKit.Net.Smtp;

using MimeKit;

using static System.Net.Mime.MediaTypeNames;

using MailKit.Security;

namespace USB_TEST

{

    public class SendMail

    {

        public void Send(string v)

        {

            string deviceID = v;

            string from_name = "USB Monitor"; // what the display name will be in the from field

            string from_email = ""; //the email address you want the notification sent from.

            string to_name = ""; //name of the person that you want the info being sent to

            string to_email = ""; //the email address you want the notifcation sent to.

            string subject = "Device Removed"; // the subject for the notification

            int port = 587; //the smtp server port

            string email_server = ""; //the email server port



            var mailMessage = new MimeMessage();

            mailMessage.From.Add(new MailboxAddress(from_name, from_email));

            mailMessage.To.Add(new MailboxAddress(to_name, to_email));

            mailMessage.Subject = subject;

            mailMessage.Body = new TextPart("plain")

            {

                Text = "The USB device" + deviceID + " was removed at " + DateTime.Now.ToString() + " have a good day."

            };

            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())

            {

                smtpClient.Connect(email_server, port, SecureSocketOptions.StartTls);

                smtpClient.Authenticate(from_email, "");

                smtpClient.Send(mailMessage);

                smtpClient.Disconnect(true);

            }





        }

    }


}

