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

        private List<Bitmap> m_arBmpItems = null;
        private List<Bitmap> m_arBmpIcons = null;
        private List<Bitmap> m_arBmpShopDefault = null;
        private Bitmap m_bmpSlotDefault = null;
        private Bitmap m_bmpUserInfoDefault = null;
        private Color[] m_arQuestColors = new Color[]
        {
            Color.FromArgb(130, 130, 130),
            Color.FromArgb(56, 234, 53),
            Color.FromArgb(249, 217, 54),
            Color.FromArgb(237, 63, 47),
            Color.FromArgb(46, 188, 232)
        };


    private readonly Color m_clrUserInfoBG = Color.FromArgb(251, 251, 251);
        private readonly int m_nMargin = 25;
        private readonly int m_nAvatar = 150;
        private readonly int m_nLevelBG = 50;
        private readonly int m_nIconX1 = 42;
        private readonly int m_nIconX2 = 122;
        private readonly int m_nIconY1 = 235;
        private readonly int m_nIconY2 = 300;
        private readonly int m_nIconY3 = 365;

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
                if (m_bmpSlotDefault == null)
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
                if (m_bmpUserInfoDefault == null)
                {
                    m_bmpUserInfoDefault = new Bitmap(425, 440);
                    using (Graphics g = Graphics.FromImage(m_bmpUserInfoDefault))
                    {
                        g.Clear(m_clrUserInfoBG);

                        // Icons
                        g.DrawImage(Icons[0], m_nIconX1, m_nIconY1);
                        g.DrawImage(Icons[1], m_nIconX2, m_nIconY1);
                        g.DrawImage(Icons[2], m_nIconX1, m_nIconY2);
                        g.DrawImage(Icons[3], m_nIconX2, m_nIconY2);
                        g.DrawImage(Icons[4], m_nIconX1, m_nIconY3);
                        g.DrawImage(Icons[5], m_nIconX2, m_nIconY3);

                        using (Pen p = new Pen(Color.FromArgb(90, 130, 190)))
                            g.DrawLine(p, m_nAvatar + m_nMargin + 17, m_nMargin,
                                m_nAvatar + m_nMargin + 17, m_bmpUserInfoDefault.Height - m_nMargin);

                        g.DrawImage(SlotGrid(SlotDefault, 2, 4), m_nAvatar + 2 * m_nMargin + 10, m_nMargin);
                    }
                }

                return m_bmpUserInfoDefault;
            }
        }

        private List<Bitmap> ShopDefault
        {
            get
            {
                if (m_arBmpShopDefault == null)
                {
                    m_arBmpShopDefault = new List<Bitmap>();

                    int nPages = Data.Singleton.ShopPages;

                    for (int i = 1; i <= nPages; ++i)
                    {
                        Bitmap bmpPage = new Bitmap(450, 380);
                        using (Graphics g = Graphics.FromImage(bmpPage))
                        {
                            g.Clear(m_clrUserInfoBG);
                            using (Font f = new Font(FontCollection.Families[0], 14, FontStyle.Bold))
                            using (SolidBrush b = new SolidBrush(Color.Black))
                                g.DrawString($"Page {i} / {nPages}", f, b, new RectangleF(m_nMargin, m_nMargin, 225, 27.5f)); 

                            g.DrawImage(SlotGrid(SlotDefault, 4, 3), m_nMargin, m_nMargin + 30);

                            List<Bitmap> arItems = CreateShopItems(i);
                            Bitmap image = ItemsGrid(arItems, 4, 3);

                            g.DrawImage(image, 25, 55);
                        }

                        m_arBmpShopDefault.Add(bmpPage);
                    }
                }

                return m_arBmpShopDefault;
            }
        }

        private List<Bitmap> Items
        {
            get
            {
                if (m_arBmpItems == null)
                {
                    m_arBmpItems = new List<Bitmap>();
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
                if (m_arBmpIcons == null)
                {
                    m_arBmpIcons = new List<Bitmap>();
                    string[] files = Directory.GetFiles(Path.Combine(FilePath, m_sUserInfoFileName, "Icons"));
                    m_arBmpIcons = files.Select(obj => new Bitmap(obj)).ToList();
                }

                return m_arBmpIcons;
            }
        }

        #endregion

        #region Generic Functions

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

            Bitmap result = new Bitmap(UserInfoDefault);

            using (Graphics g = Graphics.FromImage(result))
            using (StringFormat sf = new StringFormat())
            using (SolidBrush b = new SolidBrush(Color.Black))
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                List<Bitmap> arItems = CreateItemImages(arIDs, arAmounts);
                Bitmap bmpInventoryItems = ItemsGrid(arItems, 2, 4);
                g.DrawImage(bmpAvatar, m_nMargin, m_nMargin, m_nAvatar, m_nAvatar);
                g.DrawImage(bmpInventoryItems, m_nAvatar + 2 * m_nMargin + 10, m_nMargin);

                #region Level
                using (SolidBrush bBg = new SolidBrush(m_clrUserInfoBG))
                    g.FillPie(bBg, new Rectangle(m_nAvatar, m_nAvatar, 50, 50), 180, 90);

                using (Font f = new Font(FontCollection.Families[0], 12, FontStyle.Bold))
                    g.DrawString(user.Level.ToString(), f, b, new Rectangle(148, 159, 40, 18), sf);
                #endregion

                #region Username
                using (Font f = new Font(FontCollection.Families[0], 14, FontStyle.Bold))
                    g.DrawString(Truncate(contextUser.Username.ToUpper(), 11), f, b,
                        new Rectangle(m_nMargin, m_nAvatar + 2 * m_nMargin, m_nAvatar + 3 * m_nMargin, 28));
                #endregion

                #region XPBar
                using (SolidBrush bRectFg = new SolidBrush(Color.FromArgb(90, 130, 190)))
                using (SolidBrush bRectBg = new SolidBrush(Color.FromArgb(150, 180, 240)))
                using (SolidBrush bXP = new SolidBrush(Color.Black))
                using (Font fXP = new Font(FontCollection.Families[0], 8, FontStyle.Regular))
                {
                    g.FillRectangle(bRectBg, new Rectangle(m_nMargin, m_nMargin + m_nAvatar + 5, m_nAvatar, 20));

                    //Calculating XP Progress
                    int nOldLvlGap = 0;
                    if (user.Level != 1)
                        nOldLvlGap = Data.Singleton.Levels[user.Level - 1].XPGap;

                    int nLvlGap = Data.Singleton.Levels[user.Level].XPGap;
                    float fWidth = (user.XP - nOldLvlGap) / ((nLvlGap - nOldLvlGap) / 100);
                    fWidth *= 1.5f;

                    g.FillRectangle(bRectFg, new RectangleF(m_nMargin, m_nMargin + m_nAvatar + 5, fWidth, 20));

                    g.DrawString(user.XP.ToString() + " / " + Data.Singleton.Levels[user.Level].XPGap.ToString(),
                    fXP, b, new RectangleF(m_nMargin, 179, m_nAvatar, 20), sf);
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

        public string Shop(int nPage)
        {
            ShopDefault[nPage - 1].Save(Path.Combine(FilePath, "shop.png"), ImageFormat.Png);
            return "shop.png";
        }

        public string QuestsInfo(User user)
        {
            Bitmap result = new Bitmap(300, 240);

            using(Graphics g = Graphics.FromImage(result))
            {
                g.Clear(m_clrUserInfoBG);

                int nOffset = 0;
                for(int i = 0; i < user.CurrentQuests.Count(); ++i)
                {
                    Bitmap bmpPanel = QuestPanel(i, user);
                    g.DrawImage(bmpPanel, 0, nOffset);
                    nOffset += bmpPanel.Height;
                }

                nOffset = result.Height / 3 - 1;
                for(int i = 0; i < user.CurrentQuests.Count() - 1; ++i)
                {
                    using (Pen p = new Pen(Color.Black))
                        g.DrawLine(p, new Point(25, nOffset),
                            new Point(result.Width - 25, nOffset));

                    nOffset += result.Height / 3 - 1;
                }
            }

            result.Save(Path.Combine(FilePath, user.UserID + "Quests.png"), ImageFormat.Png);
            return user.UserID + "Quests.png";
        }

        private Bitmap QuestPanel(int nIndex, User user)
        {
            Quest quest = Data.Singleton.Quests.FirstOrDefault(
                obj => obj.QuestID == user.CurrentQuests.ToArray()[nIndex].Key);

            Bitmap result = new Bitmap(300, 80);

            using (Graphics g = Graphics.FromImage(result))
            using (StringFormat sf = new StringFormat())
            using (SolidBrush b = new SolidBrush(Color.Black))
            using (SolidBrush b2 = new SolidBrush(m_arQuestColors[quest.Difficulty]))
            {
                g.Clear(m_clrUserInfoBG);
                g.FillRectangle(b2, new Rectangle(0, 0, 5, 80));

                if (quest.QuestID == 0)
                {
                    g.DrawImage(Icons[7], 30, 23);

                    // Text
                    using (Font f = new Font(FontCollection.Families[0], 12, FontStyle.Bold))
                        g.DrawString(quest.Name, f, b, new Rectangle(84, 20, 100, 20), sf);

                    using (Font f = new Font(FontCollection.Families[0], 11, FontStyle.Regular))
                        g.DrawString(quest.Description, f, b, new Rectangle(85, 45, 200, 20), sf);

                    return result;
                }

                using (SolidBrush b3 = new SolidBrush(Color.FromArgb(125, m_arQuestColors[quest.Difficulty])))
                    g.FillEllipse(b3, new Rectangle(14, 7, 62, 62));

                int nAngleProgress = 360 / quest.Amount;
                nAngleProgress *= user.CurrentQuests.ToArray()[nIndex].Value;
                g.FillPie(b2, new Rectangle(14, 7, 62, 62), 270, nAngleProgress);

                using (SolidBrush b3 = new SolidBrush(m_clrUserInfoBG))
                    g.FillEllipse(b3, new Rectangle(20, 13, 50, 50));

                g.DrawImage(Icons[6], 30, 23);

                // Text
                using (Font f = new Font(FontCollection.Families[0], 12, FontStyle.Bold))
                    g.DrawString(quest.Name, f, b, new Rectangle(84, 15, 215, 20), sf);

                using (Font f = new Font(FontCollection.Families[0], 11, FontStyle.Regular))
                    g.DrawString(quest.Description, f, b, new Rectangle(85, 35, 215, 20), sf);

                using (Font f = new Font(FontCollection.Families[0], 8, FontStyle.Regular))
                    g.DrawString(user.CurrentQuests.ToArray()[nIndex].Value.ToString() +
                       " / " + quest.Amount.ToString(), f, b, new Rectangle(85, 48, 215, 20), sf);
            }

            return result;
        }

        #region Generic
        private Bitmap SlotGrid(Bitmap bmpSlot, int nX, int nY)
        {
            int nOffset = 10;
            int nWidth = (bmpSlot.Width + nOffset) * nX;
            int nHeight = (bmpSlot.Height + nOffset) * nY;
            Bitmap result = new Bitmap(nWidth, nHeight);
            result.MakeTransparent();

            using (Graphics g = Graphics.FromImage(result))
            {
                int nOffsetX, nOffsetY;
                nOffsetX = nOffsetY = 0;

                for (int i = 0; i < nX * nY; ++i)
                {
                    g.DrawImage(bmpSlot,
                        new Rectangle(nOffsetX, nOffsetY, bmpSlot.Width, bmpSlot.Height));

                    nOffsetX += bmpSlot.Width + nOffset;
                    if (i % nX == nX - 1)
                    {
                        nOffsetX = 0;
                        nOffsetY += bmpSlot.Height + nOffset;
                    }
                }
            }
            return result;
        }

        private Bitmap ItemsGrid(List<Bitmap> arBmpItems, int nX, int nY)
        {
            int nOffset = 10;
            int nWidth = (arBmpItems[0].Width + nOffset) * nX;
            int nHeight = (arBmpItems[0].Height + nOffset) * nY;
            Bitmap result = new Bitmap(nWidth, nHeight);
            result.MakeTransparent();

            using (Graphics g = Graphics.FromImage(result))
            {
                int nOffsetX, nOffsetY;
                nOffsetX = nOffsetY = 0;

                for (int i = 0; i < nX * nY; ++i)
                {
                    g.DrawImage(arBmpItems[i],
                        new Rectangle(nOffsetX, nOffsetY, arBmpItems[i].Width, arBmpItems[i].Height));

                    nOffsetX += arBmpItems[i].Width + nOffset;
                    if (i % nX == nX - 1)
                    {
                        nOffsetX = 0;
                        nOffsetY += arBmpItems[i].Height + nOffset;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Inventory
        private List<Bitmap> CreateItemImages(int[] nIDs, int[] nAmounts)
        {
            List<Bitmap> arItems = new List<Bitmap>();

            for (int i = 0; i < nIDs.Length; ++i)
            {
                Bitmap source2 = new Bitmap(90, 90);

                if (nIDs[i] != 0 && nAmounts[i] != 0)
                {
                    Bitmap temp = Items[nIDs[i] - 1];

                    using (Graphics g = Graphics.FromImage(source2))
                    using (Font f = new Font(FontCollection.Families[0], 12, FontStyle.Regular))
                    using (SolidBrush b = new SolidBrush(Color.Black))
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Far;
                        g.DrawImage(temp, Point.Empty);
                        g.DrawString(nAmounts[i].ToString(), f, b, new Rectangle(55, 65, 30, 18), sf);
                    }
                }

                arItems.Add(source2);
            }

            return arItems;
        }
        #endregion

        #region Shop
        private List<Bitmap> CreateShopItems(int nPage)
        {
            List<Bitmap> arItems = new List<Bitmap>();

            for (int i = (nPage - 1)* 12; i < nPage * 12; ++i)
            {
                Bitmap source2 = new Bitmap(90, 90);

                if (i < Items.Count)
                {
                    Bitmap temp = Items[i];

                    using (Graphics g = Graphics.FromImage(source2))
                    using (Font f = new Font(FontCollection.Families[0], 12, FontStyle.Regular))
                    using (SolidBrush b = new SolidBrush(Color.Black))
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Far;
                        g.DrawImage(temp, Point.Empty);
                        g.DrawImage(Icons[0], new Rectangle(65, 65, 20, 20));

                        int nPrice = Data.Singleton.Items[i].Price;
                        g.DrawString(nPrice.ToString(), f, b, new Rectangle(15, 65, 55, 18), sf);
                    }
                }

                arItems.Add(source2);
            }

            return arItems;
        }
        #endregion
    }
}
