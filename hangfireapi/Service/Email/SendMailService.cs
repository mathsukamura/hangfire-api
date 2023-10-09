using apiemail.Model;
using MailKit.Net.Smtp;
using MimeKit;

namespace hangfireapi.Service.Email;

public interface ISendMailService
{
    bool SendEmail(string title,string body);
}

public class SendMailService : ISendMailService
{
    public bool SendEmail(string title,string body)
    {
        var authentication = EmailInstance();
        var smtpClient = new SmtpClient ();
        smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        smtpClient.Timeout = 60 * 60;
        smtpClient.Authenticate(authentication.Email, authentication.password); 

        var message = SendMailMessege(authentication.Email, title, body);
        
        if (message == null)
        {
            return false;
        }

        smtpClient.Send(message);

        return true;
    }
    
    private static MimeMessage  SendMailMessege(string email ,string title, string body)
    {
        var emailMessege = new MimeMessage ();

        emailMessege.From.Add (new MailboxAddress (email, email));
        emailMessege.Body = new TextPart () {
            Text = body
        };;
        emailMessege.Subject = title;
        emailMessege.To.Add(new MailboxAddress ( email ,email));

        return emailMessege;
    }
    
    private Send EmailInstance()
    {
        var email = new Send()
        {
            Email = "matheus.yuri.melo@gmail.com",
            password = "qlvc mlla aqoo keme"
        };

        return email;
    }
}