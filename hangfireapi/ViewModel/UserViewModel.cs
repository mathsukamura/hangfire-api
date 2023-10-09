using apiemail.Model;

namespace apiemail.ViewModel;

public class UserViewModel
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    

    public User CreateUser()
    {
        return new User
        {
            Email = Email,
            Password = Password
        };
    }
}