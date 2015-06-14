using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsFormsApplication1;
using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Resources;
using Map_Converter;

namespace Map_Converter
{
    public partial class Form2 : Form
    {
        ResourceManager rm = new ResourceManager("WindowsFormsApplication1.WinFormStrings", Assembly.GetExecutingAssembly());

        public int amount;
        public int counter = 1;
        private int xCoord = 1;
        private int zCoord = 1;
        private string perColor;
        private string textColor;
        public int gridMode = 0;
        public Form2()
        {
            InitializeComponent();
            numericUpDown5.Visible = false;
            label8.Visible = false;
            if (Form1.isBW || Form1.isB2W2)
            {
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                textBox7.Visible = false;
                numericUpDown5.Visible = true;
                label8.Visible = true;
                System.IO.BinaryReader read = new System.IO.BinaryReader(File.OpenRead(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4")));
                read.BaseStream.Position = Form1.vbldOffset;
                amount = read.ReadInt32();
                for (int i = 0; i < amount; i++) // Checks the number of buildings in the map
                {
                    listBox1.Items.Add(rm.GetString("building") + counter); // Adds building to listbox
                    counter = counter + 1; // Increases index number
                }
                for (int i = 0; i < 34; i++)
                {
                    dataGridView1.Rows.Add(); // Creates 34x34 grid
                }
                if (Form1.mapType != 0x0002474E)
                {
                    read.BaseStream.Position = Form1.vpermOffset + 0x4;
                    for (int i = 0; i < 32; i++)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                            read.BaseStream.Position += 4;
                            int firstByte = read.ReadByte();
                            read.BaseStream.Position++;
                            StreamReader colors = new StreamReader(@"Data\ColorTableBW.txt");
                            for (int lineCounter = 0; lineCounter < firstByte; lineCounter++) // Chooses line
                            {
                                colors.ReadLine();
                            }
                            string colorString = colors.ReadLine();
                            dataGridView1.Rows[zCoord].Cells[xCoord].Style.BackColor = System.Drawing.ColorTranslator.FromHtml(colorString.Substring(5, 7)); // Shows backcolor
                            dataGridView1.Rows[zCoord].Cells[xCoord].Style.ForeColor = System.Drawing.ColorTranslator.FromHtml(colorString.Substring(13, 7)); // Shows forecolor
                            int secondByte = read.ReadByte();
                            if (secondByte == 129 && colorString.Substring(5, 7) == "#FFFFFF" && colorString.Substring(13, 7) == "#000000") // "No Movements"
                            {
                                dataGridView1.Rows[zCoord].Cells[xCoord].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000"); // Red
                                dataGridView1.Rows[zCoord].Cells[xCoord].Style.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF"); // White
                            }
                            colors.Close();
                            read.BaseStream.Position++;
                            xCoord++;
                        }
                        zCoord++;
                        xCoord = 1;
                    }
                }
                read.Close();
                if (listBox1.Items.Count != 0)
                {
                    listBox1.SelectedIndex = 0;
                    button3.Enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < (Form1.buildingsSize / 0x30); i++) // Checks the number of buildings in the map
                {
                    listBox1.Items.Add(rm.GetString("building") + counter); // Adds building to listbox
                    counter = counter + 1; // Increases index number
                }
                System.IO.BinaryReader readMap = new System.IO.BinaryReader(File.OpenRead(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4")));
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    readMap.BaseStream.Position = 0x10;
                }
                else
                {
                    readMap.BaseStream.Position = 0x14 + Form1.unknownSize;
                }
                for (int i = 0; i < 34; i++)
                {
                    dataGridView1.Rows.Add(); // Creates 34x34 grid
                }
                for (int i = 0; i < 32; i++)
                {
                    for (int counter = 0; counter < 32; counter++)
                    {
                        int firstByte = readMap.ReadByte();
                        StreamReader colors = new StreamReader("Data\\ColorTable.txt");
                        for (int lineCounter = 0; lineCounter < firstByte; lineCounter++) // Chooses line
                        {
                            colors.ReadLine();
                        }
                        string colorString = colors.ReadLine();
                        perColor = colorString.Substring(5, 7); // Reads backcolor
                        textColor = colorString.Substring(13, 7); // Reads forecolor
                        dataGridView1.Rows[zCoord].Cells[xCoord].Style.BackColor = System.Drawing.ColorTranslator.FromHtml(perColor); // Shows backcolor
                        dataGridView1.Rows[zCoord].Cells[xCoord].Style.ForeColor = System.Drawing.ColorTranslator.FromHtml(textColor); // Shows forecolor
                        int secondByte = readMap.ReadByte();
                        if (secondByte == 128 && colorString.Substring(5, 7) == "#FFFFFF" && colorString.Substring(13, 7) == "#000000") // "No Movements"
                        {
                            dataGridView1.Rows[zCoord].Cells[xCoord].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF0000"); // Red
                            dataGridView1.Rows[zCoord].Cells[xCoord].Style.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF"); // White
                        }
                        if (secondByte == 4 && colorString.Substring(5, 7) == "#FFFFFF" && colorString.Substring(13, 7) == "#000000") // HGSS "No Special Permissions"
                        {
                            dataGridView1.Rows[zCoord].Cells[xCoord].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#99FF66"); // Light Green
                            dataGridView1.Rows[zCoord].Cells[xCoord].Style.ForeColor = System.Drawing.ColorTranslator.FromHtml("#000000"); // Black
                        }
                        xCoord++;
                        colors.Close();
                    }
                    zCoord++;
                    xCoord = 1;
                }
                readMap.Close();
                if (listBox1.Items.Count != 0)
                {
                    listBox1.SelectedIndex = 0;
                    button3.Enabled = true;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                button3.Enabled = false;
                return;
            }
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
            numericUpDown4.Enabled = true;
            numericUpDown5.Enabled = true;
            textBox1.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            textBox9.Enabled = true;
            button3.Enabled = true;
            button2.Enabled = true;

            if (Form1.isBW || Form1.isB2W2)
            {
                System.IO.BinaryReader readMap = new System.IO.BinaryReader(File.OpenRead(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4")));
                readMap.BaseStream.Position = Form1.vbldOffset + 0x4 + listBox1.SelectedIndex * 0x10;
                textBox1.Text = Convert.ToString(readMap.ReadUInt16()); // Reads X Flag
                numericUpDown2.Value = readMap.ReadInt16(); // Reads X Tile
                textBox8.Text = Convert.ToString(readMap.ReadUInt16()); // Reads Y Flag
                numericUpDown3.Value = readMap.ReadInt16(); // Reads Y Tile
                textBox9.Text = Convert.ToString(readMap.ReadUInt16()); // Reads Z Flag
                numericUpDown4.Value = readMap.ReadInt16(); // Reads Z Tile
                readMap.BaseStream.Position++;
                numericUpDown5.Value = readMap.ReadByte(); // Reads Rotation
                numericUpDown1.Value = (readMap.ReadByte() << 8) + readMap.ReadByte(); // Reads Model ID
                readMap.Close();
                dataGridView1.Rows[(Int16)(((int)(numericUpDown4.Value) ^ 0xFFFF) + 17)].Cells[(int)(numericUpDown2.Value + 17)].Selected = true;
            }
            else
            {
                System.IO.BinaryReader read = new System.IO.BinaryReader(File.OpenRead(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4")));
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    read.BaseStream.Position = 0x10 + Form1.permissionSize + (0x0 + 0x30 * listBox1.SelectedIndex);
                }
                else
                {
                    read.BaseStream.Position = 0x14 + Form1.unknownSize + Form1.permissionSize + (0x0 + 0x30 * listBox1.SelectedIndex);
                }
                numericUpDown1.Value = read.ReadUInt32(); // Reads Model Index
                textBox1.Text = Convert.ToString(read.ReadUInt16());
                numericUpDown2.Value = read.ReadInt16(); // Reads X Coordinates
                textBox8.Text = Convert.ToString(read.ReadUInt16());
                numericUpDown3.Value = read.ReadInt16(); // Reads Y Coordinates
                textBox9.Text = Convert.ToString(read.ReadUInt16());
                numericUpDown4.Value = read.ReadInt16(); // Reads Z Coordinates
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    read.BaseStream.Position = 0x10 + Form1.permissionSize + (0x0 + 0x30 * listBox1.SelectedIndex + 0x1D);
                }
                else
                {
                    read.BaseStream.Position = 0x14 + Form1.unknownSize + Form1.permissionSize + (0x0 + 0x30 * listBox1.SelectedIndex + 0x1D);
                }
                textBox7.Text = Convert.ToString(read.ReadInt16());
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    read.BaseStream.Position = 0x10 + Form1.permissionSize + (0x0 + 0x30 * listBox1.SelectedIndex + 0x21);
                }
                else
                {
                    read.BaseStream.Position = 0x14 + Form1.unknownSize + Form1.permissionSize + (0x0 + 0x30 * listBox1.SelectedIndex + 0x21);
                }
                textBox6.Text = Convert.ToString(read.ReadInt16());
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    read.BaseStream.Position = 0x10 + Form1.permissionSize + (0x0 + 0x30 * listBox1.SelectedIndex + 0x25);
                }
                else
                {
                    read.BaseStream.Position = 0x14 + Form1.unknownSize + Form1.permissionSize + (0x0 + 0x30 * listBox1.SelectedIndex + 0x25);
                }
                textBox5.Text = Convert.ToString(read.ReadInt16());
                read.Close();
                dataGridView1.Rows[Convert.ToInt32(numericUpDown4.Value + 17)].Cells[Convert.ToInt32(numericUpDown2.Value + 17)].Selected = true;
            }
        }

        private void button3_Click(object sender, EventArgs e) // Save
        {
            if (Form1.isBW || Form1.isB2W2)
            {
                System.IO.BinaryReader readMap = new System.IO.BinaryReader(File.OpenRead(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4")));
                File.Create(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4") + "_new").Close();
                System.IO.BinaryWriter writeMap = new System.IO.BinaryWriter(File.OpenWrite(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4") + "_new"));
                for (int i = 0; i < Form1.vbldOffset + 0x4 + (0x10 * listBox1.SelectedIndex); i++)
                {
                    writeMap.Write(readMap.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                writeMap.Write(Convert.ToUInt16(textBox1.Text)); // Writes X Flag
                writeMap.Write(Convert.ToInt16(numericUpDown2.Value)); // Writes X Coordinates
                writeMap.Write(Convert.ToUInt16(textBox8.Text)); // Writes Y Flag
                writeMap.Write(Convert.ToInt16(numericUpDown3.Value)); // Writes Y Coordinates
                writeMap.Write(Convert.ToUInt16(textBox9.Text)); // Writes Z Flag
                writeMap.Write(Convert.ToInt16(numericUpDown4.Value)); // Writes Z Coordinates
                writeMap.Write((byte)0);
                writeMap.Write((byte)numericUpDown5.Value);
                writeMap.Write((byte)((int)numericUpDown1.Value >> 8));
                writeMap.Write((byte)((int)numericUpDown1.Value & 0xFF));
                readMap.BaseStream.Position = Form1.vbldOffset + 0x4 + (0x10 * listBox1.SelectedIndex) + 0x10;
                for (int i = 0; i < 0x10 * (amount - (listBox1.SelectedIndex + 1)); i++)
                {
                    writeMap.Write(readMap.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                readMap.Close();
                writeMap.Close();

                File.Delete(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4"));
                File.Move(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4") + "_new", Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4"));
                listBox1_SelectedIndexChanged(null, null);
            }
            else
            {
                System.IO.BinaryReader size = new System.IO.BinaryReader(File.OpenRead(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4")));
                File.Create(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4") + "_new").Close();
                System.IO.BinaryWriter write = new System.IO.BinaryWriter(File.OpenWrite(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4") + "_new"));
                write.BaseStream.Position = 0x0;
                size.BaseStream.Position = 0x0;
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    for (int i = 0; i < (0x10 + Form1.permissionSize + (0x30 * listBox1.SelectedIndex)); i++)
                    {
                        write.Write(size.ReadByte()); // Reads unmodified bytes and writes them to the main file
                    }
                }
                else
                {
                    for (int i = 0; i < (0x14 + Form1.unknownSize + Form1.permissionSize + (0x30 * listBox1.SelectedIndex)); i++)
                    {
                        write.Write(size.ReadByte()); // Reads unmodified bytes and writes them to the main file
                    }
                }
                write.Write((int)numericUpDown1.Value); // Writes Model Index
                write.Write(Convert.ToUInt16(textBox1.Text)); // Writes X Flag
                write.Write(Convert.ToInt16(numericUpDown2.Value)); // Writes X Coordinates
                write.Write(Convert.ToUInt16(textBox8.Text)); // Writes Y Flag
                write.Write(Convert.ToInt16(numericUpDown3.Value)); // Writes Y Coordinates
                write.Write(Convert.ToUInt16(textBox9.Text)); // Writes Z Flag
                write.Write(Convert.ToInt16(numericUpDown4.Value)); // Writes Z Coordinates
                for (int i = 0; i < 0xD; i++) write.Write((Byte)0x0); // Writes junk bytes
                write.Write(Convert.ToUInt16(textBox7.Text)); // Writes Width
                for (int i = 0; i < 2; i++) write.Write((Byte)0x0); // Writes junk bytes
                write.Write(Convert.ToUInt16(textBox6.Text)); // Writes Heigth
                for (int i = 0; i < 2; i++) write.Write((Byte)0x0); // Writes junk bytes
                write.Write(Convert.ToUInt16(textBox5.Text)); // Writes Length
                for (int i = 0; i < 9; i++) write.Write((Byte)0x0); // Writes junk bytes
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    size.BaseStream.Position = 0x10 + Form1.permissionSize + (0x30 * listBox1.SelectedIndex) + 0x30;
                }
                else
                {
                    size.BaseStream.Position = 0x14 + Form1.unknownSize + Form1.permissionSize + (0x30 * listBox1.SelectedIndex) + 0x30;
                }
                for (int i = 0; i < (0x30 * ((Form1.buildingsSize / 0x30) - (listBox1.SelectedIndex + 1)) + Form1.modelSize + Form1.terrainSize); i++)
                {
                    write.Write(size.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                size.Close();
                write.Close();

                File.Delete(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4"));
                File.Move(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4") + "_new", Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4"));
                listBox1_SelectedIndexChanged(null, null);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Add
        {
            if (Form1.isBW || Form1.isB2W2)
            {
                amount++;
                listBox1.Items.Add(rm.GetString("building") + amount); // Adds building to listbox
                System.IO.BinaryReader readMap = new System.IO.BinaryReader(File.OpenRead(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4")));
                File.Create(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4") + "_new").Close();
                System.IO.BinaryWriter writeMap = new System.IO.BinaryWriter(File.OpenWrite(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4") + "_new"));
                for (int i = 0; i < Form1.vbldOffset; i++)
                {
                    writeMap.Write(readMap.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                writeMap.Write(amount); // Writes new building count
                readMap.BaseStream.Position += 0x4;
                for (int i = 0; i < (amount - 1) * 0x10; i++)
                {
                    writeMap.Write(readMap.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                for (int i = 0; i < 0x10; i++)
                {
                    writeMap.Write((byte)0); // Writes new building
                }
                writeMap.BaseStream.Position = Form1.voffsetPos;
                Form1.vendOffset = Form1.vbldOffset + 0x4 + (0x10 * amount);
                writeMap.Write(Form1.vendOffset);
                readMap.Close();
                writeMap.Close();

                File.Delete(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4"));
                File.Move(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4") + "_new", Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4"));
            }
            else
            {
                counter = 1;
                listBox1.Items.Clear();
                for (int i = 0; i < (Form1.buildingsSize / 0x30 + 1); i++) // Index recheck
                {
                    listBox1.Items.Add(rm.GetString("building") + counter); // Adds building to listbox
                    counter = counter + 1; // Increases index number
                }

                System.IO.BinaryReader size = new System.IO.BinaryReader(File.OpenRead(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4")));
                File.Create(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4") + "_new").Close();
                System.IO.BinaryWriter write = new System.IO.BinaryWriter(File.OpenWrite(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4") + "_new"));
                write.BaseStream.Position = 0x0;
                size.BaseStream.Position = 0x0;
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    for (int i = 0; i < (0x10 + Form1.permissionSize + Form1.buildingsSize); i++)
                    {
                        write.Write(size.ReadByte()); // Reads unmodified bytes and writes them to the main file
                    }
                }
                else
                {
                    for (int i = 0; i < (0x14 + Form1.unknownSize + Form1.permissionSize + Form1.buildingsSize); i++)
                    {
                        write.Write(size.ReadByte()); // Reads unmodified bytes and writes them to the main file
                    }
                }
                write.Write((int)0x0); // Writes Model Index
                write.Write(Convert.ToUInt16(0x0)); // Writes X Flag
                write.Write((UInt16)(65519)); // Writes X Coordinates
                write.Write(Convert.ToUInt16(0x0)); // Writes Y Flag
                write.Write((UInt16)(0x1)); // Writes Y Coordinates
                write.Write(Convert.ToUInt16(0x0)); // Writes Z Flag
                write.Write((UInt16)(65519)); // Writes Z Coordinates
                for (int i = 0; i < 0xD; i++) write.Write((Byte)0x0); // Writes junk bytes
                write.Write(Convert.ToUInt16(0x10)); // Writes Width
                for (int i = 0; i < 2; i++) write.Write((Byte)0x0); // Writes junk bytes
                write.Write(Convert.ToUInt16(0x10)); // Writes Heigth
                for (int i = 0; i < 2; i++) write.Write((Byte)0x0); // Writes junk bytes
                write.Write(Convert.ToUInt16(0x10)); // Writes Length
                for (int i = 0; i < 9; i++) write.Write((Byte)0x0); // Writes junk bytes
                for (int i = 0; i < (Form1.modelSize + Form1.terrainSize); i++)
                {
                    write.Write(size.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                Form1.buildingsSize = (Form1.buildingsSize + 0x30);
                write.BaseStream.Position = 0x4;
                write.Write((int)Form1.buildingsSize); // Writes new section size to header
                size.Close();
                write.Close();

                File.Delete(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4"));
                File.Move(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4") + "_new", Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4"));
            }
        }

        private void button2_Click(object sender, EventArgs e) // Remove
        {
            if (Form1.isBW || Form1.isB2W2)
            {
                amount--;
                System.IO.BinaryReader readMap = new System.IO.BinaryReader(File.OpenRead(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4")));
                File.Create(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4") + "_new").Close();
                System.IO.BinaryWriter writeMap = new System.IO.BinaryWriter(File.OpenWrite(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4") + "_new"));
                for (int i = 0; i < Form1.vbldOffset; i++)
                {
                    writeMap.Write(readMap.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                writeMap.Write(amount); // Writes new building count
                readMap.BaseStream.Position += 0x4;
                for (int i = 0; i < 0x10 * listBox1.SelectedIndex; i++)
                {
                    writeMap.Write(readMap.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                readMap.BaseStream.Position += 0x10;
                for (int i = 0; i < 0x10 * ((listBox1.Items.Count - 1) - listBox1.SelectedIndex); i++)
                {
                    writeMap.Write(readMap.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                writeMap.BaseStream.Position = Form1.voffsetPos;
                Form1.vendOffset = Form1.vbldOffset + 0x4 + (0x10 * amount);
                writeMap.Write(Form1.vendOffset);
                readMap.Close();
                writeMap.Close();

                File.Delete(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4"));
                File.Move(Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4") + "_new", Form1.workingFolder + @"data\a\0\0\maps" + "\\" + Form1.mapIndex.ToString("D4"));
                counter = 1;
                listBox1.Items.Clear();
                for (int i = 0; i < amount; i++) // Index recheck
                {
                    listBox1.Items.Add(rm.GetString("building") + counter); // Adds building to listbox
                    counter = counter + 1; // Increases index number
                }
                if (amount == 0)
                {
                    button2.Enabled = false;
                }
                button3.Enabled = false;
            }
            else
            {
                System.IO.BinaryReader size = new System.IO.BinaryReader(File.OpenRead(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4")));
                System.IO.FileStream newfile = File.Create(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4") + "_new");
                newfile.Close();
                System.IO.BinaryWriter write = new System.IO.BinaryWriter(File.OpenWrite(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4") + "_new"));
                write.BaseStream.Position = 0x0;
                size.BaseStream.Position = 0x0;
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    for (int i = 0; i < (0x10 + Form1.permissionSize + 0x30 * listBox1.SelectedIndex); i++)
                    {
                        write.Write(size.ReadByte()); // Reads unmodified bytes and writes them to the main file
                    }
                }
                else
                {
                    for (int i = 0; i < (0x14 + Form1.unknownSize + Form1.permissionSize + 0x30 * listBox1.SelectedIndex); i++)
                    {
                        write.Write(size.ReadByte()); // Reads unmodified bytes and writes them to the main file
                    }
                }
                if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
                {
                    size.BaseStream.Position = 0x10 + Form1.permissionSize + 0x30 * (listBox1.SelectedIndex + 1);
                }
                else
                {
                    size.BaseStream.Position = 0x14 + Form1.unknownSize + Form1.permissionSize + 0x30 * (listBox1.SelectedIndex + 1);
                }
                for (int i = 0; i < (Form1.modelSize + Form1.terrainSize + 0x30 * ((listBox1.Items.Count - 1) - listBox1.SelectedIndex)); i++)
                {
                    write.Write(size.ReadByte()); // Reads unmodified bytes and writes them to the main file
                }
                counter = 1;
                listBox1.Items.Clear();
                for (int i = 0; i < (Form1.buildingsSize / 0x30 - 1); i++) // Index recheck
                {
                    listBox1.Items.Add(rm.GetString("building") + counter); // Adds building to listbox
                    counter = counter + 1; // Increases index number
                }
                if (counter == 1)
                {
                    button2.Enabled = false;
                }
                button3.Enabled = false;
                write.BaseStream.Position = 0x4;
                write.Write((int)Form1.buildingsSize - 0x30); // Writes new section size to header
                Form1.buildingsSize = Form1.buildingsSize - 0x30;
                size.Close();
                write.Close();
                {
                    File.Delete(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4"));
                    File.Move(Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4") + "_new", Form1.mapFileName + "\\" + Form1.mapIndex.ToString("D4"));
                }
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (Form1.isBW || Form1.isB2W2)
            {
                if (gridMode == 0 && (numericUpDown2.Value > -18 && numericUpDown2.Value < 17) && (numericUpDown4.Value > -18 && numericUpDown4.Value < 17))
                {
                    dataGridView1.Rows[(Int16)(((int)(numericUpDown4.Value) ^ 0xFFFF) + 17)].Cells[Convert.ToInt32(numericUpDown2.Value + 17)].Selected = true;
                }
            }
            else
            {
                if (gridMode == 0 && (numericUpDown2.Value > -18 && numericUpDown2.Value < 17) && (numericUpDown4.Value > -18 && numericUpDown4.Value < 17))
                {
                    dataGridView1.Rows[Convert.ToInt32(numericUpDown4.Value + 17)].Cells[Convert.ToInt32(numericUpDown2.Value + 17)].Selected = true;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            gridMode = 1;
            numericUpDown2.Value = dataGridView1.CurrentCellAddress.X - 17;
            if (Form1.isBW || Form1.isB2W2)
            {
                numericUpDown4.Value = (Int16)((dataGridView1.CurrentCellAddress.Y - 17) ^ 0xFFFF);
            }
            else
            {
                numericUpDown4.Value = dataGridView1.CurrentCellAddress.Y - 17;
            }
            gridMode = 0;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Form1.isBW || Form1.isB2W2)
            {
                Form6_2_Building_List f6 = (Form6_2_Building_List)Application.OpenForms["Form6_2_Building_List"];
                f6.CloseForm();
            }
            else
            {
                Form6_Building_List f6 = (Form6_Building_List)Application.OpenForms["Form6_Building_List"];
                f6.CloseForm();
            }
        }

    }
}
