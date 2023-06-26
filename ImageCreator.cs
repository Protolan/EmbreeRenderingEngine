using System.Drawing;
using System.Numerics;
using System.Text;


public class ImageCreator
{
   
    
    public static void CreatePng(Vector3[,] hdrImage, string filePath)
    {
        Vector3[,] ldrImage = HDRtoLDRReinhard(hdrImage);
    
        int width = ldrImage.GetLength(0);
        int height = ldrImage.GetLength(1);
        Bitmap bitmap = new Bitmap(width, height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 color = ldrImage[i, j];
                int r = Clamp((int)Math.Round(color.X), 0, 255);
                int g = Clamp((int)Math.Round(color.Y), 0, 255);
                int b = Clamp((int)Math.Round(color.Z), 0, 255);
                bitmap.SetPixel(i, j, Color.FromArgb(r, g, b));
            }
        }

        bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
    }

    private static int Clamp(int value, int min, int max)
    {
        if (value < min)
            return min;
        if (value > max)
            return max;
        return value;
    }

    public static Vector3[,] HDRtoLDRReinhard(Vector3[,] hdrImage)
    {
        int width = hdrImage.GetLength(0);
        int height = hdrImage.GetLength(1);

        // Compute average luminance
        float totalLuminance = 0.0f;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 pixel = hdrImage[i, j];
                totalLuminance += 0.2126f * pixel.X + 0.7152f * pixel.Y + 0.0722f * pixel.Z; // weights for human perception
            }
        }

        float avgLuminance = totalLuminance / (width * height);

        // Apply Reinhard tone mapping
        Vector3[,] ldrImage = new Vector3[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 hdrColor = hdrImage[i, j];
                Vector3 ldrColor = Vector3.Divide(hdrColor, (hdrColor + new Vector3(avgLuminance, avgLuminance, avgLuminance)));
                ldrColor *= 255; // Scale to 8 bit range
                ldrImage[j, i] = ldrColor;
            }
        }

        return ldrImage;
    }

    
    

    

}