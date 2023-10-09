namespace apiemail.Service.Hash;

public interface IHashService
{
    public string GenerateHash(string password);

    public bool VerifyHash(string password, string hash);

}

public class HashService: IHashService
{
    
    public string GenerateHash(string password)
    { 
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyHash(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}