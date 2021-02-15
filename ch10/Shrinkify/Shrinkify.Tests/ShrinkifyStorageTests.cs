using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Shrinkify.BlobOperations;
using static Shrinkify.TestManager;

namespace Shrinkify.Functional.Tests
{
    [TestClass]
    public class ShrinkifyStorageTests
    {
        private readonly IDependencies<Test> _d;

        public ShrinkifyStorageTests()
        {
            _d = GetService<IDependencies<Test>>();
        }

        [TestMethod]
        public async Task UploadPngTest()
        {
            await UploadBlobAsync(_d.Settings.StorageAccount, "images", "image.png", "image/png", forceDelete: true);
        }

        [TestMethod]
        public async Task UploadJpgTest()
        {
            await UploadBlobAsync(_d.Settings.StorageAccount, "images", "image.jpg", "image/jpeg", forceDelete: true);
        }

        [TestMethod]
        public async Task UploadJpegTest()
        {
            await UploadBlobAsync(_d.Settings.StorageAccount, "images", "image.jpeg", "image/jpeg", forceDelete: true);
        }

        [TestMethod]
        public async Task UploadStreamPngTest()
        {
            using (var s = File.OpenRead("image.png"))
            {
                await UploadBlobAsync(_d.Settings.StorageAccount, "images", "image.png", s, "image/png", forceDelete: true);
            }
        }

        [TestMethod]
        public async Task UploadStreamJpgTest()
        {
            using (var s = File.OpenRead("image.jpg"))
            {
                await UploadBlobAsync(_d.Settings.StorageAccount, "images", "image.jpg", s, "image/jpeg", forceDelete: true);
            }
        }

        [TestMethod]
        public async Task UploadStreamJpegTest()
        {
            using (var s = File.OpenRead("image.jpeg"))
            {
                await UploadBlobAsync(_d.Settings.StorageAccount, "images", "image.jpeg", s, "image/jpeg", forceDelete: true);
            }
        }

        [TestMethod]
        public async Task UploadToFolderTest()
        {
            var folder = Guid.NewGuid();

            using (var s = File.OpenRead("image.jpeg"))
            {
                await UploadBlobAsync(_d.Settings.StorageAccount, "images", $"{folder}/image.jpeg", s, "image/jpeg", forceDelete: true);
            }
        }

        [TestMethod]
        public async Task UploadAndCheckFileExistsTest()
        {
            bool exists;
            var folder = Guid.NewGuid();
            var filename = $"{folder}/image.jpeg";

            using (var s = File.OpenRead("image.jpeg"))
            {
                await UploadBlobAsync(_d.Settings.StorageAccount, "images", filename, s, "image/jpeg", forceDelete: true);
                exists = await FileOrFolderExists(_d.Settings.StorageAccount, "images", filename);
            }

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task UploadAndCheckFolderExistsTest()
        {
            bool exists;
            var folder = Guid.NewGuid();
            var filename = $"{folder}/image.jpeg";

            using (var s = File.OpenRead("image.jpeg"))
            {
                await UploadBlobAsync(_d.Settings.StorageAccount, "images", filename, s, "image/jpeg", forceDelete: true);
                exists = await FileOrFolderExists(_d.Settings.StorageAccount, "images", $"{folder}");
            }

            Assert.IsTrue(exists);
        }
    }
}
