using System;
using static Shrinkify.ShrinkifyExtensions;
using static Pineapple.Common.Preconditions;

#nullable enable

namespace Shrinkify
{

    public class ShrunkImage
    {
        private string _imageUrl;
        private string _shrunkImageUrl;
        private string _folder;

        public ShrunkImage()
        {
            _imageUrl = string.Empty;
            _shrunkImageUrl = string.Empty;
            _folder = string.Empty;
        }

        public ShrunkImage(string imageUrl, string shrunkImageUrl, string folder)
        {
            Validate(nameof(imageUrl), imageUrl);
            Validate(nameof(shrunkImageUrl), shrunkImageUrl);
            CheckIsNotNullOrWhitespace(nameof(folder), folder);

            _imageUrl = imageUrl;
            _shrunkImageUrl = shrunkImageUrl;
            _folder = folder;
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
