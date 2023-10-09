using apiemail.ViewModel;
using hangfireapi.Service.ForgotPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hangfireapi.Controllers;

public class ForgotPasswordController: ControllerBase
{
    private readonly IForgotPasswordService _forgotPasswordService;

    public ForgotPasswordController(IForgotPasswordService forgotPasswordService)
    {
        _forgotPasswordService = forgotPasswordService;
    }
    [AllowAnonymous]
    [HttpPost("/forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        var email = await _forgotPasswordService.GetByEmailAsync(model.email);
        
        if (email is null)
        {
            return BadRequest();
        }
        
        var guid = Guid.NewGuid().ToString();

        await _forgotPasswordService.SolicitarTrocaSenhaAsync(model.email, guid);
        
        await _forgotPasswordService.NotificarTrocaSenhaAsync(model, guid);
 
        return Ok(email);
    }
    [AllowAnonymous]
    [HttpPut("reset-password/{guid}")]
    public async Task<IActionResult> ResetPassword( ResetPasswordViewModel model)
    {
        var isUpdated = await _forgotPasswordService.GetByForgotPasswordAsync(model);
        
        if (!isUpdated)
        {
            return BadRequest();
        }
  
        return Ok();
    }
}