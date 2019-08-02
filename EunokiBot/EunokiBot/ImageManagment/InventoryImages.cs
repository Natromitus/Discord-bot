using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace EunokiBot.ImageManagment
{
    class InventoryImages
    {
        private static string m_sFilePath =
            Assembly.GetEntryAssembly().Location.Replace(@"EunokiBot.dll", @"Resources");

        public static string FilePath { get => m_sFilePath; set => m_sFilePath = value; }

        public static Bitmap Combine(List<Bitmap> images)
        {
            Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (Bitmap bitmap in images)
                {
                    // Update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;
                }

                // Create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                // Get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    // Set background color
                    g.Clear(Color.Black);

                    // Go through each image and draw it on the final image
                    int offset = 0;
                    foreach (Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                // Clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }

        public static void CreateInventoryImage(int[] nIDs, int[] nAmounts)
        {
            if (nIDs.Length > 3 || nAmounts.Length > 3)
                return;

            List<string> sItems = new List<string>();
            List<string> sIds = new List<string>();
            string sIDpath = Path.Combine(m_sFilePath, "ItemsByID");
            string sAmountPath = Path.Combine(m_sFilePath, "Amount");

            for(int i = 0; i < nIDs.Length; ++i)
            {
                sItems.Add(Path.Combine(sIDpath, nIDs[i].ToString() + ".png"));
                sIds.Add(Path.Combine(sAmountPath, nAmounts[i].ToString() + ".png"));
            }

            List<Bitmap> arItems = new List<Bitmap>();
            for (int i = 0; i < sItems.Count; ++i)
            {
                Bitmap source1 = new Bitmap(sItems[i]);
                if (nIDs[i] == 0 || nAmounts[i] == 0)
                {
                    arItems.Add(CombineWithAmount(source1, null));
                    continue;
                }

                Bitmap source2 = new Bitmap(sIds[i]);

                arItems.Add(CombineWithAmount(source1, source2));
            }

            Bitmap stitchedImage = Combine(arItems);
            stitchedImage.Save(Path.Combine(m_sFilePath, "buffer.png"), ImageFormat.Png);
        }

        private static Bitmap CombineWithAmount(Bitmap itemSource, Bitmap amountSource)
        {
            Bitmap bmp = new Bitmap(itemSource.Width / 2, itemSource.Height / 2);
            if (amountSource == null)
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Rectangle drect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    g.DrawImage(itemSource, drect);
                }

                return bmp;
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                Rectangle drect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                g.DrawImage(itemSource, drect);
                g.DrawImage(amountSource, drect);
            }

            return bmp;
        }
    }
}
