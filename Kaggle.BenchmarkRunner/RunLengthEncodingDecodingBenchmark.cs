using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Fourier.ConsoleApp1;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Kaggle.BenchmarkRunner
{
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    //[SimpleJob(RuntimeMoniker.NativeAot80)]
    public class RunLengthEncodingDecodingBenchmark
    {
        private readonly RunLengthEncodingDecoding runLengthEncoding = new();
        private Image<Rgba32>? image;
        private IEnumerable<string>? lines;

        [GlobalSetup]
        public void Setup()
        {
            var workingDir = @"D:\Kfk-Praktikum\";

            var imagePath = Path.Combine(workingDir, "0114f484a16c152baa2d82fdd43740880a762c93f436c8988ac461c5c9dbe7d5.png");
            var solutionPath = Path.Combine(workingDir, "stage1_solution.csv");

            lines = File.ReadAllLines(solutionPath).Where(a => a.StartsWith(new FileInfo(imagePath).Name.Replace(".png", "")));
            image = Image.Load<Rgba32>(imagePath);
        }

        [Benchmark]
        public void MarkImageUsingRunLengthEncoding() => runLengthEncoding.MarkImageUsingRunLengthDecoding(lines!, image!, new Rgba32(255, 0, 0));

        [GlobalCleanup()]
        public void Cleanup_Image() => image!.Dispose();
    }
}
