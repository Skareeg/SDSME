using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using NarcAPI;

namespace WindowsFormsApplication1
{
    public partial class Form3_2_Trainer_Class_Editor : Form
    {
        ResourceManager getChar = new ResourceManager("WindowsFormsApplication1.Resources.ReadText", Assembly.GetExecutingAssembly());
        ResourceManager getByte = new ResourceManager("WindowsFormsApplication1.Resources.WriteText", Assembly.GetExecutingAssembly());
        public string path;
        public string spritePath;
        public int stringClassCount;
        public int initialKey;
        List<string> classNames = new List<string>();
        private Bitmap m_bitmap;

        public Form3_2_Trainer_Class_Editor()
        {
            InitializeComponent();
        }

        private void Form3_2_Trainer_Class_Editor_Load(object sender, EventArgs e)
        {
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                path = Form1.workingFolder + @"data\msgdata\msg\0560";
                Narc.Open(Form1.workingFolder + @"data\poketool\trgra\trfgra.narc").ExtractToFolder(Form1.workingFolder + @"data\poketool\trgra\trfgra");
                spritePath = Form1.workingFolder + @"data\poketool\trgra\trfgra";
            }
            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                path = Form1.workingFolder + @"data\msgdata\pl_msg\0619";
                Narc.Open(Form1.workingFolder + @"data\poketool\trgra\trfgra.narc").ExtractToFolder(Form1.workingFolder + @"data\poketool\trgra\trfgra");
                spritePath = Form1.workingFolder + @"data\poketool\trgra\trfgra";
            }
            if (Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049)
            {
                path = Form1.workingFolder + @"data\a\0\2\text\0730";
                Narc.Open(Form1.workingFolder + @"data\a\0\5\8").ExtractToFolder(Form1.workingFolder + @"data\a\0\5\trfgra");
                spritePath = Form1.workingFolder + @"data\a\0\5\trfgra";
            }
            if (Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
            {
                path = Form1.workingFolder + @"data\a\0\2\text\0720";
                Narc.Open(Form1.workingFolder + @"data\a\0\5\8").ExtractToFolder(Form1.workingFolder + @"data\a\0\5\trfgra");
                spritePath = Form1.workingFolder + @"data\a\0\5\trfgra";
            }
            #region Read Trainer Class Names
            BinaryReader readText = new BinaryReader(File.OpenRead(path));
            readText.BaseStream.Position = 0x0;
            stringClassCount = (int)readText.ReadUInt16();
            initialKey = (int)readText.ReadUInt16();
            int key1 = (initialKey * 0x2FD) & 0xFFFF;
            int key2 = 0;
            int realKey = 0;
            bool specialCharON = false;
            int[] currentOffset = new int[stringClassCount];
            int[] currentSize = new int[stringClassCount];
            int car = 0;
            for (int i = 0; i < stringClassCount; i++) // Reads and stores string offsets and sizes
            {
                key2 = (key1 * (i + 1) & 0xFFFF);
                realKey = key2 | (key2 << 16);
                currentOffset[i] = ((int)readText.ReadUInt32()) ^ realKey;
                currentSize[i] = ((int)readText.ReadUInt32()) ^ realKey;
            }
            for (int i = 0; i < stringClassCount; i++) // Adds new string
            {
                key1 = (0x91BD3 * (i + 1)) & 0xFFFF;
                readText.BaseStream.Position = currentOffset[i];
                string pokemonText = "";
                for (int j = 0; j < currentSize[i]; j++) // Adds new characters to string
                {
                    car = ((int)readText.ReadUInt16()) ^ key1;
                    #region Special Characters
                    if (car == 0xE000 || car == 0x25BC || car == 0x25BD || car == 0xFFFE || car == 0xFFFF)
                    {
                        if (car == 0xE000)
                        {
                            pokemonText += @"\n";
                        }
                        if (car == 0x25BC)
                        {
                            pokemonText += @"\r";
                        }
                        if (car == 0x25BD)
                        {
                            pokemonText += @"\f";
                        }
                        if (car == 0xFFFE)
                        {
                            pokemonText += @"\v";
                            specialCharON = true;
                        }
                        if (car == 0xFFFF)
                        {
                            pokemonText += "";
                        }
                    }
                    #endregion
                    else
                    {
                        if (specialCharON == true)
                        {
                            pokemonText += car.ToString("X4");
                            specialCharON = false;
                        }
                        else
                        {
                            string character = getChar.GetString(car.ToString("X4"));
                            pokemonText += character;
                            if (character == null)
                            {
                                pokemonText += @"\x" + car.ToString("X4");
                            }
                        }
                    }
                    key1 += 0x493D;
                    key1 &= 0xFFFF;
                }
                classNames.Add(pokemonText);
            }
            readText.Close();
            for (int i = 0; i < stringClassCount; i++)
            {
                comboBox1.Items.Add(i + ": " + classNames[i]);
            }
            #endregion
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = classNames[comboBox1.SelectedIndex];
            Bitmap sprite = new Bitmap(160, 80, PixelFormat.Format4bppIndexed);
            BinaryReader readPalette;
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                readPalette = new BinaryReader(File.OpenRead(spritePath + "\\" + (comboBox1.SelectedIndex * 2 + 1).ToString("D4")));
            }
            else
            {
                readPalette = new BinaryReader(File.OpenRead(spritePath + "\\" + (comboBox1.SelectedIndex * 5 + 1).ToString("D4")));
            }
            readPalette.BaseStream.Position = 0x28;
            int[] paletteInt = new int[16];
            for (int i = 0; i < 16; i++)
            {
                paletteInt[i] = readPalette.ReadUInt16();
            }
            ColorPalette palette = sprite.Palette;
            for (int i = 0; i < 0x10; i++)
            {
                palette.Entries[i] = Color.FromArgb((paletteInt[i] & 0x1f) << 3, ((paletteInt[i] >> 5) & 0x1f) << 3, ((paletteInt[i] >> 10) & 0x1f) << 3);
            }
            readPalette.Close();
            sprite.Palette = palette;

            BinaryReader readSprite;
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                readSprite = new BinaryReader(File.OpenRead(spritePath + "\\" + (comboBox1.SelectedIndex * 2).ToString("D4")));
            }
            else
            {
                readSprite = new BinaryReader(File.OpenRead(spritePath + "\\" + (comboBox1.SelectedIndex * 5 + 4).ToString("D4")));
            }
            readSprite.BaseStream.Position = 0x30;
            ushort[] numArray = new ushort[0xC80];
            for (int i = 0; i < 0xC80; i++)
            {
                numArray[i] = readSprite.ReadUInt16();
            }
            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                uint num = numArray[0x0];
                for (int j = 0; j < 0xC80; j++)
                {
                    ushort[] numArray2;
                    IntPtr ptr2;
                    (numArray2 = numArray)[(int)(ptr2 = (IntPtr)j)] = (ushort)(numArray2[(int)ptr2] ^ ((ushort)(num & 0xffff)));
                    num *= 0x41c64e6d;
                    num += 0x6073;
                }
            }
            else
            {
                uint num = numArray[0xC7F];
                for (int j = 0xC7F; j >= 0; j--)
                {
                    ushort[] numArray2;
                    IntPtr ptr2;
                    (numArray2 = numArray)[(int)(ptr2 = (IntPtr)j)] = (ushort)(numArray2[(int)ptr2] ^ ((ushort)(num & 0xffff)));
                    num *= 0x41c64e6d;
                    num += 0x6073;
                }
            }
            m_bitmap = new Bitmap(160, 80, PixelFormat.Format8bppIndexed);
            Rectangle rect = new Rectangle(0, 0, 160, 80);
            byte[] source = new byte[0x3200];
            for (int k = 0; k < 0xC80; k++)
            {
                source[k * 4] = (byte)(numArray[k] & 15);
                source[(k * 4) + 1] = (byte)((numArray[k] >> 4) & 15);
                source[(k * 4) + 2] = (byte)((numArray[k] >> 8) & 15);
                source[(k * 4) + 3] = (byte)((numArray[k] >> 12) & 15);
            }
            BitmapData bitmapdata = m_bitmap.LockBits(rect, ImageLockMode.WriteOnly, m_bitmap.PixelFormat);
            IntPtr destination = bitmapdata.Scan0;
            Marshal.Copy(source, 0, destination, 0x3200);
            m_bitmap.UnlockBits(bitmapdata);
            m_bitmap.Palette = palette;
            pictureBox1.Image = m_bitmap;
            readSprite.Close();
        }

        private void button4_Click(object sender, EventArgs e) // Export PNG
        {
            SaveFileDialog savePNG = new SaveFileDialog();
            savePNG.Filter = "PNG (*.png)|*.png";
            if (savePNG.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(savePNG.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Import PNG
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PNG (*.png)|*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                m_bitmap = new Bitmap(dialog.FileName);
                if ((m_bitmap.Height != 80) || (m_bitmap.Width != 160))
                {
                    m_bitmap = new Bitmap(m_bitmap, 160, 80);
                }
                pictureBox1.Image = m_bitmap;
            }
        }

        private void button5_Click(object sender, EventArgs e) // Save Current
        {
            byte[] buffer = new byte[] { 
                0x52, 0x4c, 0x43, 0x4e, 0xff, 0xfe, 0, 1, 0x48, 0, 0, 0, 0x10, 0, 1, 0, 
                0x54, 0x54, 0x4c, 80, 0x38, 0, 0, 0, 4, 0, 10, 0, 0, 0, 0, 0, 
                0x20, 0, 0, 0, 0x10, 0, 0, 0};
            BinaryWriter writePalette;
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                writePalette = new BinaryWriter(File.Create(spritePath + "\\" + (comboBox1.SelectedIndex * 2 + 1).ToString("D4")));
            }
            else
            {
                writePalette = new BinaryWriter(File.Create(spritePath + "\\" + (comboBox1.SelectedIndex * 5 + 1).ToString("D4")));
            }
            writePalette.Write(buffer, 0, 40);
            ColorPalette palette = this.m_bitmap.Palette;
            ushort[] numArray = new ushort[0x10];
            for (int i = 0; i < 0x10; i++)
            {
                numArray[i] = (ushort)((((palette.Entries[i].R >> 3) & 0x1f) | (((palette.Entries[i].G >> 3) & 0x1f) << 5)) | (((palette.Entries[i].B >> 3) & 0x1f) << 10));
            }
            for (int j = 0; j < 0x10; j++)
            {
                writePalette.Write(numArray[j]);
            }
            writePalette.Close();

            BinaryWriter writeSprite;
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                writeSprite = new BinaryWriter(File.Create(spritePath + "\\" + (comboBox1.SelectedIndex * 2).ToString("D4")));
            }
            else
            {
                writeSprite= new BinaryWriter(File.Create(spritePath + "\\" + (comboBox1.SelectedIndex * 5 + 4).ToString("D4")));
            }
            Rectangle rect = new Rectangle(0, 0, 160, 80);
            BitmapData bitmapdata = this.m_bitmap.LockBits(rect, ImageLockMode.ReadOnly, this.m_bitmap.PixelFormat);
            IntPtr source = bitmapdata.Scan0;
            byte[] destination = new byte[0x3200];
            Marshal.Copy(source, destination, 0, 0x3200);
            this.m_bitmap.UnlockBits(bitmapdata);
            ushort[] numArray2 = new ushort[0xC80];
            for (int i = 0; i < 0xC80; i++)
            {
                numArray2[i] = (ushort)((((destination[i * 4] & 15) | ((destination[(i * 4) + 1] & 15) << 4)) | ((destination[(i * 4) + 2] & 15) << 8)) | ((destination[(i * 4) + 3] & 15) << 12));
            }
            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                uint num2 = 0;
                uint num = numArray[0x0];
                for (int k = 0; k < 0xC80; k++)
                {
                    ushort[] numArray3;
                    IntPtr ptr2;
                    (numArray3 = numArray2)[(int)(ptr2 = (IntPtr)k)] = (ushort)(numArray3[(int)ptr2] ^ ((ushort)(num2 & 0xffff)));
                    num2 *= 0x41c64e6d;
                    num2 += 0x6073;
                }
            }
            else
            {
                uint num2 = 0x7a53;
                for (int j = 0xc7f; j >= 0; j--)
                {
                    num2 += numArray2[j];
                }
                uint num = numArray2[0xC7F];
                for (int k = 0xC7F; k >= 0; k--)
                {
                    ushort[] numArray3;
                    IntPtr ptr2;
                    (numArray3 = numArray2)[(int)(ptr2 = (IntPtr)k)] = (ushort)(numArray3[(int)ptr2] ^ ((ushort)(num2 & 0xffff)));
                    num2 *= 0x41c64e6d;
                    num2 += 0x6073;
                }
            }
            byte[] buffer2 = new byte[] { 
                0x52, 0x47, 0x43, 0x4e, 0xff, 0xfe, 0, 1, 0x30, 0x19, 0, 0, 0x10, 0, 1, 0, 
                0x52, 0x41, 0x48, 0x43, 0x20, 0x19, 0, 0, 10, 0, 20, 0, 3, 0, 0, 0, 
                0, 0, 0, 0, 1, 0, 0, 0, 0, 0x19, 0, 0, 0x18, 0, 0, 0
             };
            for (int m = 0; m < 0x30; m++)
            {
                writeSprite.Write(buffer2[m]);
            }
            for (int n = 0; n < 0xC80; n++)
            {
                writeSprite.Write(numArray2[n]);
            }
            writeSprite.Close();
            classNames[comboBox1.SelectedIndex] = textBox1.Text;
            int selectedID = comboBox1.SelectedIndex;
            saveText();
            comboBox1.Items.Clear();
            for (int i = 0; i < classNames.Count; i++)
            {
                comboBox1.Items.Add(i + ": " + classNames[i]);
            }
            comboBox1.SelectedIndex = selectedID;
        }

        private void Form3_2_Trainer_Class_Editor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                Narc.FromFolder(Form1.workingFolder + @"data\poketool\trgra\trfgra").Save(Form1.workingFolder + @"data\poketool\trgra\trfgra.narc");
                Directory.Delete(Form1.workingFolder + @"data\poketool\trgra\trfgra", true);
            }
            if (Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
            {
                Narc.FromFolder(Form1.workingFolder + @"data\a\0\5\trfgra").Save(Form1.workingFolder + @"data\a\0\5\8");
                Directory.Delete(Form1.workingFolder + @"data\a\0\5\trfgra", true);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Add
        {
            classNames.Add("TRAINER");
            comboBox1.Items.Add(comboBox1.Items.Count + ": " + "TRAINER");
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                File.Copy(spritePath + "\\" + "0000", spritePath + "\\" + ((comboBox1.Items.Count - 1) * 2).ToString("D4"));
                File.Copy(spritePath + "\\" + "0001", spritePath + "\\" + ((comboBox1.Items.Count - 1) * 2 + 1).ToString("D4"));
            }
            else
            {
                File.Copy(spritePath + "\\" + "0000", spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5).ToString("D4"));
                File.Copy(spritePath + "\\" + "0001", spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5 + 1).ToString("D4"));
                File.Copy(spritePath + "\\" + "0002", spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5 + 2).ToString("D4"));
                File.Copy(spritePath + "\\" + "0003", spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5 + 3).ToString("D4"));
                File.Copy(spritePath + "\\" + "0004", spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5 + 4).ToString("D4"));
            }
            saveText();
            if (comboBox1.Items.Count > 1)
            {
                button2.Enabled = true;
                if (comboBox1.Items.Count == 256)
                {
                    button1.Enabled = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) // Remove
        {
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                File.Delete(spritePath + "\\" + ((comboBox1.Items.Count - 1) * 2).ToString("D4"));
                File.Delete(spritePath + "\\" + ((comboBox1.Items.Count - 1) * 2 + 1).ToString("D4"));
            }
            else
            {
                File.Delete(spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5).ToString("D4"));
                File.Delete(spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5 + 1).ToString("D4"));
                File.Delete(spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5 + 2).ToString("D4"));
                File.Delete(spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5 + 3).ToString("D4"));
                File.Delete(spritePath + "\\" + ((comboBox1.Items.Count - 1) * 5 + 4).ToString("D4"));
            }
            comboBox1.Items.RemoveAt(classNames.Count - 1);
            classNames.RemoveAt(classNames.Count - 1);
            saveText();
            if (comboBox1.Items.Count < 256)
            {
                button1.Enabled = true;
                if (comboBox1.Items.Count == 1)
                {
                    button2.Enabled = false;
                }
            }
            if (comboBox1.SelectedIndex == -1) comboBox1.SelectedIndex = 0;
        }


        private void saveText() // Save Text File
        {
            BinaryWriter writeText = new BinaryWriter(File.Create(path));
            writeText.Write((UInt16)classNames.Count);
            writeText.Write((UInt16)initialKey);
            int key = (initialKey * 0x2FD) & 0xFFFF;
            int key2 = 0;
            int realKey = 0;
            int offset = 0x4 + (classNames.Count * 8);
            int[] stringSize = new int[classNames.Count];
            for (int i = 0; i < classNames.Count; i++) // Reads and stores string offsets and sizes
            {
                key2 = (key * (i + 1) & 0xFFFF);
                realKey = key2 | (key2 << 16);
                writeText.Write(offset ^ realKey);
                int length = getStringLength(i);
                stringSize[i] = length;
                writeText.Write(length ^ realKey);
                offset += length * 2;
            }
            for (int i = 0; i < classNames.Count; i++) // Encodes strings and writes them to file
            {
                key = (0x91BD3 * (i + 1)) & 0xFFFF;
                int[] currentString = EncodeString(i, stringSize[i]);
                for (int j = 0; j < stringSize[i] - 1; j++)
                {
                    writeText.Write((UInt16)(currentString[j] ^ key));
                    key += 0x493D;
                    key &= 0xFFFF;
                }
                writeText.Write((UInt16)(0xFFFF ^ key));
            }
            writeText.Close();
        }

        private int getStringLength(int stringIndex) // Calculates string length
        {
            int count = 0;
            string currentMessage = "";
            try { currentMessage = classNames[stringIndex]; }
            catch { }
            var charArray = currentMessage.ToCharArray();
            for (int i = 0; i < currentMessage.Length; i++)
            {
                if (charArray[i] == '\\')
                {
                    if (charArray[i + 1] == 'r')
                    {
                        count++;
                        i++;
                    }
                    else
                    {
                        if (charArray[i + 1] == 'n')
                        {
                            count++;
                            i++;
                        }
                        else
                        {
                            if (charArray[i + 1] == 'f')
                            {
                                count++;
                                i++;
                            }
                            else
                            {
                                if (charArray[i + 1] == 'v')
                                {
                                    count += 2;
                                    i += 5;
                                }
                                else
                                {
                                    if (charArray[i + 1] == 'x' && charArray[i + 2] == '0' && charArray[i + 3] == '0' && charArray[i + 4] == '0' && charArray[i + 5] == '0')
                                    {
                                        count++;
                                        i += 5;
                                    }
                                    else
                                    {
                                        if (charArray[i + 1] == 'x' && charArray[i + 2] == '0' && charArray[i + 3] == '0' && charArray[i + 4] == '0' && charArray[i + 5] == '1')
                                        {
                                            count++;
                                            i += 5;
                                        }
                                        else
                                        {
                                            count++;
                                            i += 5;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (charArray[i] == '[')
                    {
                        if (charArray[i + 1] == 'P')
                        {
                            count++;
                            i += 3;
                        }
                        if (charArray[i + 1] == 'M')
                        {
                            count++;
                            i += 3;
                        }
                    }
                    else
                    {
                        count++;
                    }
                }
            }
            count++;
            return count;
        }

        private int[] EncodeString(int stringIndex, int stringSize) // Converts string to hex characters
        {
            int[] pokemonMessage = new int[stringSize - 1];
            string currentMessage = "";
            try { currentMessage = classNames[stringIndex]; }
            catch { }
            var charArray = currentMessage.ToCharArray();
            int count = 0;
            for (int i = 0; i < currentMessage.Length; i++)
            {
                if (charArray[i] == '\\')
                {
                    if (charArray[i + 1] == 'r')
                    {
                        pokemonMessage[count] = 0x25BC;
                        i++;
                    }
                    else
                    {
                        if (charArray[i + 1] == 'n')
                        {
                            pokemonMessage[count] = 0xE000;
                            i++;
                        }
                        else
                        {
                            if (charArray[i + 1] == 'f')
                            {
                                pokemonMessage[count] = 0x25BD;
                                i++;
                            }
                            else
                            {
                                if (charArray[i + 1] == 'v')
                                {
                                    pokemonMessage[count] = 0xFFFE;
                                    count++;
                                    string characterID = ((char)charArray[i + 2]).ToString() + ((char)charArray[i + 3]).ToString() + ((char)charArray[i + 4]).ToString() + ((char)charArray[i + 5]).ToString();
                                    pokemonMessage[count] = (int)Convert.ToUInt32(characterID, 16);
                                    i += 5;
                                }
                                else
                                {
                                    if (charArray[i + 1] == 'x' && charArray[i + 2] == '0' && charArray[i + 3] == '0' && charArray[i + 4] == '0' && charArray[i + 5] == '0')
                                    {
                                        pokemonMessage[count] = 0x0000;
                                        i += 5;
                                    }
                                    else
                                    {
                                        if (charArray[i + 1] == 'x' && charArray[i + 2] == '0' && charArray[i + 3] == '0' && charArray[i + 4] == '0' && charArray[i + 5] == '1')
                                        {
                                            pokemonMessage[count] = 0x0001;
                                            i += 5;
                                        }
                                        else
                                        {
                                            string characterID = ((char)charArray[i + 2]).ToString() + ((char)charArray[i + 3]).ToString() + ((char)charArray[i + 4]).ToString() + ((char)charArray[i + 5]).ToString();
                                            pokemonMessage[count] = (int)Convert.ToUInt32(characterID, 16);
                                            i += 5;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (charArray[i] == '[')
                    {
                        if (charArray[i + 1] == 'P')
                        {
                            pokemonMessage[count] = 0x01E0;
                            i += 3;
                        }
                        if (charArray[i + 1] == 'M')
                        {
                            pokemonMessage[count] = 0x01E1;
                            i += 3;
                        }
                    }
                    else
                    {
                        pokemonMessage[count] = (int)Convert.ToUInt32(getByte.GetString(((int)charArray[i]).ToString()), 16);
                    }
                }
                count++;
            }
            return pokemonMessage;
        }
    }
}
