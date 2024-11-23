/*
 * Created by SharpDevelop.
 * User: jason(jsfaint@gmail.com)
 * Date: 2009-4-21
 * Time: 10:25
 *
 */

using System;
using System.IO;

namespace PalEditor //should be modified
{
    /// <summary>
    /// Description of SaveData.
    /// </summary>
    public class RPGData
    {
        #region offset map

        // offset map
        private const uint PAL_SAVETIME_OFFSET = 0x0000; //保存次数
        private const uint PAL_CALABASH_OFFSET = 0x0018; //当前灵葫值
        private const uint PAL_MONEY_OFFSET = 0x0028; //金钱

        private readonly uint[] p1_offset =
            { 0x7C, 0x244, 0x250, 0x25c, 0x268, 0x274, 0x2c8, 0x2d4, 0x2e0, 0x2ec, 0x2f8 };

        private readonly uint[] p2_offset =
            { 0x84, 0x246, 0x252, 0x25e, 0x26a, 0x276, 0x2ca, 0x2d6, 0x2e2, 0x2ee, 0x2fa };

        private readonly uint[] p3_offset =
            { 0x8c, 0x248, 0x254, 0x260, 0x26c, 0x278, 0x2cc, 0x2d8, 0x2e4, 0x2f0, 0x2fc };

        private readonly uint[] p4_offset =
            { 0x94, 0x24a, 0x256, 0x262, 0x26e, 0x27a, 0x2ce, 0x2da, 0x2e6, 0x2f2, 0x2fe };

        private readonly uint[] p5_offset =
            { 0x9c, 0x24c, 0x258, 0x264, 0x270, 0x27e, 0x2d0, 0x2dc, 0x2e8, 0x2f4, 0x300 };

        #endregion

        //人物
        public PalExp[] palExp;

        //物品
        public PalGoods palGoods;

        //仙术
        public PalMagic palMagic;

        //其他
        public uint palMoney; //金钱
        public ushort palSaveTime; //保存次数
        public ushort palCalabash; //灵葫值
        private FileStream fStream;

        public RPGData()
        {
            palExp = new PalExp[5]
            {
                new PalExp(p1_offset), //李逍遥
                new PalExp(p2_offset), //赵灵儿
                new PalExp(p3_offset), //林月如
                new PalExp(p4_offset), //巫后
                new PalExp(p5_offset) //阿奴
            };
            palGoods = new PalGoods();
            palMagic = new PalMagic();
        }

        public bool OpenPalSave(String fileName)
        {
            if (!File.Exists(fileName))
            {
                return false; //文件未找到
            }

            try
            {
                fStream.Close();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            try
            {
                fStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                LoadFromFile();
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in RPGData.OpenPalSave(): " + e.Message);
                System.Windows.Forms.MessageBox.Show("打开存档文件失败", "错误");
                return false;
            }
        }

        #region Money 金钱

        private bool LoadMoney(System.IO.BinaryReader br)
        {
            try
            {
                fStream.Seek(PAL_MONEY_OFFSET, SeekOrigin.Begin);
                palMoney = br.ReadUInt32();
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in RPGData.LoadMoney(): " + e.Message);
                return false;
            }
        }

        private bool SaveMoney()
        {
            try
            {
                byte[] tmp = System.BitConverter.GetBytes(this.palMoney);
                WriteByte(PAL_MONEY_OFFSET, tmp);
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in RPGData.SaveMoney(): " + e.Message);
                System.Windows.Forms.MessageBox.Show("Exception in RPGData.SaveMoney(): " + e.Message);
                return false;
            }
        }

        #endregion

        #region Save Time 存档次数

        private bool LoadSaveTime(System.IO.BinaryReader br)
        {
            try
            {
                fStream.Seek(PAL_SAVETIME_OFFSET, SeekOrigin.Begin);
                palSaveTime = br.ReadUInt16();
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in RPGData.LoadSaveTime(): " + e.Message);
                return false;
            }
        }

        private bool SaveSaveTime()
        {
            try
            {
                byte[] tmp = System.BitConverter.GetBytes(this.palSaveTime);
                WriteByte(PAL_SAVETIME_OFFSET, tmp);
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in RPGData.SaveSaveTime(): " + e.Message);
                return false;
            }
        }

        #endregion

        #region Calabash 灵葫值

        private bool LoadCalabash(System.IO.BinaryReader br)
        {
            try
            {
                fStream.Seek(PAL_CALABASH_OFFSET, SeekOrigin.Begin);
                palCalabash = br.ReadUInt16();
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in RPGData.LoadCalabash(): " + e.Message);
                return false;
            }
        }

        private bool SaveCalabash()
        {
            try
            {
                byte[] tmp = System.BitConverter.GetBytes(this.palCalabash);
                WriteByte(PAL_CALABASH_OFFSET, tmp);
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in RPGData.SaveCalabash(): " + e.Message);
                return false;
            }
        }

        #endregion

        public int LoadFromFile()
        {
            try
            {
                System.IO.BinaryReader binRead = new BinaryReader(fStream);
                LoadMoney(binRead);
                LoadSaveTime(binRead);
                LoadCalabash(binRead);

                for (int ii = 0; ii < palExp.Length; ii++)
                    palExp[ii].LoadPalExp(fStream);

                palGoods.LoadPalGoods(fStream);
                palMagic.LoadPalMagic(fStream);
                return 0;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in RPGData.LoadFromFile(): " + e.Message);
                System.Windows.Forms.MessageBox.Show("Exception in RPGData.LoadFromFile(): " + e.Message);
                return -1;
            }
        }

        public int SaveToFile()
        {
            try
            {
                SaveMoney(); //金钱
                SaveCalabash(); //灵葫值
                SaveSaveTime(); //存盘次数

                for (int ii = 0; ii < palExp.Length; ii++)
                    palExp[ii].SavePalExp(fStream);

                palGoods.SavePalGoods(fStream);
                palMagic.SavePalMagic(fStream);
                fStream.Close();
                return 0;
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in RPGData.SaveToFile(): " + e.Message);
                System.Windows.Forms.MessageBox.Show("Exception in RPGData.SaveToFile(): " + e.Message);
                return -1;
            }
        }

        private void WriteByte(uint offset, byte[] byteArray)
        {
            try
            {
                fStream.Seek(offset, SeekOrigin.Begin);
                fStream.Write(byteArray, 0, byteArray.Length);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Exception in RPGData.WriteByte(): " + e.Message);
            }
        }

        //debug using
        private void MessageBoxByteArray(byte[] byteArray)
        {
            string tmp = "";

            for (int ii = 0; ii < byteArray.Length; ii++)
            {
                tmp += ii.ToString() + " = " + byteArray[ii].ToString("X") + "\n";
            }

            System.Windows.Forms.MessageBox.Show(tmp, "Information");
        }
    }
}