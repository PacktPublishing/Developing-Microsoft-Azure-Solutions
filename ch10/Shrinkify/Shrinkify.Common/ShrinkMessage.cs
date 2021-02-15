using System;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public class ShrinkMessage
    {
        private DateTimeOffset _when;
        private ShrinkImage _image;
        
        public ShrinkMessage()
        {
        }

        public ShrinkMessage(ShrinkImage image, DateTimeOffset when)
        {
            CheckIsNotNull(nameof(image), image);

            When = _when;
            _image = image;
        }

        public ShrinkMessage(string folder, string fileName, string imageUrl, DateTimeOffset when)
        {
            When = _when;
            _image = new ShrinkImage(folder, fileName, imageUrl);
        }

        public DateTimeOffset When
        {
            get { return _when; }
            set { _when = value; }
        }

        public ShrinkImage Image
        {
            get { return _image; }
            set
            {
                CheckIsNotNull(nameof(Image), value);
                _image = value;
            }
        }

        public override bool Equals(object obj)
        {
            ShrinkMessage other = obj as ShrinkMessage;

            if (other == null)
                return false;

            return other.Image.Equals(Image);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Image);
        }
    }
}
