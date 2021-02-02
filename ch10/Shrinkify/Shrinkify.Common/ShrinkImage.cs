using System;
using static Shrinkify.ShrinkifyExtensions;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public class ShrinkImage
    {
        private string _imageUrl;
        private string _fileName;
        private string _folder;

        public ShrinkImage()
        {
            _folder = string.Empty;
            _fileName = string.Empty;
            _imageUrl = string.Empty;
        }

        public ShrinkImage(string folder, string fileName, string imageUrl)
        {
            CheckIsNotNullOrWhitespace(nameof(folder), folder);
            CheckIsNotNullOrWhitespace(nameof(fileName), fileName);
            Validate(nameof(imageUrl), imageUrl);

            _folder = folder;
            _fileName = fileName;
            _imageUrl = imageUrl;
        }

        public string Folder
        {
            get { return _folder; }
            set
            {
                CheckIsNotNullOrWhitespace(nameof(Folder), value);
                _folder = value;
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                CheckIsNotNullOrWhitespace(nameof(FileName), value);

                _fileName = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                Validate(nameof(ImageUrl), value);
                _imageUrl = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ShrinkImage other))
                return false;

            return _imageUrl.Equals(other._imageUrl) &&
                _fileName.Equals(other._fileName) &&
                _folder.Equals(other._folder);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_imageUrl, _fileName, _folder);
        }

        public override string ToString()
        {
            return _imageUrl;
        }
    }
}
