using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static Pineapple.Common.Preconditions;
using static Pineapple.Common.Cleanup;

#nullable enable

namespace Shrinkify
{
    public static partial class ShrinkifyExtensions
    {
        private static List<string> _supportedImages;

        static ShrinkifyExtensions()
        {
            _supportedImages = new List<string> { ".jpeg", ".jpg", ".png" };
        }

        public static void EnsureSuccess(this Process process)
        {
            CheckIsNotNull(nameof(process), process);

            if (process.ExitCode != 0)
                throw new Exception($@"Process failed with exit code [{process.ExitCode}].");
        }

        public static void Validate(string name, string? url)
        {
            CheckIsNotNullOrWhitespace(name, url);
            CheckIsWellFormedUri(name, url, UriKind.Absolute);
        }

        public static string GetImageExtension(string fileOrUrl)
        {
            CheckIsNotNullOrWhitespace(nameof(fileOrUrl), fileOrUrl);

            var lowerCaseUrl = fileOrUrl.ToLower();
            var parts = lowerCaseUrl.Split('.');
            var num = parts.Length;

            CheckIsNotLessThanOrEqualTo(nameof(fileOrUrl), num, 1);

            var fileExtension = parts[num - 1];

            if (IsValidateImageExtension(fileExtension))
                return fileExtension;

            foreach (var extension in _supportedImages)
            {
                if (lowerCaseUrl.Contains(extension))
                {
                    return extension;
                }
            }

            throw new NotSupportedException($"[{fileExtension}] is not supported.");
        }

        public static void ValidateImageExtension(string extension)
        {
            if (!_supportedImages.Contains(extension))
                throw new NotSupportedException($"[{extension}] is not supported.");
        }

        public static bool IsValidateImageExtension(string extension)
        {
            return _supportedImages.Contains(extension);
        }

        public static void EnsureDelete(this List<FileInfo> files)
        {
            foreach (var file in files)
            {
                SafeMethod(() => file.EnsureDelete());
            }
        }

        public static void EnsureDelete(this FileInfo file, bool removeTmp = true)
        {
            if (file.Exists)
                file.Delete();

            if (removeTmp)
            {
                string fileName = file.FullName.Replace(file.Extension, ".tmp");

                var tempFile = new FileInfo(fileName);

                if (tempFile.Exists)
                    tempFile.Delete();
            }
        }

        public static string GetTempFileName(string fileExtension)
        {
            string filename = Path.GetTempFileName();

            if (!string.IsNullOrWhiteSpace(fileExtension))
                filename = filename.Replace(".tmp", fileExtension);

            return filename;
        }

        public static string GetFileNameWithNewExtension(string fileName, string newextension)
        {
            var ext = GetImageExtension(fileName);
            return fileName.Replace(ext, newextension);
        }
    }
}
