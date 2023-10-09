using apiemail.Model;
using hangfireapi.Service.Email;
using Microsoft.AspNetCore.Mvc;

namespace apiemail.Controllers;
[ApiController]
[Route("SendEmail")]
public class SendController : ControllerBase
{
    private readonly ISendEmailService _sendEmailService;

    public SendController(ISendEmailService sendEmailService)
    {
        _sendEmailService = sendEmailService;
    }
    
    [HttpPost]
    public Task<IActionResult> SendEmail(Send email)
    {
        var message = _sendEmailService.SendEmail(email);
    
        return message == false ? Task.FromResult<IActionResult>(BadRequest()) : Task.FromResult<IActionResult>(Ok("Your message has been sent successfully"));
    }
}