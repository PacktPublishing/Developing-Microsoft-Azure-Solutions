using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shrinkify.Web.Models
{
    public class HomeViewModel
    {
        [BindProperty]
        public ImageUpload ImageUpload { get; set; }
    }
}
