// See https://aka.ms/new-console-template for more information
using Fourier.ConsoleApp1;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


//https://www.kaggle.com/code/stkbailey/teaching-notebook-for-total-imaging-newbies

var workingDir = @"D:\Kfk-Praktikum\";

//var imagePath = Path.Combine(workingDir, "test.png");
var imagePath = Path.Combine(workingDir, "0a849e0eb15faa8a6d7329c3dd66aabe9a294cccb52ed30a90c8ca99092ae732.png");
//var imagePath = Path.Combine(workingDir, "0114f484a16c152baa2d82fdd43740880a762c93f436c8988ac461c5c9dbe7d5.png"); //y %


var solutionPath = Path.Combine(workingDir, "stage1_solution.csv");

var lines = File.ReadAllLines(solutionPath).Where(a => a.StartsWith(new FileInfo(imagePath).Name.Replace(".png", "")));
using var image = await Image.LoadAsync<Rgba32>(imagePath);

var red = new Rgba32(255, 0, 0); // Define border color
var rle = new RunLengthEncodingDecoding();
rle.MarkImageUsingRunLengthDecoding(lines, image, red);

var outputImagePath = Path.Combine(workingDir, "output.png");
image.Save(outputImagePath); // Save the modified image with borders

Console.WriteLine("Border drawn successfully. Output image saved at: " + outputImagePath);


