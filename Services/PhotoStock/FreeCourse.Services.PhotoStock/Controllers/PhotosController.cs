using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            try
            {
                  var now = DateTime.Now;
                  var extension = Path.GetExtension(photo.FileName).ToLower();
                  var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos");
                  var relativeFolder = Path.Combine(now.Year.ToString(), now.Month.ToString(), now.Day.ToString());
                  var folderPath = Path.Combine(rootPath, relativeFolder);

                  if (!Directory.Exists(folderPath))
                  {
                      Directory.CreateDirectory(folderPath);
                  }

                  var fileName = $"{Guid.NewGuid().ToString().Substring(0, 8)}-{Path.GetFileNameWithoutExtension(photo.FileName)}{extension}";
                  var fullPath = Path.Combine(folderPath, fileName);

                  using var stream = new FileStream(fullPath, FileMode.Create);
                  await photo.CopyToAsync(stream, cancellationToken); // fotoyu kaydet

                var returnPath = $"{relativeFolder}/{fileName}".Replace("\\", "/");

                  PhotoDto photoDto = new() { Url = returnPath };

                  return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            } catch (Exception ex)
                {
                return CreateActionResultInstance(Response<PhotoDto>.Fail($"photo is empty. {ex.Message}", 400));
            }
        }

        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            try
            {
                var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos");
                var folderPath = Path.Combine(rootPath, photoUrl);
                if (!System.IO.File.Exists(folderPath))
                {
                    return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
                }

                System.IO.File.Delete(folderPath);

                return CreateActionResultInstance(Response<NoContent>.Success(204));
            }
            catch (Exception ex)
            {
                return CreateActionResultInstance(Response<NoContent>.Fail($"photo is empty. {ex.Message}", 400));
            }
        }
    }
}
