using apiemail.ViewModel;
using hangfireapi.Service.CreateUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hangfireapi.Controllers;

[Route("User")]
public class UserController : ControllerBase
{
    private readonly IUserservice _userservice;
    
    public UserController(IUserservice userservice)
    {
        _userservice = userservice;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var user = await _userservice.GetAsync();
    
        return Ok(user);
    }
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var user = await _userservice.GetByIdAsync(id);
        
        return Ok(user);
    }
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> PostAsync(UserViewModel model)
    {
        var user = await _userservice.PostAsync(model);

        return Ok(user);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(UserViewModel model, int id)
    {
        var user = await _userservice.PutAsync(model, id);

        return Ok(user);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _userservice.DeleteAsync(id);

        return Ok("Deletado com sucesso");
    }
}
