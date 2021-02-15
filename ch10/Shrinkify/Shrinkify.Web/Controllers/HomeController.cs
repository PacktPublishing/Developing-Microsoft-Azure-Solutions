using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shrinkify.Web.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;
using static Shrinkify.ShrinkifyExtensions;
using static Shrinkify.BlobOperations;
using static Shrinkify.QueueOperations;
using System.Net;

namespace Shrinkify.Web.Controllers
{
    public class HomeControllerDependencies : Dependencies<HomeController>
    {
        public HomeControllerDependencies(ILogger<HomeController> logger, AppSettings settings) : base(logger, settings)
        {

        }
    }

    public class WaitForImage
    {
        public string Image { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly IDependencies<HomeController> _d;

        public HomeController(IDependencies<HomeController> dependencies)
        {
            CheckIsNotNull(nameof(dependencies), dependencies);

            _d = dependencies;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("wait")]
        public async Task WaitForImage([FromBody] WaitForImage waitForImage)
        {
            CheckIsNotNull(nameof(waitForImage), waitForImage);

            Func<Task<bool>> check = async () =>
            {
                return await FileOrFolderExists(_d.Settings.StorageAccount, "images", waitForImage.Image);
            };

            try
            {
                await using (var sb = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, "notifications", Guid.NewGuid().ToString(), waitForImage.Image, check))
                {
                    _d.Logger.LogDebug("Entering wait for image.");
                    await sb.WaitAsync();
                }
            }
            catch (Exception ex)
            {
                _d.Logger.LogError(ex.Message);
                // DO NOT RETHROW
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(UploadViewModel model)
        {
            CheckIsNotNull(nameof(model), model);

            var image = model.Upload.Image;

            try
            {
                long size = image.Length;

                // full path to file in temp location
                var imageExt = GetImageExtension(image.FileName);
                var imageName = GetTempFileName(imageExt);
                var imageInfo = new FileInfo(imageName);
                Uri imageUri = null;
                string folder = model.RequestId.ToString();
                string imageShortName = imageInfo.Name;

                if (image.Length > 0)
                {
                    // Removed for brevity
                    using (var stream = image.OpenReadStream())
                    {
                        imageUri = await UploadBlobAsync(_d.Settings.StorageAccount, "images", $"{folder}/{imageShortName}", stream, image.ContentType);
                    }

                    var shrinkMessage = new ShrinkMessage { Image = new ShrinkImage(folder, imageShortName, $"{imageUri}") };

                    await SendMessageAsync(_d.Settings.StorageAccount, "shrink", shrinkMessage);
                }

                var vm = new ConvertViewModel
                {
                    Folder = folder,
                    OriginalImageUrl = imageUri.ToString(),
                    NewImageUrl = imageUri.ToString().Replace(imageExt, ".webp")
                };

                return View("Convert", vm);
            }
            catch (Exception ex)
            {
                _d.Logger.LogError(ex.Message);
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
