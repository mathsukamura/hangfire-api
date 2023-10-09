using apiemail.ViewModel;

namespace apiemail.Model;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public string EsqueciMinhaSenha { get; set; }

    public void update(UserViewModel model)
    {
        if (model is null)
        {
            return;
        }

        Email = model.Email;
        Password = model.Password;
    }
}