using apiemail.Model;
using MailKit.Net.Smtp;
using MimeKit;

namespace hangfireapi.Service.Email;

public interface ISendEmailService
{
    public bool SendEmail(Send email);
}

public class SendEmailService : ISendEmailService
{
    public bool SendEmail(Send email)
    {
        var smtpClient = new SmtpClient ();
        smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        smtpClient.Timeout = 60 * 60;
        smtpClient.Authenticate(email.Email, email.password); 

        var message = SendMailMessege(email);
        
        if (message == null)
        {
            return false;
        }

        smtpClient.Send(message);

        return true;
    }

    private static MimeMessage  SendMailMessege(Send email)
    {
        var emailMessege = new MimeMessage ();

        emailMessege.From.Add (new MailboxAddress (email.User, email.Email));
        emailMessege.Body = new TextPart () {
            Text = email.Body
        };;
        emailMessege.Subject = email.Title;
        emailMessege.To.Add(new MailboxAddress ( email.NameRecipient ,email.Recipient));

        return emailMessege;
    }
}