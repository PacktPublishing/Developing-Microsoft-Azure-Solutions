using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shrinkify.Web
{
    public class AllowedContentTypeAttribute : ValidationAttribute
    {
        private readonly List<string> _contentTypes;

        public AllowedContentTypeAttribute(string[] contentTypes)
        {
            _contentTypes = new List<string>();

            foreach (var contentType in contentTypes)
            {
                _contentTypes.Add(contentType.ToLower());
            }
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var contentType = file.ContentType.ToLower();

                if (!_contentTypes.Contains(contentType))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This file content type is not supported.";
        }
    }
}
