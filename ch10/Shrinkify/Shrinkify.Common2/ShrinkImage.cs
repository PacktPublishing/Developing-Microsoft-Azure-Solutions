using System;
using static Shrinkify.ShrinkifyExtensions;

#nullable enable

namespace Shrinkify
{
    public class ShrinkImage
    {
        private string _imageUrl;

        public ShrinkImage()
        {
            _imageUrl = string.Empty;
        }

        public ShrinkImage(string imageUrl)
        {
            Validate(nameof(imageUrl), imageUrl);

            _imageUrl = imageUrl;
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

        public override bool Equals(object? obj)
        {
            if (obj is not ShrinkImage other)
                return false;

            return _imageUrl.Equals(other._imageUrl);
        }

        public override int GetHashCode()
        {
            return _imageUrl.GetHashCode();
        }

        public override string ToString()
        {
            return _imageUrl;
        }
    }
}
