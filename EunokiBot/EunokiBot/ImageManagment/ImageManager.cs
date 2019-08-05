using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EunokiBot.ImageManagment
{
    class ImageManager
    {
        #region Fields
        private static readonly ImageManager m_singleton = new ImageManager();

        private string m_sIDFileName = "ItemsByID";
        private string m_sAmountsFileName = "Amount";

        private List<Bitmap> m_arBmpItems = new List<Bitmap>();
        private List<Bitmap> m_arBmpAmounts = new List<Bitmap>();
        private List<Bitmap> m_arBmpPrices = new List<Bitmap>();
        #endregion

        #region Properties
        public static ImageManager Singleton => m_singleton;

        private List<Bitmap> Items
        {
            get
            {
                if(m_arBmpItems.Count == 0)
                {
                    string[] files = Directory.GetFiles(Path.Combine(FilePath, m_sIDFileName));
                    m_arBmpItems = files.Select(obj => new Bitmap(obj)).ToList();
                }

                return m_arBmpItems;
            }
        }

        private List<Bitmap> Amounts
        {
            get
            {
                if (m_arBmpAmounts.Count == 0)
                {
                    string[] files = Directory.GetFiles(Path.Combine(FilePath, m_sAmountsFileName));
                    m_arBmpAmounts = files.Select(obj => new Bitmap(obj)).ToList();
                }

                return m_arBmpAmounts;
            }
        }

        public string FilePath
        {
            get
            {
                return Assembly.GetEntryAssembly().Location.Replace(@"EunokiBot.dll", @"Resources");;
            }
        }
        #endregion

        #region Generic Functions
        private Bitmap SourceOver(Bitmap source1, Bitmap source2)
        {
            Bitmap result = new Bitmap(source1.Width, source1.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                Rectangle drect = new Rectangle(Point.Empty, result.Size);
                g.DrawImage(source1, drect);

                if (source2 != null)
                    g.DrawImage(source2, drect);
            }

            return result;
        }

        private Bitmap CreateInventoryImage(List<Bitmap> images)
        {
            Bitmap finalImage = null;
            try
            {
                int width = images[0].Width * 3;
                int height = images[0].Height * 3;

                finalImage = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    g.Clear(Color.Black);

                    int offsetX = 0;
                    int offsetY = 0;
                    for(int i = 0; i < images.Count(); ++i)
                    {
                        g.DrawImage(images[i], new Rectangle(offsetX, offsetY, images[i].Width, images[i].Height));
                        offsetX += images[i].Width;
                        if (i % 3 == 1)
                        {
                            offsetX = 0;
                            offsetY += images[i].Height;
                        }
                    }

                    foreach (Bitmap image in images)
                    {
                        g.DrawImage(image, new Rectangle(offsetX, 0, image.Width, image.Height));
                        offsetX += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                finalImage?.Dispose();
                throw ex;
            }
            finally
            {
                images.ForEach(obj => obj.Dispose());
            }
        }
        #endregion




        // Inventory
        public string CreateInventoryImage(int[] nIDs, int[] nAmounts)
        {
            List<Bitmap> arItems = new List<Bitmap>();

            for (int i = 0; i < nIDs.Length; ++i)
            {
                Bitmap source1 = Items[nIDs[i]];
                Bitmap source2 = null;

                if (nIDs[i] != 0 && nAmounts[i] != 0)
                    source2 = Amounts[nAmounts[i] - 1];

                arItems.Add(SourceOver(source1, source2));
            }

            Bitmap stitchedImage = CreateInventoryImage(arItems);

            string sFileName = Guid.NewGuid().ToString() + ".png";
            stitchedImage.Save(Path.Combine(FilePath, sFileName), ImageFormat.Png);
            return sFileName;
        }
    }
}
