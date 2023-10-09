using apiemail.Model;
using apiemail.Service.Hash;
using apiemail.ViewModel;
using hangfireapi.Data;
using Microsoft.EntityFrameworkCore;

namespace hangfireapi.Service.CreateUser;

public interface IUserservice
{
    public Task<IList<User>> GetAsync();
    public Task<User> GetByIdAsync(int id);
    public Task<User> PostAsync(UserViewModel viewModel);
    public Task<User> PutAsync(UserViewModel viewModel, int id);
    public Task<bool> DeleteAsync(int id);
}


public class UserService : IUserservice
{
    private readonly HangContext _emailContext;
    
    private readonly IHashService _hashService;
    
    public UserService(HangContext emailContext, IHashService hashService)
    {
        _emailContext = emailContext;
        _hashService = hashService;
    }
    
    public async Task<IList<User>> GetAsync()
    {
        var user = await _emailContext.Users.AsNoTracking().ToListAsync();
    
        return user;
    }
    
    public async Task<User> GetByIdAsync(int id)
    {
        var user = await _emailContext.Users.AsNoTracking().FirstOrDefaultAsync(u=> u.Id == id);
    
        if (user == null)
        {
            return null;
        }
        
        return user;
    }
    
    public async Task<User> PostAsync(UserViewModel viewModel)
    {
        var user = viewModel.CreateUser();
        
        user.Password =  _hashService.GenerateHash(user.Password);
    
        await _emailContext.Users.AddAsync(user);
    
        await _emailContext.SaveChangesAsync();
    
        return user;
    }
    
    public async Task<User> PutAsync(UserViewModel viewModel, int id)
    {
        var user = await _emailContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
        {
            return null;
        }
    
        user.update(viewModel);
        
        user.Password =  _hashService.GenerateHash(user.Password);
    
        await _emailContext.SaveChangesAsync();
    
        return user;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _emailContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    
        if (user == null)
        {
            return false;
        }
    
        _emailContext.Users.Remove(user);
    
        await _emailContext.SaveChangesAsync();
    
        return true;
    }
}