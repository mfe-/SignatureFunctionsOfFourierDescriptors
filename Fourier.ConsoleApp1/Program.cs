// See https://aka.ms/new-console-template for more information
using Fourier.ConsoleApp1;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;



var solutionPath = SetUpData.GetSolutionPath("stage1_solution.csv");
var stageFolder = SetUpData.GetStageFolderPath("stage1_test");

var images = SetUpData.FindAllPngImages(stageFolder);

var red = new Rgba32(255, 0, 0); // Define border color
foreach (var imagePath in images)
{
    //find all related lines to the image from the solution file
    var lines = File.ReadAllLines(solutionPath).Where(a => a.StartsWith(new FileInfo(imagePath).Name.Replace(".png", "")));

    using var image = Image.Load<Rgba32>(imagePath);
    var rle = new RunLengthEncodingDecoding();
    rle.MarkImageUsingRunLengthDecoding(lines, image, red);

    var workingDir = Path.GetDirectoryName(imagePath);
    var outputImagePath = Path.Combine(workingDir!, "output.png");
    image.Save(outputImagePath); // Save the modified image with borders
    Console.WriteLine("Border drawn successfully. Output image saved at: " + outputImagePath);
}















