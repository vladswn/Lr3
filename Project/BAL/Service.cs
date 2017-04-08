using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace Project.BAL
{
    public class Service
    {
        public void SendEmail(string from, string yourName, string body)
        {
            string name = "slepsys@yandex.ru";
            string password = "usercu26afm";
            MailMessage mail = new MailMessage(name, "starvladislav@gmail.com");
            using(SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = "smtp.yandex.ru";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential(name, password);
                smtp.EnableSsl = true;
                mail.Subject = String.Format("Email :{0}, Name {1}", from, yourName);
                mail.Body = body;
                smtp.Send(mail);
            }
            
        }
    }
}