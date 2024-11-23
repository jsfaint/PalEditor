/*
 * Created by SharpDevelop.
 * User: jason(jsfaint@gmail.com)
 * Date: 2009-5-14
 * Time: 10:28
 *
 */

using System;
using System.IO;

namespace PalEditor
{
    #region struct

    public struct PALGoods
    {
        public ushort goodsID;
        public ushort goodsCnt;

        public PALGoods(ushort objID, ushort objCnt)
        {
            this.goodsID = objID;
            this.goodsCnt = objCnt;
        }
    }

    public struct PalGoodsInfo
    {
        public ushort goodsID; //物品编号
        public string goodsDescript; //物品名称
        public ushort goodsCnt;

        public PalGoodsInfo(ushort objID, string objDescript)
        {
            this.goodsID = objID;
            this.goodsDescript = objDescript;
            this.goodsCnt = 0;
        }
    }

    #endregion

    public class PalGoods
    {
        private uint PAL_GOODS_OFFSET = 0x6c0;
        private byte[] gBuf; //缓存
        private PALGoods[] goods; //内存数组
        private const ushort GOODS_CNT = 234;

        #region PalGoodsInfo

        public PalGoodsInfo[] goodsInfo = new PalGoodsInfo[GOODS_CNT] //显示用数组
        {
            new PalGoodsInfo(0x003d, "观音符"),
            new PalGoodsInfo(0x003e, "圣灵符"),
            new PalGoodsInfo(0x003f, "金刚符"),
            new PalGoodsInfo(0x0040, "净衣符"),
            new PalGoodsInfo(0x0041, "灵心符"),
            new PalGoodsInfo(0x0042, "天师符"),
            new PalGoodsInfo(0x0043, "风灵符"),
            new PalGoodsInfo(0x0044, "雷灵符"),
            new PalGoodsInfo(0x0045, "水灵符"),
            new PalGoodsInfo(0x0046, "火灵符"),
            new PalGoodsInfo(0x0047, "土灵符"),
            new PalGoodsInfo(0x0048, "舍利子"),
            new PalGoodsInfo(0x0049, "玉菩提"),
            new PalGoodsInfo(0x004a, "银杏子"),
            new PalGoodsInfo(0x004b, "糯米"),
            new PalGoodsInfo(0x004c, "糯米糕"),
            new PalGoodsInfo(0x004d, "盐巴"),
            new PalGoodsInfo(0x004e, "茶叶蛋"),
            new PalGoodsInfo(0x004f, "鸡蛋"),
            new PalGoodsInfo(0x0050, "糖葫芦"),
            new PalGoodsInfo(0x0051, "蜡烛"),
            new PalGoodsInfo(0x0052, "符纸"),
            new PalGoodsInfo(0x0053, "檀香"),
            new PalGoodsInfo(0x0054, "大蒜"),
            new PalGoodsInfo(0x0055, "黑狗血"),
            new PalGoodsInfo(0x0056, "酒"),
            new PalGoodsInfo(0x0057, "雄黄"),
            new PalGoodsInfo(0x0058, "雄黄酒"),
            new PalGoodsInfo(0x0059, "九节菖蒲"),
            new PalGoodsInfo(0x005a, "驱魔香"),
            new PalGoodsInfo(0x005b, "十里香"),
            new PalGoodsInfo(0x005c, "水果"),
            new PalGoodsInfo(0x005d, "烧肉"),
            new PalGoodsInfo(0x005e, "腌肉"),
            new PalGoodsInfo(0x005f, "还魂香 "),
            new PalGoodsInfo(0x0060, "赎魂灯"),
            new PalGoodsInfo(0x0061, "孟婆汤"),
            new PalGoodsInfo(0x0062, "天香续命露"),
            new PalGoodsInfo(0x0063, "止血草"),
            new PalGoodsInfo(0x0064, "行军丹"),
            new PalGoodsInfo(0x0065, "金创药"),
            new PalGoodsInfo(0x0066, "蟠果"),
            new PalGoodsInfo(0x0067, "紫青玉容膏"),
            new PalGoodsInfo(0x0068, "鼠儿果"),
            new PalGoodsInfo(0x0069, "还神丹"),
            new PalGoodsInfo(0x006a, "龙涎草"),
            new PalGoodsInfo(0x006b, "灵山仙芝"),
            new PalGoodsInfo(0x006c, "雪莲子"),
            new PalGoodsInfo(0x006d, "天仙玉露"),
            new PalGoodsInfo(0x006e, "神仙茶"),
            new PalGoodsInfo(0x006f, "灵胡仙丹"),
            new PalGoodsInfo(0x0070, "试炼果"),
            new PalGoodsInfo(0x0071, "女娲石"),
            new PalGoodsInfo(0x0072, "八仙石"),
            new PalGoodsInfo(0x0073, "蜂巢"),
            new PalGoodsInfo(0x0074, "尸腐肉"),
            new PalGoodsInfo(0x0075, "毒蛇卵"),
            new PalGoodsInfo(0x0076, "毒蝎卵"),
            new PalGoodsInfo(0x0077, "毒蟾卵"),
            new PalGoodsInfo(0x0078, "蜘蛛卵"),
            new PalGoodsInfo(0x0079, "蜈蚣卵"),
            new PalGoodsInfo(0x007a, "鹤顶红"),
            new PalGoodsInfo(0x007b, "孔雀胆"),
            new PalGoodsInfo(0x007c, "血海棠"),
            new PalGoodsInfo(0x007d, "断肠草"),
            new PalGoodsInfo(0x007e, "醍醐香"),
            new PalGoodsInfo(0x007f, "忘魂花"),
            new PalGoodsInfo(0x0080, "紫罂粟"),
            new PalGoodsInfo(0x0081, "鬼枯藤"),
            new PalGoodsInfo(0x0082, "腹蛇涎"),
            new PalGoodsInfo(0x0083, "蜂王蜜"),
            new PalGoodsInfo(0x0084, "雪蛤蟆"),
            new PalGoodsInfo(0x0085, "赤蝎粉"),
            new PalGoodsInfo(0x0086, "化尸水"),
            new PalGoodsInfo(0x0087, "迷魂香"),
            new PalGoodsInfo(0x0088, "九阴散"),
            new PalGoodsInfo(0x0089, "无影毒"),
            new PalGoodsInfo(0x008a, "三尸蛊"),
            new PalGoodsInfo(0x008b, "金蚕蛊"),
            new PalGoodsInfo(0x008c, "幻蛊"),
            new PalGoodsInfo(0x008d, "隐蛊"),
            new PalGoodsInfo(0x008e, "冰蚕蛊"),
            new PalGoodsInfo(0x008f, "火蚕蛊"),
            new PalGoodsInfo(0x0090, "食妖蛊"),
            new PalGoodsInfo(0x0091, "灵蛊"),
            new PalGoodsInfo(0x0092, "爆烈蛊"),
            new PalGoodsInfo(0x0093, "碧血蛊"),
            new PalGoodsInfo(0x0094, "蛊"),
            new PalGoodsInfo(0x0095, "赤血蛊"),
            new PalGoodsInfo(0x0096, "金蚕王"),
            new PalGoodsInfo(0x0097, "引路蜂"),
            new PalGoodsInfo(0x0098, "傀儡蛊"),
            new PalGoodsInfo(0x0099, "梅花镖"),
            new PalGoodsInfo(0x009a, "袖里剑"),
            new PalGoodsInfo(0x009b, "透骨丁"),
            new PalGoodsInfo(0x009c, "雷火珠"),
            new PalGoodsInfo(0x009d, "毒龙砂"),
            new PalGoodsInfo(0x009e, "吸星锁"),
            new PalGoodsInfo(0x009f, "缠魂丝"),
            new PalGoodsInfo(0x00a0, "捆仙绳"),
            new PalGoodsInfo(0x00a1, "无影神针"),
            new PalGoodsInfo(0x00a2, "血玲珑"),
            new PalGoodsInfo(0x00a3, "长鞭"),
            new PalGoodsInfo(0x00a4, "九节鞭"),
            new PalGoodsInfo(0x00a5, "金蛇鞭"),
            new PalGoodsInfo(0x00a6, "木剑"),
            new PalGoodsInfo(0x00a7, "短刀"),
            new PalGoodsInfo(0x00a8, "铁剑"),
            new PalGoodsInfo(0x00a9, "大刀"),
            new PalGoodsInfo(0x00aa, "仙女剑"),
            new PalGoodsInfo(0x00ab, "长剑"),
            new PalGoodsInfo(0x00ac, "红缨刀"),
            new PalGoodsInfo(0x00ad, "越女剑"),
            new PalGoodsInfo(0x00ae, "戒刀"),
            new PalGoodsInfo(0x00af, "玄铁剑"),
            new PalGoodsInfo(0x00b0, "芙蓉刀"),
            new PalGoodsInfo(0x00b1, "柳月刀"),
            new PalGoodsInfo(0x00b2, "青锋剑"),
            new PalGoodsInfo(0x00b3, "苗刀"),
            new PalGoodsInfo(0x00b4, "凤鸣刀"),
            new PalGoodsInfo(0x00b5, "双龙剑"),
            new PalGoodsInfo(0x00b6, "玉女剑"),
            new PalGoodsInfo(0x00b7, "金童剑"),
            new PalGoodsInfo(0x00b8, "龙泉剑"),
            new PalGoodsInfo(0x00b9, "鬼牙刀"),
            new PalGoodsInfo(0x00ba, "七星剑"),
            new PalGoodsInfo(0x00bb, "玄冥宝刀"),
            new PalGoodsInfo(0x00bc, "巫月神刀"),
            new PalGoodsInfo(0x00bd, "磐龙剑"),
            new PalGoodsInfo(0x00be, "太极剑"),
            new PalGoodsInfo(0x00bf, "无尘剑"),
            new PalGoodsInfo(0x00c0, "青蛇杖"),
            new PalGoodsInfo(0x00c1, "鬼头杖"),
            new PalGoodsInfo(0x00c2, "冥蛇杖"),
            new PalGoodsInfo(0x00c3, "天蛇杖"),
            new PalGoodsInfo(0x00c4, "头巾"),
            new PalGoodsInfo(0x00c5, "青丝巾"),
            new PalGoodsInfo(0x00c6, "发饰"),
            new PalGoodsInfo(0x00c7, "银钗 "),
            new PalGoodsInfo(0x00c8, "翠玉金钗"),
            new PalGoodsInfo(0x00c9, "皮帽"),
            new PalGoodsInfo(0x00ca, "珍珠冠"),
            new PalGoodsInfo(0x00cb, "天师帽"),
            new PalGoodsInfo(0x00cc, "紫金冠"),
            new PalGoodsInfo(0x00cd, "天蚕丝带"),
            new PalGoodsInfo(0x00ce, "凤凰羽毛"),
            new PalGoodsInfo(0x00cf, "冲天冠"),
            new PalGoodsInfo(0x00d0, "布袍"),
            new PalGoodsInfo(0x00d1, "藤甲"),
            new PalGoodsInfo(0x00d2, "丝衣"),
            new PalGoodsInfo(0x00d3, "铁锁衣"),
            new PalGoodsInfo(0x00d4, "夜行衣"),
            new PalGoodsInfo(0x00d5, "青铜甲"),
            new PalGoodsInfo(0x00d6, "罗汉袍"),
            new PalGoodsInfo(0x00d7, "铁鳞甲"),
            new PalGoodsInfo(0x00d8, "天师道袍"),
            new PalGoodsInfo(0x00d9, "精铁战甲"),
            new PalGoodsInfo(0x00da, "金镂衣"),
            new PalGoodsInfo(0x00db, "鬼针胄"),
            new PalGoodsInfo(0x00dc, "天蚕宝衣"),
            new PalGoodsInfo(0x00dd, "青龙宝甲"),
            new PalGoodsInfo(0x00de, "白虎之铠"),
            new PalGoodsInfo(0x00df, "玄武战袍"),
            new PalGoodsInfo(0x00e0, "朱雀战衣"),
            new PalGoodsInfo(0x00e1, "披风"),
            new PalGoodsInfo(0x00e2, "护肩"),
            new PalGoodsInfo(0x00e3, "武士披风"),
            new PalGoodsInfo(0x00e4, "护心镜"),
            new PalGoodsInfo(0x00e5, "霓虹羽衣"),
            new PalGoodsInfo(0x00e6, "菩提袈裟"),
            new PalGoodsInfo(0x00e7, "虎纹披风"),
            new PalGoodsInfo(0x00e8, "凤纹披风"),
            new PalGoodsInfo(0x00e9, "龙纹披风"),
            new PalGoodsInfo(0x00ea, "圣灵披风"),
            new PalGoodsInfo(0x00eb, "草鞋"),
            new PalGoodsInfo(0x00ec, "木鞋"),
            new PalGoodsInfo(0x00ed, "布靴"),
            new PalGoodsInfo(0x00ee, "绣花鞋"),
            new PalGoodsInfo(0x00ef, "铁履"),
            new PalGoodsInfo(0x00f0, "武僧靴"),
            new PalGoodsInfo(0x00f1, "鹿皮靴"),
            new PalGoodsInfo(0x00f2, "疾风靴"),
            new PalGoodsInfo(0x00f3, "莲花靴"),
            new PalGoodsInfo(0x00f4, "虎皮靴"),
            new PalGoodsInfo(0x00f5, "龙鳞靴"),
            new PalGoodsInfo(0x00f6, "步云靴"),
            new PalGoodsInfo(0x00f7, "魅影神靴"),
            new PalGoodsInfo(0x00f8, "香袋"),
            new PalGoodsInfo(0x00f9, "护腕"),
            new PalGoodsInfo(0x00fa, "铁护腕"),
            new PalGoodsInfo(0x00fb, "竹笛 "),
            new PalGoodsInfo(0x00fc, "珍珠"),
            new PalGoodsInfo(0x00fd, "玉镯"),
            new PalGoodsInfo(0x00fe, "念珠"),
            new PalGoodsInfo(0x00ff, "银针"),
            new PalGoodsInfo(0x0100, "铜镜"),
            new PalGoodsInfo(0x0101, "八卦镜"),
            new PalGoodsInfo(0x0102, "乾坤镜"),
            new PalGoodsInfo(0x0103, "豹牙手环"),
            new PalGoodsInfo(0x0104, "圣灵珠"),
            new PalGoodsInfo(0x0105, "金罡珠"),
            new PalGoodsInfo(0x0106, "五毒珠"),
            new PalGoodsInfo(0x0107, "风灵珠"),
            new PalGoodsInfo(0x0108, "雷灵珠"),
            new PalGoodsInfo(0x0109, "水灵珠"),
            new PalGoodsInfo(0x010a, "火灵珠"),
            new PalGoodsInfo(0x010b, "土灵珠"),
            new PalGoodsInfo(0x010c, "炼蛊器"),
            new PalGoodsInfo(0x010d, "寿葫芦"),
            new PalGoodsInfo(0x010e, "紫金葫芦"),
            new PalGoodsInfo(0x010f, "布包"),
            new PalGoodsInfo(0x0110, "桂花酒"),
            new PalGoodsInfo(0x0111, "紫金丹"),
            new PalGoodsInfo(0x0112, "玉佛珠"),
            new PalGoodsInfo(0x0113, "金凤凰蛋壳"),
            new PalGoodsInfo(0x0114, "火眼麒麟角"),
            new PalGoodsInfo(0x0115, "青龙碧血玉"),
            new PalGoodsInfo(0x0116, "毒龙胆"),
            new PalGoodsInfo(0x0117, "破天槌"),
            new PalGoodsInfo(0x0118, "包袱"),
            new PalGoodsInfo(0x0119, "银杏果"),
            new PalGoodsInfo(0x011a, "鲤鱼"),
            new PalGoodsInfo(0x011b, "鹿茸"),
            new PalGoodsInfo(0x011c, "钓竿"),
            new PalGoodsInfo(0x011d, "捕兽夹"),
            new PalGoodsInfo(0x011e, "六神丹"),
            new PalGoodsInfo(0x011f, "情书"),
            new PalGoodsInfo(0x0120, "玉佩"),
            new PalGoodsInfo(0x0121, "石钥匙"),
            new PalGoodsInfo(0x0122, "天书"),
            new PalGoodsInfo(0x0123, "香蕉"),
            new PalGoodsInfo(0x0124, "凤纹手绢"),
            new PalGoodsInfo(0x0125, "手卷"),
            new PalGoodsInfo(0x0126, "芦苇漂")
        };

        #endregion

        public PalGoods()
        {
            gBuf = new byte[GOODS_CNT * 6]; //缓存
            goods = new PALGoods[GOODS_CNT]; //内存数组
        }

        #region Load Save

        public void LoadPalGoods(System.IO.FileStream fStream)
        {
            try
            {
                //文件到缓存
                fStream.Seek(PAL_GOODS_OFFSET, SeekOrigin.Begin);
                fStream.Read(gBuf, 0, gBuf.Length);

                //缓存到内存数组
                for (int ii = 0; ii < GOODS_CNT; ii++)
                {
                    ushort id = System.BitConverter.ToUInt16(gBuf, ii * 6);
                    ushort cnt = System.BitConverter.ToUInt16(gBuf, ii * 6 + 2);
                    goods[ii] = new PALGoods(id, cnt);
                }

                //内存数组到显示用数组
                for (int ii = 0; ii < GOODS_CNT; ii++)
                {
                    if (goods[ii].goodsID == 0) //id为0时说明箱子中无此物品
                        continue;
                    for (int jj = 0; jj < GOODS_CNT; jj++)
                    {
                        if (goods[ii].goodsID == goodsInfo[jj].goodsID)
                        {
                            goodsInfo[jj].goodsCnt = goods[ii].goodsCnt;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Exception in PalGoods.LoadPalGoods(): " + e.Message);
            }
        }

        public void SavePalGoods(System.IO.FileStream fStream)
        {
            try
            {
                //显示用数组到内存数组, ii:显示, jj=内存
                for (int ii = 0; ii < GOODS_CNT; ii++)
                {
                    if (goodsInfo[ii].goodsCnt == 0)
                        continue;

                    bool bExist = false;

                    for (int jj = 0; jj < GOODS_CNT; jj++)
                    {
                        if (goods[jj].goodsID == 0)
                        {
                            if (!bExist)
                            {
                                goods[jj].goodsID = goodsInfo[ii].goodsID;
                                goods[jj].goodsCnt = goodsInfo[ii].goodsCnt;
                            }

                            break;
                        }

                        if (goods[jj].goodsID == goodsInfo[ii].goodsID)
                        {
                            goods[jj].goodsCnt = goodsInfo[ii].goodsCnt;
                            bExist = true;
                            break;
                        }
                    }
                }

                //内存数组到缓存
                for (int ii = 0; ii < GOODS_CNT; ii++)
                {
                    byte[] tmp1 = System.BitConverter.GetBytes(goods[ii].goodsID);
                    byte[] tmp2 = System.BitConverter.GetBytes(goods[ii].goodsCnt);

                    gBuf[ii * 6 + 0] = tmp1[0];
                    gBuf[ii * 6 + 1] = tmp1[1];
                    gBuf[ii * 6 + 2] = tmp2[0];
                    gBuf[ii * 6 + 3] = tmp2[1];
                }

                //缓存到文件
                fStream.Seek(PAL_GOODS_OFFSET, SeekOrigin.Begin);
                fStream.Write(gBuf, 0, gBuf.Length);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Exception in PalGoods.SavePalGoods(): " + e.Message);
            }
        }

        #endregion

        public void allGoods99()
        {
            for (int ii = 0; ii < GOODS_CNT; ii++)
            {
                goodsInfo[ii].goodsCnt = 99;
            }
        }

        #region debug

#if DEBUG
        //the following functions only for debug.
        private void outputDbgFile(string fileName)
        {
            String strout = "";

            for (int ii = 0; ii < GOODS_CNT; ii++)
            {
                strout = strout + goodsInfo[ii].goodsDescript + ", " + goodsInfo[ii].goodsCnt + "\r\n";
            }

            TextWriter stringWriter = new System.IO.StreamWriter(fileName);
            stringWriter.Write(strout);
            stringWriter.Close();
        }

        private void DumpHex(byte[] tmp)
        {
            String strout = "";
            int count = 0;

            for (int ii = 0; ii < tmp.Length; ii++)
            {
                strout = strout + tmp[ii].ToString() + ", ";
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
#endif

        #endregion
    }
}