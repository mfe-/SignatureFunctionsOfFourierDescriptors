// See https://aka.ms/new-console-template for more information
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


//https://www.kaggle.com/code/stkbailey/teaching-notebook-for-total-imaging-newbies

var workingDir = @"D:\Kfk-Praktikum\";

var imagePath = Path.Combine(workingDir, "0114f484a16c152baa2d82fdd43740880a762c93f436c8988ac461c5c9dbe7d5.png");
var solutionPath = Path.Combine(workingDir, "stage1_solution.csv");

var lines = File.ReadAllLines(solutionPath).Where(a => a.StartsWith(new FileInfo(imagePath).Name.Replace(".png", "")));
using var image = await Image.LoadAsync<Rgba32>(imagePath);

var red = new Rgba32(255, 0, 0); // Define border color

foreach (var line in lines)
{
    var parts = line.Split(',');
    var encodedPixels = parts[1].Split(' ');

    for (int i = 0; i < encodedPixels.Length; i += 2)
    {
        int start = int.Parse(encodedPixels[i]);
        int length = int.Parse(encodedPixels[i + 1]);

        // Decode the RLE
        for (int j = 0; j < length; j++)
        {
            int pixelIndex = start + j - 1;
            int y = pixelIndex / image.Width;
            int x = pixelIndex % image.Width;

            if (x < image.Width && y < image.Height)
            {
                image[x, y] = red;
            }
        }
    }
}


var outputImagePath = Path.Combine(workingDir, "output.png");
image.Save(outputImagePath); // Save the modified image with borders

Console.WriteLine("Border drawn successfully. Output image saved at: " + outputImagePath);