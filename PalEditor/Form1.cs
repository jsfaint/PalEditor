using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PalEditor
{
    public partial class Form1 : Form
    {
        bool bFileLoaded = false;
        System.PlatformID platID;
        RPGData rpgData = new RPGData();

        enum PAL
        {
            exp = 0x00,
            rank,
            hp,
            maxHp,
            mp,
            maxMp,
            power,
            wakan,
            defence,
            speed,
            luck,
            money = 0x100,
            saveTime,
            calabash,
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            FormAbout frmAbout = new FormAbout();
            frmAbout.ShowDialog();
        }

        private void menuItemSave_Click(object sender, EventArgs e)
        {
            if (this.text2Varibles() == false)
                return;

            MessageBoxButtons MBB = MessageBoxButtons.YesNo;
            MessageBoxIcon MBI = MessageBoxIcon.Question;
            MessageBoxDefaultButton MBDI = MessageBoxDefaultButton.Button1;
            switch (MessageBox.Show("是否确定保存修改的内容?", "提示", MBB, MBI, MBDI))
            {
                case DialogResult.Yes:
                    if (checkBoxGoods.Checked)
                        rpgData.palGoods.allGoods99();
                    else
                        dataGridToGoodsInfo();

                    rpgData.SaveToFile();
                    tabControl1.Visible = false;
                    menuItemSave.Enabled = false;
                    MessageBox.Show("恭喜您, 存档修改成功!", "信息");
                    break;

                case DialogResult.No:
                    break;
            }
        }

        private void initialUI()
        {
            textBoxMoney.Text = rpgData.palMoney.ToString();
            textBoxCalabash.Text = rpgData.palCalabash.ToString();
            textBoxSaveTime.Text = rpgData.palSaveTime.ToString();
            setComboContent(this.comboBox1.SelectedIndex);
            initDataGrid();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            string path;
            platID = System.Environment.OSVersion.Platform;

            if (platID == PlatformID.WinCE)
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName()
                    .CodeBase);
            else
                path = System.IO.Directory.GetCurrentDirectory();

            if (System.IO.File.Exists(path + "\\0.rpg") == false)
            {
                MessageBoxButtons MBB = MessageBoxButtons.OK;
                MessageBoxIcon MBI = MessageBoxIcon.Hand;
                MessageBoxDefaultButton MBDB = MessageBoxDefaultButton.Button1;
                MessageBox.Show("请将本程序放入仙剑奇侠传Dos版的安装目录中运行, 谢谢! :)", "提示", MBB, MBI, MBDB);

                System.Windows.Forms.Application.Exit();
                return;
            }

            checkBoxSaveTime.Checked = false;
            checkBoxGoods.Checked = false;

            this.MaximizeBox = false;
            tabControl1.Visible = false;
            menuItemSave.Enabled = false;
            textBoxMoney.MaxLength = 7;
            textBoxCalabash.MaxLength = 2;
            textBoxSaveTime.MaxLength = 4;

            comboBox1.SelectedIndex = 0;

            textBoxHp.MaxLength = 3; //max hp, mp 999
            textBoxMaxHp.MaxLength = 3;
            textBoxMp.MaxLength = 3;
            textBoxMaxMp.MaxLength = 3;
            textBoxExp.MaxLength = 5; //max exp 32000
            textBoxNowRank.MaxLength = 2; //max now_rank 99
            textBoxPower.MaxLength = 4; // max 5000
            textBoxWakan.MaxLength = 4;
            textBoxDefence.MaxLength = 4;
            textBoxSpeed.MaxLength = 4;
            textBoxLuck.MaxLength = 4;
            buttonGoods.Enabled = false;
        }

        private void setComboContent(int ii)
        {
            textBoxNowRank.Text = rpgData.palExp[ii].now_rank.ToString();
            textBoxExp.Text = rpgData.palExp[ii].exp.ToString();
            //textBoxRank.Text = rpgData.palExp[ii].rank.ToString();
            textBoxHp.Text = rpgData.palExp[ii].hp.ToString();
            textBoxMaxHp.Text = rpgData.palExp[ii].maxHP.ToString();
            textBoxMp.Text = rpgData.palExp[ii].mp.ToString();
            textBoxMaxMp.Text = rpgData.palExp[ii].maxMP.ToString();

            textBoxPower.Text = rpgData.palExp[ii].power.ToString();
            textBoxWakan.Text = rpgData.palExp[ii].wakan.ToString();
            textBoxDefence.Text = rpgData.palExp[ii].defence.ToString();
            textBoxSpeed.Text = rpgData.palExp[ii].speed.ToString();
            textBoxLuck.Text = rpgData.palExp[ii].luck.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            setComboContent(comboBox1.SelectedIndex);
        }

        private bool checkComboContent()
        {
            if (textBoxExp.Text == "")
            {
                message(PAL.exp);
                return false;
            }

            if (textBoxNowRank.Text == "")
            {
                message(PAL.rank);
                return false;
            }

            if (textBoxHp.Text == "")
            {
                message(PAL.hp);
                return false;
            }

            if (textBoxMaxHp.Text == "")
            {
                message(PAL.maxHp);
                return false;
            }

            if (textBoxMp.Text == "")
            {
                message(PAL.mp);
                return false;
            }

            if (textBoxMaxMp.Text == "")
            {
                message(PAL.maxMp);
                return false;
            }

            if (textBoxPower.Text == "")
            {
                message(PAL.power);
                return false;
            }

            if (textBoxWakan.Text == "")
            {
                message(PAL.wakan);
                return false;
            }

            if (textBoxDefence.Text == "")
            {
                message(PAL.power);
                return false;
            }

            if (textBoxSpeed.Text == "")
            {
                message(PAL.speed);
                return false;
            }

            if (textBoxLuck.Text == "")
            {
                message(PAL.luck);
                return false;
            }

            return true;
        }

        private void getComboContent(int ii)
        {
            rpgData.palExp[ii].now_rank = System.UInt16.Parse(textBoxNowRank.Text);
            rpgData.palExp[ii].exp = System.UInt32.Parse(textBoxExp.Text);
            //rpgData.palExp[ii].rank = System.UInt32.Parse(textBoxRank.Text);
            rpgData.palExp[ii].hp = System.UInt16.Parse(textBoxHp.Text);
            rpgData.palExp[ii].maxHP = System.UInt16.Parse(textBoxMaxHp.Text);
            rpgData.palExp[ii].mp = System.UInt16.Parse(textBoxMp.Text);
            rpgData.palExp[ii].maxMP = System.UInt16.Parse(textBoxMaxMp.Text);

            rpgData.palExp[ii].power = System.UInt16.Parse(textBoxPower.Text);
            rpgData.palExp[ii].wakan = System.UInt16.Parse(textBoxWakan.Text);
            rpgData.palExp[ii].defence = System.UInt16.Parse(textBoxDefence.Text);
            rpgData.palExp[ii].speed = System.UInt16.Parse(textBoxSpeed.Text);
            rpgData.palExp[ii].luck = System.UInt16.Parse(textBoxLuck.Text);
        }

        private bool text2Varibles()
        {
            if (textBoxMoney.Text == "")
            {
                message(PAL.money);
                return false;
            }

            if (textBoxCalabash.Text == "")
            {
                message(PAL.calabash);
                return false;
            }

            if (textBoxSaveTime.Text == "")
            {
                message(PAL.saveTime);
                return false;
            }

            rpgData.palMoney = System.UInt32.Parse(textBoxMoney.Text);
            rpgData.palCalabash = System.UInt16.Parse(textBoxCalabash.Text);
            if (checkBoxSaveTime.Checked)
                rpgData.palSaveTime = 0;
            else
                rpgData.palSaveTime = System.UInt16.Parse(textBoxSaveTime.Text);

            if (checkComboContent() == false)
                return false;
            getComboContent(comboBox1.SelectedIndex);

            return true;
        }

        private void comboBox1_GotFocus(object sender, EventArgs e)
        {
            getComboContent(comboBox1.SelectedIndex);
        }

        //只能输入数字, 通过textBox的keyPress()事件实现
        private void digit_Handle(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        #region textChange

        private void textBoxMoney_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            uint money = System.UInt32.Parse(textBoxMoney.Text);
            if (money < 0 || money > 9999999)
            {
                message(PAL.money);
                textBoxMoney.Text = rpgData.palMoney.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxCalabash_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            ushort calabash = System.UInt16.Parse(textBoxCalabash.Text);
            if (calabash < 0 || calabash > 99)
            {
                message(PAL.calabash);
                textBoxCalabash.Text = rpgData.palCalabash.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxSaveTime_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            ushort saveTime;
            saveTime = System.UInt16.Parse(textBoxSaveTime.Text);
            if (saveTime < 0 || saveTime > 9999)
            {
                message(PAL.saveTime);
                textBoxSaveTime.Text = rpgData.palSaveTime.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxExp_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int exp = System.Int32.Parse(textBoxExp.Text);
            if (exp < 0 || exp > 32000)
            {
                message(PAL.exp);
                textBoxExp.Text = rpgData.palExp[comboBox1.SelectedIndex].exp.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxNowRank_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int now_rank = System.Int32.Parse(textBoxNowRank.Text);
            if (now_rank < 1 || now_rank > 99)
            {
                message(PAL.rank);
                textBoxNowRank.Text = rpgData.palExp[comboBox1.SelectedIndex].now_rank.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxHp_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int hp = System.Int32.Parse(textBoxHp.Text);
            if (hp < 0 || hp > 999)
            {
                message(PAL.hp);
                textBoxHp.Text = rpgData.palExp[comboBox1.SelectedIndex].hp.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxMaxHp_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int maxHp = System.Int32.Parse(textBoxMaxHp.Text);
            if (maxHp < 0 || maxHp > 999)
            {
                message(PAL.maxHp);
                textBoxMaxHp.Text = rpgData.palExp[comboBox1.SelectedIndex].maxHP.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxMp_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int mp = System.Int32.Parse(textBoxMp.Text);
            if (mp < 0 || mp > 999)
            {
                message(PAL.mp);
                textBoxMp.Text = rpgData.palExp[comboBox1.SelectedIndex].mp.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxMaxMp_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int maxMp = System.Int32.Parse(textBoxMaxMp.Text);
            if (maxMp < 0 || maxMp > 999)
            {
                message(PAL.maxMp);
                textBoxMaxMp.Text = rpgData.palExp[comboBox1.SelectedIndex].maxMP.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxPower_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int power = System.Int32.Parse(textBoxPower.Text);
            if (power < 1 || power > 5000)
            {
                message(PAL.power);
                textBoxPower.Text = rpgData.palExp[comboBox1.SelectedIndex].power.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxWakan_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int wakan = System.Int32.Parse(textBoxWakan.Text);
            if (wakan < 1 || wakan > 5000)
            {
                message(PAL.wakan);
                textBoxWakan.Text = rpgData.palExp[comboBox1.SelectedIndex].wakan.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxDefence_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int defence = System.Int32.Parse(textBoxDefence.Text);
            if (defence < 1 || defence > 5000)
            {
                message(PAL.defence);
                textBoxDefence.Text = rpgData.palExp[comboBox1.SelectedIndex].defence.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxSpeed_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int speed = System.Int32.Parse(textBoxSpeed.Text);
            if (speed < 1 || speed > 5000)
            {
                message(PAL.speed);
                textBoxSpeed.Text = rpgData.palExp[comboBox1.SelectedIndex].speed.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        private void textBoxLuck_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false || ((TextBox)sender).Text == "")
                return;

            int luck = System.Int32.Parse(textBoxLuck.Text);
            if (luck < 1 || luck > 5000)
            {
                message(PAL.luck);
                textBoxLuck.Text = rpgData.palExp[comboBox1.SelectedIndex].luck.ToString();
            }

            ((TextBox)sender).BackColor = Color.White;
        }

        #endregion

        //打开对应的存档, 并读取数据
        private void OpenSaveFile(uint index)
        {
            string path;
            if (platID == PlatformID.WinCE)
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName()
                    .CodeBase);
            else
                path = System.IO.Directory.GetCurrentDirectory();

            string filename = path + "\\" + index + ".rpg";

            rpgData.OpenPalSave(filename);

            initialUI();

            tabControl1.Visible = true;
            menuItemSave.Enabled = true;
            bFileLoaded = true;
            labelGoods.Text = "";
            textBoxGoods.Text = "";
            buttonGoods.Enabled = false;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            OpenSaveFile(1);
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            OpenSaveFile(2);
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            OpenSaveFile(3);
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            OpenSaveFile(4);
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            OpenSaveFile(5);
        }

        private void initDataGrid()
        {
            DataTable dt = new DataTable();
            DataColumn dcName = new DataColumn();
            DataColumn dcCnt = new DataColumn();
            dcName.ColumnName = "名称";
            dcCnt.ColumnName = "数量";
            dt.Columns.Add(dcName);
            dt.Columns.Add(dcCnt);

            DataGridTableStyle ts = new DataGridTableStyle();
            DataGridColumnStyle dgName = new DataGridTextBoxColumn();
            dgName.MappingName = dcName.ColumnName;
            dgName.HeaderText = dcName.ColumnName;
            dgName.Width = 100;
            ts.GridColumnStyles.Add(dgName);

            DataGridColumnStyle dgCnt = new DataGridTextBoxColumn();
            dgCnt.MappingName = dcCnt.ColumnName;
            dgCnt.HeaderText = dcCnt.ColumnName;
            dgCnt.Width = 40;
            ts.GridColumnStyles.Add(dgCnt);

            dataGridGoods.TableStyles.Add(ts);


            this.dataGridGoods.CurrentCellChanged +=
                new System.EventHandler(dataGrid_CurrentCellChanged); // // DataGridEditing //


            const ushort GOODS_CNT = 234;
            for (int ii = 0; ii < GOODS_CNT; ii++)
            {
                DataRow dr = dt.NewRow();
                dr[dcName] = rpgData.palGoods.goodsInfo[ii].goodsDescript;
                dr[dcCnt] = rpgData.palGoods.goodsInfo[ii].goodsCnt;
                dt.Rows.Add(dr);
            }

            dataGridGoods.DataSource = dt;
        }

        private void checkBoxGoods_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBoxGoods.Checked)
                dataGridGoods.Enabled = false;
            else
                dataGridGoods.Enabled = true;
        }

        private void dataGrid_CurrentCellChanged(object o, EventArgs e)
        {
            DataGridCell currentCell;

            currentCell = dataGridGoods.CurrentCell;

            labelGoods.Text = (string)dataGridGoods[currentCell.RowNumber, 0];
            textBoxGoods.Text = (string)dataGridGoods[currentCell.RowNumber, 1];
            buttonGoods.Enabled = true;
        }

        private void textBoxGoods_TextChanged(object sender, EventArgs e)
        {
            if (bFileLoaded == false)
                return;

            if (textBoxGoods.Text == "")
                return;

            int goodsCnt = System.Int16.Parse(textBoxGoods.Text);
            if (goodsCnt < 0 || goodsCnt > 5000)
            {
                MessageBox.Show("物品的有效值范围为0-99\n请输入正确的值!", "提示");
                textBoxGoods.Text = (string)dataGridGoods[dataGridGoods.CurrentCell.RowNumber, 1];
            }
        }

        private void buttonGoods_Click(object sender, EventArgs e)
        {
            DataGridCell currentCell = dataGridGoods.CurrentCell;
            if (textBoxGoods.Text == "")
                textBoxGoods.Text = "0";
            dataGridGoods[currentCell.RowNumber, 1] = textBoxGoods.Text;
        }

        private void dataGridToGoodsInfo()
        {
            const ushort GOODS_CNT = 234;
            for (int ii = 0; ii < GOODS_CNT; ii++)
            {
                rpgData.palGoods.goodsInfo[ii].goodsCnt = System.UInt16.Parse((string)dataGridGoods[ii, 1]);
            }
        }

        private void buttonMagic_Click(object sender, EventArgs e)
        {
            FormMagic formMagic = new FormMagic(rpgData.palMagic, comboBox1.SelectedIndex);
            formMagic.Owner = this;
            formMagic.ShowDialog();
        }

        private void message(PAL index)
        {
            switch (index)
            {
                case PAL.exp:
                    MessageBox.Show("经验值的有效范围为0-32000\n请输入正确的值!", "提示");
                    break;
                case PAL.rank:
                    MessageBox.Show("修行的有效值范围为0-99\n请输入正确的值!", "提示");
                    break;
                case PAL.hp:
                    MessageBox.Show("体力的有效值范围为0-999\n请输入正确的值!", "提示");
                    break;
                case PAL.maxHp:
                    MessageBox.Show("体力上限的有效值范围为0-999\n请输入正确的值!", "提示");
                    break;
                case PAL.mp:
                    MessageBox.Show("真气的有效值范围为0-999\n请输入正确的值!", "提示");
                    break;
                case PAL.maxMp:
                    MessageBox.Show("真气上限的有效值范围为0-999\n请输入正确的值!", "提示");
                    break;
                case PAL.power:
                    MessageBox.Show("武力的有效值范围为1-5000\n请输入正确的值!", "提示");
                    break;
                case PAL.wakan:
                    MessageBox.Show("灵力的有效值范围为1-5000\n请输入正确的值!", "提示");
                    break;
                case PAL.defence:
                    MessageBox.Show("防御的有效值范围为1-5000\n请输入正确的值!", "提示");
                    break;
                case PAL.speed:
                    MessageBox.Show("身法的有效值范围为1-5000\n请输入正确的值!", "提示");
                    break;
                case PAL.luck:
                    MessageBox.Show("吉运的有效值范围为1-5000\n请输入正确的值!", "提示");
                    break;
                case PAL.money:
                    MessageBox.Show("金钱的有效值范围为0-9999999", "提示");
                    break;
                case PAL.saveTime:
                    MessageBox.Show("存盘次数的有效值范围为0-9999", "提示");
                    break;
                case PAL.calabash:
                    MessageBox.Show("灵葫值的有效值范围为0-99", "提示");
                    break;
            }
        }

        //private PAL textBoxToEnum(TextBox obj)
        //{
        //    PAL tmp = new PAL();
        //    if (obj.Equals(textBoxExp))
        //        tmp = PAL.exp;
        //    else if (obj.Equals(textBoxNowRank))
        //        tmp = PAL.rank;
        //    else if (obj.Equals(textBoxHp))
        //        tmp = PAL.hp;
        //    else if (obj.Equals(textBoxMaxHp))
        //        tmp = PAL.maxHp;
        //    else if (obj.Equals(textBoxMp))
        //        tmp = PAL.mp;
        //    else if (obj.Equals(textBoxMaxMp))
        //        tmp = PAL.maxMp;
        //    else if (obj.Equals(textBoxPower))
        //        tmp = PAL.power;
        //    else if (obj.Equals(textBoxWakan))
        //        tmp = PAL.wakan;
        //    else if (obj.Equals(textBoxDefence))
        //        tmp = PAL.defence;
        //    else if (obj.Equals(textBoxSpeed))
        //        tmp = PAL.speed;
        //    else if (obj.Equals(textBoxLuck))
        //        tmp = PAL.luck;
        //    else if (obj.Equals(textBoxMoney))
        //        tmp = PAL.money;
        //    else if (obj.Equals(textBoxSaveTime))
        //        tmp = PAL.saveTime;
        //    else if (obj.Equals(textBoxCalabash))
        //        tmp = PAL.calabash;

        //    return tmp;
        //}

        private void textBox_LostFocus(object sender, EventArgs e)
        {
            TextBox tmp = (TextBox)sender;

            if (tmp.Text == "")
            {
                //this.message(textBoxToEnum(tmp));
                tmp.Focus();
                tmp.BackColor = Color.Red;
            }
        }
    }
}