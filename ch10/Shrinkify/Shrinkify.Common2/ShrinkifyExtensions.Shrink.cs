using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static Shrinkify.BlobOperations;

namespace Shrinkify
{
    public static partial class ShrinkifyExtensions
    {
        public static async Task<ShrunkImage> ShrinkImageAsync(ShrinkImage image, AppSettings settings)
        {
            ShrunkImage result;
            var filesToDelete = new List<FileInfo>();

            try
            {
                string fileExtension = GetImageExtension(image.ImageUrl);
                string filename = GetTempFileName(fileExtension);
                var fileToShrink = new FileInfo(filename);
                filesToDelete.Add(fileToShrink);

                // Download image
                var downloadPsi = new ProcessStartInfo { FileName = "curl" };
                downloadPsi.Arguments = $@"{image} -o {fileToShrink}";

                using (var download = Process.Start(downloadPsi))
                {
                    download.Start();
                    await download.WaitForExitAsync();
                    download.EnsureSuccess();
                }

                // Shrink image
                filename = GetTempFileName(".webp");
                var shrunkFile = new FileInfo(filename);
                filesToDelete.Add(shrunkFile);

                var convertPsi = new ProcessStartInfo { FileName = "cwebp" };
                convertPsi.Arguments = $@"{fileToShrink} -q 75 -o {shrunkFile}";

                using (var shrink = Process.Start(convertPsi))
                {
                    shrink.Start();
                    await shrink.WaitForExitAsync();
                    shrink.EnsureSuccess();
                }

                var shrunkUri = await UploadBlobAsync(settings.StorageAccount, "images", shrunkFile.FullName, contentType: "image/webp");

                result = new ShrunkImage(image.ImageUrl, shrunkUri.ToString());
            }
            finally
            {
                //  Delete all files created during this operation
                filesToDelete.EnsureDelete();
            }

            return result;
        }
    }
}
