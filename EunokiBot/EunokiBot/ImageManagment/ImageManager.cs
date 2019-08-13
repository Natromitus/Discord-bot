using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using Discord.WebSocket;
using EunokiBot.Model;

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
        #endregion

        public string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + (char)0x2026;
        }

        public string UserInfo(SocketUser contextUser, User user, Inventory inventory)
        {
            int[] arIDs = new int[]
            {
                inventory.ItemID1, inventory.ItemID2, inventory.ItemID3,
                inventory.ItemID4, inventory.ItemID5, inventory.ItemID6,
                inventory.ItemID7, inventory.ItemID8, inventory.ItemID9
            };

            int[] arAmounts = new int[]
            {
                inventory.Amount1, inventory.Amount2, inventory.Amount3,
                inventory.Amount4, inventory.Amount5, inventory.Amount6,
                inventory.Amount7, inventory.Amount8, inventory.Amount9
            };

            Bitmap bmpInventory = CreateInventoryImage(arIDs, arAmounts);
            System.Net.WebRequest request = System.Net.WebRequest.Create(contextUser.GetAvatarUrl());
            System.Net.WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Bitmap bmpAvatar = new Bitmap(responseStream);

            Bitmap result = new Bitmap(1200, 850);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.FromArgb(251, 251, 251));
                g.DrawImage(bmpAvatar, 50, 50, 300, 300);
                g.DrawImage(bmpInventory, 400, 50);

                using (SolidBrush b = new SolidBrush(Color.Black))
                {
                    // USERNAME
                    using (Font f = new Font(new FontFamily("Century Gothic"), 32, FontStyle.Bold))
                        g.DrawString(Truncate(contextUser.Username.ToUpper(), 11), f, b, new RectangleF(45, 400, 450, 55));

                    // LINE
                    g.FillRectangle(b, new Rectangle(48, 450, 145, 10));

                    using (Font f = new Font(new FontFamily("Century Gothic"), 24, FontStyle.Bold))
                    {
                        // LEVEL
                        g.DrawString("LEVEL " + user.Level.ToString(), f, b, new RectangleF(45, 460, 150, 35));

                        using(StringFormat sf = new StringFormat())
                        {
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Alignment = StringAlignment.Center;

                            // MONEY
                            string sMoney = String.Empty;
                            if (user.Money >= 100000)
                                sMoney = "+99999";
                            else
                                sMoney = user.Money.ToString();
                            g.DrawString(sMoney, f, b, new RectangleF(40, 570, 150, 35), sf);

                            // MESSAGES
                            string sMessages = String.Empty;
                            if (user.Messages >= 100000)
                                sMessages = "+99999";
                            else
                                sMessages = user.Messages.ToString();
                            g.DrawString(sMessages, f, b, new RectangleF(230, 570, 150, 35), sf);

                            // DUELS
                            // WINS
                            string sWins = String.Empty;
                            if (user.Wins >= 100000)
                                sWins = "+99999";
                            else
                                sWins = user.Wins.ToString();
                            g.DrawString(sWins, f, b, new RectangleF(40, 665, 150, 35), sf);

                            // LOST
                            string sLost = String.Empty;
                            if (user.Lost >= 100000)
                                sLost = "+99999";
                            else
                                sLost = user.Lost.ToString();
                            g.DrawString(sLost, f, b, new RectangleF(230, 665, 150, 35), sf);


                            // QUESTS
                            string sQuests = String.Empty;
                            if (user.Quests >= 100000)
                                sQuests = "+99999";
                            else
                                sQuests = user.Quests.ToString();
                            g.DrawString(sQuests, f, b, new RectangleF(40, 760, 150, 35), sf);

                            // WARNINGS
                            g.DrawString(user.Warnings.ToString(), f, b, new RectangleF(230, 760, 150, 35), sf);
                        }
                    }
                }
            }

            result.Save(Path.Combine(FilePath, "test.png"), ImageFormat.Png);
            return "test.png";
        }


        // Inventory
        public Bitmap CreateInventoryImage(int[] nIDs, int[] nAmounts)
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

            Bitmap stitchedImage = CreateInventoryGrid(arItems);

            string sFileName = Guid.NewGuid().ToString() + ".png";
            return stitchedImage;

            //stitchedImage.Save(Path.Combine(FilePath, sFileName), ImageFormat.Png);
            //return sFileName;
        }

        private Bitmap CreateInventoryGrid(List<Bitmap> images)
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
                        if (i % 3 == 2)
                        {
                            offsetX = 0;
                            offsetY += images[i].Height;
                        }
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
    }
}
