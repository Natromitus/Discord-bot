using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
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
        private string m_sUserInfoFileName = "UserInfo";

        private PrivateFontCollection m_fCollection = new PrivateFontCollection();

        private List<Bitmap> m_arBmpItems = new List<Bitmap>();
        private List<Bitmap> m_arBmpIcons = new List<Bitmap>();
        private Bitmap m_bmpInventorySlot = null;
        #endregion

        #region Properties
        public static ImageManager Singleton => m_singleton;

        public string FilePath
        {
            get
            {
                return Assembly.GetEntryAssembly().Location.Replace(@"EunokiBot.dll", @"Resources"); ;
            }
        }
        
        private PrivateFontCollection FontCollection
        {
            get
            {
                if (m_fCollection.Families.Count() == 0)
                {
                    m_fCollection.AddFontFile(Path.Combine(FilePath, "gothicb.ttf"));
                    m_fCollection.AddFontFile(Path.Combine(FilePath, "gothic.ttf"));
                }

                return m_fCollection;
            }
        }

        private Bitmap InventorySlot
        {
            get
            {
                if(m_bmpInventorySlot == null)
                {
                    m_bmpInventorySlot = new Bitmap(Path.Combine(
                        FilePath, m_sUserInfoFileName, "InventorySlot.png"));
                }

                return m_bmpInventorySlot;
            }
        }

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

        private List<Bitmap> Icons
        {
            get
            {
                if (m_arBmpIcons.Count == 0)
                {
                    string[] files = Directory.GetFiles(Path.Combine(FilePath, m_sUserInfoFileName, "Icons"));
                    m_arBmpIcons = files.Select(obj => new Bitmap(obj)).ToList();
                }

                return m_arBmpIcons;
            }
        }

        #endregion

        #region Generic Functions
        private Bitmap SourceOver(Bitmap source1, Bitmap source2, int x = 0, int y = 0)
        {
            Bitmap result = new Bitmap(source1.Width, source1.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(source1, new Rectangle(Point.Empty, result.Size));

                if (source2 != null)
                    g.DrawImage(source2, new Rectangle(x, y, source2.Width, source2.Height));
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
            #region Arrays
            int[] arIDs = new int[]
            {
                inventory.ItemID1, inventory.ItemID2,
                inventory.ItemID3, inventory.ItemID4,
                inventory.ItemID5, inventory.ItemID6,
                inventory.ItemID7, inventory.ItemID8
            };

            int[] arAmounts = new int[]
            {
                inventory.Amount1, inventory.Amount2,
                inventory.Amount3, inventory.Amount4,
                inventory.Amount5, inventory.Amount6,
                inventory.Amount7, inventory.Amount8
            };
            #endregion

            // Getting User's Avatar
            Bitmap bmpInventory = CreateInventoryImage(arIDs, arAmounts);
            System.Net.WebRequest request = System.Net.WebRequest.Create(contextUser.GetAvatarUrl());
            System.Net.WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Bitmap bmpAvatar = new Bitmap(responseStream);

            Bitmap result = new Bitmap(425, 450);

            using (Graphics g = Graphics.FromImage(result))
            using (StringFormat sf = new StringFormat())
            using (SolidBrush b = new SolidBrush(Color.Black))
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                g.Clear(Color.FromArgb(251, 251, 251));
                g.DrawImage(bmpAvatar, 25, 25, 150, 150);
                g.DrawImage(bmpInventory, 200, 25);

                #region Level
                using (SolidBrush bEllip = new SolidBrush(Color.FromArgb(251, 251, 251)))
                    g.FillEllipse(bEllip, 150, 150, 50, 50);

                using (Font f = new Font(FontCollection.Families[0], 12, FontStyle.Bold))
                    g.DrawString(user.Level.ToString(), f, b, new Rectangle(148, 159, 40, 17), sf);
                #endregion

                #region Username
                using (Font f = new Font(FontCollection.Families[0], 14, FontStyle.Bold))
                    g.DrawString(Truncate(contextUser.Username.ToUpper(), 11), f, b, new RectangleF(22.5f, 200, 225, 27.5f));
                #endregion

                #region XPBar
                using (SolidBrush bRectFg = new SolidBrush(Color.FromArgb(90, 130, 190)))
                using (SolidBrush bRectBg = new SolidBrush(Color.FromArgb(150, 180, 240)))
                using (SolidBrush bXP = new SolidBrush(Color.FromArgb(0, 0, 0)))
                using (Font fXP = new Font(FontCollection.Families[0], 8, FontStyle.Regular))
                {
                    g.FillRectangle(bRectBg, new Rectangle(25, 180, 150, 20));


                    int nOldLvlGap = Data.Singleton.Levels[user.Level].XPGap;
                    int nLvlGap = Data.Singleton.Levels[user.Level + 1].XPGap;

                    float fWidth = (user.XP - nOldLvlGap) / ((nLvlGap - nOldLvlGap) / 100);
                    fWidth *= 1.5f;
                    g.FillRectangle(bRectFg, new RectangleF(25, 180, fWidth, 20));

                    g.DrawString(user.XP.ToString() + " / " + Data.Singleton.Levels[user.Level + 1].XPGap.ToString(),
                    fXP, b, new RectangleF(25, 179, 150, 20), sf);
                }
                #endregion

                #region Stats
                using (Font f = new Font(FontCollection.Families[0], 12, FontStyle.Regular))
                {
                    #region Money
                    g.DrawImage(Icons[0], 42f, 235f);

                    string sMoney = String.Empty;
                    if (user.Money >= 100000)
                        sMoney = "+99999";
                    else
                        sMoney = user.Money.ToString();
                    g.DrawString(sMoney, f, b, new RectangleF(20, 270, 75, 17.5f), sf);
                    #endregion

                    #region Messages
                    g.DrawImage(Icons[1], 122f, 235f);

                    string sMessages = String.Empty;
                    if (user.Messages >= 100000)
                        sMessages = "+99999";
                    else
                        sMessages = user.Messages.ToString();
                    g.DrawString(sMessages, f, b, new RectangleF(100, 270, 75, 17.5f), sf);
                    #endregion

                    #region Duels
                    #region Wins
                    g.DrawImage(Icons[2], 42f, 300f);

                    string sWins = String.Empty;
                    if (user.Wins >= 100000)
                        sWins = "+99999";
                    else
                        sWins = user.Wins.ToString();
                    g.DrawString(sWins, f, b, new RectangleF(20, 335, 75, 17.5f), sf);
                    #endregion
                    #region Lost
                    g.DrawImage(Icons[3], 122f, 300f);


                    string sLost = String.Empty;
                    if (user.Lost >= 100000)
                        sLost = "+99999";
                    else
                        sLost = user.Lost.ToString();
                    g.DrawString(sLost, f, b, new RectangleF(100, 335, 75, 17.5f), sf);
                    #endregion
                    #endregion

                    #region Quests
                    g.DrawImage(Icons[4], 42, 365f);

                    string sQuests = String.Empty;
                    if (user.Quests >= 100000)
                        sQuests = "+99999";
                    else
                        sQuests = user.Quests.ToString();
                    g.DrawString(sQuests, f, b, new RectangleF(20, 400, 75, 17.5f), sf);
                    #endregion

                    #region Warnings
                    g.DrawImage(Icons[5], 122f, 365f);
                    g.DrawString(user.Warnings.ToString(), f, b, new RectangleF(100, 400, 75, 17.5f), sf);
                    #endregion
                }
                #endregion
            }

            result.Save(Path.Combine(FilePath, user.UserID + ".png"), ImageFormat.Png);
            return user.UserID + ".png";
        }

        #region Inventory
        public Bitmap CreateInventoryImage(int[] nIDs, int[] nAmounts)
        {
            List<Bitmap> arItems = new List<Bitmap>();

            for(int i = 0; i < nIDs.Length; ++i)
            {
                Bitmap source2 = null;

                if (nIDs[i] != 0 && nAmounts[i] != 0)
                {
                    Bitmap temp = Items[nIDs[i] - 1];

                    source2 = new Bitmap(temp.Width, temp.Height);
                    using (Graphics g = Graphics.FromImage(source2))
                    using (Font f = new Font(FontCollection.Families[0], 12, FontStyle.Bold))
                    using (SolidBrush b = new SolidBrush(Color.Black))
                    {
                        g.DrawImage(temp, Point.Empty);
                        g.DrawString(nAmounts[i].ToString(), f, b, new Rectangle(55, 60, 40, 18));
                    }
                }

                arItems.Add(SourceOver(InventorySlot, source2, 5, 5));
            }

            Bitmap stitchedImage = CreateInventoryGrid(arItems);
            return stitchedImage;
        }

        private Bitmap CreateInventoryGrid(List<Bitmap> images)
        {
            Bitmap finalImage = null;
            try
            {
                int nOffset = 10;
                int width = (images[0].Width + nOffset) * 2;
                int height = (images[0].Height + nOffset) * 4;

                finalImage = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    g.Clear(Color.FromArgb(251, 251, 251));

                    int offsetX = 0;
                    int offsetY = 0;
                    for(int i = 0; i < images.Count(); ++i)
                    {
                        g.DrawImage(images[i], new Rectangle(offsetX, offsetY, images[i].Width, images[i].Height));
                        offsetX += images[i].Width + nOffset;
                        if (i % 2 == 1)
                        {
                            offsetX = 0;
                            offsetY += images[i].Height + nOffset;
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
        #endregion
    }
}
