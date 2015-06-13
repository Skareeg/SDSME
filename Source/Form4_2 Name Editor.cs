using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace WindowsFormsApplication1
{
    public partial class Form4_2 : Form
    {
        ResourceManager getChar = new ResourceManager("WindowsFormsApplication1.Resources.ReadText", Assembly.GetExecutingAssembly());

        public Form4_2()
        {
            InitializeComponent();
        }

        private void Form4_2_Load(object sender, EventArgs e)
        {
            #region Map Names
            string path;
            int mainKey = 31881;
            if (Form1.isBW)
            {
                path = "0089";
            }
            else if (Form1.isB2W2)
            {
                path = "0109";
            }
            else
            {
                loadGenIV();
                return;
            }
            System.IO.BinaryReader readText = new System.IO.BinaryReader(File.OpenRead(Form1.workingFolder + @"data\a\0\0\texts\" + path));
            int nameSections = readText.ReadUInt16();
            uint[] sectionOffset = new uint[3];
            uint[] sectionSize = new uint[3];
            int nameCount = readText.ReadUInt16();
            int stringOffset;
            int stringSize;
            int[] stringUnknown = new int[3];
            sectionSize[0] = readText.ReadUInt32();
            readText.ReadUInt32();
            int key;
            for (int i = 0; i < nameSections; i++)
            {
                sectionOffset[i] = readText.ReadUInt32();
            }
            for (int j = 0; j < nameCount; j++)
            {
                #region Layer 1
                readText.BaseStream.Position = sectionOffset[0];
                sectionSize[0] = readText.ReadUInt32();
                readText.BaseStream.Position += j * 8;
                stringOffset = (int)readText.ReadUInt32();
                stringSize = readText.ReadUInt16();
                stringUnknown[0] = readText.ReadUInt16();
                readText.BaseStream.Position = sectionOffset[0] + stringOffset;
                UInt16[] encodedString = new UInt16[stringSize];
                UInt16[] decodedString = new UInt16[stringSize];
                string pokemonText = "";
                key = mainKey;
                for (int k = 0; k < stringSize; k++)
                {
                    int car = Convert.ToUInt16(readText.ReadUInt16() ^ key);
                    if (car == 0xFFFF)
                    {
                    }
                    else if (car == 0xF100)
                    {
                        pokemonText += @"\xF100";
                    }
                    else if (car == 0xFFFE)
                    {
                        pokemonText += @"\n";
                    }
                    else if (car > 20 && car <= 0xFFF0 && car != 0xF000 && Char.GetUnicodeCategory(Convert.ToChar(car)) != UnicodeCategory.OtherNotAssigned)
                    {
                        pokemonText += Convert.ToChar(car);
                    }
                    else
                    {
                        pokemonText += @"\x" + car.ToString("X4");
                    }
                    key = ((key << 3) | (key >> 13)) & 0xFFFF;
                }
                #endregion
                comboBox1.Items.Add(pokemonText);
                mainKey += 0x2983;
                if (mainKey > 0xFFFF) mainKey -= 0x10000;
            }
            readText.Close();
            #endregion
            comboBox1.SelectedIndex = Form1.wildIndex;
        }

        private void loadGenIV()
        {
            #region Names
            string path;
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041)
            {
                path = Form1.workingFolder + @"data\msgdata\msg\0382";
            }
            else if (Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041)
            {
                path = Form1.workingFolder + @"data\msgdata\msg\0374";
            }
            else if (Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                path = Form1.workingFolder + @"data\msgdata\msg\0376";
            }
            else if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043)
            {
                path = Form1.workingFolder + @"data\msgdata\pl_msg\0433";
            }
            else if (Form1.gameID == 0x4A555043)
            {
                path = Form1.workingFolder + @"data\msgdata\pl_msg\0427";
            }
            else if (Form1.gameID == 0x4A555043)
            {
                path = Form1.workingFolder + @"data\msgdata\pl_msg\0428";
            }
            else if (Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049)
            {
                path = Form1.workingFolder + @"data\a\0\2\text\0279";
            }
            else if (Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049)
            {
                path = Form1.workingFolder + @"data\a\0\2\text\0272";
            }
            else
            {
                path = Form1.workingFolder + @"data\a\0\2\text\0274";
            }
            System.IO.BinaryReader readText = new System.IO.BinaryReader(File.OpenRead(path));
            int stringCount = (int)readText.ReadUInt16();
            int initialKey = (int)readText.ReadUInt16();
            int key1 = (initialKey * 0x2FD) & 0xFFFF;
            int key2 = 0;
            int realKey = 0;
            bool specialCharON = false;
            int[] currentOffset = new int[stringCount];
            int[] currentSize = new int[stringCount];
            string[] currentPokemon = new string[stringCount];
            int car = 0;
            for (int i = 0; i < stringCount; i++) // Reads and stores string offsets and sizes
            {
                key2 = (key1 * (i + 1) & 0xFFFF);
                realKey = key2 | (key2 << 16);
                currentOffset[i] = ((int)readText.ReadUInt32()) ^ realKey;
                currentSize[i] = ((int)readText.ReadUInt32()) ^ realKey;
            }
            for (int i = 0; i < stringCount; i++) // Adds new string
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
                comboBox1.Items.Add(pokemonText);
            }
            readText.Close();
            #endregion
            comboBox1.SelectedIndex = Form1.wildIndex;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.wildIndex = comboBox1.SelectedIndex;
            this.Close();
        }
    }
}
