using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Infrastructure;
using MimeMapping;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.BlobStoring;
namespace MicroStore.Client.PublicWeb.Controllers
{
    [Route("api/cdn")]
    public class CDNController :AbpController
    {

        private IBlobContainer<MultiMediaBlobContainer> _blobContainer;

        //private const string STREAM_CONTENT_TYPE = @"application/octet-stream";
        public CDNController(IBlobContainer<MultiMediaBlobContainer> blobContainer)
        {
            _blobContainer = blobContainer;
        }

        [HttpGet]
        [Route("{fileName}")]
        public async Task<IActionResult> GetFileStream(string fileName)
        {
            var stream = await _blobContainer.GetAsync(fileName);

            return File(stream, MimeUtility.GetMimeMapping(fileName));
        }
    }
}
