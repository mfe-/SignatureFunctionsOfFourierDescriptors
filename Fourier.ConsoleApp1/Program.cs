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
    var workingDir = Environment.CurrentDirectory;
    //find all related lines to the image from the solution file
    var lines = File.ReadAllLines(solutionPath).Where(a => a.StartsWith(new FileInfo(imagePath).Name.Replace(".png", "")));
    //copy imagePath to workingDir
    File.Copy(imagePath, Path.Combine(workingDir!, Path.GetFileName(imagePath)), true);

    using var image = Image.Load<Rgba32>(imagePath);


    var rle = new RunLengthEncodingDecoding();
    rle.MarkImageUsingRunLengthDecoding(lines, image);

    ////var workingDir = Path.GetDirectoryName(imagePath);
    //var imageMaskName = Path.GetFileNameWithoutExtension(imagePath) + "_mask.png";
    var imageMaskName = Path.Combine(workingDir!, new FileInfo(imagePath).Name.Replace(".png", "") + "_mask.png");
    var outputImagePath = Path.Combine(workingDir!, imageMaskName);
    image.Save(outputImagePath); // Save the modified image with borders
    Console.WriteLine("Border drawn successfully. Output image saved at: " + outputImagePath);
}















