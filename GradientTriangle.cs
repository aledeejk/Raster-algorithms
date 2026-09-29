using System;
using System.Drawing;

namespace RasterAlgorithms
{
    public static class GradientTriangle
    {
        public static void Draw(Graphics graphics,
                                Point p1, Color c1,
                                Point p2, Color c2,
                                Point p3, Color c3)
        {
            int minX = Math.Min(p1.X, Math.Min(p2.X, p3.X));
            int maxX = Math.Max(p1.X, Math.Max(p2.X, p3.X));
            int minY = Math.Min(p1.Y, Math.Min(p2.Y, p3.Y));
            int maxY = Math.Max(p1.Y, Math.Max(p2.Y, p3.Y));

            float area = (p1.X * (p2.Y - p3.Y) +
                          p2.X * (p3.Y - p1.Y) +
                          p3.X * (p1.Y - p2.Y));

            if (Math.Abs(area) < 0.0001f) return;

            int width = maxX - minX + 1;
            int height = maxY - minY + 1;

            using (Bitmap bmp = new Bitmap(width, height))
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        float w1 = ((p2.Y - p3.Y) * (x - p3.X) + (p3.X - p2.X) * (y - p3.Y)) / area;
                        float w2 = ((p3.Y - p1.Y) * (x - p3.X) + (p1.X - p3.X) * (y - p3.Y)) / area;
                        float w3 = 1f - w1 - w2;

                        if (w1 >= -0.001f && w2 >= -0.001f && w3 >= -0.001f)
                        {
                            int r = (int)(w1 * c1.R + w2 * c2.R + w3 * c3.R);
                            int g = (int)(w1 * c1.G + w2 * c2.G + w3 * c3.G);
                            int b = (int)(w1 * c1.B + w2 * c2.B + w3 * c3.B);

                            r = r < 0 ? 0 : (r > 255 ? 255 : r);
                            g = g < 0 ? 0 : (g > 255 ? 255 : g);
                            b = b < 0 ? 0 : (b > 255 ? 255 : b);

                            bmp.SetPixel(x - minX, y - minY, Color.FromArgb(r, g, b));
                        }
                    }
                }

                graphics.DrawImage(bmp, minX, minY);
            }
            using (Pen pen = new Pen(Color.Black, 1))
            {
                graphics.DrawPolygon(pen, new[] { p1, p2, p3 });
            }
        }
    }
}