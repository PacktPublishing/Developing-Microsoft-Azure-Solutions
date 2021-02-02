using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Pineapple.Common.Preconditions;
using static Shrinkify.ShrinkifyExtensions;
using static Shrinkify.BlobOperations;
using Microsoft.Extensions.Logging;

namespace Shrinkify
{
    public class ShrinkDependencies : Dependencies<ShrinkController>
    {
        public ShrinkDependencies(ILogger<ShrinkController> logger, AppSettings settings)
            : base(logger, settings)
        {

        }
    }

    [ApiController]
    [Route("[controller]")]
    public class ShrinkController : ControllerBase
    {
        private readonly IDependencies<ShrinkController> _d;

        public ShrinkController(IDependencies<ShrinkController> dependencies)
        {
            CheckIsNotNull(nameof(dependencies), dependencies);

            _d = dependencies;
        }

        [HttpPost]
        public async Task<ShrunkImage> Shrink(ShrinkImage image)
        {
            CheckIsNotNull(nameof(image), image);

            ShrunkImage result;

            try
            {
                result = await ShrinkImageAsync(image, _d.Settings);
            }
            catch (Exception ex)
            {
                _d.Logger.LogError(ex.Message);
                throw;
            }

            return result;
        }
    }
}
