using apiemail.Service.AWSService;
using Hangfire;
using hangfireapi.Service.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hangfireapi.Controllers;

[AllowAnonymous]
[Route("aws")]
public class SendFileS3Controller : ControllerBase
{
    private readonly ISendMailService _sendMailService;
    private readonly IAwsService _awsService;

    public SendFileS3Controller(IAwsService awsService, ISendMailService sendMailService)
    {
        _awsService = awsService;
        _sendMailService = sendMailService;
    }
    [HttpGet]
    public async Task<IActionResult> GetBucket()
    {
        var buckets = await _awsService.ListBucketAsync();

        return Ok(buckets);
    }
    [HttpPost]
    public async Task<IActionResult> CreateBucket(string nameBucket)
    {
        var bucket = await _awsService.CreateBucketAsync(nameBucket);

        if (bucket is null)
        {
            return BadRequest("Could not create bucket");
        }
        
        return Ok(bucket);
    }
    [AllowAnonymous]
    [HttpPost("uploadfile")]
    public async Task<IActionResult> UpLoadAsync(List<IFormFile> files, string nameBucket)
    {
        var file = await _awsService.UploadFileInS3(files, nameBucket);

        if (file == false)
        {
            return BadRequest();
        }

        return Ok(file);
    }

    [HttpDelete]
    public Task<IActionResult> DeleteFileAsync(List<string> file, string nameBucket)
    {
        var deleteFile = BackgroundJob.Enqueue<IAwsService>(a => a.DeleteFileAsync(file, nameBucket) );

        BackgroundJob.ContinueJobWith(deleteFile, () => _sendMailService.SendEmail("Arquivo Deletado", "Seus arquivos foram deletados com sucesso"));
        
        return Task.FromResult<IActionResult>(Ok(true));
    }
}