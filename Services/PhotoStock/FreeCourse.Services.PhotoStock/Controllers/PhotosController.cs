using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Services.PhotoStock.Utils;
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
                if (photo != null && photo.Length > 0)
                {
                    var now = DateTime.Now;
                    var relativeFolder = $"\\{now.Year}\\{now.Month}\\{now.Day}";
                    var fileName =
                        $"{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8)}-{photo.FileName.ToLink()}";
                    var relativePath = Path.Combine(relativeFolder, fileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", relativePath);

                    using var stream = new FileStream(path, FileMode.Create);
                    await photo.CopyToAsync(stream, cancellationToken);

                    var returnPath = "photos/" + path;

                    PhotoDto photoDto = new() { Url = returnPath };

                    return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
                }
            } catch (Exception ex)
                {
                return CreateActionResultInstance(Response<PhotoDto>.Fail($"photo is empty. {ex.Message}", 400));
            }
            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty.", 400));
        }

        public IActionResult PhotoDelete(string photoUrl)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
                if (!System.IO.File.Exists(path))
                {
                    return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
                }

                System.IO.File.Delete(path);

                return CreateActionResultInstance(Response<NoContent>.Success(204));
            }
            catch (Exception ex)
            {
                return CreateActionResultInstance(Response<NoContent>.Fail($"photo is empty. {ex.Message}", 400));
            }
        }
    }
}
