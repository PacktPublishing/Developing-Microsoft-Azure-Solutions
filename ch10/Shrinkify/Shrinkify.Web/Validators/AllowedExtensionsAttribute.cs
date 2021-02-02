using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shrinkify.Web
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly List<string> _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = new List<string>();

            foreach (var e in extensions)
            {
                _extensions.Add(e.ToLower());
            }
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This file extension is not allowed!";
        }
    }
}
