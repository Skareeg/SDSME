using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Tao.OpenGl;
using Tao.Platform;

namespace WindowsFormsApplication1
{
    public partial class Form7 : Form
    {
        public int matrixWidth;
        public int matrixHeight;

        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            System.IO.BinaryReader readMatrix = new System.IO.BinaryReader(File.OpenRead(Form1.matrixEditorPath));
            matrixWidth = readMatrix.ReadByte();
            matrixHeight = readMatrix.ReadByte();
            progressBar1.Maximum = matrixHeight * matrixHeight;
        }

    }
}
