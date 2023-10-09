using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;
namespace apiemail.Controllers;

public class UpLoad : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files,
        [FromServices] IWebHostEnvironment hostingEnvironment)
    {
        var tipoPermitidos = new List<string>()
        {
            "application/pdf",

        };

        var projectFolderPath = Path.Combine(hostingEnvironment.ContentRootPath, "Files");

        if (!Directory.Exists(projectFolderPath))
        {
            Directory.CreateDirectory(projectFolderPath);
        }

        foreach (var formFile in files)
        {
            if (formFile.Length > 0)
            {
                if (tipoPermitidos.All(tipo => tipo != formFile.ContentType))
                {
                    return BadRequest();
                }

                var fileName = Path.GetFileName(formFile.FileName);

                var newFileName = Guid.NewGuid() + "." + fileName.Split(".")[1];

                var filePath = Path.Combine(projectFolderPath, newFileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await formFile.CopyToAsync(stream); 
                }

            }
        }

        return Ok(new { count = files.Count });
    }
    
    private static async Task<ListBucketsResponse> ListBucketsAsync()
    {
        var s3Client = new AmazonS3Client();
        var response = await s3Client.ListBucketsAsync();
        return response;
    }

    /*
     * Salvar no banco o filePath, fileName
     */
    // mensagem...
    /*
     * 1. Tipo arquivo... .pdf, .doc, docx, .xls, xlsx,  .jpeg, .jpg, .png, .git, .zip, .rar
     * 2. Tamanho arquivo -> 10MB
     * 3. Bloqueio do arquivo para download sem autenticacao
     * 4. Tomar cuidado para nao sobrescrever arquivo. Mesmo nome.
     */
}