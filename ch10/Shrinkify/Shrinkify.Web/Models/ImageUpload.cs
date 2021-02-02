using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shrinkify.Web.Models
{
    public class ImageUpload
    {
        [Required]
        [Display(Name = "Image")]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtensions(new[] { ".jpeg", ".png" })]
        [AllowedContentType(new[] { "image/jpeg", "image/png" })]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}
