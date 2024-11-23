using System;
using System.Drawing;
using System.Windows.Forms;

namespace PalEditor
{
    public partial class FormMagic : Form
    {
        private const ushort Magic_CNT = 103;
        private const ushort Magic_PP_MAX = 32;
        private CheckBox[] checkBoxMagic;
        private PalMagic palMagic;
        private int pIndex;
        private ushort mCount = 0;

        public FormMagic()
        {
            InitializeComponent();
        }

        public FormMagic(PalMagic palMagic, int pIndex)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.palMagic = palMagic;
            this.pIndex = pIndex;

            palMagic.ArrayToDisplay(pIndex);

            checkBoxMagic = new CheckBox[Magic_CNT];
            this.AutoScaleMode = AutoScaleMode.None;


            if (this.Width >= 480)
            {
                int top = 30;
                int space = 20;
                for (int ii = 0; ii < Magic_CNT; ii++)
                {
                    checkBoxMagic[ii] = new CheckBox();
                    checkBoxMagic[ii].Font = new Font("Tahoma", 8, FontStyle.Regular);
                    checkBoxMagic[ii].Text = palMagic.magicList[ii].MagicDesc;
                    checkBoxMagic[ii].Checked = palMagic.magicList[ii].Enabled;
                    checkBoxMagic[ii].Width = 150;
                    checkBoxMagic[ii].Height = 32;

                    switch (ii % 3)
                    {
                        case 0:
                            checkBoxMagic[ii].Left = 5;
                            checkBoxMagic[ii].Top = top + (ii / 3) * checkBoxMagic[ii].Height + space;
                            break;
                        case 1:
                            checkBoxMagic[ii].Left = 155;
                            checkBoxMagic[ii].Top = top + ((ii - 1) / 3) * checkBoxMagic[ii].Height + space;
                            break;

                        case 2:
                            checkBoxMagic[ii].Left = 305;
                            checkBoxMagic[ii].Top = top + ((ii - 2) / 3) * checkBoxMagic[ii].Height + space;
                            break;
                    }

                    this.Controls.Add(checkBoxMagic[ii]);
                    checkBoxMagic[ii].CheckStateChanged += new System.EventHandler(this.checkBox_CheckStateChanged);

                    if (palMagic.magicList[ii].Enabled)
                        this.mCount++;
                }
            }
            else if (this.Width < 480)
            {
                int top = 20;

                for (int ii = 0; ii < Magic_CNT; ii++)
                {
                    checkBoxMagic[ii] = new CheckBox();
                    checkBoxMagic[ii].Font = new Font("Tahoma", 8, FontStyle.Regular);
                    checkBoxMagic[ii].Text = palMagic.magicList[ii].MagicDesc;
                    checkBoxMagic[ii].Checked = palMagic.magicList[ii].Enabled;

                    if (ii % 2 == 0)
                    {
                        checkBoxMagic[ii].Left = 5;
                        checkBoxMagic[ii].Top = top + (ii + 1) / 2 * checkBoxMagic[ii].Height;
                    }
                    else
                    {
                        checkBoxMagic[ii].Left = 110;
                        checkBoxMagic[ii].Top = top + ii / 2 * checkBoxMagic[ii].Height;
                    }

                    this.Controls.Add(checkBoxMagic[ii]);
                    checkBoxMagic[ii].CheckStateChanged += new System.EventHandler(this.checkBox_CheckStateChanged);

                    if (palMagic.magicList[ii].Enabled)
                        this.mCount++;
                }
            }

            labelMagic.Text = "每人最多拥有32项仙术, 已有 " + mCount + " 项";
        }

        private void FormMagic_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.ControlBox = false;
        }

        private void menuItemCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItemOK_Click(object sender, EventArgs e)
        {
            //保存checkbox选项回magic
            for (int ii = 0; ii < Magic_CNT; ii++)
                palMagic.magicList[ii].Enabled = checkBoxMagic[ii].Checked;

            palMagic.DisplayToArray(pIndex);

            this.Close();
        }

        private void checkBox_CheckStateChanged(object sender, EventArgs e)
        {
            this.mCount = 0;
            CheckBox cb = (CheckBox)sender;

            for (int ii = 0; ii < Magic_CNT; ii++)
            {
                if (checkBoxMagic[ii].Checked)
                {
                    mCount++;
                }

                if (mCount > Magic_PP_MAX)
                {
                    mCount = Magic_PP_MAX;
                    System.Windows.Forms.MessageBox.Show("每人最多拥有32项仙术!", "错误");
                    cb.Checked = false;
                }
            }

            labelMagic.Text = "每人最多拥有32项仙术, 已有 " + mCount + " 项";
        }
    }
}