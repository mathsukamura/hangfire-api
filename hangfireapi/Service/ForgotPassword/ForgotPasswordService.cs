using apiemail.Model;
using apiemail.Service.Hash;
using apiemail.ViewModel;
using hangfireapi.Data;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace hangfireapi.Service.ForgotPassword;

public interface IForgotPasswordService
{
    public Task<User?> GetByEmailAsync(string modelEmail);
    public Task<bool> GetByForgotPasswordAsync(ResetPasswordViewModel model);
    public Task<bool> NotificarTrocaSenhaAsync(ForgotPasswordViewModel email, string guid);
    public Task<bool> SolicitarTrocaSenhaAsync(string email, string guid);

}

public class ForgotPasswordService: IForgotPasswordService
{
    private readonly HangContext _emailContext;
    
    private readonly IHashService _hashService;

    public ForgotPasswordService(HangContext emailContext, IHashService hashService)
    {
        _emailContext = emailContext;
        _hashService = hashService;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _emailContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        
        return user;
    }

    public async Task<bool> SolicitarTrocaSenhaAsync(string email, string guid)
    {
        var user = await _emailContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        if (user == null)
        {
            return false;
        }

        user.EsqueciMinhaSenha = guid;
        
        await _emailContext.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> NotificarTrocaSenhaAsync(ForgotPasswordViewModel email,string guid)
    {
        var smtpClient = new SmtpClient ();
        await smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        //smtpClient.Timeout = 60 * 6000;
        await smtpClient.AuthenticateAsync("matheus.yuri.melo@gmail.com", "eqqq cebq fnuo nrgp"); 

        var message = SendMailMessege(email.email, guid);

        try
        {
            await smtpClient.SendAsync(message);
        }
        catch (Exception e)
        {
            //tomar decisao para enviar depois...
            return false;
        }
        
        return true;
    }
    
    public async Task<bool> GetByForgotPasswordAsync(ResetPasswordViewModel model)
    {
        if (model.Password != model.ConfirmPassword)
        {
            return false;
        }
        
        var user = await _emailContext.Users.FirstOrDefaultAsync(u => u.EsqueciMinhaSenha == model.Id.ToString());

        if (user is null)
        {
            return false;
        }
        
        user.Password = _hashService.GenerateHash(model.Password);
        
        await _emailContext.SaveChangesAsync();
        
        return true;
    }
    
    private static MimeMessage  SendMailMessege(string email, string guid)
    {
        var emailMessege = new MimeMessage ();

        emailMessege.From.Add (new MailboxAddress ("", email));
        emailMessege.Body = new TextPart () {
            Text = $"To reset your password use the code {guid} in the following link"
        };;
        emailMessege.Subject = "reset password";
        emailMessege.To.Add(new MailboxAddress ( "" ,email));

        return emailMessege;
    }
    
}