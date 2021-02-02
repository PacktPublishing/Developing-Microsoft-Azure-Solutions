using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static Shrinkify.BlobOperations;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public static partial class ShrinkifyExtensions
    {
        public static async Task<ShrunkImage> ShrinkImageAsync(ShrinkImage image, AppSettings settings)
        {
            CheckIsNotNull(nameof(image), image);
            CheckIsNotNull(nameof(settings), settings);

            ShrunkImage result;
            var filesToDelete = new List<FileInfo>();

            try
            {
                string fileExtension = GetImageExtension(image.ImageUrl);
                var fileToShrink = new FileInfo(image.FileName);
                filesToDelete.Add(fileToShrink);

                // Download image
                var downloadPsi = new ProcessStartInfo { FileName = "curl" };
                downloadPsi.Arguments = $@"{image} -o {fileToShrink}";

                using (var download = Process.Start(downloadPsi))
                {
                    download.Start();
                    download.WaitForExit();
                    download.EnsureSuccess();
                }

                // Shrink image
                var filename = GetFileNameWithNewExtension(image.FileName, ".webp");
                var shrunkFile = new FileInfo(filename);
                filesToDelete.Add(shrunkFile);

                var convertPsi = new ProcessStartInfo { FileName = "cwebp" };
                convertPsi.Arguments = $@"{fileToShrink} -q 75 -o {shrunkFile}";

                using (var shrink = Process.Start(convertPsi))
                {
                    shrink.Start();
                    shrink.WaitForExit();
                    shrink.EnsureSuccess();
                }

                Uri shrunkUri;

                using (var stream = shrunkFile.OpenRead())
                {
                    shrunkUri = await UploadBlobAsync(settings.StorageAccount, "images", $"{image.Folder}/{shrunkFile.Name}", stream, contentType: "image/webp");
                }

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
