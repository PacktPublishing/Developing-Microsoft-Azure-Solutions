using System;
using static Shrinkify.ShrinkifyExtensions;

#nullable enable

namespace Shrinkify
{

    public class ShrunkImage
    {
        private string _imageUrl;
        private string _shrunkImageUrl;

        public ShrunkImage()
        {
            _imageUrl = string.Empty;
            _shrunkImageUrl = string.Empty;
        }

        public ShrunkImage(string imageUrl, string shrunkImageUrl)
        {
            Validate(nameof(imageUrl), imageUrl);
            Validate(nameof(shrunkImageUrl), shrunkImageUrl);

            _imageUrl = imageUrl;
            _shrunkImageUrl = shrunkImageUrl;
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                Validate(nameof(ImageUrl), value);
                _imageUrl = value;
            }
        }

        public string ShrunkImageUrl
        {
            get { return _shrunkImageUrl; }
            set
            {
                Validate(nameof(ShrunkImageUrl), value);
                _shrunkImageUrl = value;
            }
        }
    }
}
