using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form10 : Form
    {
        NSBMD_File nsbmd;

        public Form10()
        {
            InitializeComponent();
        }

        public struct NSBMD_File
        {
            public header Header;
            public struct header
            {
                public string ID;
                public byte[] Magic;
                public Int32 file_size;
                public Int16 header_size;
                public Int16 nSection;
                public Int32[] Section_Offset;
            }

            public mdl0 MDL0;
            public struct mdl0
            {
                public string ID;
                public Int32 Section_size;
                public byte Padding1;
                public byte Model_count;
                public Int16 Section2_size;
                public Int16 Constant;
                public Int16 Subsection_size;
                public Int32 Constant2;
                public Int32[] Unknown;
                public Int16 Constant3;
                public Int16 Data_section_size;
                public Int32[] Model_offset;
                public string[] Model_name;
            }

            public texInfo TexInfo;
            public struct texInfo
            {
                public byte dummy;
                public byte num_objs;
                public short section_size;
                public UnknownBlock unknownBlock;
                public Info infoBlock;
                public List<string> names;

                public struct UnknownBlock
                {
                    public short header_size;
                    public short section_size;
                    public int constant; // 0x017F

                    public List<short> unknown1;
                    public List<short> unknown2;
                }
                public struct Info
                {
                    public short header_size;
                    public short data_size;

                    public texInfo[] TexInfo;

                    public struct texInfo
                    {
                        public Int32 Texture_Offset; //shift << 3, relative to start of Texture Data
                        public Int16 Parameters;
                        public byte Width;
                        public byte Unknown1;
                        public byte Height;
                        public byte Unknown2;

                        public byte[] Image;
                        public byte[] spData;
                        // Parameters
                        public byte repeat_X;   // 0 = freeze; 1 = repeat
                        public byte repeat_Y;   // 0 = freeze; 1 = repeat
                        public byte flip_X;     // 0 = no; 1 = flip each 2nd texture (requires repeat)
                        public byte flip_Y;     // 0 = no; 1 = flip each 2nd texture (requires repeat)
                        public ushort width;      // 8 << width
                        public ushort height;     // 8 << height
                        public byte format;     // Texture format
                        public byte color0; // 0 = displayed; 1 = transparent
                        public byte coord_transf; // Texture coordination transformation mode

                        public byte depth;
                        public uint compressedDataStart;
                    }
                }
            }

            public palInfo PalInfo;
            public struct palInfo
            {
                public byte dummy;
                public byte num_objs;
                public short section_size;
                public UnknownBlock unknownBlock;
                public Info infoBlock;
                public List<string> names;

                public struct UnknownBlock
                {
                    public short header_size;
                    public short section_size;
                    public int constant; // 0x017F

                    public List<short> unknown1;
                    public List<short> unknown2;
                }
                public struct Info
                {
                    public short header_size;
                    public short data_size;

                    public palInfo[] PalInfo;

                    public struct palInfo
                    {
                        public Int32 Palette_Offset; //shift << 3, relative to start of Palette Data
                        public Int16 Color0;
                        public Color[] pal;
                    }
                }
            }
        }

        public void modelOpen()
        {
            string path = textBox1.Text;
            EndianBinaryReader readModel = new EndianBinaryReader(File.OpenRead(path), Endianness.LittleEndian);
            nsbmd = new NSBMD_File();
            if (readModel.ReadString(Encoding.ASCII, 4) != "BMD0")
            {
                readModel.Close();
                return;
            }
            else
            {
                nsbmd.Header.ID = "BMD0";
                nsbmd.Header.Magic = readModel.ReadBytes(4);
                nsbmd.Header.file_size = readModel.ReadInt32();
                nsbmd.Header.header_size = readModel.ReadInt16();
                nsbmd.Header.nSection = readModel.ReadInt16();
                nsbmd.Header.Section_Offset = new Int32[nsbmd.Header.nSection];
                for (int i = 0; i < nsbmd.Header.nSection; i++)
                {
                    nsbmd.Header.Section_Offset[i] = readModel.ReadInt32();
                }
                nsbmd.MDL0.ID = readModel.ReadString(Encoding.ASCII, 4);
                if (nsbmd.MDL0.ID != "MDL0")
                {
                    readModel.Close();
                    return;
                }

                nsbmd.MDL0.Section_size = readModel.ReadInt32();
                nsbmd.MDL0.Padding1 = readModel.ReadByte();
                nsbmd.MDL0.Model_count = readModel.ReadByte();
                nsbmd.MDL0.Section_size = readModel.ReadInt16();
                nsbmd.MDL0.Constant = readModel.ReadInt16();
                nsbmd.MDL0.Subsection_size = readModel.ReadInt16();
                nsbmd.MDL0.Constant2 = readModel.ReadInt32();
                nsbmd.MDL0.Unknown = new int[nsbmd.MDL0.Model_count];
                for (int i = 0; i < nsbmd.MDL0.Model_count; i++)
                {
                    nsbmd.MDL0.Unknown[i] = readModel.ReadInt32();
                }
                nsbmd.MDL0.Constant3 = readModel.ReadInt16();
                nsbmd.MDL0.Section2_size = readModel.ReadInt16();
                nsbmd.MDL0.Model_offset = new int[nsbmd.MDL0.Model_count];
                for (int i = 0; i < nsbmd.MDL0.Model_count; i++)
                {
                    nsbmd.MDL0.Model_offset[i] = readModel.ReadInt32();
                }
                nsbmd.MDL0.Model_name = new string[nsbmd.MDL0.Model_count];
                for (int i = 0; i < nsbmd.MDL0.Model_count; i++)
                {
                    nsbmd.MDL0.Model_name[i] = readModel.ReadString(Encoding.ASCII, 16);
                }
                readModel.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            modelOpen();
            StreamWriter write = new StreamWriter(File.Create(@"C:\dump.txt"));
            write.WriteLine("SDSME 2.0.0 Model Editor\r\n");
            write.WriteLine("Model '" + textBox1.Text + "' opened\r\n");

            write.WriteLine("Magic ID: " + nsbmd.Header.ID);
            write.WriteLine("Constant: " + (nsbmd.Header.Magic[0] + nsbmd.Header.Magic[1] + nsbmd.Header.Magic[2] + nsbmd.Header.Magic[3]));
            write.WriteLine("File size: " + nsbmd.Header.file_size);
            write.WriteLine("Header size: " + nsbmd.Header.header_size);
            write.WriteLine("Sections: " + nsbmd.Header.nSection);
            for (int i = 0; i < nsbmd.Header.nSection; i++)
            {
                write.WriteLine("Section " + (i + 1) + " offset: " + nsbmd.Header.Section_Offset[i]);
            }
            write.WriteLine("\r\nMagic ID: " + nsbmd.MDL0.ID);
            write.WriteLine("Section size: " + nsbmd.MDL0.Section_size);
            write.WriteLine("Padding: " + nsbmd.MDL0.Padding1);
            write.WriteLine("Number of models: " + nsbmd.MDL0.Model_count);
            write.WriteLine("Section 2 size: " + nsbmd.MDL0.Section2_size);
            write.WriteLine("Constant: " + nsbmd.MDL0.Constant);
            write.WriteLine("Subsection size: " + nsbmd.MDL0.Subsection_size);
            write.WriteLine("Constant: " + nsbmd.MDL0.Constant2);
            write.WriteLine("\r");
            for (int i = 0; i < nsbmd.MDL0.Model_count; i++)
            {
                write.WriteLine("Model " + (i + 1) + " unknown data: " + nsbmd.MDL0.Unknown[i]);
            }
            write.WriteLine("\r\nConstant: " + nsbmd.MDL0.Constant3);
            write.WriteLine("Data section size: " + nsbmd.MDL0.Data_section_size);
            write.WriteLine("\r");
            for (int i = 0; i < nsbmd.MDL0.Model_count; i++)
            {
                write.WriteLine("Model " + (i + 1) + " offset: " + nsbmd.MDL0.Model_offset[i]);
            }
            write.WriteLine("\r");
            for (int i = 0; i < nsbmd.MDL0.Model_count; i++)
            {
                write.WriteLine("Model " + (i + 1) + " name: " + nsbmd.MDL0.Model_name[i]);
            }
            write.Close();
        }
    }
}
