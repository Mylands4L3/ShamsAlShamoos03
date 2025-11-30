using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShamsAlShamoos01.Infrastructure.Services
{
    public interface IFaceRecognitionService
    {
        List<string> FindSimilarFaces(string targetImagePath, string folderPath, double threshold = 0.6);
    }

    public class FaceRecognitionService : IFaceRecognitionService
    {
        private readonly Net _faceDetector;
        private readonly Net _faceEmbedder;

        public FaceRecognitionService()
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "face_models");

            // Load face detector
            string protoPath = Path.Combine(basePath, "deploy.prototxt");
            string modelPath = Path.Combine(basePath, "res10_300x300_ssd_iter_140000.caffemodel");
            if (!File.Exists(protoPath) || !File.Exists(modelPath))
                throw new FileNotFoundException("Face detector model files missing");

            _faceDetector = CvDnn.ReadNetFromCaffe(protoPath, modelPath);

            // Load face embedder
            string embedderPath = Path.Combine(basePath, "openface_nn4.small2.v1.t7");
            if (!File.Exists(embedderPath))
                throw new FileNotFoundException("Face embedder model file missing");

            _faceEmbedder = CvDnn.ReadNetFromTorch(embedderPath);
        }

        public List<string> FindSimilarFaces(string targetImagePath, string folderPath, double threshold = 0.6)
        {
            var matches = new List<string>();
            string resultFolder = @"D:\upload\Result01";

            // بررسی وجود فایل هدف
            if (!File.Exists(targetImagePath))
            {
                Console.WriteLine("Target image file not found.");
                return matches;
            }

            // ایجاد پوشه نتیجه در صورت عدم وجود
            if (!Directory.Exists(resultFolder))
                Directory.CreateDirectory(resultFolder);

            var targetEmbeddings = GetEmbeddings(targetImagePath, out var targetFacesImagePath);

            if (targetEmbeddings == null || targetEmbeddings.Count == 0)
            {
                Console.WriteLine("No face detected in target image.");
                return matches;
            }

            // فقط از اولین چهره در تصویر هدف استفاده می‌کنیم
            var targetEmbedding = targetEmbeddings[0];

            foreach (var file in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(f => f.ToLower().EndsWith(".jpg") || f.ToLower().EndsWith(".jpeg") ||
                           f.ToLower().EndsWith(".png") || f.ToLower().EndsWith(".bmp")))
            {
                var embeddings = GetEmbeddings(file, out var facesImgPath);
                if (embeddings == null || embeddings.Count == 0)
                {
                    Console.WriteLine($"No face detected in file: {Path.GetFileName(file)}");
                    continue;
                }

                foreach (var emb in embeddings)
                {
                    double dist = CosineDistance(targetEmbedding, emb);
                    Console.WriteLine($"Distance to {Path.GetFileName(file)}: {dist:F4}");

                    if (dist < threshold)
                    {
                        matches.Add(file);

                        // کپی فایل به پوشه نتیجه
                        string destPath = Path.Combine(resultFolder, Path.GetFileName(file));
                        try
                        {
                            File.Copy(file, destPath, overwrite: true);
                            Console.WriteLine($"Copied {Path.GetFileName(file)} to result folder.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to copy {Path.GetFileName(file)}: {ex.Message}");
                        }

                        break; // اگر یک چهره مشابه پیدا شد، به فایل بعدی برو
                    }
                }
            }

            return matches;
        }

        private List<Rect> DetectFaces(Mat img, float confidenceThreshold = 0.2f)
        {
            var faces = new List<Rect>();

            Mat blob = CvDnn.BlobFromImage(img, 1.0, new Size(300, 300),
                new Scalar(104, 177, 123), false, false);

            _faceDetector.SetInput(blob);
            Mat detections = _faceDetector.Forward();

            int width = img.Cols;
            int height = img.Rows;

            for (int i = 0; i < detections.Size(2); i++)
            {
                float confidence = detections.At<float>(0, 0, i, 2);
                if (confidence > confidenceThreshold)
                {
                    int x1 = (int)(detections.At<float>(0, 0, i, 3) * width);
                    int y1 = (int)(detections.At<float>(0, 0, i, 4) * height);
                    int x2 = (int)(detections.At<float>(0, 0, i, 5) * width);
                    int y2 = (int)(detections.At<float>(0, 0, i, 6) * height);

                    // اطمینان از قرار گرفتن مختصات در محدوده تصویر
                    x1 = Math.Max(0, Math.Min(x1, width - 1));
                    y1 = Math.Max(0, Math.Min(y1, height - 1));
                    x2 = Math.Max(0, Math.Min(x2, width - 1));
                    y2 = Math.Max(0, Math.Min(y2, height - 1));

                    int faceWidth = x2 - x1;
                    int faceHeight = y2 - y1;

                    // حذف فیلتر برای چهره‌های کوچک
                    if (faceWidth > 5 && faceHeight > 5)
                    {
                        faces.Add(new Rect(x1, y1, faceWidth, faceHeight));
                    }
                }
            }

            return faces;
        }

        private List<float[]> GetEmbeddings(string imagePath, out string debugImagePath)
        {
            Mat img = Cv2.ImRead(imagePath);
            debugImagePath = Path.Combine(Path.GetTempPath(), "debug_" + Path.GetFileName(imagePath));

            if (img.Empty())
            {
                Console.WriteLine($"Failed to read image: {imagePath}");
                return null;
            }

            var faces = DetectFaces(img);
            if (faces.Count == 0)
                return null;

            var embeddings = new List<float[]>();

            foreach (var face in faces)
            {
                Mat faceImg = new Mat(img, face);

                // پیش‌پردازش برای embedder
                Mat blob = CvDnn.BlobFromImage(faceImg, 1.0 / 255, new Size(96, 96),
                    new Scalar(0, 0, 0), true, false);

                _faceEmbedder.SetInput(blob);
                Mat output = _faceEmbedder.Forward();

                // تبدیل خروجی به آرایه
                output.GetArray(out float[] embedding);

                NormalizeVector(embedding);
                embeddings.Add(embedding);

                // رسم چهره‌ها روی تصویر برای بررسی
                Cv2.Rectangle(img, face, Scalar.Red, 2);
            }

            // ذخیره تصویر برای دیباگ
            Cv2.ImWrite(debugImagePath, img);

            return embeddings;
        }

        private void NormalizeVector(float[] vector)
        {
            double sum = 0;
            for (int i = 0; i < vector.Length; i++)
                sum += vector[i] * vector[i];

            double norm = Math.Sqrt(sum);
            if (norm > 1e-6)
            {
                for (int i = 0; i < vector.Length; i++)
                    vector[i] = (float)(vector[i] / norm);
            }
        }

        private double CosineDistance(float[] a, float[] b)
        {
            if (a.Length != b.Length)
                return 1.0;

            double dot = 0, na = 0, nb = 0;
            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                na += a[i] * a[i];
                nb += b[i] * b[i];
            }

            na = Math.Sqrt(na);
            nb = Math.Sqrt(nb);

            if (na < 1e-6 || nb < 1e-6)
                return 1.0;

            double similarity = dot / (na * nb);
            return 1.0 - similarity;
        }
    }
}
