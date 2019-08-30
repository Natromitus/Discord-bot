using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EunokiBot.ImageManagment
{
    public static class GraphicsExtensions
    {
        public static Bitmap SourceOver(this Graphics g, Bitmap source1, Bitmap source2, int x = 0, int y = 0)
        {
            Bitmap result = new Bitmap(source1.Width, source1.Height);
            g = Graphics.FromImage(result);
                g.DrawImage(source1, new Rectangle(Point.Empty, result.Size));

                if (source2 != null)
                    g.DrawImage(source2, new Rectangle(x, y, source2.Width, source2.Height));

            return result;
        }
    }
}
