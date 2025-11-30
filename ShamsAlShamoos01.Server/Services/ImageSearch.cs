using ShamsAlShamoos01.Infrastructure.Services;
using System;

namespace ShamsAlShamoos01.Server.Services
{
    public class ImageSearch
    {
        private readonly IImageSearchService _imageSearchService;

        public ImageSearch(IImageSearchService imageSearchService)
        {
            _imageSearchService = imageSearchService;
        }

        public void Test()
        {
            string target = @"C:\MyProjects\BlazorCleanArchitecture\ShamsAlShamoos01\Server\QrFiles\qr_638998171860254153.png";
            string folder = @"C:\MyProjects\BlazorCleanArchitecture\ShamsAlShamoos01\Server\QrFiles";

            var similar = _imageSearchService.FindSimilarImages(target, folder);
            foreach (var file in similar)
                Console.WriteLine(file);
        }
    }
}
