/*
 * Created by SharpDevelop.
 * User: jason (jsfaint@gmail.com)
 * Date: 2009-5-18
 * Time: 13:36
 *
 */

using System;
using System.IO;

namespace PalEditor
{
    #region struct

    public struct PALMagicInfo
    {
        public ushort MagicID;
        public string MagicDesc;
        public bool Enabled;

        public PALMagicInfo(ushort MagicID, string MagicDesc)
        {
            this.MagicID = MagicID;
            this.MagicDesc = MagicDesc;
            this.Enabled = false;
        }
    }

    #endregion

    public class PalMagic
    {
        private const ushort PAL_MAGIC_OFFSET = 0x037C;
        private const ushort Magic_CNT = 103;
        private const ushort Magic_PP_MAX = 32;
        private byte[] mBuf; //缓存

        public ushort[,] magic = new ushort[6, Magic_PP_MAX]; //每人最大仙术数，总人数

        #region MagicList

        public PALMagicInfo[] magicList = new PALMagicInfo[Magic_CNT]
        {
            new PALMagicInfo(0x0127, "梦蛇"),
            new PALMagicInfo(0x0128, "气疗术"),
            new PALMagicInfo(0x0129, "观音咒"),
            new PALMagicInfo(0x012A, "凝神归元"),
            new PALMagicInfo(0x012B, "元灵归心术"),
            new PALMagicInfo(0x012C, "五气朝元"),
            new PALMagicInfo(0x012D, "还魂咒"),
            new PALMagicInfo(0x012E, "赎魂"),
            new PALMagicInfo(0x012F, "回梦"),
            new PALMagicInfo(0x0130, "夺魂"),
            new PALMagicInfo(0x0131, "鬼降"),
            new PALMagicInfo(0x0132, "净衣咒"),
            new PALMagicInfo(0x0133, "冰心诀"),
            new PALMagicInfo(0x0134, "灵血咒"),
            new PALMagicInfo(0x0135, "金刚咒"),
            new PALMagicInfo(0x0136, "真元护体"),
            new PALMagicInfo(0x0137, "天罡战气"),
            new PALMagicInfo(0x0138, "风咒"),
            new PALMagicInfo(0x0139, "旋风咒"),
            new PALMagicInfo(0x013A, "风卷残云"),
            new PALMagicInfo(0x013B, "风神"),
            new PALMagicInfo(0x013C, "雷咒"),
            new PALMagicInfo(0x013D, "五雷咒"),
            new PALMagicInfo(0x013E, "天雷破"),
            new PALMagicInfo(0x013F, "狂雷 "),
            new PALMagicInfo(0x0140, "雷神"),
            new PALMagicInfo(0x0141, "冰咒"),
            new PALMagicInfo(0x0142, "玄冰咒"),
            new PALMagicInfo(0x0143, "风雪冰天"),
            new PALMagicInfo(0x0144, "风雪冰天1"),
            new PALMagicInfo(0x0145, "雪妖"),
            new PALMagicInfo(0x0146, "火术"),
            new PALMagicInfo(0x0147, "炎咒 "),
            new PALMagicInfo(0x0148, "三昧真火"),
            new PALMagicInfo(0x0149, "炎杀咒"),
            new PALMagicInfo(0x014A, "炼狱真火"),
            new PALMagicInfo(0x014B, "火龙"),
            new PALMagicInfo(0x014C, "土咒"),
            new PALMagicInfo(0x014D, "飞岩术"),
            new PALMagicInfo(0x014E, "地裂天崩"),
            new PALMagicInfo(0x014F, "泰山压顶 "),
            new PALMagicInfo(0x0150, "山神"),
            new PALMagicInfo(0x0151, "气剑指"),
            new PALMagicInfo(0x0152, "弦月斩"),
            new PALMagicInfo(0x0153, "弦月斩1"),
            new PALMagicInfo(0x0154, "一阳指"),
            new PALMagicInfo(0x0155, "七诀剑气"),
            new PALMagicInfo(0x0156, "斩龙诀"),
            new PALMagicInfo(0x0157, "暗器"),
            new PALMagicInfo(0x0158, "铜钱镖"),
            new PALMagicInfo(0x0159, "御剑术"),
            new PALMagicInfo(0x015A, "万剑诀"),
            new PALMagicInfo(0x015B, "心剑合一"),
            new PALMagicInfo(0x015C, "天剑"),
            new PALMagicInfo(0x015D, "天师符法"),
            new PALMagicInfo(0x015E, "斩魔刀"),
            new PALMagicInfo(0x015F, "武神 "),
            new PALMagicInfo(0x0160, "三尸咒"),
            new PALMagicInfo(0x0161, "御蜂术"),
            new PALMagicInfo(0x0162, "万蚁蚀象"),
            new PALMagicInfo(0x0163, "天女散花"),
            new PALMagicInfo(0x0164, "剑气"),
            new PALMagicInfo(0x0165, "炼狱爪"),
            new PALMagicInfo(0x0166, "血魔神功"),
            new PALMagicInfo(0x0167, "狂风术"),
            new PALMagicInfo(0x0168, "鞭击"),
            new PALMagicInfo(0x0169, "御剑伏魔"),
            new PALMagicInfo(0x016A, "御剑伏魔1"),
            new PALMagicInfo(0x016B, "剑神"),
            new PALMagicInfo(0x016C, "风灵符法"),
            new PALMagicInfo(0x016D, "雷灵符法"),
            new PALMagicInfo(0x016E, "水灵符法"),
            new PALMagicInfo(0x016F, "火灵符法 "),
            new PALMagicInfo(0x0170, "土灵符法"),
            new PALMagicInfo(0x0171, "气吞天下"),
            new PALMagicInfo(0x0172, "酒神"),
            new PALMagicInfo(0x0173, "瘴气"),
            new PALMagicInfo(0x0174, "万蛊蚀天"),
            new PALMagicInfo(0x0175, "毒吞天下"),
            new PALMagicInfo(0x0176, "爆炸蛊"),
            new PALMagicInfo(0x0177, "镇天鼎"),
            new PALMagicInfo(0x0178, "咒蛇"),
            new PALMagicInfo(0x0179, "飞龙探云手"),
            new PALMagicInfo(0x017A, "火龙掌"),
            new PALMagicInfo(0x017B, "灭绝一击"),
            new PALMagicInfo(0x017C, "横扫千军"),
            new PALMagicInfo(0x017D, "爆炸"),
            new PALMagicInfo(0x017E, "魔掌天下"),
            new PALMagicInfo(0x017F, "佛法无边 "),
            new PALMagicInfo(0x0180, "灵葫咒"),
            new PALMagicInfo(0x0181, "气魔焰"),
            new PALMagicInfo(0x0182, "合体气功"),
            new PALMagicInfo(0x0183, "气功"),
            new PALMagicInfo(0x0184, "剑气斩"),
            new PALMagicInfo(0x0185, "火神"),
            new PALMagicInfo(0x0186, "醉仙望月步"),
            new PALMagicInfo(0x0187, "投掷"),
            new PALMagicInfo(0x0188, "金蝉脱壳"),
            new PALMagicInfo(0x0189, "仙风云体术"),
            new PALMagicInfo(0x018A, "乾坤一掷"),
            new PALMagicInfo(0x018B, "大咒蛇"),
            new PALMagicInfo(0x018C, "腥风血雨"),
            new PALMagicInfo(0x018D, "群魔乱舞")
        };

        #endregion

        public PalMagic()
        {
            mBuf = new byte[Magic_PP_MAX * 6 * 2];
        }

        #region Load Save

        public void LoadPalMagic(System.IO.FileStream fStream)
        {
            try
            {
                fStream.Seek(PAL_MAGIC_OFFSET, SeekOrigin.Begin);
                fStream.Read(mBuf, 0, mBuf.Length);

                for (int mIndex = 0; mIndex < Magic_PP_MAX; mIndex++)
                {
                    for (int pIndex = 0; pIndex < 6; pIndex++)
                    {
                        magic[pIndex, mIndex] = System.BitConverter.ToUInt16(mBuf, (mIndex * 6 + pIndex) * 2);
                    }
                }

#if DEBUG
                this.outputDbgFile();
#endif
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Exception in PalMagic.LoadPalMagic(): " + e.Message);
            }
        }

        public void SavePalMagic(System.IO.FileStream fStream)
        {
            try
            {
                for (int mIndex = 0; mIndex < Magic_PP_MAX; mIndex++)
                {
                    for (int pIndex = 0; pIndex < 6; pIndex++)
                    {
                        byte[] tmp1 = System.BitConverter.GetBytes(magic[pIndex, mIndex]);
                        mBuf[(mIndex * 6 + pIndex) * 2 + 0] = tmp1[0];
                        mBuf[(mIndex * 6 + pIndex) * 2 + 1] = tmp1[1];
                    }
                }

                //缓存到文件
                fStream.Seek(PAL_MAGIC_OFFSET, SeekOrigin.Begin);
                fStream.Write(mBuf, 0, mBuf.Length);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Exception in PalMagic.SavePalMagic(): " + e.Message);
            }
        }

        public void ArrayToDisplay(int pIndex)
        {
            //清空
            for (int ii = 0; ii < Magic_CNT; ii++)
                magicList[ii].Enabled = false;

            for (int ii = 0; ii < Magic_CNT; ii++)
            {
                for (int mIndex = 0; mIndex < Magic_PP_MAX; mIndex++)
                {
                    if (magic[pIndex, mIndex] == 0)
                        continue;
                    if (magicList[ii].MagicID == magic[pIndex, mIndex])
                    {
                        magicList[ii].Enabled = true;
                        break;
                    }
                }
            }
        }

        public void DisplayToArray(int pIndex)
        {
            //清空
            for (int mIndex = 0; mIndex < Magic_PP_MAX; mIndex++)
                magic[pIndex, mIndex] = 0;

            for (int ii = 0; ii < Magic_CNT; ii++)
            {
                if (magicList[ii].Enabled == false)
                    continue;
                for (int mIndex = 0; mIndex < Magic_PP_MAX; mIndex++)
                {
                    if (magic[pIndex, mIndex] == 0)
                    {
                        magic[pIndex, mIndex] = magicList[ii].MagicID;
                        break;
                    }
                }
            }
        }

        #endregion

        #region debug

#if DEBUG
        //the following function only for debug.
        private void DumpHex(byte[] tmp)
        {
            String strout = "";
            int count = 0;

            for (int ii = 0; ii < tmp.Length; ii++)
            {
                strout = strout + tmp[ii].ToString("X2") + ", ";
                count++;

                if (count % 10 == 0)
                {
                    strout += "\r\n";
                    count = 0;
                }
            }

            TextWriter stringWriter = new System.IO.StreamWriter("dumpHex.txt");
            stringWriter.Write(strout);
            stringWriter.Close();
        }

        private void outputDbgFile()
        {
            String strout = "";
            TextWriter stringWriter = new System.IO.StreamWriter("debug.txt");

            for (int magicIdx = 0; magicIdx < Magic_PP_MAX; magicIdx++)
            {
                strout = "第" + magicIdx.ToString("D2") + "项仙术: ";
                for (int personIdx = 0; personIdx < 6; personIdx++)
                {
                    strout = strout + magic[personIdx, magicIdx].ToString("X4") + ",";
                }

                strout += "\r\n";
                stringWriter.Write(strout);
            }

            stringWriter.Close();
        }
#endif

        #endregion
    }
}