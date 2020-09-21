using System.IO;
using System;

namespace DeckOfCards.ImageGallery
{
    public class ImageItem
    {
        public ImageItem(string filename)
        {
            FullName = filename;
            FileName = new FileInfo(filename).Name;
        }

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(FullName);
            }
        }

        public string FileName { get; set; }

        public string FullName { get; set; }
    }
}
