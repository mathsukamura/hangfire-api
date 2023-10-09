using apiemail.Model;
using apiemail.Service.Hash;
using hangfireapi.Data;
using Microsoft.EntityFrameworkCore;

namespace hangfireapi.Service.Access;

public interface ILoginService
{
    Task<User?> AutenticacaoAsync(Login logins);
}
public class LoginService : ILoginService
{
    private readonly HangContext _context;

    private readonly IHashService _hashService;

    public LoginService(HangContext context, IHashService hashService)
    {
        _context = context;
        _hashService = hashService;
    }

    public async Task<User?> AutenticacaoAsync(Login login)
    {
        if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
        {
            throw new ArgumentException("Email e senha são obrigatórios.");
        }
        
        var usuario = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == login.Email);

        if (usuario != null && _hashService.VerifyHash(login.Password, usuario.Password))
        {
            return usuario;
        }

        return null;
    }
}