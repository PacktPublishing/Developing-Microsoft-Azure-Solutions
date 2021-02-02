using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shrinkify.Web.Models
{
    public class UploadViewModel
    {
        public UploadViewModel()
        {
            RequestId = Guid.NewGuid();
        }

        public Guid RequestId { get; set; }
        public ImageUpload Upload { get; set; }
    }
}
