﻿using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System.Numerics;

namespace Fourier.ConsoleApp1;

public sealed class RunLengthEncodingDecoding
{

    /// <summary>
    /// Converts a run length encoded value to x and y coordinates and sets the of value <paramref name="pixelMarker"/>.
    /// </summary>
    /// <remarks>
    /// In order to reduce the file size, run-length encoding can represent multiple pixel values. 
    /// Pairs of values contain a start position and a run length. E.g. '1 3' implies starting at pixel 1 and running a total of 3 pixels (1,2,3).
    /// The competition format requires a space delimited list of pairs. For example, '1 3 10 5' implies pixels 1,2,3,10,11,12,13,14 are to be included in the mask.
    /// The pixels are one-indexed. 
    /// The Run Length decoding works as follows:
    /// The encoded pixels are indexed from left to right within a row. As the encoding is one-dimensional, there is no differentiation on the y-axis. Y pixels will be simply appended.
    /// Keep also in mind that the indexing starts at 1 (one-indexed). To convert the one-dimensional value to a y and x coordinate, proceed as follows:
    /// Row (y): y = (pixelIndex - 1) / width
    /// Column (x): x = (pixelIndex - 1) % width
    /// 
    /// Sample:
    /// An image with 256x256 pixels has a total of 65536 pixels. We want to decode pixel 257.
    /// 1. Convert the one-indexed pixel index to zero-indexed: 257 - 1 = 256
    /// 2. Row (y): 256 % 256 = 1
    /// 3. Column (x): 256 / 256 = 0
    /// </remarks>
    /// <param name="lines">Line which contains the encoded run-length in the csv format: <code>ImageId,EncodedPixels,Height,Width,Usage</code></param>
    /// <param name="image">The image to mark.</param>
    /// <param name="pixelMarker">The pixel marker value.</param>
    public List<Point>[] GetRunLengthDecoding(IEnumerable<string> lines, Image<Rgba32> image)
    {
        var rand = new Random();
        var maskedObjectPoints = new List<Point>[lines.Count()];
        
        int l = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            var encodedPixels = parts[1].Split(' ');

            var r = (byte)rand.Next(0, 255);
            var g = (byte)rand.Next(0, 255);
            var b = (byte)rand.Next(0, 255);
            var pixelMarker = new Rgba32(r, g, b, 255);

            List<Point> boundaryPoints = new();
            for (int i = 0; i < encodedPixels.Length; i += 2)
            {
                int start = int.Parse(encodedPixels[i]);
                int length = int.Parse(encodedPixels[i + 1]);


                // Decode the RLE
                for (int j = 0; j < length; j++)
                {
                    int pixelIndex = start + j - 1;
                    int y = pixelIndex % image.Height;
                    int x = pixelIndex / image.Height;

                    if (x < image.Width && y < image.Height)
                    {
                        image[x, y] = pixelMarker;
                        boundaryPoints.Add(new Point(x, y));
                    }
                }
            }
            maskedObjectPoints[l] = boundaryPoints;
            l++;
        }
        return maskedObjectPoints;
    }
}
