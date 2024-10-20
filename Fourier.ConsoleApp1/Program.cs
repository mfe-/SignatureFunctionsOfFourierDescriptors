// See https://aka.ms/new-console-template for more information
using Fourier.ConsoleApp1;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;



var solutionPath = SetUpData.GetSolutionPath("stage1_solution.csv");
var stageFolder = SetUpData.GetStageFolderPath("stage1_test");

var images = SetUpData.FindAllPngImages(stageFolder);

var red = new Rgba32(255, 0, 0); // Define border color
foreach (var imagePath in images)
{
    var workingDir = Environment.CurrentDirectory;
    //find all related lines to the image from the solution file
    var lines = File.ReadAllLines(solutionPath).Where(a => a.StartsWith(new FileInfo(imagePath).Name.Replace(".png", "")));
    //copy imagePath to workingDir
    File.Copy(imagePath, Path.Combine(workingDir!, Path.GetFileName(imagePath)), true);

    // load the training image
    using var image = Image.Load<Rgba32>(imagePath);
    // generate output image path
    var imageMaskName = Path.Combine(workingDir!, new FileInfo(imagePath).Name.Replace(".png", ""));
    //run length encoding points
    var rle = new RunLengthEncodingDecoding();
    var maskedObjectPoints = rle.GetRunLengthDecoding(lines, image);
    //generate for each mask boundary tracing
    for (int i = 0; i < maskedObjectPoints.Count(); i++)
    {
        var points = maskedObjectPoints[i];

        var pXmax = points.Max(p => p.X);
        var pYmax = points.Max(p => p.Y);

        //generate a new image per mask
        Image<Rgba32> maskedImage = new(pXmax + 2, pYmax + 2);

        for (int j = 0; j < points.Count; j++)
        {
            var point = points[j];
            //mark the point in the image for the boundary tracing
            maskedImage[point.X, point.Y] = new Rgba32(255, 255, 255);
        }

        try
        {
            var contour = BoundaryProcessingTrace.BoundaryProcessingTraceAsync(maskedImage);

            if (new FileInfo(imagePath).Name == "0114f484a16c152baa2d82fdd43740880a762c93f436c8988ac461c5c9dbe7d5.png")
            {
                maskedImage.Save($"{imageMaskName}_mask_{i}.png", new PngEncoder());

                using var onlyCountourImage = new Image<Rgba32>(maskedImage.Width, maskedImage.Height);
                foreach (var point in contour)
                {
                    onlyCountourImage[point.X, point.Y] = new Rgba32(217, 30, 24, 255);
                }
                onlyCountourImage.SaveAsPng(imageMaskName + "_mask_contour_" + i + ".png");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{imageMaskName} {ex.Message}");
        }
    }

    imageMaskName = imageMaskName + "_mask.png";
    var outputImagePath = Path.Combine(workingDir!, imageMaskName);

    image.Save(outputImagePath); // Save the modified image with borders
    Console.WriteLine("Border drawn successfully. Output image saved at: " + outputImagePath);

}















