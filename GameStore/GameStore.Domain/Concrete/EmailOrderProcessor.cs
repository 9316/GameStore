using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "test123451868@gmail.com";

        public string MailFromAddress = "test123450101@gmail.com";

        public bool UseSsl = true;

        public string UserName = "test123450101@gmail.com";

        public string Password = "Admin2018";

        public string ServerName = "smtp.gmail.com";

        public int ServerPort = 587;

        public bool WriteAsFile = false;

        public string FileLocation = @"C:\game_store_emails";
    }


    public class EmailOrderProcessor : IOrderProcessor
    {

        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void OrderProcess(Cart cart, Entities.ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {

                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                .AppendLine("New order processed")
                .AppendLine(string.Empty)
                .AppendLine("Goods:");

                foreach (var line in cart.Lines)
                {
                    var subTotal = line.Game.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (Total: {2:c})", line.Quantity, line.Game.Name, subTotal);
                    body.AppendLine(string.Empty);
                        
                }

                body.AppendFormat("Total cost: {0:c}", cart.ComputeTotalValue())
                    .AppendLine(string.Empty)
                    .AppendLine("Delivery")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? string.Empty)
                    .AppendLine(shippingDetails.Line3 ?? string.Empty)
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine(string.Empty)
                    .AppendFormat("Gift Wrap: {0}", shippingDetails.GiftWrap ? "Yes" : "No");

                MailMessage mailMessage = new MailMessage(
                                      emailSettings.MailFromAddress,	// От кого
                                      emailSettings.MailToAddress,		// Кому
                                      "New order sent!",		// Тема
                                      body.ToString()); 				// Тело письма

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                smtpClient.Send(mailMessage); //google email account-ში უნდა ჩავრთოთ Acess for less secure apps წინააღმდეგ შემთხვევაში დაგენერირდება smtpexception

            }
        }
    }
}
