/*
 * Created by SharpDevelop.
 * User: jason(jsfaint@gmail.com)
 * Date: 2009-4-22
 * Time: 11:19
 *
 */

using System;
using System.IO;

namespace PalEditor
{
    /// <summary>
    /// Description of PalExp.
    /// </summary>
    public class PalExp
    {
        //offset map
        private uint exp_offset;
        private uint now_rank_offset;
        private uint MaxHP_offset;
        private uint MaxMP_offset;
        private uint HP_offset;
        private uint MP_offset;
        private uint power_offset;
        private uint wakan_offset;
        private uint defence_offset;
        private uint speed_offset;
        private uint luck_offset;

        //variable
        public uint exp; //经验
        public uint rank;
        public ushort now_rank; //现在的等级
        public ushort maxHP;
        public ushort maxMP;
        public ushort hp;
        public ushort mp;
        public ushort power; //武力
        public ushort wakan; //灵力
        public ushort defence; //防御
        public ushort speed; //身法
        public ushort luck; //吉运


        public PalExp(uint[] offset)
        {
            exp_offset = offset[0];
            now_rank_offset = offset[1];
            MaxHP_offset = offset[2];
            MaxMP_offset = offset[3];
            HP_offset = offset[4];
            MP_offset = offset[5];
            power_offset = offset[6];
            wakan_offset = offset[7];
            defence_offset = offset[8];
            speed_offset = offset[9];
            luck_offset = offset[10];
        }

        public void LoadPalExp(System.IO.FileStream fStream)
        {
            try
            {
                System.IO.BinaryReader br = new BinaryReader(fStream);
                fStream.Seek(exp_offset, SeekOrigin.Begin);
                exp = br.ReadUInt32();
                rank = br.ReadUInt32();

                now_rank = Loadushort(br, fStream, now_rank_offset);
                maxHP = Loadushort(br, fStream, MaxHP_offset);
                maxMP = Loadushort(br, fStream, MaxMP_offset);
                hp = Loadushort(br, fStream, HP_offset);
                mp = Loadushort(br, fStream, MP_offset);
                power = Loadushort(br, fStream, power_offset);
                wakan = Loadushort(br, fStream, wakan_offset);
                defence = Loadushort(br, fStream, defence_offset);
                speed = Loadushort(br, fStream, speed_offset);
                luck = Loadushort(br, fStream, luck_offset);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in PalExp.LoadPalExp(): " + e.Message);
                System.Windows.Forms.MessageBox.Show("Exception in PalExp.LoadPalExp(): " + e.Message);
            }
        }

        public void SavePalExp(System.IO.FileStream fStream)
        {
            try
            {
                byte[] expTmp = System.BitConverter.GetBytes(exp);
                byte[] rankTmp = System.BitConverter.GetBytes(rank);
                fStream.Seek(exp_offset, SeekOrigin.Begin);
                fStream.Write(expTmp, 0, expTmp.Length); //exp
                fStream.Write(rankTmp, 0, rankTmp.Length); // exp rest

                Saveushort(fStream, now_rank_offset, this.now_rank);
                Saveushort(fStream, MaxHP_offset, this.maxHP);
                Saveushort(fStream, MaxMP_offset, this.maxMP);
                Saveushort(fStream, HP_offset, this.hp);
                Saveushort(fStream, MP_offset, this.mp);
                Saveushort(fStream, power_offset, this.power);
                Saveushort(fStream, wakan_offset, this.wakan);
                Saveushort(fStream, defence_offset, this.defence);
                Saveushort(fStream, speed_offset, this.speed);
                Saveushort(fStream, luck_offset, this.luck);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in PalExp.SavePalExp(): " + e.Message);
                System.Windows.Forms.MessageBox.Show("Exception in PalExp.SavePalExp(): " + e.Message);
            }
        }

        public ushort Loadushort(System.IO.BinaryReader br, System.IO.FileStream fStream, uint offset)
        {
            fStream.Seek(offset, SeekOrigin.Begin);
            return br.ReadUInt16();
        }

        public void Saveushort(System.IO.FileStream fStream, uint offset, ushort number)
        {
            byte[] tmp = System.BitConverter.GetBytes(number);
            fStream.Seek(offset, SeekOrigin.Begin);
            fStream.Write(tmp, 0, tmp.Length);
        }
    }
}