using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;
using System.Resources;
using System.Reflection;
using System.Media;

namespace WindowsFormsApplication1
{
    public partial class Form8 : Form
    {
        ResourceManager rm = new ResourceManager("WindowsFormsApplication1.WinFormStrings", Assembly.GetExecutingAssembly());
        public string soundPath = "";
        SoundPlayer playSSEQ = new SoundPlayer();
        int infoOffset;
        int dataOffset;
        public List<int> offsetList = new List<int>();
        public List<int> sizeList = new List<int>();
        public List<int> infoDataID = new List<int>();

        public Form8()
        {
            InitializeComponent();
        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.soundON = false;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            if (Form1.isBW == true)
            {
                soundPath = Form1.workingFolder + @"data\wb_sound_data.sdat";
            }
            else if (Form1.isB2W2 == true)
            {
                soundPath = Form1.workingFolder + @"data\swan_sound_data.sdat";
            }
            else if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                soundPath = Form1.workingFolder + @"data\data\sound\sound_data.sdat";
            }
            else if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                soundPath = Form1.workingFolder + @"data\data\sound\pl_sound_data.sdat";
            }
            else
            {
                soundPath = Form1.workingFolder + @"data\data\sound\gs_sound_data.sdat";
            }
            BinaryReader readSound = new BinaryReader(File.OpenRead(soundPath));
            readSound.BaseStream.Position = 0x18;
            infoOffset = (int)readSound.ReadUInt32();
            readSound.BaseStream.Position = 0x20;
            dataOffset = (int)readSound.ReadUInt32();
            readSound.BaseStream.Position = 0x80;
            int soundCount = (int)readSound.ReadUInt32();
            List<int> NameOffset = new List<int>();
            for (int i = 0; i < soundCount; i++)
            {
                NameOffset.Add((int)readSound.ReadUInt32());
            }
            for (int i = 0; i < soundCount; i++)
            {
                if (NameOffset[i] != 0x0)
                {
                    readSound.BaseStream.Position = NameOffset[i] + 0x40;
                    string soundName = "";
                    while (true)
                    {
                        int currentByte = readSound.ReadByte();
                        if (currentByte == 0x0)
                        {
                            break;
                        }
                        byte[] soundBytes = new Byte[] { Convert.ToByte(currentByte) }; // Reads sound name
                        soundName += Encoding.UTF8.GetString(soundBytes);
                    }
                    dataGridView1.Rows.Add(i, soundName);
                }
            }

            readSound.BaseStream.Position = infoOffset + 0x40;
            int infoCount = (int)readSound.ReadUInt32();
            List<int> infoDataOffset = new List<int>();
            for (int i = 0; i < infoCount; i++)
            {
                infoDataOffset.Add((int)readSound.ReadUInt32());
            }
            for (int i = 0; i < infoCount; i++)
            {
                if (infoDataOffset[i] != 0x0)
                {
                    readSound.BaseStream.Position = infoDataOffset[i] + infoOffset;
                    infoDataID.Add((int)readSound.ReadUInt32());
                }
                else
                {
                    infoDataID.Add(0);
                }
            }

            readSound.BaseStream.Position = dataOffset + 0x8;
            int fileAmount = (int)readSound.ReadUInt32();
            for (int i = 0; i < fileAmount; i++)
            {
                offsetList.Add((int)readSound.ReadUInt32());
                sizeList.Add((int)readSound.ReadUInt32());
                readSound.BaseStream.Position += 0x8;
            }
            readSound.Close();
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
        }

        private void button3_Click(object sender, EventArgs e) // Export SSEQ
        {
            SaveFileDialog export = new SaveFileDialog();
            export.Title = rm.GetString("exportSSEQ");
            export.Filter = rm.GetString("sseqFilter");
            export.FileName = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();
            if (export.ShowDialog() == DialogResult.OK)
            {
                int index = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value);
                System.IO.BinaryReader readSound = new System.IO.BinaryReader(File.OpenRead(soundPath));
                readSound.BaseStream.Position = offsetList[infoDataID[index]];
                System.IO.BinaryWriter exportSound = new System.IO.BinaryWriter(File.Create(export.FileName));
                for (int i = 0; i < sizeList[infoDataID[index]]; i++)
                {
                    exportSound.Write((byte)readSound.ReadByte());
                }
                readSound.Close();
                exportSound.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e) // Play SSEQ
        {
            SoundPlayer player = new SoundPlayer(@"C:\sound.mid");
            player.Play();
        }

        private void button2_Click(object sender, EventArgs e) // Stop SSEQ
        {
        }
    }
}
