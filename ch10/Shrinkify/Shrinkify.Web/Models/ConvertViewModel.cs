using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shrinkify.Web.Models
{
    public class ConvertViewModel
    {
        public ConvertViewModel()
        {
        }

        public string OriginalImageUrl { get; set; }

        public string NewImageUrl { get; set; }
    }
}
