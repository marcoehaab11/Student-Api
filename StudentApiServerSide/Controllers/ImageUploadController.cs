using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controllers
{
    [Route("api/ImageUpload")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        [HttpPost("UploadImage",Name = "UploadImage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            //Check if no file is Upload
            if (imageFile==null||imageFile.Length==0)
            {
                return BadRequest("Invalid Data");
            }

            //dirc where file will uplaod
            var uploadDirctory = @"D:\images";
 
            //Genrate a uniqe filename
            var fileName=Guid.NewGuid().ToString()+Path.GetExtension(imageFile.FileName);
            var filepath=Path.Combine(uploadDirctory, fileName);

            //ensure the upload dircetory exsts , create if it dosent
            if (!Directory.Exists(uploadDirctory))
            {
                Directory.CreateDirectory(uploadDirctory);
            }

            using (var stream= new FileStream(filepath,FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);    
            }
            return Ok(new { filepath });

        }


        [HttpGet("GetImage/{fileName}",Name ="GetImage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetImage(string fileName) 
        {
            //Directory where files are stored
            var uploadDirctory = @"D:\images";
            var filePath=Path.Combine(uploadDirctory, fileName);

            //check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Image not found");
            }

            //open hte image file for reading 
            var image = System.IO.File.OpenRead(filePath);
            var mimeType = GetMimeType(filePath);

            return File(image, mimeType);
        }

        private string GetMimeType(string filePath)
        {
            var ext =Path.GetExtension(filePath).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }


    }
}
