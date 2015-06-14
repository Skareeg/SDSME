using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Reflection;

namespace WindowsFormsApplication1
{
    public partial class Form5 : Form
    {
        ResourceManager rm = new ResourceManager("WindowsFormsApplication1.WinFormStrings", Assembly.GetExecutingAssembly());

        public string eventPath;
        public int furnitureCount;
        public int overworldCount;
        public int warpCount;
        public int triggerCount;
        public Form5()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = true;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            #region Reset
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            numericUpDown9.Value = 0;
            numericUpDown10.Value = 0;
            numericUpDown11.Value = 0;
            numericUpDown12.Value = 0;
            numericUpDown13.Value = 1;
            numericUpDown14.Value = 0;
            numericUpDown15.Value = 0;
            numericUpDown16.Value = 0;
            numericUpDown17.Value = 0;
            numericUpDown18.Value = 0;
            numericUpDown19.Value = 0;
            numericUpDown20.Value = 0;
            numericUpDown21.Value = 0;
            numericUpDown22.Value = 0;
            numericUpDown23.Value = 0;
            numericUpDown24.Value = 0;
            numericUpDown25.Value = 0;
            numericUpDown26.Value = 0;
            numericUpDown27.Value = 0;
            numericUpDown28.Value = 0;
            numericUpDown29.Value = 0;
            numericUpDown30.Value = 0;
            numericUpDown31.Value = 0;
            numericUpDown32.Value = 0;
            numericUpDown33.Value = 0;
            numericUpDown34.Value = 0;
            numericUpDown35.Value = 0;
            numericUpDown36.Value = 0;
            numericUpDown37.Value = 0;
            numericUpDown38.Value = 0;
            numericUpDown39.Value = 0;
            numericUpDown40.Value = 0;
            #endregion
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            furnitureCount = readEvent.ReadByte() + (readEvent.ReadByte() << 8) + (readEvent.ReadByte() << 16) + (readEvent.ReadByte() << 24);
            readEvent.BaseStream.Position += furnitureCount * 0x14;
            overworldCount = readEvent.ReadByte() + (readEvent.ReadByte() << 8) + (readEvent.ReadByte() << 16) + (readEvent.ReadByte() << 24);
            readEvent.BaseStream.Position += overworldCount * 0x20;
            warpCount = readEvent.ReadByte() + (readEvent.ReadByte() << 8) + (readEvent.ReadByte() << 16) + (readEvent.ReadByte() << 24);
            readEvent.BaseStream.Position += warpCount * 0xc;
            triggerCount = readEvent.ReadByte() + (readEvent.ReadByte() << 8) + (readEvent.ReadByte() << 16) + (readEvent.ReadByte() << 24);
            for (int i = 0; i < furnitureCount; i++)
            {
                listBox1.Items.Add(rm.GetString("furniture") + (i + 1));
            }
            for (int i = 0; i < overworldCount; i++)
            {
                listBox2.Items.Add(rm.GetString("overworld") + (i + 1));
            }
            for (int i = 0; i < warpCount; i++)
            {
                listBox3.Items.Add(rm.GetString("warp") + (i + 1));
            }
            for (int i = 0; i < triggerCount; i++)
            {
                listBox4.Items.Add(rm.GetString("trigger") + (i + 1));
            }
            readEvent.Close();
            if (listBox1.Items.Count != 0)
            {
                listBox1.SelectedIndex = 0;
            }
            if (listBox2.Items.Count != 0)
            {
                listBox2.SelectedIndex = 0;
            }
            if (listBox3.Items.Count != 0)
            {
                listBox3.SelectedIndex = 0;
            }
            if (listBox4.Items.Count != 0)
            {
                listBox4.SelectedIndex = 0;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            readEvent.BaseStream.Position = 0x4 + 0x14*listBox1.SelectedIndex;
            numericUpDown1.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown2.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown3.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown4.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown5.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown10.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown9.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown8.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown7.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown6.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            readEvent.Close();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                button4.Enabled = false;
                return;
            }
            button4.Enabled = true;
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            readEvent.BaseStream.Position = 0x8 + 0x14*furnitureCount + 0x20*listBox2.SelectedIndex;
            numericUpDown34.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown33.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown32.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown31.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown30.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown36.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown38.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown40.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown29.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown28.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown27.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown26.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown25.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown35.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown37.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown39.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            readEvent.Close();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex == -1)
            {
                button6.Enabled = false;
                return;
            }
            button6.Enabled = true;
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            readEvent.BaseStream.Position = 0xc + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * listBox3.SelectedIndex;
            numericUpDown16.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown15.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown14.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown13.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8) + 1;
            numericUpDown12.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown11.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            readEvent.Close();
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex == -1)
            {
                button8.Enabled = false;
                return;
            }
            button8.Enabled = true;
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            readEvent.BaseStream.Position = 0x10 + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * warpCount + 0x10 * listBox4.SelectedIndex;
            numericUpDown24.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown18.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown21.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown23.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown20.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown17.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown19.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            numericUpDown22.Value = readEvent.ReadByte() + (readEvent.ReadByte() << 8);
            readEvent.Close();
        }

        private void button3_Click(object sender, EventArgs e) // Save Current
        {
            if (tabControl1.SelectedIndex == 0 && listBox1.SelectedIndex !=-1)
            {
                System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                writeEvent.BaseStream.Position = 0x4 + 0x14 * listBox1.SelectedIndex;
                writeEvent.Write((Int16)numericUpDown1.Value);
                writeEvent.Write((Int16)numericUpDown2.Value);
                writeEvent.Write((Int16)numericUpDown3.Value);
                writeEvent.Write((Int16)numericUpDown4.Value);
                writeEvent.Write((Int16)numericUpDown5.Value);
                writeEvent.Write((Int16)numericUpDown10.Value);
                writeEvent.Write((Int16)numericUpDown9.Value);
                writeEvent.Write((Int16)numericUpDown8.Value);
                writeEvent.Write((Int16)numericUpDown7.Value);
                writeEvent.Write((Int16)numericUpDown6.Value);
                writeEvent.Close();
            }
            if (tabControl1.SelectedIndex == 1 && listBox2.SelectedIndex != -1)
            {
                System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                writeEvent.BaseStream.Position = 0x8 + 0x14 * furnitureCount + 0x20 * listBox2.SelectedIndex;
                writeEvent.Write((Int16)numericUpDown34.Value);
                writeEvent.Write((Int16)numericUpDown33.Value);
                writeEvent.Write((Int16)numericUpDown32.Value);
                writeEvent.Write((Int16)numericUpDown31.Value);
                writeEvent.Write((Int16)numericUpDown30.Value);
                writeEvent.Write((Int16)numericUpDown36.Value);
                writeEvent.Write((Int16)numericUpDown38.Value);
                writeEvent.Write((Int16)numericUpDown40.Value);
                writeEvent.Write((Int16)numericUpDown29.Value);
                writeEvent.Write((Int16)numericUpDown28.Value);
                writeEvent.Write((Int16)numericUpDown27.Value);
                writeEvent.Write((Int16)numericUpDown26.Value);
                writeEvent.Write((Int16)numericUpDown25.Value);
                writeEvent.Write((Int16)numericUpDown35.Value);
                writeEvent.Write((Int16)numericUpDown37.Value);
                writeEvent.Write((Int16)numericUpDown38.Value);
                writeEvent.Close();
            }
            if (tabControl1.SelectedIndex == 2 && listBox3.SelectedIndex != -1)
            {
                System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                writeEvent.BaseStream.Position = 0xc + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * listBox3.SelectedIndex;
                writeEvent.Write((Int16)numericUpDown16.Value);
                writeEvent.Write((Int16)numericUpDown15.Value);
                writeEvent.Write((Int16)numericUpDown14.Value);
                writeEvent.Write((Int16)(numericUpDown13.Value - 1));
                writeEvent.Write((Int16)numericUpDown12.Value);
                writeEvent.Write((Int16)numericUpDown11.Value);
                writeEvent.Close();
            }
            if (tabControl1.SelectedIndex == 3 && listBox4.SelectedIndex != -1)
            {
                System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                writeEvent.BaseStream.Position = 0x10 + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * warpCount + 0x10 * listBox4.SelectedIndex;
                writeEvent.Write((Int16)numericUpDown24.Value);
                writeEvent.Write((Int16)numericUpDown18.Value);
                writeEvent.Write((Int16)numericUpDown21.Value);
                writeEvent.Write((Int16)numericUpDown23.Value);
                writeEvent.Write((Int16)numericUpDown20.Value);
                writeEvent.Write((Int16)numericUpDown17.Value);
                writeEvent.Write((Int16)numericUpDown19.Value);
                writeEvent.Write((Int16)numericUpDown22.Value);
                writeEvent.Close();
            }
        }

        #region Add

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            File.Create(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new").Close();
            System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new"));
            for (int i = 0; i < (0x4 + 0x14 * furnitureCount); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.Write((byte)0x1);
            for (int i = 0; i < (0x13); i++)
            {
                writeEvent.Write((byte)0x0); // Writes new furniture
            }
            for (int i = 0; i < (readEvent.BaseStream.Length - (0x4 + 0x14 * furnitureCount)); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.BaseStream.Position = 0x0;
            writeEvent.Write(furnitureCount + 1);
            furnitureCount++;
            readEvent.Close();
            writeEvent.Close();
            File.Delete(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            File.Move(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new", eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            File.Create(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new").Close();
            System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new"));
            for (int i = 0; i < (0x8 + 0x14 * furnitureCount + 0x20 * overworldCount); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            for (int i = 0; i < (0x20); i++)
            {
                writeEvent.Write((byte)0x0); // Writes new overworld
            }
            for (int i = 0; i < (readEvent.BaseStream.Length-(0x8 + 0x14 * furnitureCount + 0x20 * overworldCount)); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.BaseStream.Position = 0x4 + furnitureCount * 0x14;
            writeEvent.Write(overworldCount + 1);
            overworldCount++;
            readEvent.Close();
            writeEvent.Close();
            File.Delete(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            File.Move(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new", eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            File.Create(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new").Close();
            System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new"));
            for (int i = 0; i < (0xc + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * warpCount); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            for (int i = 0; i < (0xc); i++)
            {
                writeEvent.Write((byte)0x0); // Writes new warp
            }
            for (int i = 0; i < (readEvent.BaseStream.Length-(0xc + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * warpCount)); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.BaseStream.Position = 0x8 + furnitureCount * 0x14 + overworldCount * 0x20;
            writeEvent.Write(warpCount + 1);
            warpCount++;
            readEvent.Close();
            writeEvent.Close();
            File.Delete(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            File.Move(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new", eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            File.Create(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new").Close();
            System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new"));
            for (int i = 0; i < (0x10 + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * warpCount + 0x10 * triggerCount); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.Write((byte)0x1);
            for (int i = 0; i < (0xf); i++)
            {
                writeEvent.Write((byte)0x0); // Writes new trigger
            }
            for (int i = 0; i < (readEvent.BaseStream.Length - (0x10 + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * warpCount + 0x10 * triggerCount)); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.BaseStream.Position = 0xc + furnitureCount * 0x14 + overworldCount * 0x20 + warpCount * 0xc;
            writeEvent.Write(triggerCount + 1);
            triggerCount++;
            readEvent.Close();
            writeEvent.Close();
            File.Delete(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            File.Move(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new", eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            comboBox1_SelectedIndexChanged(null, null);
        }

        #endregion

        #region Remove

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            File.Create(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new").Close();
            System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new"));
            for (int i = 0; i < (0x4 + 0x14 * listBox1.SelectedIndex); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            readEvent.BaseStream.Position += 0x14;
            for (int i = 0; i < (readEvent.BaseStream.Length - (0x4 + 0x14 * listBox1.SelectedIndex + 0x14)); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.BaseStream.Position = 0x0;
            writeEvent.Write((int)(furnitureCount -1));
            readEvent.Close();
            writeEvent.Close();
            File.Delete(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            File.Move(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new", eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1) return;
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            File.Create(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new").Close();
            System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new"));
            for (int i = 0; i < (0x8 + 0x14 * furnitureCount + 0x20 * listBox2.SelectedIndex); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            readEvent.BaseStream.Position += 0x20;
            for (int i = 0; i < (readEvent.BaseStream.Length - (0x8 + 0x14 * furnitureCount + 0x20 * listBox2.SelectedIndex + 0x20)); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.BaseStream.Position = 0x4 + furnitureCount * 0x14;
            writeEvent.Write((int)(overworldCount - 1));
            readEvent.Close();
            writeEvent.Close();
            File.Delete(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            File.Move(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new", eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex == -1) return;
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            File.Create(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new").Close();
            System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new"));
            for (int i = 0; i < (0xc + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * listBox3.SelectedIndex); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            readEvent.BaseStream.Position += 0xc;
            for (int i = 0; i < (readEvent.BaseStream.Length - (0xc + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * listBox3.SelectedIndex + 0xc)); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.BaseStream.Position = 0x8 + furnitureCount * 0x14 + overworldCount * 0x20;
            writeEvent.Write((int)(warpCount - 1));
            readEvent.Close();
            writeEvent.Close();
            File.Delete(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            File.Move(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new", eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            comboBox1_SelectedIndexChanged(null, null);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex == -1) return;
            System.IO.BinaryReader readEvent = new System.IO.BinaryReader(File.OpenRead(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            File.Create(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new").Close();
            System.IO.BinaryWriter writeEvent = new System.IO.BinaryWriter(File.OpenWrite(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new"));
            for (int i = 0; i < (0x10 + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * warpCount + 0x10 * listBox4.SelectedIndex); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            readEvent.BaseStream.Position += 0x10;
            for (int i = 0; i < (readEvent.BaseStream.Length - (0x10 + 0x14 * furnitureCount + 0x20 * overworldCount + 0xc * warpCount + 0x10 * listBox4.SelectedIndex + 0x10)); i++)
            {
                writeEvent.Write(readEvent.ReadByte()); // Reads unmodified bytes and writes them to the main file
            }
            writeEvent.BaseStream.Position = 0xc + furnitureCount * 0x14 + overworldCount * 0x20 + warpCount * 0xc;
            writeEvent.Write((int)(triggerCount - 1));
            readEvent.Close();
            writeEvent.Close();
            File.Delete(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            File.Move(eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4") + "_new", eventPath + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            comboBox1_SelectedIndexChanged(null, null);
        }

        #endregion

        private void Form5_Load(object sender, EventArgs e)
        {
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                NarcAPI.Narc.Open(Form1.workingFolder + @"data\fielddata\eventdata\zone_event_release.narc").ExtractToFolder(Form1.workingFolder + @"data\fielddata\eventdata\zone_event_release");
                eventPath = Form1.workingFolder + @"data\fielddata\eventdata\zone_event_release";
            }
            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041)
            {
                NarcAPI.Narc.Open(Form1.workingFolder + @"data\fielddata\eventdata\zone_event.narc").ExtractToFolder(Form1.workingFolder + @"data\fielddata\eventdata\zone_event");
                eventPath = Form1.workingFolder + @"data\fielddata\eventdata\zone_event";
            }
            if (Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
            {
                NarcAPI.Narc.Open(Form1.workingFolder + @"data\a\0\3\2").ExtractToFolder(Form1.workingFolder + @"data\a\0\3\event");
                eventPath = Form1.workingFolder + @"data\a\0\3\event";
            }
            for (int i = 0; i < Directory.GetFiles(eventPath).Length; i++)
            {
                comboBox1.Items.Add(rm.GetString("eventList") + i);
            }
            comboBox1.SelectedIndex = Form1.eventIndex;
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\fielddata\eventdata\zone_event_release").Save(Form1.workingFolder + @"data\fielddata\eventdata\zone_event_release.narc");
                Directory.Delete(Form1.workingFolder + @"data\fielddata\eventdata\zone_event_release", true);
            }
            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\fielddata\eventdata\zone_event").Save(Form1.workingFolder + @"data\fielddata\eventdata\zone_event.narc");
                Directory.Delete(Form1.workingFolder + @"data\fielddata\eventdata\zone_event", true);
            }
            if (Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\a\0\3\event").Save(Form1.workingFolder + @"data\a\0\3\2");
                Directory.Delete(Form1.workingFolder + @"data\a\0\3\event", true);
            }
        }
    }
}
