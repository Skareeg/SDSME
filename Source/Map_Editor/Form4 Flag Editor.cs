using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            byte value = Convert.ToByte(Form1.mapFlags);
            if ((value & (1 << 7)) != 0) checkBox1.Checked = true;
            if ((value & (1 << 6)) != 0) checkBox2.Checked = true;
            if ((value & (1 << 5)) != 0) checkBox3.Checked = true;
            if ((value & (1 << 4)) != 0) checkBox4.Checked = true;
            if ((value & (1 << 3)) != 0) checkBox5.Checked = true;
            if ((value & (1 << 2)) != 0) checkBox6.Checked = true;
            if ((value & (1 << 1)) != 0) checkBox7.Checked = true;
            if ((value & (1 << 0)) != 0) checkBox8.Checked = true;
            if (Form1.isBW || Form1.isB2W2)
            {
                string name = checkBox4.Text;
                string name2 = checkBox1.Text;
                string name3 = checkBox7.Text;
                checkBox3.Text = name;
                checkBox5.Text = name3;
                checkBox6.Text = name2;
                checkBox1.Text = "Flag 1";
                checkBox4.Text = "Flag 4";
                checkBox7.Text = "Flag 7";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.mapFlags = 0x0;
            if (checkBox1.Checked == true) Form1.mapFlags = Form1.mapFlags | (1 << 7);
            if (checkBox2.Checked == true) Form1.mapFlags = Form1.mapFlags | (1 << 6);
            if (checkBox3.Checked == true) Form1.mapFlags = Form1.mapFlags | (1 << 5);
            if (checkBox4.Checked == true) Form1.mapFlags = Form1.mapFlags | (1 << 4);
            if (checkBox5.Checked == true) Form1.mapFlags = Form1.mapFlags | (1 << 3);
            if (checkBox6.Checked == true) Form1.mapFlags = Form1.mapFlags | (1 << 2);
            if (checkBox7.Checked == true) Form1.mapFlags = Form1.mapFlags | (1 << 1);
            if (checkBox8.Checked == true) Form1.mapFlags = Form1.mapFlags | (1 << 0);
            this.Close();
        }
    }
}
