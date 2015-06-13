using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Globalization;

namespace WindowsFormsApplication1
{
    public partial class Form9 : Form
    {
        ResourceManager rm = new ResourceManager("WindowsFormsApplication1.WinFormStrings", Assembly.GetExecutingAssembly());
        ResourceManager getChar = new ResourceManager("WindowsFormsApplication1.Resources.ReadText", Assembly.GetExecutingAssembly());

        public string textPath;
        public string wildPath;
        public string game;
        public bool editON = false;

        public Form9()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            if (Form1.isBW || Form1.isB2W2)
            {
                loadGenV();
                return;
            }
            #region Read Pokémon Names
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041)
            {
                textPath = Form1.workingFolder + @"data\msgdata\msg\0362";
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
            }
            if (Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041)
            {
                textPath = Form1.workingFolder + @"data\msgdata\msg\0356";
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
            }
            if (Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                textPath = Form1.workingFolder + @"data\msgdata\msg\0357";
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
            }
            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043)
            {
                textPath = Form1.workingFolder + @"data\msgdata\pl_msg\0412";
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
            }
            if (Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                textPath = Form1.workingFolder + @"data\msgdata\pl_msg\0408";
                tabControl1.TabPages.Remove(tabPage3);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
            }
            if (Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049)
            {
                textPath = Form1.workingFolder + @"data\a\0\2\text\0237";
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
            }
            if (Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
            {
                textPath = Form1.workingFolder + @"data\a\0\2\text\0232";
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Remove(tabPage4);
                tabControl1.TabPages.Remove(tabPage5);
            }
            System.IO.BinaryReader readText = new System.IO.BinaryReader(File.OpenRead(textPath));
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
                currentPokemon[i] = pokemonText;
            }
            readText.Close();
            #endregion
            #region Adds Pokémon to Lists
            for (int i = 0; i < stringCount; i++)
            {
                comboBox2.Items.Add(currentPokemon[i]);
                comboBox3.Items.Add(currentPokemon[i]);
                comboBox4.Items.Add(currentPokemon[i]);
                comboBox5.Items.Add(currentPokemon[i]);
                comboBox6.Items.Add(currentPokemon[i]);
                comboBox7.Items.Add(currentPokemon[i]);
                comboBox8.Items.Add(currentPokemon[i]);
                comboBox9.Items.Add(currentPokemon[i]);
                comboBox10.Items.Add(currentPokemon[i]);
                comboBox11.Items.Add(currentPokemon[i]);
                comboBox12.Items.Add(currentPokemon[i]);
                comboBox13.Items.Add(currentPokemon[i]);
                comboBox14.Items.Add(currentPokemon[i]);
                comboBox15.Items.Add(currentPokemon[i]);
                comboBox16.Items.Add(currentPokemon[i]);
                comboBox17.Items.Add(currentPokemon[i]);
                comboBox18.Items.Add(currentPokemon[i]);
                comboBox19.Items.Add(currentPokemon[i]);
                comboBox20.Items.Add(currentPokemon[i]);
                comboBox21.Items.Add(currentPokemon[i]);
                comboBox22.Items.Add(currentPokemon[i]);
                comboBox23.Items.Add(currentPokemon[i]);
                comboBox24.Items.Add(currentPokemon[i]);
                comboBox25.Items.Add(currentPokemon[i]);
                comboBox26.Items.Add(currentPokemon[i]);
                comboBox27.Items.Add(currentPokemon[i]);
                comboBox28.Items.Add(currentPokemon[i]);
                comboBox29.Items.Add(currentPokemon[i]);
                comboBox30.Items.Add(currentPokemon[i]);
                comboBox31.Items.Add(currentPokemon[i]);
                comboBox32.Items.Add(currentPokemon[i]);
                comboBox33.Items.Add(currentPokemon[i]);
                comboBox34.Items.Add(currentPokemon[i]);
                comboBox35.Items.Add(currentPokemon[i]);
                comboBox36.Items.Add(currentPokemon[i]);
                comboBox37.Items.Add(currentPokemon[i]);
                comboBox38.Items.Add(currentPokemon[i]);
                comboBox39.Items.Add(currentPokemon[i]);
                comboBox40.Items.Add(currentPokemon[i]);
                comboBox41.Items.Add(currentPokemon[i]);
                comboBox42.Items.Add(currentPokemon[i]);
                comboBox43.Items.Add(currentPokemon[i]);
                comboBox44.Items.Add(currentPokemon[i]);
                comboBox45.Items.Add(currentPokemon[i]);
                comboBox46.Items.Add(currentPokemon[i]);
                comboBox47.Items.Add(currentPokemon[i]);
                comboBox48.Items.Add(currentPokemon[i]);
                comboBox49.Items.Add(currentPokemon[i]);
                comboBox50.Items.Add(currentPokemon[i]);
                comboBox51.Items.Add(currentPokemon[i]);
                comboBox52.Items.Add(currentPokemon[i]);
                comboBox53.Items.Add(currentPokemon[i]);
                comboBox54.Items.Add(currentPokemon[i]);
                comboBox55.Items.Add(currentPokemon[i]);
                comboBox56.Items.Add(currentPokemon[i]);
                comboBox57.Items.Add(currentPokemon[i]);
                comboBox58.Items.Add(currentPokemon[i]);
                comboBox59.Items.Add(currentPokemon[i]);
                comboBox60.Items.Add(currentPokemon[i]);
                comboBox61.Items.Add(currentPokemon[i]);
                comboBox62.Items.Add(currentPokemon[i]);
                comboBox63.Items.Add(currentPokemon[i]);
                comboBox64.Items.Add(currentPokemon[i]);
                comboBox65.Items.Add(currentPokemon[i]);
                comboBox66.Items.Add(currentPokemon[i]);
                comboBox67.Items.Add(currentPokemon[i]);
                comboBox68.Items.Add(currentPokemon[i]);
                comboBox69.Items.Add(currentPokemon[i]);
                comboBox70.Items.Add(currentPokemon[i]);
                comboBox71.Items.Add(currentPokemon[i]);
                comboBox72.Items.Add(currentPokemon[i]);
                comboBox73.Items.Add(currentPokemon[i]);
                comboBox74.Items.Add(currentPokemon[i]);
                comboBox75.Items.Add(currentPokemon[i]);
                comboBox76.Items.Add(currentPokemon[i]);
                comboBox77.Items.Add(currentPokemon[i]);
                comboBox78.Items.Add(currentPokemon[i]);
                comboBox79.Items.Add(currentPokemon[i]);
                comboBox80.Items.Add(currentPokemon[i]);
                comboBox81.Items.Add(currentPokemon[i]);
                comboBox82.Items.Add(currentPokemon[i]);
                comboBox83.Items.Add(currentPokemon[i]);
                comboBox84.Items.Add(currentPokemon[i]);
                comboBox85.Items.Add(currentPokemon[i]);
                comboBox86.Items.Add(currentPokemon[i]);
                comboBox87.Items.Add(currentPokemon[i]);
                comboBox88.Items.Add(currentPokemon[i]);
                comboBox89.Items.Add(currentPokemon[i]);
                comboBox90.Items.Add(currentPokemon[i]);
                comboBox91.Items.Add(currentPokemon[i]);
                comboBox92.Items.Add(currentPokemon[i]);
                comboBox93.Items.Add(currentPokemon[i]);
                comboBox94.Items.Add(currentPokemon[i]);
                comboBox95.Items.Add(currentPokemon[i]);
                comboBox96.Items.Add(currentPokemon[i]);
                comboBox97.Items.Add(currentPokemon[i]);
                comboBox98.Items.Add(currentPokemon[i]);
                comboBox99.Items.Add(currentPokemon[i]);
            }
            #endregion

            checkBox1.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton3.Visible = false;
            radioButton4.Visible = false;
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x53414441 || Form1.gameID == 0x46414441 || Form1.gameID == 0x49414441 || Form1.gameID == 0x44414441 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4B414441)
            {
                game = "dppt";
                wildPath = Form1.workingFolder + @"data\fielddata\encountdata\d_enc_data";
                NarcAPI.Narc.Open(Form1.workingFolder + @"data\fielddata\encountdata\d_enc_data.narc").ExtractToFolder(Form1.workingFolder + @"data\fielddata\encountdata\d_enc_data");
            }
            if (Form1.gameID == 0x45415041 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44415041 ||  Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B415041)
            {
                game = "dppt";
                wildPath = Form1.workingFolder + @"data\fielddata\encountdata\p_enc_data";
                NarcAPI.Narc.Open(Form1.workingFolder + @"data\fielddata\encountdata\p_enc_data.narc").ExtractToFolder(Form1.workingFolder + @"data\fielddata\encountdata\p_enc_data");
            }
            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                game = "dppt";
                wildPath = Form1.workingFolder + @"data\fielddata\encountdata\pl_enc_data";
                NarcAPI.Narc.Open(Form1.workingFolder + @"data\fielddata\encountdata\pl_enc_data.narc").ExtractToFolder(Form1.workingFolder + @"data\fielddata\encountdata\pl_enc_data");
            }
            if (Form1.gameID == 0x454B5049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4B4B5049)
            {
                game = "hgss";
                wildPath = Form1.workingFolder + @"data\a\0\3\wild";
                NarcAPI.Narc.Open(Form1.workingFolder + @"data\a\0\3\7").ExtractToFolder(Form1.workingFolder + @"data\a\0\3\wild");
            }
            if (Form1.gameID == 0x45475049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B475049)
            {
                game = "hgss";
                wildPath = Form1.workingFolder + @"data\a\1\3\wild";
                NarcAPI.Narc.Open(Form1.workingFolder + @"data\a\1\3\6").ExtractToFolder(Form1.workingFolder + @"data\a\1\3\wild");
            }
            for (int i = 0; i < Directory.GetFiles(wildPath).Length; i++)
            {
                comboBox1.Items.Add(rm.GetString("wildPokemon") + i);
            }
            comboBox1.SelectedIndex = Form1.wildIndex;
        }

        private void loadGenV()
        {
            tabControl1.TabPages.Remove(tabPage1);
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
            if (Form1.isBW) NarcAPI.Narc.Open(Form1.workingFolder + @"data\a\1\2\6").ExtractToFolder(Form1.workingFolder + @"data\a\1\2\wild");
            else NarcAPI.Narc.Open(Form1.workingFolder + @"data\a\1\2\7").ExtractToFolder(Form1.workingFolder + @"data\a\1\2\wild");
            List<string> names = new List<string>();
            #region Pokémon Names
            string path;
            int mainKey = 31881;
            if (Form1.isBW)
            {
                path = "0070";
            }
            else
            {
                path = "0090";
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
                    else if (car == 0x246D)
                    {
                        pokemonText += "♂";
                    }
                    else if (car == 0x246E)
                    {
                        pokemonText += "♀";
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
                mainKey += 0x2983;
                if (mainKey > 0xFFFF) mainKey -= 0x10000;
                names.Add(pokemonText);
            }
            readText.Close();
            #endregion
            #region Add Names to Lists
            for (int i = 0; i < names.Count; i++)
            {
                comboBox100.Items.Add(names[i]);
                comboBox101.Items.Add(names[i]);
                comboBox102.Items.Add(names[i]);
                comboBox103.Items.Add(names[i]);
                comboBox104.Items.Add(names[i]);
                comboBox105.Items.Add(names[i]);
                comboBox106.Items.Add(names[i]);
                comboBox107.Items.Add(names[i]);
                comboBox108.Items.Add(names[i]);
                comboBox109.Items.Add(names[i]);
                comboBox110.Items.Add(names[i]);
                comboBox111.Items.Add(names[i]);
                comboBox112.Items.Add(names[i]);
                comboBox113.Items.Add(names[i]);
                comboBox114.Items.Add(names[i]);
                comboBox115.Items.Add(names[i]);
                comboBox116.Items.Add(names[i]);
                comboBox117.Items.Add(names[i]);
                comboBox118.Items.Add(names[i]);
                comboBox119.Items.Add(names[i]);
                comboBox120.Items.Add(names[i]);
                comboBox121.Items.Add(names[i]);
                comboBox122.Items.Add(names[i]);
                comboBox123.Items.Add(names[i]);
                comboBox124.Items.Add(names[i]);
                comboBox125.Items.Add(names[i]);
                comboBox126.Items.Add(names[i]);
                comboBox127.Items.Add(names[i]);
                comboBox128.Items.Add(names[i]);
                comboBox129.Items.Add(names[i]);
                comboBox130.Items.Add(names[i]);
                comboBox131.Items.Add(names[i]);
                comboBox132.Items.Add(names[i]);
                comboBox133.Items.Add(names[i]);
                comboBox134.Items.Add(names[i]);
                comboBox135.Items.Add(names[i]);
                comboBox136.Items.Add(names[i]);
                comboBox137.Items.Add(names[i]);
                comboBox138.Items.Add(names[i]);
                comboBox139.Items.Add(names[i]);
                comboBox140.Items.Add(names[i]);
                comboBox141.Items.Add(names[i]);
                comboBox142.Items.Add(names[i]);
                comboBox143.Items.Add(names[i]);
                comboBox144.Items.Add(names[i]);
                comboBox145.Items.Add(names[i]);
                comboBox146.Items.Add(names[i]);
                comboBox147.Items.Add(names[i]);
                comboBox148.Items.Add(names[i]);
                comboBox149.Items.Add(names[i]);
                comboBox150.Items.Add(names[i]);
                comboBox151.Items.Add(names[i]);
                comboBox152.Items.Add(names[i]);
                comboBox153.Items.Add(names[i]);
                comboBox154.Items.Add(names[i]);
                comboBox155.Items.Add(names[i]);
            }
            #endregion
            for (int i = 0; i < Directory.GetFiles(Form1.workingFolder + @"data\a\1\2\wild").Length; i++)
            {
                comboBox1.Items.Add(rm.GetString("wildPokemon") + i);
            }
            comboBox1.SelectedIndex = Form1.wildIndex;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                button1.Enabled = true;
                #region Gen V
                if (Form1.isBW || Form1.isB2W2)
                {
                    editON = false;
                    int pokemon1;
                    System.IO.BinaryReader readWild = new System.IO.BinaryReader(File.OpenRead(Form1.workingFolder + @"data\a\1\2\wild" + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                    if (readWild.BaseStream.Length == 0xE8)
                    {
                        checkBox1.Checked = false;
                        radioButton1.Enabled = false;
                        radioButton2.Enabled = false;
                        radioButton3.Enabled = false;
                        radioButton4.Enabled = false;
                        radioButton1.Checked = true;
                    }
                    else checkBox1.Checked = true;
                    if (checkBox1.Checked)
                    {
                        if (radioButton2.Checked) readWild.BaseStream.Position = 0xE8;
                        if (radioButton3.Checked) readWild.BaseStream.Position = 0x1D0;
                        if (radioButton4.Checked) readWild.BaseStream.Position = 0x2B8;
                    }
                    numericUpDown244.Value = readWild.ReadByte();
                    numericUpDown245.Value = readWild.ReadByte();
                    numericUpDown246.Value = readWild.ReadByte();
                    numericUpDown247.Value = readWild.ReadByte();
                    numericUpDown248.Value = readWild.ReadByte();
                    numericUpDown249.Value = readWild.ReadByte();
                    numericUpDown250.Value = readWild.ReadByte();
                    readWild.BaseStream.Position++;
                    // Standard
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown188.Value = pokemon1 / 2048;
                    comboBox100.SelectedIndex = pokemon1 - ((int)numericUpDown188.Value * 2048);
                    numericUpDown76.Value = readWild.ReadByte();
                    numericUpDown77.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown189.Value = pokemon1 / 2048;
                    comboBox101.SelectedIndex = pokemon1 - ((int)numericUpDown189.Value * 2048);
                    numericUpDown78.Value = readWild.ReadByte();
                    numericUpDown79.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown190.Value = pokemon1 / 2048;
                    comboBox102.SelectedIndex = pokemon1 - ((int)numericUpDown190.Value * 2048);
                    numericUpDown80.Value = readWild.ReadByte();
                    numericUpDown81.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown191.Value = pokemon1 / 2048;
                    comboBox103.SelectedIndex = pokemon1 - ((int)numericUpDown191.Value * 2048);
                    numericUpDown82.Value = readWild.ReadByte();
                    numericUpDown83.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown192.Value = pokemon1 / 2048;
                    comboBox104.SelectedIndex = pokemon1 - ((int)numericUpDown192.Value * 2048);
                    numericUpDown84.Value = readWild.ReadByte();
                    numericUpDown85.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown193.Value = pokemon1 / 2048;
                    comboBox105.SelectedIndex = pokemon1 - ((int)numericUpDown193.Value * 2048);
                    numericUpDown86.Value = readWild.ReadByte();
                    numericUpDown87.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown194.Value = pokemon1 / 2048;
                    comboBox106.SelectedIndex = pokemon1 - ((int)numericUpDown194.Value * 2048);
                    numericUpDown88.Value = readWild.ReadByte();
                    numericUpDown89.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown195.Value = pokemon1 / 2048;
                    comboBox107.SelectedIndex = pokemon1 - ((int)numericUpDown195.Value * 2048);
                    numericUpDown90.Value = readWild.ReadByte();
                    numericUpDown91.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown196.Value = pokemon1 / 2048;
                    comboBox108.SelectedIndex = pokemon1 - ((int)numericUpDown196.Value * 2048);
                    numericUpDown92.Value = readWild.ReadByte();
                    numericUpDown93.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown197.Value = pokemon1 / 2048;
                    comboBox109.SelectedIndex = pokemon1 - ((int)numericUpDown197.Value * 2048);
                    numericUpDown94.Value = readWild.ReadByte();
                    numericUpDown95.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown198.Value = pokemon1 / 2048;
                    comboBox110.SelectedIndex = pokemon1 - ((int)numericUpDown198.Value * 2048);
                    numericUpDown96.Value = readWild.ReadByte();
                    numericUpDown97.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown199.Value = pokemon1 / 2048;
                    comboBox111.SelectedIndex = pokemon1 - ((int)numericUpDown199.Value * 2048);
                    numericUpDown98.Value = readWild.ReadByte();
                    numericUpDown99.Value = readWild.ReadByte();
                    // Double
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown200.Value = pokemon1 / 2048;
                    comboBox123.SelectedIndex = pokemon1 - ((int)numericUpDown200.Value * 2048);
                    numericUpDown123.Value = readWild.ReadByte();
                    numericUpDown122.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown201.Value = pokemon1 / 2048;
                    comboBox122.SelectedIndex = pokemon1 - ((int)numericUpDown201.Value * 2048);
                    numericUpDown121.Value = readWild.ReadByte();
                    numericUpDown120.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown202.Value = pokemon1 / 2048;
                    comboBox121.SelectedIndex = pokemon1 - ((int)numericUpDown202.Value * 2048);
                    numericUpDown119.Value = readWild.ReadByte();
                    numericUpDown118.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown203.Value = pokemon1 / 2048;
                    comboBox120.SelectedIndex = pokemon1 - ((int)numericUpDown203.Value * 2048);
                    numericUpDown117.Value = readWild.ReadByte();
                    numericUpDown116.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown204.Value = pokemon1 / 2048;
                    comboBox119.SelectedIndex = pokemon1 - ((int)numericUpDown204.Value * 2048);
                    numericUpDown115.Value = readWild.ReadByte();
                    numericUpDown114.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown205.Value = pokemon1 / 2048;
                    comboBox118.SelectedIndex = pokemon1 - ((int)numericUpDown205.Value * 2048);
                    numericUpDown113.Value = readWild.ReadByte();
                    numericUpDown112.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown206.Value = pokemon1 / 2048;
                    comboBox117.SelectedIndex = pokemon1 - ((int)numericUpDown206.Value * 2048);
                    numericUpDown111.Value = readWild.ReadByte();
                    numericUpDown110.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown207.Value = pokemon1 / 2048;
                    comboBox116.SelectedIndex = pokemon1 - ((int)numericUpDown207.Value * 2048);
                    numericUpDown109.Value = readWild.ReadByte();
                    numericUpDown108.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown208.Value = pokemon1 / 2048;
                    comboBox115.SelectedIndex = pokemon1 - ((int)numericUpDown208.Value * 2048);
                    numericUpDown107.Value = readWild.ReadByte();
                    numericUpDown106.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown209.Value = pokemon1 / 2048;
                    comboBox114.SelectedIndex = pokemon1 - ((int)numericUpDown209.Value * 2048);
                    numericUpDown105.Value = readWild.ReadByte();
                    numericUpDown104.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown210.Value = pokemon1 / 2048;
                    comboBox113.SelectedIndex = pokemon1 - ((int)numericUpDown210.Value * 2048);
                    numericUpDown103.Value = readWild.ReadByte();
                    numericUpDown102.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown211.Value = pokemon1 / 2048;
                    comboBox112.SelectedIndex = pokemon1 - ((int)numericUpDown211.Value * 2048);
                    numericUpDown101.Value = readWild.ReadByte();
                    numericUpDown100.Value = readWild.ReadByte();
                    // Special
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown212.Value = pokemon1 / 2048;
                    comboBox135.SelectedIndex = pokemon1 - ((int)numericUpDown212.Value * 2048);
                    numericUpDown147.Value = readWild.ReadByte();
                    numericUpDown146.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown213.Value = pokemon1 / 2048;
                    comboBox134.SelectedIndex = pokemon1 - ((int)numericUpDown213.Value * 2048);
                    numericUpDown145.Value = readWild.ReadByte();
                    numericUpDown144.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown214.Value = pokemon1 / 2048;
                    comboBox133.SelectedIndex = pokemon1 - ((int)numericUpDown214.Value * 2048);
                    numericUpDown143.Value = readWild.ReadByte();
                    numericUpDown142.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown215.Value = pokemon1 / 2048;
                    comboBox132.SelectedIndex = pokemon1 - ((int)numericUpDown215.Value * 2048);
                    numericUpDown141.Value = readWild.ReadByte();
                    numericUpDown140.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown216.Value = pokemon1 / 2048;
                    comboBox131.SelectedIndex = pokemon1 - ((int)numericUpDown216.Value * 2048);
                    numericUpDown139.Value = readWild.ReadByte();
                    numericUpDown138.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown217.Value = pokemon1 / 2048;
                    comboBox130.SelectedIndex = pokemon1 - ((int)numericUpDown217.Value * 2048);
                    numericUpDown137.Value = readWild.ReadByte();
                    numericUpDown136.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown218.Value = pokemon1 / 2048;
                    comboBox129.SelectedIndex = pokemon1 - ((int)numericUpDown218.Value * 2048);
                    numericUpDown135.Value = readWild.ReadByte();
                    numericUpDown134.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown219.Value = pokemon1 / 2048;
                    comboBox128.SelectedIndex = pokemon1 - ((int)numericUpDown219.Value * 2048);
                    numericUpDown133.Value = readWild.ReadByte();
                    numericUpDown132.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown220.Value = pokemon1 / 2048;
                    comboBox127.SelectedIndex = pokemon1 - ((int)numericUpDown220.Value * 2048);
                    numericUpDown131.Value = readWild.ReadByte();
                    numericUpDown130.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown221.Value = pokemon1 / 2048;
                    comboBox126.SelectedIndex = pokemon1 - ((int)numericUpDown221.Value * 2048);
                    numericUpDown129.Value = readWild.ReadByte();
                    numericUpDown128.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown222.Value = pokemon1 / 2048;
                    comboBox125.SelectedIndex = pokemon1 - ((int)numericUpDown222.Value * 2048);
                    numericUpDown127.Value = readWild.ReadByte();
                    numericUpDown126.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown223.Value = pokemon1 / 2048;
                    comboBox124.SelectedIndex = pokemon1 - ((int)numericUpDown223.Value * 2048);
                    numericUpDown125.Value = readWild.ReadByte();
                    numericUpDown124.Value = readWild.ReadByte();
                    // Surf
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown224.Value = pokemon1 / 2048;
                    comboBox136.SelectedIndex = pokemon1 - ((int)numericUpDown224.Value * 2048);
                    numericUpDown148.Value = readWild.ReadByte();
                    numericUpDown149.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown225.Value = pokemon1 / 2048;
                    comboBox137.SelectedIndex = pokemon1 - ((int)numericUpDown225.Value * 2048);
                    numericUpDown150.Value = readWild.ReadByte();
                    numericUpDown151.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown226.Value = pokemon1 / 2048;
                    comboBox138.SelectedIndex = pokemon1 - ((int)numericUpDown226.Value * 2048);
                    numericUpDown152.Value = readWild.ReadByte();
                    numericUpDown153.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown227.Value = pokemon1 / 2048;
                    comboBox139.SelectedIndex = pokemon1 - ((int)numericUpDown227.Value * 2048);
                    numericUpDown154.Value = readWild.ReadByte();
                    numericUpDown155.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown228.Value = pokemon1 / 2048;
                    comboBox140.SelectedIndex = pokemon1 - ((int)numericUpDown228.Value * 2048);
                    numericUpDown156.Value = readWild.ReadByte();
                    numericUpDown157.Value = readWild.ReadByte();
                    // Surf Special
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown229.Value = pokemon1 / 2048;
                    comboBox145.SelectedIndex = pokemon1 - ((int)numericUpDown229.Value * 2048);
                    numericUpDown167.Value = readWild.ReadByte();
                    numericUpDown166.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown230.Value = pokemon1 / 2048;
                    comboBox144.SelectedIndex = pokemon1 - ((int)numericUpDown230.Value * 2048);
                    numericUpDown165.Value = readWild.ReadByte();
                    numericUpDown164.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown231.Value = pokemon1 / 2048;
                    comboBox143.SelectedIndex = pokemon1 - ((int)numericUpDown231.Value * 2048);
                    numericUpDown163.Value = readWild.ReadByte();
                    numericUpDown162.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown232.Value = pokemon1 / 2048;
                    comboBox142.SelectedIndex = pokemon1 - ((int)numericUpDown232.Value * 2048);
                    numericUpDown161.Value = readWild.ReadByte();
                    numericUpDown160.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown233.Value = pokemon1 / 2048;
                    comboBox141.SelectedIndex = pokemon1 - ((int)numericUpDown233.Value * 2048);
                    numericUpDown159.Value = readWild.ReadByte();
                    numericUpDown158.Value = readWild.ReadByte();
                    // Super Rod
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown234.Value = pokemon1 / 2048;
                    comboBox150.SelectedIndex = pokemon1 - ((int)numericUpDown234.Value * 2048);
                    numericUpDown177.Value = readWild.ReadByte();
                    numericUpDown176.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown235.Value = pokemon1 / 2048;
                    comboBox149.SelectedIndex = pokemon1 - ((int)numericUpDown235.Value * 2048);
                    numericUpDown175.Value = readWild.ReadByte();
                    numericUpDown174.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown236.Value = pokemon1 / 2048;
                    comboBox148.SelectedIndex = pokemon1 - ((int)numericUpDown236.Value * 2048);
                    numericUpDown173.Value = readWild.ReadByte();
                    numericUpDown172.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown237.Value = pokemon1 / 2048;
                    comboBox147.SelectedIndex = pokemon1 - ((int)numericUpDown237.Value * 2048);
                    numericUpDown171.Value = readWild.ReadByte();
                    numericUpDown170.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown238.Value = pokemon1 / 2048;
                    comboBox146.SelectedIndex = pokemon1 - ((int)numericUpDown238.Value * 2048);
                    numericUpDown169.Value = readWild.ReadByte();
                    numericUpDown168.Value = readWild.ReadByte();
                    // Super Rod Special
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown239.Value = pokemon1 / 2048;
                    comboBox155.SelectedIndex = pokemon1 - ((int)numericUpDown239.Value * 2048);
                    numericUpDown187.Value = readWild.ReadByte();
                    numericUpDown186.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown240.Value = pokemon1 / 2048;
                    comboBox154.SelectedIndex = pokemon1 - ((int)numericUpDown240.Value * 2048);
                    numericUpDown185.Value = readWild.ReadByte();
                    numericUpDown184.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown241.Value = pokemon1 / 2048;
                    comboBox153.SelectedIndex = pokemon1 - ((int)numericUpDown241.Value * 2048);
                    numericUpDown183.Value = readWild.ReadByte();
                    numericUpDown182.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown242.Value = pokemon1 / 2048;
                    comboBox152.SelectedIndex = pokemon1 - ((int)numericUpDown242.Value * 2048);
                    numericUpDown181.Value = readWild.ReadByte();
                    numericUpDown180.Value = readWild.ReadByte();
                    pokemon1 = readWild.ReadUInt16();
                    numericUpDown243.Value = pokemon1 / 2048;
                    comboBox151.SelectedIndex = pokemon1 - ((int)numericUpDown243.Value * 2048);
                    numericUpDown179.Value = readWild.ReadByte();
                    numericUpDown178.Value = readWild.ReadByte();
                    readWild.Close();
                    editON = true;
                }
                #endregion
                #region DPPt
                if (game == "dppt")
                {
                    System.IO.BinaryReader readWild = new System.IO.BinaryReader(File.OpenRead(wildPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                    numericUpDown13.Value = readWild.ReadUInt32();
                    numericUpDown1.Value = readWild.ReadUInt32();
                    comboBox2.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown2.Value = readWild.ReadUInt32();
                    comboBox3.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown3.Value = readWild.ReadUInt32();
                    comboBox4.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown4.Value = readWild.ReadUInt32();
                    comboBox5.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown5.Value = readWild.ReadUInt32();
                    comboBox6.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown6.Value = readWild.ReadUInt32();
                    comboBox7.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown7.Value = readWild.ReadUInt32();
                    comboBox8.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown8.Value = readWild.ReadUInt32();
                    comboBox9.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown9.Value = readWild.ReadUInt32();
                    comboBox10.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown10.Value = readWild.ReadUInt32();
                    comboBox11.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown11.Value = readWild.ReadUInt32();
                    comboBox12.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown12.Value = readWild.ReadUInt32();
                    comboBox13.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox14.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox15.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox16.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox17.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox18.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox19.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox20.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox21.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox22.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox23.SelectedIndex = (int)readWild.ReadUInt32();
                    readWild.BaseStream.Position = 0xA4;
                    comboBox24.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox25.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox26.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox27.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox28.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox29.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox30.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox31.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox32.SelectedIndex = (int)readWild.ReadUInt32();
                    comboBox33.SelectedIndex = (int)readWild.ReadUInt32();
                    // Surfing
                    numericUpDown24.Value = readWild.ReadUInt32();
                    numericUpDown15.Value = readWild.ReadByte();
                    numericUpDown14.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox34.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown16.Value = readWild.ReadByte();
                    numericUpDown17.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox35.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown18.Value = readWild.ReadByte();
                    numericUpDown19.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox36.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown20.Value = readWild.ReadByte();
                    numericUpDown21.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox37.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown22.Value = readWild.ReadByte();
                    numericUpDown23.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox38.SelectedIndex = (int)readWild.ReadUInt32();
                    readWild.BaseStream.Position = 0x124;
                    // Old Rod
                    numericUpDown25.Value = readWild.ReadUInt32();
                    numericUpDown34.Value = readWild.ReadByte();
                    numericUpDown35.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox43.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown32.Value = readWild.ReadByte();
                    numericUpDown33.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox42.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown30.Value = readWild.ReadByte();
                    numericUpDown31.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox41.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown28.Value = readWild.ReadByte();
                    numericUpDown29.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox40.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown26.Value = readWild.ReadByte();
                    numericUpDown27.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox39.SelectedIndex = (int)readWild.ReadUInt32();
                    // Good Rod
                    numericUpDown36.Value = readWild.ReadUInt32();
                    numericUpDown45.Value = readWild.ReadByte();
                    numericUpDown46.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox48.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown43.Value = readWild.ReadByte();
                    numericUpDown44.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox47.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown41.Value = readWild.ReadByte();
                    numericUpDown42.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox46.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown39.Value = readWild.ReadByte();
                    numericUpDown40.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox45.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown37.Value = readWild.ReadByte();
                    numericUpDown38.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox44.SelectedIndex = (int)readWild.ReadUInt32();
                    // Super Rod
                    numericUpDown47.Value = readWild.ReadUInt32();
                    numericUpDown56.Value = readWild.ReadByte();
                    numericUpDown57.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox53.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown54.Value = readWild.ReadByte();
                    numericUpDown55.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox52.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown52.Value = readWild.ReadByte();
                    numericUpDown53.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox51.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown50.Value = readWild.ReadByte();
                    numericUpDown51.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox50.SelectedIndex = (int)readWild.ReadUInt32();
                    numericUpDown48.Value = readWild.ReadByte();
                    numericUpDown49.Value = readWild.ReadByte();
                    readWild.BaseStream.Position += 0x2;
                    comboBox49.SelectedIndex = (int)readWild.ReadUInt32();
                    readWild.Close();
                }
                #endregion
                #region HGSS
                if (game == "hgss")
                {
                    System.IO.BinaryReader readWild = new System.IO.BinaryReader(File.OpenRead(wildPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                    // Rates
                    numericUpDown58.Value = readWild.ReadByte();
                    numericUpDown24.Value = readWild.ReadByte();
                    numericUpDown71.Value = readWild.ReadByte();
                    numericUpDown25.Value = readWild.ReadByte();
                    numericUpDown36.Value = readWild.ReadByte();
                    numericUpDown47.Value = readWild.ReadByte();
                    // Walking
                    readWild.BaseStream.Position = 0x8;
                    numericUpDown70.Value = readWild.ReadByte();
                    numericUpDown69.Value = readWild.ReadByte();
                    numericUpDown68.Value = readWild.ReadByte();
                    numericUpDown67.Value = readWild.ReadByte();
                    numericUpDown66.Value = readWild.ReadByte();
                    numericUpDown65.Value = readWild.ReadByte();
                    numericUpDown64.Value = readWild.ReadByte();
                    numericUpDown63.Value = readWild.ReadByte();
                    numericUpDown62.Value = readWild.ReadByte();
                    numericUpDown61.Value = readWild.ReadByte();
                    numericUpDown60.Value = readWild.ReadByte();
                    numericUpDown59.Value = readWild.ReadByte();
                    comboBox85.SelectedIndex = readWild.ReadUInt16();
                    comboBox84.SelectedIndex = readWild.ReadUInt16();
                    comboBox83.SelectedIndex = readWild.ReadUInt16();
                    comboBox82.SelectedIndex = readWild.ReadUInt16();
                    comboBox81.SelectedIndex = readWild.ReadUInt16();
                    comboBox80.SelectedIndex = readWild.ReadUInt16();
                    comboBox79.SelectedIndex = readWild.ReadUInt16();
                    comboBox78.SelectedIndex = readWild.ReadUInt16();
                    comboBox77.SelectedIndex = readWild.ReadUInt16();
                    comboBox76.SelectedIndex = readWild.ReadUInt16();
                    comboBox75.SelectedIndex = readWild.ReadUInt16();
                    comboBox74.SelectedIndex = readWild.ReadUInt16();
                    comboBox69.SelectedIndex = readWild.ReadUInt16();
                    comboBox68.SelectedIndex = readWild.ReadUInt16();
                    comboBox67.SelectedIndex = readWild.ReadUInt16();
                    comboBox66.SelectedIndex = readWild.ReadUInt16();
                    comboBox65.SelectedIndex = readWild.ReadUInt16();
                    comboBox64.SelectedIndex = readWild.ReadUInt16();
                    comboBox63.SelectedIndex = readWild.ReadUInt16();
                    comboBox62.SelectedIndex = readWild.ReadUInt16();
                    comboBox61.SelectedIndex = readWild.ReadUInt16();
                    comboBox60.SelectedIndex = readWild.ReadUInt16();
                    comboBox59.SelectedIndex = readWild.ReadUInt16();
                    comboBox58.SelectedIndex = readWild.ReadUInt16();
                    comboBox93.SelectedIndex = readWild.ReadUInt16();
                    comboBox92.SelectedIndex = readWild.ReadUInt16();
                    comboBox91.SelectedIndex = readWild.ReadUInt16();
                    comboBox90.SelectedIndex = readWild.ReadUInt16();
                    comboBox89.SelectedIndex = readWild.ReadUInt16();
                    comboBox88.SelectedIndex = readWild.ReadUInt16();
                    comboBox87.SelectedIndex = readWild.ReadUInt16();
                    comboBox86.SelectedIndex = readWild.ReadUInt16();
                    comboBox73.SelectedIndex = readWild.ReadUInt16();
                    comboBox72.SelectedIndex = readWild.ReadUInt16();
                    comboBox71.SelectedIndex = readWild.ReadUInt16();
                    comboBox70.SelectedIndex = readWild.ReadUInt16();
                    // Hoenn Sound
                    comboBox94.SelectedIndex = readWild.ReadUInt16();
                    comboBox95.SelectedIndex = readWild.ReadUInt16();
                    // Sinnoh Sound
                    comboBox96.SelectedIndex = readWild.ReadUInt16();
                    comboBox97.SelectedIndex = readWild.ReadUInt16();
                    // Surfing
                    numericUpDown14.Value = readWild.ReadByte();
                    numericUpDown15.Value = readWild.ReadByte();
                    comboBox34.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown17.Value = readWild.ReadByte();
                    numericUpDown16.Value = readWild.ReadByte();
                    comboBox35.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown19.Value = readWild.ReadByte();
                    numericUpDown18.Value = readWild.ReadByte();
                    comboBox36.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown21.Value = readWild.ReadByte();
                    numericUpDown20.Value = readWild.ReadByte();
                    comboBox37.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown23.Value = readWild.ReadByte();
                    numericUpDown24.Value = readWild.ReadByte();
                    comboBox38.SelectedIndex = readWild.ReadUInt16();
                    // Rock Smash
                    numericUpDown75.Value = readWild.ReadByte();
                    numericUpDown74.Value = readWild.ReadByte();
                    comboBox98.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown73.Value = readWild.ReadByte();
                    numericUpDown72.Value = readWild.ReadByte();
                    comboBox99.SelectedIndex = readWild.ReadUInt16();
                    // Old Rod
                    numericUpDown35.Value = readWild.ReadByte();
                    numericUpDown34.Value = readWild.ReadByte();
                    comboBox43.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown33.Value = readWild.ReadByte();
                    numericUpDown32.Value = readWild.ReadByte();
                    comboBox42.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown31.Value = readWild.ReadByte();
                    numericUpDown30.Value = readWild.ReadByte();
                    comboBox41.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown29.Value = readWild.ReadByte();
                    numericUpDown28.Value = readWild.ReadByte();
                    comboBox40.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown27.Value = readWild.ReadByte();
                    numericUpDown26.Value = readWild.ReadByte();
                    comboBox39.SelectedIndex = readWild.ReadUInt16();
                    // Good Rod
                    numericUpDown46.Value = readWild.ReadByte();
                    numericUpDown45.Value = readWild.ReadByte();
                    comboBox48.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown44.Value = readWild.ReadByte();
                    numericUpDown43.Value = readWild.ReadByte();
                    comboBox47.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown42.Value = readWild.ReadByte();
                    numericUpDown41.Value = readWild.ReadByte();
                    comboBox46.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown40.Value = readWild.ReadByte();
                    numericUpDown39.Value = readWild.ReadByte();
                    comboBox45.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown38.Value = readWild.ReadByte();
                    numericUpDown37.Value = readWild.ReadByte();
                    comboBox44.SelectedIndex = readWild.ReadUInt16();
                    // Super Rod
                    numericUpDown57.Value = readWild.ReadByte();
                    numericUpDown56.Value = readWild.ReadByte();
                    comboBox53.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown55.Value = readWild.ReadByte();
                    numericUpDown54.Value = readWild.ReadByte();
                    comboBox52.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown53.Value = readWild.ReadByte();
                    numericUpDown52.Value = readWild.ReadByte();
                    comboBox51.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown51.Value = readWild.ReadByte();
                    numericUpDown50.Value = readWild.ReadByte();
                    comboBox50.SelectedIndex = readWild.ReadUInt16();
                    numericUpDown49.Value = readWild.ReadByte();
                    numericUpDown48.Value = readWild.ReadByte();
                    comboBox49.SelectedIndex = readWild.ReadUInt16();
                    // Swarms
                    comboBox57.SelectedIndex = readWild.ReadUInt16();
                    comboBox56.SelectedIndex = readWild.ReadUInt16();
                    comboBox55.SelectedIndex = readWild.ReadUInt16();
                    comboBox54.SelectedIndex = readWild.ReadUInt16();
                    readWild.Close();
                }
                #endregion
            }
        }

        private void button1_Click(object sender, EventArgs e) // Writes Wild Pokémon
        {
            #region Gen V
            if (Form1.isBW || Form1.isB2W2)
            {
                System.IO.BinaryWriter writeWild = new System.IO.BinaryWriter(File.OpenWrite(Form1.workingFolder + @"data\a\1\2\wild" + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                if (checkBox1.Checked)
                {
                    if (radioButton2.Checked) writeWild.BaseStream.Position = 0xE8;
                    if (radioButton3.Checked) writeWild.BaseStream.Position = 0x1D0;
                    if (radioButton4.Checked) writeWild.BaseStream.Position = 0x2B8;
                }
                writeWild.Write((Byte)numericUpDown244.Value);
                writeWild.Write((Byte)numericUpDown245.Value);
                writeWild.Write((Byte)numericUpDown246.Value);
                writeWild.Write((Byte)numericUpDown247.Value);
                writeWild.Write((Byte)numericUpDown248.Value);
                writeWild.Write((Byte)numericUpDown249.Value);
                writeWild.Write((Byte)numericUpDown250.Value);
                writeWild.Write((Byte)0);
                // Standard
                writeWild.Write((UInt16)(comboBox100.SelectedIndex + (numericUpDown188.Value * 2048)));
                writeWild.Write((Byte)numericUpDown76.Value);
                writeWild.Write((Byte)numericUpDown77.Value);
                writeWild.Write((UInt16)(comboBox101.SelectedIndex + (numericUpDown189.Value * 2048)));
                writeWild.Write((Byte)numericUpDown78.Value);
                writeWild.Write((Byte)numericUpDown79.Value);
                writeWild.Write((UInt16)(comboBox102.SelectedIndex + (numericUpDown190.Value * 2048)));
                writeWild.Write((Byte)numericUpDown80.Value);
                writeWild.Write((Byte)numericUpDown81.Value);
                writeWild.Write((UInt16)(comboBox103.SelectedIndex + (numericUpDown191.Value * 2048)));
                writeWild.Write((Byte)numericUpDown82.Value);
                writeWild.Write((Byte)numericUpDown83.Value);
                writeWild.Write((UInt16)(comboBox104.SelectedIndex + (numericUpDown192.Value * 2048)));
                writeWild.Write((Byte)numericUpDown84.Value);
                writeWild.Write((Byte)numericUpDown85.Value);
                writeWild.Write((UInt16)(comboBox105.SelectedIndex + (numericUpDown193.Value * 2048)));
                writeWild.Write((Byte)numericUpDown86.Value);
                writeWild.Write((Byte)numericUpDown87.Value);
                writeWild.Write((UInt16)(comboBox106.SelectedIndex + (numericUpDown194.Value * 2048)));
                writeWild.Write((Byte)numericUpDown88.Value);
                writeWild.Write((Byte)numericUpDown89.Value);
                writeWild.Write((UInt16)(comboBox107.SelectedIndex + (numericUpDown195.Value * 2048)));
                writeWild.Write((Byte)numericUpDown90.Value);
                writeWild.Write((Byte)numericUpDown91.Value);
                writeWild.Write((UInt16)(comboBox108.SelectedIndex + (numericUpDown196.Value * 2048)));
                writeWild.Write((Byte)numericUpDown92.Value);
                writeWild.Write((Byte)numericUpDown93.Value);
                writeWild.Write((UInt16)(comboBox109.SelectedIndex + (numericUpDown197.Value * 2048)));
                writeWild.Write((Byte)numericUpDown94.Value);
                writeWild.Write((Byte)numericUpDown95.Value);
                writeWild.Write((UInt16)(comboBox110.SelectedIndex + (numericUpDown198.Value * 2048)));
                writeWild.Write((Byte)numericUpDown96.Value);
                writeWild.Write((Byte)numericUpDown97.Value);
                writeWild.Write((UInt16)(comboBox111.SelectedIndex + (numericUpDown199.Value * 2048)));
                writeWild.Write((Byte)numericUpDown98.Value);
                writeWild.Write((Byte)numericUpDown99.Value);
                // Double
                writeWild.Write((UInt16)(comboBox123.SelectedIndex + (numericUpDown200.Value * 2048)));
                writeWild.Write((Byte)numericUpDown123.Value);
                writeWild.Write((Byte)numericUpDown122.Value);
                writeWild.Write((UInt16)(comboBox122.SelectedIndex + (numericUpDown201.Value * 2048)));
                writeWild.Write((Byte)numericUpDown121.Value);
                writeWild.Write((Byte)numericUpDown120.Value);
                writeWild.Write((UInt16)(comboBox121.SelectedIndex + (numericUpDown202.Value * 2048)));
                writeWild.Write((Byte)numericUpDown119.Value);
                writeWild.Write((Byte)numericUpDown118.Value);
                writeWild.Write((UInt16)(comboBox120.SelectedIndex + (numericUpDown203.Value * 2048)));
                writeWild.Write((Byte)numericUpDown117.Value);
                writeWild.Write((Byte)numericUpDown116.Value);
                writeWild.Write((UInt16)(comboBox119.SelectedIndex + (numericUpDown204.Value * 2048)));
                writeWild.Write((Byte)numericUpDown115.Value);
                writeWild.Write((Byte)numericUpDown114.Value);
                writeWild.Write((UInt16)(comboBox118.SelectedIndex + (numericUpDown205.Value * 2048)));
                writeWild.Write((Byte)numericUpDown113.Value);
                writeWild.Write((Byte)numericUpDown112.Value);
                writeWild.Write((UInt16)(comboBox117.SelectedIndex + (numericUpDown206.Value * 2048)));
                writeWild.Write((Byte)numericUpDown111.Value);
                writeWild.Write((Byte)numericUpDown110.Value);
                writeWild.Write((UInt16)(comboBox116.SelectedIndex + (numericUpDown207.Value * 2048)));
                writeWild.Write((Byte)numericUpDown109.Value);
                writeWild.Write((Byte)numericUpDown108.Value);
                writeWild.Write((UInt16)(comboBox115.SelectedIndex + (numericUpDown208.Value * 2048)));
                writeWild.Write((Byte)numericUpDown107.Value);
                writeWild.Write((Byte)numericUpDown106.Value);
                writeWild.Write((UInt16)(comboBox114.SelectedIndex + (numericUpDown209.Value * 2048)));
                writeWild.Write((Byte)numericUpDown105.Value);
                writeWild.Write((Byte)numericUpDown104.Value);
                writeWild.Write((UInt16)(comboBox113.SelectedIndex + (numericUpDown210.Value * 2048)));
                writeWild.Write((Byte)numericUpDown103.Value);
                writeWild.Write((Byte)numericUpDown102.Value);
                writeWild.Write((UInt16)(comboBox112.SelectedIndex + (numericUpDown211.Value * 2048)));
                writeWild.Write((Byte)numericUpDown101.Value);
                writeWild.Write((Byte)numericUpDown100.Value);
                // Special
                writeWild.Write((UInt16)(comboBox135.SelectedIndex + (numericUpDown212.Value * 2048)));
                writeWild.Write((Byte)numericUpDown147.Value);
                writeWild.Write((Byte)numericUpDown146.Value);
                writeWild.Write((UInt16)(comboBox134.SelectedIndex + (numericUpDown213.Value * 2048)));
                writeWild.Write((Byte)numericUpDown145.Value);
                writeWild.Write((Byte)numericUpDown144.Value);
                writeWild.Write((UInt16)(comboBox133.SelectedIndex + (numericUpDown214.Value * 2048)));
                writeWild.Write((Byte)numericUpDown143.Value);
                writeWild.Write((Byte)numericUpDown142.Value);
                writeWild.Write((UInt16)(comboBox132.SelectedIndex + (numericUpDown215.Value * 2048)));
                writeWild.Write((Byte)numericUpDown141.Value);
                writeWild.Write((Byte)numericUpDown140.Value);
                writeWild.Write((UInt16)(comboBox131.SelectedIndex + (numericUpDown216.Value * 2048)));
                writeWild.Write((Byte)numericUpDown139.Value);
                writeWild.Write((Byte)numericUpDown138.Value);
                writeWild.Write((UInt16)(comboBox130.SelectedIndex + (numericUpDown217.Value * 2048)));
                writeWild.Write((Byte)numericUpDown137.Value);
                writeWild.Write((Byte)numericUpDown136.Value);
                writeWild.Write((UInt16)(comboBox129.SelectedIndex + (numericUpDown218.Value * 2048)));
                writeWild.Write((Byte)numericUpDown135.Value);
                writeWild.Write((Byte)numericUpDown134.Value);
                writeWild.Write((UInt16)(comboBox128.SelectedIndex + (numericUpDown219.Value * 2048)));
                writeWild.Write((Byte)numericUpDown133.Value);
                writeWild.Write((Byte)numericUpDown132.Value);
                writeWild.Write((UInt16)(comboBox127.SelectedIndex + (numericUpDown220.Value * 2048)));
                writeWild.Write((Byte)numericUpDown131.Value);
                writeWild.Write((Byte)numericUpDown130.Value);
                writeWild.Write((UInt16)(comboBox126.SelectedIndex + (numericUpDown221.Value * 2048)));
                writeWild.Write((Byte)numericUpDown129.Value);
                writeWild.Write((Byte)numericUpDown128.Value);
                writeWild.Write((UInt16)(comboBox125.SelectedIndex + (numericUpDown222.Value * 2048)));
                writeWild.Write((Byte)numericUpDown127.Value);
                writeWild.Write((Byte)numericUpDown126.Value);
                writeWild.Write((UInt16)(comboBox124.SelectedIndex + (numericUpDown223.Value * 2048)));
                writeWild.Write((Byte)numericUpDown125.Value);
                writeWild.Write((Byte)numericUpDown124.Value);
                // Surf
                writeWild.Write((UInt16)(comboBox136.SelectedIndex + (numericUpDown224.Value * 2048)));
                writeWild.Write((Byte)numericUpDown148.Value);
                writeWild.Write((Byte)numericUpDown149.Value);
                writeWild.Write((UInt16)(comboBox137.SelectedIndex + (numericUpDown225.Value * 2048)));
                writeWild.Write((Byte)numericUpDown150.Value);
                writeWild.Write((Byte)numericUpDown151.Value);
                writeWild.Write((UInt16)(comboBox138.SelectedIndex + (numericUpDown226.Value * 2048)));
                writeWild.Write((Byte)numericUpDown152.Value);
                writeWild.Write((Byte)numericUpDown153.Value);
                writeWild.Write((UInt16)(comboBox139.SelectedIndex + (numericUpDown227.Value * 2048)));
                writeWild.Write((Byte)numericUpDown154.Value);
                writeWild.Write((Byte)numericUpDown155.Value);
                writeWild.Write((UInt16)(comboBox140.SelectedIndex + (numericUpDown228.Value * 2048)));
                writeWild.Write((Byte)numericUpDown156.Value);
                writeWild.Write((Byte)numericUpDown157.Value);
                // Surf Special
                writeWild.Write((UInt16)(comboBox145.SelectedIndex + (numericUpDown229.Value * 2048)));
                writeWild.Write((Byte)numericUpDown167.Value);
                writeWild.Write((Byte)numericUpDown166.Value);
                writeWild.Write((UInt16)(comboBox144.SelectedIndex + (numericUpDown230.Value * 2048)));
                writeWild.Write((Byte)numericUpDown165.Value);
                writeWild.Write((Byte)numericUpDown164.Value);
                writeWild.Write((UInt16)(comboBox143.SelectedIndex + (numericUpDown231.Value * 2048)));
                writeWild.Write((Byte)numericUpDown163.Value);
                writeWild.Write((Byte)numericUpDown162.Value);
                writeWild.Write((UInt16)(comboBox142.SelectedIndex + (numericUpDown232.Value * 2048)));
                writeWild.Write((Byte)numericUpDown161.Value);
                writeWild.Write((Byte)numericUpDown160.Value);
                writeWild.Write((UInt16)(comboBox141.SelectedIndex + (numericUpDown233.Value * 2048)));
                writeWild.Write((Byte)numericUpDown159.Value);
                writeWild.Write((Byte)numericUpDown158.Value);
                // Super Rod
                writeWild.Write((UInt16)(comboBox150.SelectedIndex + (numericUpDown234.Value * 2048)));
                writeWild.Write((Byte)numericUpDown177.Value);
                writeWild.Write((Byte)numericUpDown176.Value);
                writeWild.Write((UInt16)(comboBox149.SelectedIndex + (numericUpDown235.Value * 2048)));
                writeWild.Write((Byte)numericUpDown175.Value);
                writeWild.Write((Byte)numericUpDown174.Value);
                writeWild.Write((UInt16)(comboBox148.SelectedIndex + (numericUpDown236.Value * 2048)));
                writeWild.Write((Byte)numericUpDown173.Value);
                writeWild.Write((Byte)numericUpDown172.Value);
                writeWild.Write((UInt16)(comboBox147.SelectedIndex + (numericUpDown237.Value * 2048)));
                writeWild.Write((Byte)numericUpDown171.Value);
                writeWild.Write((Byte)numericUpDown170.Value);
                writeWild.Write((UInt16)(comboBox146.SelectedIndex + (numericUpDown238.Value * 2048)));
                writeWild.Write((Byte)numericUpDown169.Value);
                writeWild.Write((Byte)numericUpDown168.Value);
                // Super Rod Special
                writeWild.Write((UInt16)(comboBox155.SelectedIndex + (numericUpDown239.Value * 2048)));
                writeWild.Write((Byte)numericUpDown187.Value);
                writeWild.Write((Byte)numericUpDown186.Value);
                writeWild.Write((UInt16)(comboBox154.SelectedIndex + (numericUpDown240.Value * 2048)));
                writeWild.Write((Byte)numericUpDown185.Value);
                writeWild.Write((Byte)numericUpDown184.Value);
                writeWild.Write((UInt16)(comboBox153.SelectedIndex + (numericUpDown241.Value * 2048)));
                writeWild.Write((Byte)numericUpDown183.Value);
                writeWild.Write((Byte)numericUpDown182.Value);
                writeWild.Write((UInt16)(comboBox152.SelectedIndex + (numericUpDown242.Value * 2048)));
                writeWild.Write((Byte)numericUpDown181.Value);
                writeWild.Write((Byte)numericUpDown180.Value);
                writeWild.Write((UInt16)(comboBox151.SelectedIndex + (numericUpDown243.Value * 2048)));
                writeWild.Write((Byte)numericUpDown179.Value);
                writeWild.Write((Byte)numericUpDown178.Value);
                writeWild.Close();
            }
            #endregion
            #region DPPt
            if (game == "dppt")
            {
                System.IO.BinaryWriter writeWild = new System.IO.BinaryWriter(File.OpenWrite(wildPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                writeWild.Write((uint)numericUpDown13.Value);
                writeWild.Write((uint)numericUpDown1.Value);
                writeWild.Write((uint)comboBox2.SelectedIndex);
                writeWild.Write((uint)numericUpDown2.Value);
                writeWild.Write((uint)comboBox3.SelectedIndex);
                writeWild.Write((uint)numericUpDown3.Value);
                writeWild.Write((uint)comboBox4.SelectedIndex);
                writeWild.Write((uint)numericUpDown4.Value);
                writeWild.Write((uint)comboBox5.SelectedIndex);
                writeWild.Write((uint)numericUpDown5.Value);
                writeWild.Write((uint)comboBox6.SelectedIndex);
                writeWild.Write((uint)numericUpDown6.Value);
                writeWild.Write((uint)comboBox7.SelectedIndex);
                writeWild.Write((uint)numericUpDown7.Value);
                writeWild.Write((uint)comboBox8.SelectedIndex);
                writeWild.Write((uint)numericUpDown8.Value);
                writeWild.Write((uint)comboBox9.SelectedIndex);
                writeWild.Write((uint)numericUpDown9.Value);
                writeWild.Write((uint)comboBox10.SelectedIndex);
                writeWild.Write((uint)numericUpDown10.Value);
                writeWild.Write((uint)comboBox11.SelectedIndex);
                writeWild.Write((uint)numericUpDown11.Value);
                writeWild.Write((uint)comboBox12.SelectedIndex);
                writeWild.Write((uint)numericUpDown12.Value);
                writeWild.Write((uint)comboBox13.SelectedIndex);
                writeWild.Write((uint)comboBox14.SelectedIndex);
                writeWild.Write((uint)comboBox15.SelectedIndex);
                writeWild.Write((uint)comboBox16.SelectedIndex);
                writeWild.Write((uint)comboBox17.SelectedIndex);
                writeWild.Write((uint)comboBox18.SelectedIndex);
                writeWild.Write((uint)comboBox19.SelectedIndex);
                writeWild.Write((uint)comboBox20.SelectedIndex);
                writeWild.Write((uint)comboBox21.SelectedIndex);
                writeWild.Write((uint)comboBox22.SelectedIndex);
                writeWild.Write((uint)comboBox23.SelectedIndex);
                writeWild.BaseStream.Position = 0xA4;
                writeWild.Write((uint)comboBox24.SelectedIndex);
                writeWild.Write((uint)comboBox25.SelectedIndex);
                writeWild.Write((uint)comboBox26.SelectedIndex);
                writeWild.Write((uint)comboBox27.SelectedIndex);
                writeWild.Write((uint)comboBox28.SelectedIndex);
                writeWild.Write((uint)comboBox29.SelectedIndex);
                writeWild.Write((uint)comboBox30.SelectedIndex);
                writeWild.Write((uint)comboBox31.SelectedIndex);
                writeWild.Write((uint)comboBox32.SelectedIndex);
                writeWild.Write((uint)comboBox33.SelectedIndex);
                // Surfing
                writeWild.Write((uint)numericUpDown24.Value);
                writeWild.Write((byte)numericUpDown15.Value);
                writeWild.Write((byte)numericUpDown14.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox34.SelectedIndex);
                writeWild.Write((byte)numericUpDown16.Value);
                writeWild.Write((byte)numericUpDown17.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox35.SelectedIndex);
                writeWild.Write((byte)numericUpDown18.Value);
                writeWild.Write((byte)numericUpDown19.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox36.SelectedIndex);
                writeWild.Write((byte)numericUpDown20.Value);
                writeWild.Write((byte)numericUpDown21.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox37.SelectedIndex);
                writeWild.Write((byte)numericUpDown22.Value);
                writeWild.Write((byte)numericUpDown23.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox38.SelectedIndex);
                writeWild.BaseStream.Position += 0x124;
                // Old Rod
                writeWild.Write((uint)numericUpDown25.Value);
                writeWild.Write((byte)numericUpDown34.Value);
                writeWild.Write((byte)numericUpDown35.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox43.SelectedIndex);
                writeWild.Write((byte)numericUpDown32.Value);
                writeWild.Write((byte)numericUpDown33.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox42.SelectedIndex);
                writeWild.Write((byte)numericUpDown30.Value);
                writeWild.Write((byte)numericUpDown31.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox41.SelectedIndex);
                writeWild.Write((byte)numericUpDown28.Value);
                writeWild.Write((byte)numericUpDown29.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox40.SelectedIndex);
                writeWild.Write((byte)numericUpDown26.Value);
                writeWild.Write((byte)numericUpDown27.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox39.SelectedIndex);
                // Good Rod
                writeWild.Write((uint)numericUpDown36.Value);
                writeWild.Write((byte)numericUpDown45.Value);
                writeWild.Write((byte)numericUpDown46.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox48.SelectedIndex);
                writeWild.Write((byte)numericUpDown43.Value);
                writeWild.Write((byte)numericUpDown44.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox47.SelectedIndex);
                writeWild.Write((byte)numericUpDown41.Value);
                writeWild.Write((byte)numericUpDown42.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox46.SelectedIndex);
                writeWild.Write((byte)numericUpDown39.Value);
                writeWild.Write((byte)numericUpDown40.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox45.SelectedIndex);
                writeWild.Write((byte)numericUpDown37.Value);
                writeWild.Write((byte)numericUpDown38.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox44.SelectedIndex);
                // Super Rod
                writeWild.Write((uint)numericUpDown47.Value);
                writeWild.Write((byte)numericUpDown56.Value);
                writeWild.Write((byte)numericUpDown57.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox53.SelectedIndex);
                writeWild.Write((byte)numericUpDown54.Value);
                writeWild.Write((byte)numericUpDown55.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox52.SelectedIndex);
                writeWild.Write((byte)numericUpDown52.Value);
                writeWild.Write((byte)numericUpDown53.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox51.SelectedIndex);
                writeWild.Write((byte)numericUpDown50.Value);
                writeWild.Write((byte)numericUpDown51.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox50.SelectedIndex);
                writeWild.Write((byte)numericUpDown48.Value);
                writeWild.Write((byte)numericUpDown49.Value);
                writeWild.BaseStream.Position += 0x2;
                writeWild.Write((uint)comboBox49.SelectedIndex);
                writeWild.Close();
            }
            #endregion
            #region HGSS
            if (game == "hgss")
            {
                System.IO.BinaryWriter writeWild = new System.IO.BinaryWriter(File.OpenWrite(wildPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                // Rates
                writeWild.Write((byte)numericUpDown58.Value);
                writeWild.Write((byte)numericUpDown24.Value);
                writeWild.Write((byte)numericUpDown71.Value);
                writeWild.Write((byte)numericUpDown25.Value);
                writeWild.Write((byte)numericUpDown36.Value);
                writeWild.Write((byte)numericUpDown47.Value);
                // Walking
                writeWild.BaseStream.Position = 0x8;
                writeWild.Write((byte)numericUpDown70.Value);
                writeWild.Write((byte)numericUpDown69.Value);
                writeWild.Write((byte)numericUpDown67.Value);
                writeWild.Write((byte)numericUpDown68.Value);
                writeWild.Write((byte)numericUpDown66.Value);
                writeWild.Write((byte)numericUpDown65.Value);
                writeWild.Write((byte)numericUpDown64.Value);
                writeWild.Write((byte)numericUpDown63.Value);
                writeWild.Write((byte)numericUpDown62.Value);
                writeWild.Write((byte)numericUpDown61.Value);
                writeWild.Write((byte)numericUpDown60.Value);
                writeWild.Write((byte)numericUpDown59.Value);
                writeWild.Write((UInt16)comboBox85.SelectedIndex);
                writeWild.Write((UInt16)comboBox84.SelectedIndex);
                writeWild.Write((UInt16)comboBox83.SelectedIndex);
                writeWild.Write((UInt16)comboBox82.SelectedIndex);
                writeWild.Write((UInt16)comboBox81.SelectedIndex);
                writeWild.Write((UInt16)comboBox80.SelectedIndex);
                writeWild.Write((UInt16)comboBox79.SelectedIndex);
                writeWild.Write((UInt16)comboBox78.SelectedIndex);
                writeWild.Write((UInt16)comboBox77.SelectedIndex);
                writeWild.Write((UInt16)comboBox76.SelectedIndex);
                writeWild.Write((UInt16)comboBox75.SelectedIndex);
                writeWild.Write((UInt16)comboBox74.SelectedIndex);
                writeWild.Write((UInt16)comboBox69.SelectedIndex);
                writeWild.Write((UInt16)comboBox68.SelectedIndex);
                writeWild.Write((UInt16)comboBox67.SelectedIndex);
                writeWild.Write((UInt16)comboBox66.SelectedIndex);
                writeWild.Write((UInt16)comboBox65.SelectedIndex);
                writeWild.Write((UInt16)comboBox64.SelectedIndex);
                writeWild.Write((UInt16)comboBox63.SelectedIndex);
                writeWild.Write((UInt16)comboBox62.SelectedIndex);
                writeWild.Write((UInt16)comboBox61.SelectedIndex);
                writeWild.Write((UInt16)comboBox60.SelectedIndex);
                writeWild.Write((UInt16)comboBox59.SelectedIndex);
                writeWild.Write((UInt16)comboBox58.SelectedIndex);
                writeWild.Write((UInt16)comboBox93.SelectedIndex);
                writeWild.Write((UInt16)comboBox92.SelectedIndex);
                writeWild.Write((UInt16)comboBox91.SelectedIndex);
                writeWild.Write((UInt16)comboBox90.SelectedIndex);
                writeWild.Write((UInt16)comboBox89.SelectedIndex);
                writeWild.Write((UInt16)comboBox88.SelectedIndex);
                writeWild.Write((UInt16)comboBox87.SelectedIndex);
                writeWild.Write((UInt16)comboBox86.SelectedIndex);
                writeWild.Write((UInt16)comboBox73.SelectedIndex);
                writeWild.Write((UInt16)comboBox72.SelectedIndex);
                writeWild.Write((UInt16)comboBox71.SelectedIndex);
                writeWild.Write((UInt16)comboBox70.SelectedIndex);
                // Hoenn Sound
                writeWild.Write((UInt16)comboBox94.SelectedIndex);
                writeWild.Write((UInt16)comboBox95.SelectedIndex);
                // Sinnoh Sound
                writeWild.Write((UInt16)comboBox96.SelectedIndex);
                writeWild.Write((UInt16)comboBox97.SelectedIndex);
                // Surfing
                writeWild.Write((byte)numericUpDown14.Value);
                writeWild.Write((byte)numericUpDown15.Value);
                writeWild.Write((UInt16)comboBox34.SelectedIndex);
                writeWild.Write((byte)numericUpDown17.Value);
                writeWild.Write((byte)numericUpDown16.Value);
                writeWild.Write((UInt16)comboBox35.SelectedIndex);
                writeWild.Write((byte)numericUpDown19.Value);
                writeWild.Write((byte)numericUpDown18.Value);
                writeWild.Write((UInt16)comboBox36.SelectedIndex);
                writeWild.Write((byte)numericUpDown21.Value);
                writeWild.Write((byte)numericUpDown20.Value);
                writeWild.Write((UInt16)comboBox37.SelectedIndex);
                writeWild.Write((byte)numericUpDown23.Value);
                writeWild.Write((byte)numericUpDown24.Value);
                writeWild.Write((UInt16)comboBox38.SelectedIndex);
                // Rock Smash
                writeWild.Write((byte)numericUpDown75.Value);
                writeWild.Write((byte)numericUpDown74.Value);
                writeWild.Write((UInt16)comboBox98.SelectedIndex);
                writeWild.Write((byte)numericUpDown73.Value);
                writeWild.Write((byte)numericUpDown72.Value);
                writeWild.Write((UInt16)comboBox99.SelectedIndex);
                // Old Rod
                writeWild.Write((byte)numericUpDown35.Value);
                writeWild.Write((byte)numericUpDown34.Value);
                writeWild.Write((UInt16)comboBox43.SelectedIndex);
                writeWild.Write((byte)numericUpDown33.Value);
                writeWild.Write((byte)numericUpDown32.Value);
                writeWild.Write((UInt16)comboBox42.SelectedIndex);
                writeWild.Write((byte)numericUpDown31.Value);
                writeWild.Write((byte)numericUpDown30.Value);
                writeWild.Write((UInt16)comboBox41.SelectedIndex);
                writeWild.Write((byte)numericUpDown29.Value);
                writeWild.Write((byte)numericUpDown28.Value);
                writeWild.Write((UInt16)comboBox40.SelectedIndex);
                writeWild.Write((byte)numericUpDown27.Value);
                writeWild.Write((byte)numericUpDown26.Value);
                writeWild.Write((UInt16)comboBox39.SelectedIndex);
                // Good Rod
                writeWild.Write((byte)numericUpDown46.Value);
                writeWild.Write((byte)numericUpDown45.Value);
                writeWild.Write((UInt16)comboBox48.SelectedIndex);
                writeWild.Write((byte)numericUpDown44.Value);
                writeWild.Write((byte)numericUpDown43.Value);
                writeWild.Write((UInt16)comboBox47.SelectedIndex);
                writeWild.Write((byte)numericUpDown42.Value);
                writeWild.Write((byte)numericUpDown41.Value);
                writeWild.Write((UInt16)comboBox46.SelectedIndex);
                writeWild.Write((byte)numericUpDown40.Value);
                writeWild.Write((byte)numericUpDown39.Value);
                writeWild.Write((UInt16)comboBox45.SelectedIndex);
                writeWild.Write((byte)numericUpDown38.Value);
                writeWild.Write((byte)numericUpDown37.Value);
                writeWild.Write((UInt16)comboBox44.SelectedIndex);
                // Super Rod
                writeWild.Write((byte)numericUpDown57.Value);
                writeWild.Write((byte)numericUpDown56.Value);
                writeWild.Write((UInt16)comboBox53.SelectedIndex);
                writeWild.Write((byte)numericUpDown55.Value);
                writeWild.Write((byte)numericUpDown54.Value);
                writeWild.Write((UInt16)comboBox52.SelectedIndex);
                writeWild.Write((byte)numericUpDown53.Value);
                writeWild.Write((byte)numericUpDown52.Value);
                writeWild.Write((UInt16)comboBox51.SelectedIndex);
                writeWild.Write((byte)numericUpDown51.Value);
                writeWild.Write((byte)numericUpDown50.Value);
                writeWild.Write((UInt16)comboBox50.SelectedIndex);
                writeWild.Write((byte)numericUpDown49.Value);
                writeWild.Write((byte)numericUpDown48.Value);
                writeWild.Write((UInt16)comboBox49.SelectedIndex);
                // Swarms
                writeWild.Write((UInt16)comboBox57.SelectedIndex);
                writeWild.Write((UInt16)comboBox56.SelectedIndex);
                writeWild.Write((UInt16)comboBox55.SelectedIndex);
                writeWild.Write((UInt16)comboBox54.SelectedIndex);
                writeWild.Close();
            }
            #endregion
        }

        private void Form9_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Form1.isBW)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\a\1\2\wild").Save(Form1.workingFolder + @"data\a\1\2\6");
                Directory.Delete(Form1.workingFolder + @"data\a\1\2\wild", true);
            }
            if (Form1.isB2W2)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\a\1\2\wild").Save(Form1.workingFolder + @"data\a\1\2\7");
                Directory.Delete(Form1.workingFolder + @"data\a\1\2\wild", true);
            }
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x53414441 || Form1.gameID == 0x46414441 || Form1.gameID == 0x49414441 || Form1.gameID == 0x44414441 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4B414441)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\fielddata\encountdata\d_enc_data").Save(Form1.workingFolder + @"data\fielddata\encountdata\d_enc_data.narc");
                Directory.Delete(Form1.workingFolder + @"data\fielddata\encountdata\d_enc_data", true);
            }
            if (Form1.gameID == 0x45415041 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B415041)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\fielddata\encountdata\p_enc_data").Save(Form1.workingFolder + @"data\fielddata\encountdata\p_enc_data.narc");
                Directory.Delete(Form1.workingFolder + @"data\fielddata\encountdata\p_enc_data", true);
            }
            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\fielddata\encountdata\pl_enc_data").Save(Form1.workingFolder + @"data\fielddata\encountdata\pl_enc_data.narc");
                Directory.Delete(Form1.workingFolder + @"data\fielddata\encountdata\pl_enc_data", true);
            }
            if (Form1.gameID == 0x454B5049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4B4B5049)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\a\0\3\wild").Save(Form1.workingFolder + @"data\a\0\3\7");
                Directory.Delete(Form1.workingFolder + @"data\a\0\3\wild", true);
            }
            if (Form1.gameID == 0x45475049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B475049)
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\a\1\3\wild").Save(Form1.workingFolder + @"data\a\1\3\6");
                Directory.Delete(Form1.workingFolder + @"data\a\1\3\wild", true);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                if (editON)
                {
                    File.Create(Form1.workingFolder + @"data\a\1\2\wild" + "\\" + comboBox1.SelectedIndex.ToString("D4")).Close();
                    button1_Click(null, null);
                }
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton4.Enabled = false;
            }
            else
            {
                if (editON)
                {
                    System.IO.BinaryWriter writeWild = new System.IO.BinaryWriter(File.Create(Form1.workingFolder + @"data\a\1\2\wild" + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                    byte[] dummy = new byte[928];
                    writeWild.Write(dummy);
                    writeWild.Close();
                    button1_Click(null, null);
                }
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
            }
        }

    }
}
