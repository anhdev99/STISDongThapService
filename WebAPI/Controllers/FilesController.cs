using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class FilesController(ILogger<FilesController> logger, IFileService fileService) : ApiControllerBase(logger)
{
    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        var id = await fileService.UploadFileAsync(file, cancellationToken);
        return Ok(new { id });
    }

    [HttpPost]
    [Route("delete/{fileId}")]
    public async Task<IActionResult> Delete(string fileId, CancellationToken cancellationToken)
    {
        try
        {
            await fileService.DeleteFileAsync(fileId, cancellationToken);
            return NoContent();
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpGet]
    [Route("download/{fileId}")]
    public async Task<ActionResult<dynamic>> Download(string fileId, CancellationToken cancellationToken)
    {
        var resultFile =  await fileService.DownloadByFileIdAsync(fileId, cancellationToken);;
        return File(resultFile.Data, "application/octet-stream", resultFile.FileName);
    }   
}