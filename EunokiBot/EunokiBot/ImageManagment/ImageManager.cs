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
        private Bitmap m_bmpSlotDefault = null;
        private Bitmap m_bmpUserInfoDefault = null;
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

        private Bitmap SlotDefault
        {
            get
            {
                if(m_bmpSlotDefault == null)
                {
                    m_bmpSlotDefault = new Bitmap(Path.Combine(
                        FilePath, m_sUserInfoFileName, "InventorySlot.png"));
                }

                return m_bmpSlotDefault;
            }
        }

        private Bitmap UserInfoDefault
        {
            get
            {
                if(m_bmpUserInfoDefault == null)
                {
                    m_bmpUserInfoDefault = new Bitmap(425, 450);
                    using (Graphics g = Graphics.FromImage(m_bmpUserInfoDefault))
                    {
                        g.Clear(Color.FromArgb(251, 251, 251));

                        // Icons
                        g.DrawImage(Icons[0], 42f, 235f);
                        g.DrawImage(Icons[1], 122f, 235f);
                        g.DrawImage(Icons[2], 42f, 300f);
                        g.DrawImage(Icons[3], 122f, 300f);
                        g.DrawImage(Icons[4], 42, 365f);
                        g.DrawImage(Icons[5], 122f, 365f);

                        g.DrawImage(EmptyInventoryGrid(SlotDefault), 200, 25);
                    }
                }

                return m_bmpUserInfoDefault;
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
            System.Net.WebRequest request = System.Net.WebRequest.Create(contextUser.GetAvatarUrl());
            System.Net.WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Bitmap bmpAvatar = new Bitmap(responseStream);

            Bitmap result = UserInfoDefault;

            using (Graphics g = Graphics.FromImage(result))
            using (StringFormat sf = new StringFormat())
            using (SolidBrush b = new SolidBrush(Color.Black))
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                List<Bitmap> arItems = CreateItemImages(arIDs, arAmounts);
                Bitmap bmpInventoryItems = ItemsInventoryGrid(arItems);
                g.DrawImage(bmpInventoryItems, 200, 25);
                g.DrawImage(bmpAvatar, 25, 25, 150, 150);

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
                    string sMoney = String.Empty;
                    if (user.Money >= 100000)
                        sMoney = "+99999";
                    else
                        sMoney = user.Money.ToString();
                    g.DrawString(sMoney, f, b, new RectangleF(20, 270, 75, 17.5f), sf);
                    #endregion

                    #region Messages
                    string sMessages = String.Empty;
                    if (user.Messages >= 100000)
                        sMessages = "+99999";
                    else
                        sMessages = user.Messages.ToString();
                    g.DrawString(sMessages, f, b, new RectangleF(100, 270, 75, 17.5f), sf);
                    #endregion

                    #region Duels
                    #region Wins
                    string sWins = String.Empty;
                    if (user.Wins >= 100000)
                        sWins = "+99999";
                    else
                        sWins = user.Wins.ToString();
                    g.DrawString(sWins, f, b, new RectangleF(20, 335, 75, 17.5f), sf);
                    #endregion
                    #region Lost
                    string sLost = String.Empty;
                    if (user.Lost >= 100000)
                        sLost = "+99999";
                    else
                        sLost = user.Lost.ToString();
                    g.DrawString(sLost, f, b, new RectangleF(100, 335, 75, 17.5f), sf);
                    #endregion
                    #endregion

                    #region Quests
                    string sQuests = String.Empty;
                    if (user.Quests >= 100000)
                        sQuests = "+99999";
                    else
                        sQuests = user.Quests.ToString();
                    g.DrawString(sQuests, f, b, new RectangleF(20, 400, 75, 17.5f), sf);
                    #endregion

                    #region Warnings
                    g.DrawString(user.Warnings.ToString(), f, b, new RectangleF(100, 400, 75, 17.5f), sf);
                    #endregion
                }
                #endregion
            }

            result.Save(Path.Combine(FilePath, user.UserID + ".png"), ImageFormat.Png);
            return user.UserID + ".png";
        }

        #region Inventory
        public Bitmap EmptyInventoryGrid(Bitmap bmpImage)
        {
            int nOffset = 10;
            int nWidth = (bmpImage.Width + nOffset) * 2;
            int nHeight = (bmpImage.Height + nOffset) * 4;
            Bitmap result = new Bitmap(nWidth, nHeight);
            result.MakeTransparent();

            using(Graphics g = Graphics.FromImage(result))
            {
                int nOffsetX, nOffsetY;
                nOffsetX = nOffsetY = 0;

                for (int i = 0; i < 8; ++i)
                {
                    g.DrawImage(bmpImage,
                        new Rectangle(nOffsetX, nOffsetY, bmpImage.Width, bmpImage.Height));

                    nOffsetX += bmpImage.Width + nOffset;
                    if (i % 2 == 1)
                    {
                        nOffsetX = 0;
                        nOffsetY += bmpImage.Height + nOffset;
                    }
                }
            }
            return result;
        }

        public Bitmap ItemsInventoryGrid(List<Bitmap> arBmpItems)
        {
            int nOffset = 10;
            int nWidth = (arBmpItems[0].Width + nOffset) * 2;
            int nHeight = (arBmpItems[0].Height + nOffset) * 4;
            Bitmap result = new Bitmap(nWidth, nHeight);
            result.MakeTransparent();

            using (Graphics g = Graphics.FromImage(result))
            {
                int nOffsetX, nOffsetY;
                nOffsetX = nOffsetY = 0;

                for (int i = 0; i < arBmpItems.Count(); ++i)
                {
                    g.DrawImage(arBmpItems[i],
                        new Rectangle(nOffsetX, nOffsetY, arBmpItems[i].Width, arBmpItems[i].Height));

                    nOffsetX += arBmpItems[i].Width + nOffset;
                    if (i % 2 == 1)
                    {
                        nOffsetX = 0;
                        nOffsetY += arBmpItems[i].Height + nOffset;
                    }
                }
            }

            return result;
        }

        public List<Bitmap> CreateItemImages(int[] nIDs, int[] nAmounts)
        {
            List<Bitmap> arItems = new List<Bitmap>();

            for(int i = 0; i < nIDs.Length; ++i)
            {
                Bitmap source2 = new Bitmap(90, 90);

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

                arItems.Add(source2);
            }

            return arItems;
        }
        #endregion
    }
}
