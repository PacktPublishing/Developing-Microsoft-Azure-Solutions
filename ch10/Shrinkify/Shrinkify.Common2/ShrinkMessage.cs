using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace Shrinkify
{
    public class ShrinkMessage
    {
        private string _folder;
        private ShrinkImage _image;
        
        public ShrinkMessage()
        {
            _folder = Guid.NewGuid().ToString();
        }

        public ShrinkMessage(string folder, string imageUrl)
        {
            CheckIsNotNullOrWhitespace(nameof(folder), folder);

            _image = new ShrinkImage(imageUrl);
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

        public ShrinkImage Image
        {
            get { return _image; }
            set
            {
                CheckIsNotNull(nameof(Image), value);
                _image = value;
            }
        }
    }
}
