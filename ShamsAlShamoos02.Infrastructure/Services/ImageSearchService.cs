using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShamsAlShamoos01.Infrastructure.Services
{

    public interface IImageSearchService
    {
        List<string> FindSimilarImages(string targetImagePath, string folderPath);
        List<string> FindSimilarImages(string targetImagePath, string folderPath, double threshold);
    }
    public class ImageSearchService : IImageSearchService
    {
        private const double DefaultThreshold = 1000000;

        public List<string> FindSimilarImages(string targetImagePath, string folderPath)
        {
            return FindSimilarImages(targetImagePath, folderPath, DefaultThreshold);
        }

        public List<string> FindSimilarImages(string targetImagePath, string folderPath, double threshold)
        {
            var similarImages = new List<string>();

            if (!File.Exists(targetImagePath) || !Directory.Exists(folderPath))
            {
                return similarImages;
            }
            Mat target = Cv2.ImRead(targetImagePath, ImreadModes.Grayscale);

            foreach (var file in Directory.GetFiles(folderPath))
            {
                Mat img = Cv2.ImRead(file, ImreadModes.Grayscale);

                if (img.Empty() || img.Size() != target.Size())
                {
                    continue;
                }
                Mat diff = new Mat();
                Cv2.Absdiff(target, img, diff);
                double error = Cv2.Sum(diff)[0];

                if (error < threshold)
                {
                    similarImages.Add(file);
                }
            }

            return similarImages;
        }
    }
}
