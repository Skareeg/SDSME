using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using NarcAPI;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        ResourceManager rm = new ResourceManager("WindowsFormsApplication1.WinFormStrings", Assembly.GetExecutingAssembly());
        ResourceManager getChar = new ResourceManager("WindowsFormsApplication1.Resources.ReadText", Assembly.GetExecutingAssembly());
        ResourceManager getByte = new ResourceManager("WindowsFormsApplication1.Resources.WriteText", Assembly.GetExecutingAssembly());
        public string textPokePath;
        public string textItemPath;
        public string textAttackPath;
        public string textClassPath;
        public string textNamePath;
        string trdataPath;
        string trpokePath;
        public int initialKey;
        public string[] currentTrainerClass;
        public List<string> names = new List<string>();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            #region Read Pokémon Names
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041)
            {
                textPokePath = Form1.workingFolder + @"data\msgdata\msg\0362";
                textItemPath = Form1.workingFolder + @"data\msgdata\msg\0344";
                textAttackPath = Form1.workingFolder + @"data\msgdata\msg\0588";
                textClassPath = Form1.workingFolder + @"data\msgdata\msg\0560";
                textNamePath = Form1.workingFolder + @"data\msgdata\msg\0559";
            }
            if (Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041)
            {
                textPokePath = Form1.workingFolder + @"data\msgdata\msg\0356";
                textItemPath = Form1.workingFolder + @"data\msgdata\msg\0341";
                textAttackPath = Form1.workingFolder + @"data\msgdata\msg\0575";
                textClassPath = Form1.workingFolder + @"data\msgdata\msg\0560";
                textNamePath = Form1.workingFolder + @"data\msgdata\msg\0559";
            }
            if (Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041)
            {
                textPokePath = Form1.workingFolder + @"data\msgdata\msg\0357";
                textItemPath = Form1.workingFolder + @"data\msgdata\msg\0342";
                textAttackPath = Form1.workingFolder + @"data\msgdata\msg\0577";
                textClassPath = Form1.workingFolder + @"data\msgdata\msg\0560";
                textNamePath = Form1.workingFolder + @"data\msgdata\msg\0559";
            }
            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043)
            {
                textPokePath = Form1.workingFolder + @"data\msgdata\pl_msg\0412";
                textItemPath = Form1.workingFolder + @"data\msgdata\pl_msg\0392";
                textAttackPath = Form1.workingFolder + @"data\msgdata\pl_msg\0647";
                textClassPath = Form1.workingFolder + @"data\msgdata\pl_msg\0619";
                textNamePath = Form1.workingFolder + @"data\msgdata\pl_msg\0618";
            }
            if (Form1.gameID == 0x4A555043)
            {
                textPokePath = Form1.workingFolder + @"data\msgdata\pl_msg\0408";
                textItemPath = Form1.workingFolder + @"data\msgdata\pl_msg\0390";
                textAttackPath = Form1.workingFolder + @"data\msgdata\pl_msg\0636";
                textClassPath = Form1.workingFolder + @"data\msgdata\pl_msg\0619";
                textNamePath = Form1.workingFolder + @"data\msgdata\pl_msg\0618";
            }
            if (Form1.gameID == 0x4B555043)
            {
                textPokePath = Form1.workingFolder + @"data\msgdata\pl_msg\0408";
                textItemPath = Form1.workingFolder + @"data\msgdata\pl_msg\0390";
                textAttackPath = Form1.workingFolder + @"data\msgdata\pl_msg\0667";
                textClassPath = Form1.workingFolder + @"data\msgdata\pl_msg\0619";
                textNamePath = Form1.workingFolder + @"data\msgdata\pl_msg\0618";
            }
            if (Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049)
            {
                textPokePath = Form1.workingFolder + @"data\a\0\2\text\0237";
                textItemPath = Form1.workingFolder + @"data\a\0\2\text\0222";
                textAttackPath = Form1.workingFolder + @"data\a\0\2\text\0750";
                textClassPath = Form1.workingFolder + @"data\a\0\2\text\0730";
                textNamePath = Form1.workingFolder + @"data\a\0\2\text\0729";
            }
            if (Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
            {
                textPokePath = Form1.workingFolder + @"data\a\0\2\text\0232";
                textItemPath = Form1.workingFolder + @"data\a\0\2\text\0219";
                textAttackPath = Form1.workingFolder + @"data\a\0\2\text\0739";
                textClassPath = Form1.workingFolder + @"data\a\0\2\text\0720";
                textNamePath = Form1.workingFolder + @"data\a\0\2\text\0719";
            }
            System.IO.BinaryReader readText = new System.IO.BinaryReader(File.OpenRead(textPokePath));
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
                comboBox10.Items.Add(currentPokemon[i]);
                comboBox13.Items.Add(currentPokemon[i]);
                comboBox19.Items.Add(currentPokemon[i]);
                comboBox25.Items.Add(currentPokemon[i]);
                comboBox31.Items.Add(currentPokemon[i]);
                comboBox37.Items.Add(currentPokemon[i]);
            }
            #endregion

            #region Read Item Names
            readText = new System.IO.BinaryReader(File.OpenRead(textItemPath));
            readText.BaseStream.Position = 0x0;
            int stringItemCount = (int)readText.ReadUInt16();
            initialKey = (int)readText.ReadUInt16();
            key1 = (initialKey * 0x2FD) & 0xFFFF;
            key2 = 0;
            realKey = 0;
            specialCharON = false;
            currentOffset = new int[stringItemCount];
            currentSize = new int[stringItemCount];
            string[] currentItem = new string[stringItemCount];
            car = 0;
            for (int i = 0; i < stringItemCount; i++) // Reads and stores string offsets and sizes
            {
                key2 = (key1 * (i + 1) & 0xFFFF);
                realKey = key2 | (key2 << 16);
                currentOffset[i] = ((int)readText.ReadUInt32()) ^ realKey;
                currentSize[i] = ((int)readText.ReadUInt32()) ^ realKey;
            }
            for (int i = 0; i < stringItemCount; i++) // Adds new string
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
                currentItem[i] = pokemonText;
            }
            readText.Close();
            #endregion
            #region Adds Items to Lists
            for (int i = 0; i < stringItemCount; i++)
            {
                comboBox2.Items.Add(currentItem[i]);
                comboBox3.Items.Add(currentItem[i]);
                comboBox4.Items.Add(currentItem[i]);
                comboBox5.Items.Add(currentItem[i]);
                comboBox11.Items.Add(currentItem[i]);
                comboBox12.Items.Add(currentItem[i]);
                comboBox18.Items.Add(currentItem[i]);
                comboBox24.Items.Add(currentItem[i]);
                comboBox30.Items.Add(currentItem[i]);
                comboBox36.Items.Add(currentItem[i]);
            }
            comboBox11.SelectedIndex = 0;
            comboBox12.SelectedIndex = 0;
            comboBox18.SelectedIndex = 0;
            comboBox24.SelectedIndex = 0;
            comboBox30.SelectedIndex = 0;
            comboBox36.SelectedIndex = 0;
            #endregion

            #region Read Attack Names
            readText = new System.IO.BinaryReader(File.OpenRead(textAttackPath));
            readText.BaseStream.Position = 0x0;
            int stringAttackCount = (int)readText.ReadUInt16();
            initialKey = (int)readText.ReadUInt16();
            key1 = (initialKey * 0x2FD) & 0xFFFF;
            key2 = 0;
            realKey = 0;
            specialCharON = false;
            currentOffset = new int[stringAttackCount];
            currentSize = new int[stringAttackCount];
            string[] currentAttack = new string[stringAttackCount];
            car = 0;
            for (int i = 0; i < stringAttackCount; i++) // Reads and stores string offsets and sizes
            {
                key2 = (key1 * (i + 1) & 0xFFFF);
                realKey = key2 | (key2 << 16);
                currentOffset[i] = ((int)readText.ReadUInt32()) ^ realKey;
                currentSize[i] = ((int)readText.ReadUInt32()) ^ realKey;
            }
            for (int i = 0; i < stringAttackCount; i++) // Adds new string
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
                currentAttack[i] = pokemonText;
            }
            readText.Close();
            #endregion
            #region Adds Attacks to Lists
            for (int i = 0; i < stringAttackCount; i++)
            {
                comboBox6.Items.Add(currentAttack[i]);
                comboBox7.Items.Add(currentAttack[i]);
                comboBox9.Items.Add(currentAttack[i]);
                comboBox8.Items.Add(currentAttack[i]);
                comboBox17.Items.Add(currentAttack[i]);
                comboBox16.Items.Add(currentAttack[i]);
                comboBox15.Items.Add(currentAttack[i]);
                comboBox14.Items.Add(currentAttack[i]);
                comboBox23.Items.Add(currentAttack[i]);
                comboBox22.Items.Add(currentAttack[i]);
                comboBox21.Items.Add(currentAttack[i]);
                comboBox20.Items.Add(currentAttack[i]);
                comboBox29.Items.Add(currentAttack[i]);
                comboBox28.Items.Add(currentAttack[i]);
                comboBox27.Items.Add(currentAttack[i]);
                comboBox26.Items.Add(currentAttack[i]);
                comboBox35.Items.Add(currentAttack[i]);
                comboBox34.Items.Add(currentAttack[i]);
                comboBox33.Items.Add(currentAttack[i]);
                comboBox32.Items.Add(currentAttack[i]);
                comboBox41.Items.Add(currentAttack[i]);
                comboBox40.Items.Add(currentAttack[i]);
                comboBox39.Items.Add(currentAttack[i]);
                comboBox38.Items.Add(currentAttack[i]);
            }
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox9.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;
            comboBox17.SelectedIndex = 0;
            comboBox16.SelectedIndex = 0;
            comboBox15.SelectedIndex = 0;
            comboBox14.SelectedIndex = 0;
            comboBox23.SelectedIndex = 0;
            comboBox22.SelectedIndex = 0;
            comboBox21.SelectedIndex = 0;
            comboBox20.SelectedIndex = 0;
            comboBox29.SelectedIndex = 0;
            comboBox28.SelectedIndex = 0;
            comboBox27.SelectedIndex = 0;
            comboBox26.SelectedIndex = 0;
            comboBox35.SelectedIndex = 0;
            comboBox34.SelectedIndex = 0;
            comboBox33.SelectedIndex = 0;
            comboBox32.SelectedIndex = 0;
            comboBox41.SelectedIndex = 0;
            comboBox40.SelectedIndex = 0;
            comboBox39.SelectedIndex = 0;
            comboBox38.SelectedIndex = 0;
            #endregion

            #region Read Trainer Class Names
            readText = new System.IO.BinaryReader(File.OpenRead(textClassPath));
            readText.BaseStream.Position = 0x0;
            int stringClassCount = (int)readText.ReadUInt16();
            initialKey = (int)readText.ReadUInt16();
            key1 = (initialKey * 0x2FD) & 0xFFFF;
            key2 = 0;
            realKey = 0;
            specialCharON = false;
            currentOffset = new int[stringClassCount];
            currentSize = new int[stringClassCount];
            string[] currentClass = new string[stringClassCount];
            car = 0;
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
                currentClass[i] = pokemonText;
            }
            readText.Close();
            #endregion
            #region Read Trainer Names
            readText = new System.IO.BinaryReader(File.OpenRead(textNamePath));
            readText.BaseStream.Position = 0x0;
            int stringNameCount = (int)readText.ReadUInt16();
            initialKey = (int)readText.ReadUInt16();
            key1 = (initialKey * 0x2FD) & 0xFFFF;
            key2 = 0;
            realKey = 0;
            specialCharON = false;
            currentOffset = new int[stringNameCount];
            currentSize = new int[stringNameCount];
            car = 0;
            bool compressed = false;
            for (int i = 0; i < stringNameCount; i++) // Reads and stores string offsets and sizes
            {
                key2 = (key1 * (i + 1) & 0xFFFF);
                realKey = key2 | (key2 << 16);
                currentOffset[i] = ((int)readText.ReadUInt32()) ^ realKey;
                currentSize[i] = ((int)readText.ReadUInt32()) ^ realKey;
            }
            for (int i = 0; i < stringNameCount; i++) // Adds new string
            {
                key1 = (0x91BD3 * (i + 1)) & 0xFFFF;
                readText.BaseStream.Position = currentOffset[i];
                string pokemonText = "";
                for (int j = 0; j < currentSize[i]; j++) // Adds new characters to string
                {
                    car = readText.ReadUInt16() ^ key1;
                    #region Special Characters
                    if (car == 0xE000 || car == 0x25BC || car == 0x25BD || car == 0xF100 || car == 0xFFFE || car == 0xFFFF)
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
                        if (car == 0xF100)
                        {
                            compressed = true;
                        }
                        if (car == 0xFFFE)
                        {
                            pokemonText += @"\v";
                            specialCharON = true;
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
                        else if (compressed)
                        {
                            #region Compressed String
                            int shift = 0;
                            int trans = 0;
                            string uncomp = "";
                            while (true)
                            {
                                int tmp = car >> shift;
                                int tmp1 = tmp;
                                if (shift >= 0xF)
                                {
                                    shift -= 0xF;
                                    if (shift > 0)
                                    {
                                        tmp1 = (trans | ((car << (9 - shift)) & 0x1FF));
                                        if ((tmp1 & 0xFF) == 0xFF)
                                        {
                                            break;
                                        }
                                        if (tmp1 != 0x0 && tmp1 != 0x1)
                                        {
                                            string character = getChar.GetString(tmp1.ToString("X4"));
                                            pokemonText += character;
                                            if (character == null)
                                            {
                                                pokemonText += @"\x" + tmp1.ToString("X4");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    tmp1 = ((car >> shift) & 0x1FF);
                                    if ((tmp1 & 0xFF) == 0xFF)
                                    {
                                        break;
                                    }
                                    if (tmp1 != 0x0 && tmp1 != 0x1)
                                    {
                                        string character = getChar.GetString(tmp1.ToString("X4"));
                                        pokemonText += character;
                                        if (character == null)
                                        {
                                            pokemonText += @"\x" + tmp1.ToString("X4");
                                        }
                                    }
                                    shift += 9;
                                    if (shift < 0xF)
                                    {
                                        trans = ((car >> shift) & 0x1FF);
                                        shift += 9;
                                    }
                                    key1 += 0x493D;
                                    key1 &= 0xFFFF;
                                    car = Convert.ToUInt16(readText.ReadUInt16() ^ key1);
                                    j++;
                                }
                            }
                            #endregion
                            pokemonText += uncomp;
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
                names.Add(pokemonText);
                compressed = false;
            }
            readText.Close();
            #endregion
            #region Adds Trainer Classes to Lists
            currentTrainerClass = currentClass;
            for (int i = 0; i < stringClassCount; i++)
            {
                listBox1.Items.Add(i + ": " + currentClass[i]);
            }
            #endregion

            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                Narc.Open(Form1.workingFolder + @"data\poketool\trainer\trdata.narc").ExtractToFolder(Form1.workingFolder + @"data\poketool\trainer\trdata\");
                Narc.Open(Form1.workingFolder + @"data\poketool\trainer\trpoke.narc").ExtractToFolder(Form1.workingFolder + @"data\poketool\trainer\trpoke\");
                trdataPath = Form1.workingFolder + @"data\poketool\trainer\trdata";
                trpokePath = Form1.workingFolder + @"data\poketool\trainer\trpoke";
            }
            else
            {
                Narc.Open(Form1.workingFolder + @"data\a\0\5\5").ExtractToFolder(Form1.workingFolder + @"data\a\0\5\trdata\");
                Narc.Open(Form1.workingFolder + @"data\a\0\5\6").ExtractToFolder(Form1.workingFolder + @"data\a\0\5\trpoke\");
                trdataPath = Form1.workingFolder + @"data\a\0\5\trdata";
                trpokePath = Form1.workingFolder + @"data\a\0\5\trpoke";
            }
            for (int i = 0; i < Directory.GetFiles(trdataPath).Length; i++)
            {
                BinaryReader readTrainer = new BinaryReader(File.OpenRead(trdataPath + "\\" + i.ToString("D4")));
                readTrainer.BaseStream.Position = 0x1;
                comboBox1.Items.Add(i + ": " + currentClass[readTrainer.ReadByte()] + " " + names[i]);
                readTrainer.Close();
            }
            comboBox1.SelectedIndex = 0;
            numericUpDown1_ValueChanged(null, null);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBoxAttacks_CheckedChanged(null, null);
            textBox1.Text = names[comboBox1.SelectedIndex];
            BinaryReader readTrainer = new BinaryReader(File.OpenRead(trdataPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            int atckitm = readTrainer.ReadByte(); // Attacks & Items
            checkBoxAttacks.Checked = false;
            checkBoxItems.Checked = false;
            if (atckitm != 0)
            {
                if (atckitm == 0x03)
                {
                    checkBoxAttacks.Checked = true;
                    checkBoxItems.Checked = true;
                }
                if (atckitm == 0x02)
                {
                    checkBoxAttacks.Checked = false;
                    checkBoxItems.Checked = true;
                }
                if (atckitm == 0x01)
                {
                    checkBoxAttacks.Checked = true;
                    checkBoxItems.Checked = false;
                }
            }
            listBox1.SelectedIndex = readTrainer.ReadByte(); // Trainer Class
            readTrainer.BaseStream.Position += 0x1;
            numericUpDown1.Value = readTrainer.ReadByte(); // Amount of Pokémon
            comboBox2.SelectedIndex = readTrainer.ReadUInt16(); // Item 1
            comboBox3.SelectedIndex = readTrainer.ReadUInt16(); // Item 2
            comboBox4.SelectedIndex = readTrainer.ReadUInt16(); // Item 3
            comboBox5.SelectedIndex = readTrainer.ReadUInt16(); // Item 4
            numericUpDown2.Value = (int)readTrainer.ReadUInt32(); // Artificial Intelligence
            checkBoxDouble.Checked = false;
            if (readTrainer.ReadByte() == 0x2)
            {
                checkBoxDouble.Checked = true; // Double Battle
            }
            readTrainer.Close();

            if (numericUpDown1.Value != 0)
            {
                BinaryReader readPokemon = new BinaryReader(File.OpenRead(trpokePath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                numericUpDown5.Value = readPokemon.ReadUInt16();
                numericUpDown3.Value = readPokemon.ReadUInt16();
                int pokemon1 = readPokemon.ReadUInt16();
                numericUpDown4.Value = pokemon1 / 1024;
                comboBox10.SelectedIndex = pokemon1 - ((int)numericUpDown4.Value * 1024);
                if (checkBoxItems.Checked == true)
                {
                    comboBox11.SelectedIndex = readPokemon.ReadUInt16();
                }
                if (checkBoxAttacks.Checked == true)
                {
                    comboBox6.SelectedIndex = readPokemon.ReadUInt16();
                    comboBox7.SelectedIndex = readPokemon.ReadUInt16();
                    comboBox9.SelectedIndex = readPokemon.ReadUInt16();
                    comboBox8.SelectedIndex = readPokemon.ReadUInt16();
                }
                if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                {
                    readPokemon.BaseStream.Position += 0x2;
                }
                if (numericUpDown1.Value > 1)
                {
                    numericUpDown6.Value = readPokemon.ReadUInt16();
                    numericUpDown8.Value = readPokemon.ReadUInt16();
                    pokemon1 = readPokemon.ReadUInt16();
                    numericUpDown7.Value = pokemon1 / 1024;
                    comboBox13.SelectedIndex = pokemon1 - ((int)numericUpDown7.Value * 1024);
                    if (checkBoxItems.Checked == true)
                    {
                        comboBox12.SelectedIndex = readPokemon.ReadUInt16();
                    }
                    if (checkBoxAttacks.Checked == true)
                    {
                        comboBox17.SelectedIndex = readPokemon.ReadUInt16();
                        comboBox16.SelectedIndex = readPokemon.ReadUInt16();
                        comboBox15.SelectedIndex = readPokemon.ReadUInt16();
                        comboBox14.SelectedIndex = readPokemon.ReadUInt16();
                    }
                    if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                    {
                        readPokemon.BaseStream.Position += 0x2;
                    }
                    if (numericUpDown1.Value > 2)
                    {
                        numericUpDown9.Value = readPokemon.ReadUInt16();
                        numericUpDown11.Value = readPokemon.ReadUInt16();
                        pokemon1 = readPokemon.ReadUInt16();
                        numericUpDown10.Value = pokemon1 / 1024;
                        comboBox19.SelectedIndex = pokemon1 - ((int)numericUpDown10.Value * 1024);
                        if (checkBoxItems.Checked == true)
                        {
                            comboBox18.SelectedIndex = readPokemon.ReadUInt16();
                        }
                        if (checkBoxAttacks.Checked == true)
                        {
                            comboBox23.SelectedIndex = readPokemon.ReadUInt16();
                            comboBox22.SelectedIndex = readPokemon.ReadUInt16();
                            comboBox21.SelectedIndex = readPokemon.ReadUInt16();
                            comboBox20.SelectedIndex = readPokemon.ReadUInt16();
                        }
                        if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                        {
                            readPokemon.BaseStream.Position += 0x2;
                        }
                        if (numericUpDown1.Value > 3)
                        {
                            numericUpDown12.Value = readPokemon.ReadUInt16();
                            numericUpDown14.Value = readPokemon.ReadUInt16();
                            pokemon1 = readPokemon.ReadUInt16();
                            numericUpDown13.Value = pokemon1 / 1024;
                            comboBox25.SelectedIndex = pokemon1 - ((int)numericUpDown13.Value * 1024);
                            if (checkBoxItems.Checked == true)
                            {
                                comboBox24.SelectedIndex = readPokemon.ReadUInt16();
                            }
                            if (checkBoxAttacks.Checked == true)
                            {
                                comboBox29.SelectedIndex = readPokemon.ReadUInt16();
                                comboBox28.SelectedIndex = readPokemon.ReadUInt16();
                                comboBox27.SelectedIndex = readPokemon.ReadUInt16();
                                comboBox26.SelectedIndex = readPokemon.ReadUInt16();
                            }
                            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                            {
                                readPokemon.BaseStream.Position += 0x2;
                            }
                            if (numericUpDown1.Value > 4)
                            {
                                numericUpDown15.Value = readPokemon.ReadUInt16();
                                numericUpDown17.Value = readPokemon.ReadUInt16();
                                pokemon1 = readPokemon.ReadUInt16();
                                numericUpDown16.Value = pokemon1 / 1024;
                                comboBox31.SelectedIndex = pokemon1 - ((int)numericUpDown16.Value * 1024);
                                if (checkBoxItems.Checked == true)
                                {
                                    comboBox30.SelectedIndex = readPokemon.ReadUInt16();
                                }
                                if (checkBoxAttacks.Checked == true)
                                {
                                    comboBox35.SelectedIndex = readPokemon.ReadUInt16();
                                    comboBox34.SelectedIndex = readPokemon.ReadUInt16();
                                    comboBox33.SelectedIndex = readPokemon.ReadUInt16();
                                    comboBox32.SelectedIndex = readPokemon.ReadUInt16();
                                }
                                if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                                {
                                    readPokemon.BaseStream.Position += 0x2;
                                }
                                if (numericUpDown1.Value > 5)
                                {
                                    numericUpDown18.Value = readPokemon.ReadUInt16();
                                    numericUpDown20.Value = readPokemon.ReadUInt16();
                                    pokemon1 = readPokemon.ReadUInt16();
                                    numericUpDown19.Value = pokemon1 / 1024;
                                    comboBox37.SelectedIndex = pokemon1 - ((int)numericUpDown19.Value * 1024);
                                    if (checkBoxItems.Checked == true)
                                    {
                                        comboBox36.SelectedIndex = readPokemon.ReadUInt16();
                                    }
                                    if (checkBoxAttacks.Checked == true)
                                    {
                                        comboBox41.SelectedIndex = readPokemon.ReadUInt16();
                                        comboBox40.SelectedIndex = readPokemon.ReadUInt16();
                                        comboBox39.SelectedIndex = readPokemon.ReadUInt16();
                                        comboBox38.SelectedIndex = readPokemon.ReadUInt16();
                                    }
                                }
                            }
                        }
                    }
                }
                readPokemon.Close();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage7);
            tabControl1.TabPages.Remove(tabPage8);
            tabControl1.TabPages.Remove(tabPage9);
            tabControl1.TabPages.Remove(tabPage10);
            checkBoxAttacks_CheckedChanged(null, null);
            if (numericUpDown1.Value > 1)
            {
                tabControl1.TabPages.Add(tabPage2);
                if (numericUpDown1.Value > 2)
                {
                    tabControl1.TabPages.Add(tabPage7);
                    if (numericUpDown1.Value > 3)
                    {
                        tabControl1.TabPages.Add(tabPage8);
                        if (numericUpDown1.Value > 4)
                        {
                            tabControl1.TabPages.Add(tabPage9);
                            if (numericUpDown1.Value > 5)
                            {
                                tabControl1.TabPages.Add(tabPage10);
                            }
                        }
                    }
                }
            }
            {
                if (numericUpDown1.Value > 1)
                {
                    checkBoxDouble.Enabled = true;
                }
                else
                {
                    checkBoxDouble.Checked = false;
                    checkBoxDouble.Enabled = false;
                }
            }
        }

        private void checkBoxAttacks_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAttacks.Checked == false)
            {
                comboBox6.Enabled = false;
                comboBox7.Enabled = false;
                comboBox9.Enabled = false;
                comboBox8.Enabled = false;
                comboBox17.Enabled = false;
                comboBox16.Enabled = false;
                comboBox15.Enabled = false;
                comboBox14.Enabled = false;
                comboBox23.Enabled = false;
                comboBox22.Enabled = false;
                comboBox21.Enabled = false;
                comboBox20.Enabled = false;
                comboBox29.Enabled = false;
                comboBox28.Enabled = false;
                comboBox27.Enabled = false;
                comboBox26.Enabled = false;
                comboBox35.Enabled = false;
                comboBox34.Enabled = false;
                comboBox33.Enabled = false;
                comboBox32.Enabled = false;
                comboBox41.Enabled = false;
                comboBox40.Enabled = false;
                comboBox39.Enabled = false;
                comboBox38.Enabled = false;

                if (checkBoxItems.Checked == false)
                {
                    comboBox11.Enabled = false;
                    comboBox12.Enabled = false;
                    comboBox18.Enabled = false;
                    comboBox24.Enabled = false;
                    comboBox30.Enabled = false;
                    comboBox36.Enabled = false;
                    return;
                }
                else
                {
                    comboBox11.Enabled = true;
                    comboBox12.Enabled = true;
                    comboBox18.Enabled = true;
                    comboBox24.Enabled = true;
                    comboBox30.Enabled = true;
                    comboBox36.Enabled = true;
                    return;
                }
            }


            if (checkBoxAttacks.Checked == true)
            {
                comboBox6.Enabled = true;
                comboBox7.Enabled = true;
                comboBox9.Enabled = true;
                comboBox8.Enabled = true;
                comboBox17.Enabled = true;
                comboBox16.Enabled = true;
                comboBox15.Enabled = true;
                comboBox14.Enabled = true;
                comboBox23.Enabled = true;
                comboBox22.Enabled = true;
                comboBox21.Enabled = true;
                comboBox20.Enabled = true;
                comboBox29.Enabled = true;
                comboBox28.Enabled = true;
                comboBox27.Enabled = true;
                comboBox26.Enabled = true;
                comboBox35.Enabled = true;
                comboBox34.Enabled = true;
                comboBox33.Enabled = true;
                comboBox32.Enabled = true;
                comboBox41.Enabled = true;
                comboBox40.Enabled = true;
                comboBox39.Enabled = true;
                comboBox38.Enabled = true;

                if (checkBoxItems.Checked == false)
                {
                    comboBox11.Enabled = false;
                    comboBox12.Enabled = false;
                    comboBox18.Enabled = false;
                    comboBox24.Enabled = false;
                    comboBox30.Enabled = false;
                    comboBox36.Enabled = false;
                    return;
                }
                else
                {
                    comboBox11.Enabled = true;
                    comboBox12.Enabled = true;
                    comboBox18.Enabled = true;
                    comboBox24.Enabled = true;
                    comboBox30.Enabled = true;
                    comboBox36.Enabled = true;
                    return;
                }
            }
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                Narc.FromFolder(Form1.workingFolder + @"data\poketool\trainer\trdata\").Save(Form1.workingFolder + @"data\poketool\trainer\trdata.narc");
                Narc.FromFolder(Form1.workingFolder + @"data\poketool\trainer\trpoke\").Save(Form1.workingFolder + @"data\poketool\trainer\trpoke.narc");
                Directory.Delete(Form1.workingFolder + @"data\poketool\trainer\trdata\", true);
                Directory.Delete(Form1.workingFolder + @"data\poketool\trainer\trpoke\", true);
            }
            else
            {
                Narc.FromFolder(Form1.workingFolder + @"data\a\0\5\trdata\").Save(Form1.workingFolder + @"data\a\0\5\5");
                Narc.FromFolder(Form1.workingFolder + @"data\a\0\5\trpoke\").Save(Form1.workingFolder + @"data\a\0\5\6");
                Directory.Delete(Form1.workingFolder + @"data\a\0\5\trdata\", true);
                Directory.Delete(Form1.workingFolder + @"data\a\0\5\trpoke\", true);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Add New Trainer
        {
            int newTrainerID = comboBox1.Items.Count;
            File.Create(trdataPath + "\\" + newTrainerID.ToString("D4")).Close();
            File.Create(trpokePath + "\\" + newTrainerID.ToString("D4")).Close();
            BinaryWriter writeNew = new BinaryWriter(File.OpenWrite(trdataPath + "\\" + newTrainerID.ToString("D4")));
            for (int i = 0; i < 0x14; i++)
            {
                writeNew.Write((byte)0x0);
            }
            writeNew.Close();
            comboBox1.Items.Add(newTrainerID + ": " + currentTrainerClass[0] + " ???");
            names.Add("???");
            saveText();
            comboBox1.SelectedIndex = newTrainerID;
        }

        private void button2_Click(object sender, EventArgs e) // Save Trainer
        {
            BinaryWriter writeTrainer = new BinaryWriter(File.OpenWrite(trdataPath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
            if (checkBoxAttacks.Checked == false)
            {
                if (checkBoxItems.Checked == false) writeTrainer.Write((byte)0x0);
                else writeTrainer.Write((byte)0x2);
            }
            else
            {
                if (checkBoxItems.Checked == false) writeTrainer.Write((byte)0x1);
                else writeTrainer.Write((byte)0x3);
            }
            writeTrainer.Write((byte)listBox1.SelectedIndex); // Trainer Class
            writeTrainer.BaseStream.Position += 0x1;
            writeTrainer.Write((byte)numericUpDown1.Value); // Amount of Pokémon
            writeTrainer.Write((UInt16)comboBox2.SelectedIndex); // Item 1
            writeTrainer.Write((UInt16)comboBox3.SelectedIndex); // Item 1
            writeTrainer.Write((UInt16)comboBox4.SelectedIndex); // Item 1
            writeTrainer.Write((UInt16)comboBox5.SelectedIndex); // Item 1
            writeTrainer.Write((UInt32)numericUpDown2.Value); // Artificial Intelligence
            if (checkBoxDouble.Checked == true) writeTrainer.Write((byte)0x2); // Double Battle
            else writeTrainer.Write((byte)0x0); // Single Battle
            writeTrainer.Close();

            File.Create(trpokePath + "\\" + comboBox1.SelectedIndex.ToString("D4")).Close();
            if (numericUpDown1.Value != 0)
            {
                BinaryWriter writePokemon = new BinaryWriter(File.OpenWrite(trpokePath + "\\" + comboBox1.SelectedIndex.ToString("D4")));
                writePokemon.Write((UInt16)numericUpDown5.Value);
                writePokemon.Write((UInt16)numericUpDown3.Value);
                writePokemon.Write((UInt16)(comboBox10.SelectedIndex + (numericUpDown4.Value * 1024)));
                if (checkBoxItems.Checked == true)
                {
                    writePokemon.Write((UInt16)comboBox11.SelectedIndex);
                }
                if (checkBoxAttacks.Checked == true)
                {
                    writePokemon.Write((UInt16)comboBox6.SelectedIndex);
                    writePokemon.Write((UInt16)comboBox7.SelectedIndex);
                    writePokemon.Write((UInt16)comboBox9.SelectedIndex);
                    writePokemon.Write((UInt16)comboBox8.SelectedIndex);
                }
                if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                {
                    writePokemon.Write((UInt16)0x0);
                }
                if (numericUpDown1.Value > 1)
                {
                    writePokemon.Write((UInt16)numericUpDown6.Value);
                    writePokemon.Write((UInt16)numericUpDown8.Value);
                    writePokemon.Write((UInt16)(comboBox13.SelectedIndex + (numericUpDown7.Value * 1024)));
                    if (checkBoxItems.Checked == true)
                    {
                        writePokemon.Write((UInt16)comboBox12.SelectedIndex);
                    }
                    if (checkBoxAttacks.Checked == true)
                    {
                        writePokemon.Write((UInt16)comboBox17.SelectedIndex);
                        writePokemon.Write((UInt16)comboBox16.SelectedIndex);
                        writePokemon.Write((UInt16)comboBox15.SelectedIndex);
                        writePokemon.Write((UInt16)comboBox14.SelectedIndex);
                    }
                    if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                    {
                        writePokemon.Write((UInt16)0x0);
                    }
                    if (numericUpDown1.Value > 2)
                    {
                        writePokemon.Write((UInt16)numericUpDown9.Value);
                        writePokemon.Write((UInt16)numericUpDown11.Value);
                        writePokemon.Write((UInt16)(comboBox19.SelectedIndex + (numericUpDown10.Value * 1024)));
                        if (checkBoxItems.Checked == true)
                        {
                            writePokemon.Write((UInt16)comboBox18.SelectedIndex);
                        }
                        if (checkBoxAttacks.Checked == true)
                        {
                            writePokemon.Write((UInt16)comboBox23.SelectedIndex);
                            writePokemon.Write((UInt16)comboBox22.SelectedIndex);
                            writePokemon.Write((UInt16)comboBox21.SelectedIndex);
                            writePokemon.Write((UInt16)comboBox20.SelectedIndex);
                        }
                        if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                        {
                            writePokemon.Write((UInt16)0x0);
                        }
                        if (numericUpDown1.Value > 3)
                        {
                            writePokemon.Write((UInt16)numericUpDown12.Value);
                            writePokemon.Write((UInt16)numericUpDown14.Value);
                            writePokemon.Write((UInt16)(comboBox25.SelectedIndex + (numericUpDown13.Value * 1024)));
                            if (checkBoxItems.Checked == true)
                            {
                                writePokemon.Write((UInt16)comboBox24.SelectedIndex);
                            }
                            if (checkBoxAttacks.Checked == true)
                            {
                                writePokemon.Write((UInt16)comboBox29.SelectedIndex);
                                writePokemon.Write((UInt16)comboBox28.SelectedIndex);
                                writePokemon.Write((UInt16)comboBox27.SelectedIndex);
                                writePokemon.Write((UInt16)comboBox26.SelectedIndex);
                            }
                            if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                            {
                                writePokemon.Write((UInt16)0x0);
                            }
                            if (numericUpDown1.Value > 4)
                            {
                                writePokemon.Write((UInt16)numericUpDown15.Value);
                                writePokemon.Write((UInt16)numericUpDown17.Value);
                                writePokemon.Write((UInt16)(comboBox31.SelectedIndex + (numericUpDown16.Value * 1024)));
                                if (checkBoxItems.Checked == true)
                                {
                                    writePokemon.Write((UInt16)comboBox30.SelectedIndex);
                                }
                                if (checkBoxAttacks.Checked == true)
                                {
                                    writePokemon.Write((UInt16)comboBox35.SelectedIndex);
                                    writePokemon.Write((UInt16)comboBox34.SelectedIndex);
                                    writePokemon.Write((UInt16)comboBox33.SelectedIndex);
                                    writePokemon.Write((UInt16)comboBox32.SelectedIndex);
                                }
                                if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                                {
                                    writePokemon.Write((UInt16)0x0);
                                }
                                if (numericUpDown1.Value > 5)
                                {
                                    writePokemon.Write((UInt16)numericUpDown18.Value);
                                    writePokemon.Write((UInt16)numericUpDown20.Value);
                                    writePokemon.Write((UInt16)(comboBox37.SelectedIndex + (numericUpDown19.Value * 1024)));
                                    if (checkBoxItems.Checked == true)
                                    {
                                        writePokemon.Write((UInt16)comboBox36.SelectedIndex);
                                    }
                                    if (checkBoxAttacks.Checked == true)
                                    {
                                        writePokemon.Write((UInt16)comboBox41.SelectedIndex);
                                        writePokemon.Write((UInt16)comboBox40.SelectedIndex);
                                        writePokemon.Write((UInt16)comboBox39.SelectedIndex);
                                        writePokemon.Write((UInt16)comboBox38.SelectedIndex);
                                    }
                                    if (Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043 || Form1.gameID == 0x454B5049 || Form1.gameID == 0x45475049 || Form1.gameID == 0x534B5049 || Form1.gameID == 0x53475049 || Form1.gameID == 0x464B5049 || Form1.gameID == 0x46475049 || Form1.gameID == 0x494B5049 || Form1.gameID == 0x49475049 || Form1.gameID == 0x444B5049 || Form1.gameID == 0x44475049 || Form1.gameID == 0x4A4B5049 || Form1.gameID == 0x4A475049 || Form1.gameID == 0x4B4B5049 || Form1.gameID == 0x4B475049)
                                    {
                                        writePokemon.Write((UInt16)0x0);
                                    }
                                }
                            }
                        }
                    }
                }
                writePokemon.Close();
            }
            int selectedID = comboBox1.SelectedIndex;
            int selectedClass = listBox1.SelectedIndex;
            names[comboBox1.SelectedIndex] = textBox1.Text;
            saveText();
            comboBox1.Items.RemoveAt(selectedID);
            comboBox1.Items.Insert(selectedID, selectedID + ": " + currentTrainerClass[selectedClass] + " " + names[selectedID]);
            comboBox1.SelectedIndex = selectedID;
        }

        private void button3_Click(object sender, EventArgs e) // Load Trainer Class Editor
        {
            int selectedID = listBox1.SelectedIndex;
            Form3_2_Trainer_Class_Editor trainerClassEditor = new Form3_2_Trainer_Class_Editor();
            trainerClassEditor.ShowDialog(this);
            listBox1.Items.Clear();
            #region Read Trainer Class Names
            BinaryReader readText = new System.IO.BinaryReader(File.OpenRead(textClassPath));
            readText.BaseStream.Position = 0x0;
            int stringClassCount = (int)readText.ReadUInt16();
            int initialKey = (int)readText.ReadUInt16();
            int key1 = (initialKey * 0x2FD) & 0xFFFF;
            int key2 = 0;
            int realKey = 0;
            bool specialCharON = false;
            int[] currentOffset = new int[stringClassCount];
            int[] currentSize = new int[stringClassCount];
            string[] currentClass = new string[stringClassCount];
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
                currentClass[i] = pokemonText;
            }
            readText.Close();
            #endregion
            #region Adds Trainer Classes to Lists
            currentTrainerClass = currentClass;
            for (int i = 0; i < stringClassCount; i++)
            {
                listBox1.Items.Add(i + ": " + currentClass[i]);
            }
            #endregion
            listBox1.SelectedIndex = selectedID;
        }


        private void saveText() // Save Text File
        {
            BinaryWriter writeText = new BinaryWriter(File.Create(textNamePath));
            writeText.Write((UInt16)names.Count);
            writeText.Write((UInt16)initialKey);
            int key = (initialKey * 0x2FD) & 0xFFFF;
            int key2 = 0;
            int realKey = 0;
            int offset = 0x4 + (names.Count * 8);
            int[] stringSize = new int[names.Count];
            for (int i = 0; i < names.Count; i++) // Reads and stores string offsets and sizes
            {
                key2 = (key * (i + 1) & 0xFFFF);
                realKey = key2 | (key2 << 16);
                writeText.Write(offset ^ realKey);
                int length = getStringLength(i);
                stringSize[i] = length;
                writeText.Write(length ^ realKey);
                offset += length * 2;
            }
            for (int i = 0; i < names.Count; i++) // Encodes strings and writes them to file
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
            try { currentMessage = names[stringIndex]; }
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
            try { currentMessage = names[stringIndex]; }
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
