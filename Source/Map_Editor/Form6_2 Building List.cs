using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Map_Converter;
using Tao.OpenGl;
using LibNDSFormats.NSBMD;
using LibNDSFormats.NSBTX;
using System.Reflection;
using System.Resources;
using MKDS_Course_Editor.NSBTA;
using MKDS_Course_Editor.NSBTP;
using MKDS_Course_Editor.NSBCA;
using NarcAPI;
using AB_API;

namespace WindowsFormsApplication1
{
    public partial class Form6_2_Building_List : Form
    {
        ResourceManager rm = new ResourceManager("WindowsFormsApplication1.WinFormStrings", Assembly.GetExecutingAssembly());

        public string editorTileset;
        public static string bldPath;
        public string file_2 = "";
        public static float ang = 0.0f;
        public static float dist = 25.0f;
        public static float elev = 50.0f;
        private static NsbmdGlRenderer renderer = new NsbmdGlRenderer();
        private static Nsbmd _nsbmd;
        MKDS_Course_Editor.NSBTA.NSBTA.NSBTA_File ani;
        MKDS_Course_Editor.NSBTP.NSBTP.NSBTP_File tp;
        MKDS_Course_Editor.NSBCA.NSBCA.NSBCA_File ca;

        public Form6_2_Building_List()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            simpleOpenGlControl1.MouseWheel += new MouseEventHandler(simpleOpenGlControl1_MouseWheel);
            Render();
        }

        private void Form6_Building_List_Load(object sender, EventArgs e)
        {
            Form2 bldEditor = new Form2();
            bldEditor.Show(this);
            if (Form1.isBW)
            {
                Narc.Open(Form1.workingFolder + @"data\a\2\2\9").ExtractToFolder(Form1.workingFolder + @"data\a\2\2\exBld");
                Narc.Open(Form1.workingFolder + @"data\a\2\3\0").ExtractToFolder(Form1.workingFolder + @"data\a\2\3\inBld");
            }
            else
            {
                Narc.Open(Form1.workingFolder + @"data\a\2\2\5").ExtractToFolder(Form1.workingFolder + @"data\a\2\2\exBld");
                Narc.Open(Form1.workingFolder + @"data\a\2\2\6").ExtractToFolder(Form1.workingFolder + @"data\a\2\2\inBld");
            }
            bldPath = Form1.workingFolder + @"data\a\2\2\exBld";
            for (int i = 0; i < Directory.GetFiles(bldPath).Length; i++)
            {
                comboBox1.Items.Add(rm.GetString("buildingPackList") + i);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            AB.Extract(bldPath + "\\" + comboBox1.SelectedIndex.ToString("D4"), bldPath);
            for (int i = 0; i < Directory.GetFiles(bldPath + "\\" + "header").Length; i++)
            {
                string bldName = "";
                System.IO.BinaryReader readID = new System.IO.BinaryReader(File.OpenRead(bldPath + "\\" + "header" + "\\" + i.ToString("D4")));
                System.IO.BinaryReader read = new System.IO.BinaryReader(File.OpenRead(bldPath + "\\" + "model" + "\\" + i.ToString("D4")));
                read.BaseStream.Position = 0x14;
                if (read.ReadUInt32() == 0x304C444D)
                {
                    read.BaseStream.Position = 0x34;
                }
                else read.BaseStream.Position = 0x38;
                for (int nameLength = 0; nameLength < 16; nameLength++)
                {
                    int currentByte = read.ReadByte();
                    byte[] mapBytes = new Byte[] { Convert.ToByte(currentByte) }; // Reads map name
                    if (currentByte != 0) bldName = bldName + Encoding.UTF8.GetString(mapBytes);
                }
                listBox1.Items.Add(i.ToString("D2") + ": " + bldName + " (" + readID.ReadInt16() + ")");
                read.Close();
                readID.Close();
            }
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fileStream = new FileStream(bldPath + "\\" + "model" + "\\" + listBox1.SelectedIndex.ToString("D4"), FileMode.Open);
            file_2 = bldPath + "\\" + "model" + "\\" + listBox1.SelectedIndex.ToString("D4");
            _nsbmd = NsbmdLoader.LoadNsbmd(fileStream);
            if (!checkBox1.Checked)
            {
                _nsbmd.materials = LibNDSFormats.NSBTX.NsbtxLoader.LoadNsbtx(new MemoryStream(File.ReadAllBytes(Form1.workingFolder + @"data\a\1\7\bldtilesets" + "\\" + comboBox1.SelectedIndex.ToString("D4"))), out _nsbmd.Textures, out _nsbmd.Palettes);
            }
            else
            {
                _nsbmd.materials = LibNDSFormats.NSBTX.NsbtxLoader.LoadNsbtx(new MemoryStream(File.ReadAllBytes(Form1.workingFolder + @"data\a\1\7\bld2tilesets" + "\\" + comboBox1.SelectedIndex.ToString("D4"))), out _nsbmd.Textures, out _nsbmd.Palettes);
            }
            try
            {
                _nsbmd.MatchTextures();
            }
            catch { }
            RenderBuilding(null, null);
            fileStream.Close();
            System.IO.BinaryReader readHeader = new System.IO.BinaryReader(File.OpenRead(bldPath + "\\" + "header" + "\\" + listBox1.SelectedIndex.ToString("D4")));
            numericUpDown1.Value = readHeader.ReadUInt16(); // ID
            readHeader.BaseStream.Position += 2;
            numericUpDown2.Value = readHeader.ReadUInt16(); // Door ID
            numericUpDown3.Value = readHeader.ReadInt16(); // X
            numericUpDown4.Value = readHeader.ReadInt16(); // Y
            numericUpDown5.Value = readHeader.ReadInt16(); // Z
            readHeader.Close();
        }

        private void button1_Click(object sender, EventArgs e) // Save
        {
            if (!checkBox1.Checked)
            {
                AB.Pack(Form1.workingFolder + @"data\a\2\2\exBld", Form1.workingFolder + @"data\a\2\2\exBld" + "\\" + comboBox1.SelectedIndex.ToString("D4"));
            }
            else
            {
                if (Form1.isBW)
                {
                    AB.Pack(Form1.workingFolder + @"data\a\2\3\inBld", Form1.workingFolder + @"data\a\2\3\inBld" + "\\" + comboBox1.SelectedIndex.ToString("D4"));
                }
                else
                {
                    AB.Pack(Form1.workingFolder + @"data\a\2\2\inBld", Form1.workingFolder + @"data\a\2\2\inBld" + "\\" + comboBox1.SelectedIndex.ToString("D4"));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) // Write Header
        {
            BinaryWriter writeHeader = new BinaryWriter(File.OpenWrite(bldPath + "\\" + "header" + "\\" + listBox1.SelectedIndex.ToString("D4")));
            writeHeader.Write((UInt16)numericUpDown1.Value);
            writeHeader.BaseStream.Position += 2;
            writeHeader.Write((UInt16)numericUpDown2.Value);
            writeHeader.Write((Int16)numericUpDown3.Value);
            writeHeader.Write((Int16)numericUpDown4.Value);
            writeHeader.Write((Int16)numericUpDown5.Value);
            writeHeader.Close();
            int index = listBox1.SelectedIndex;
            button1_Click(null, null);
            comboBox1_SelectedIndexChanged(null, null);
            listBox1.SelectedIndex = index;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            if (checkBox1.Checked)
            {
                if (Form1.isBW)
                {
                    bldPath = Form1.workingFolder + @"data\a\2\3\inBld";
                }
                else
                {
                    bldPath = Form1.workingFolder + @"data\a\2\2\inBld";
                }
            }
            else
            {
                bldPath = Form1.workingFolder + @"data\a\2\2\exBld";
            }
            for (int i = 0; i < Directory.GetFiles(bldPath).Length; i++)
            {
                comboBox1.Items.Add("AB " + i);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void exportBtn_Click(object sender, EventArgs e) // Export model
        {
            SaveFileDialog ef = new SaveFileDialog();
            ef.Title = rm.GetString("exportModelBld");
            ef.Filter = rm.GetString("modelFile");
            if (ef.ShowDialog() == DialogResult.OK)
            {
                if (ef.FileName.EndsWith(".obj"))
                {
                    try
                    {
                        NsbmdGlRenderer rendererOBJ = new NsbmdGlRenderer
                        {
                            Model = _nsbmd.models[0]
                        };
                        rendererOBJ.RipModel(ef.FileName);
                    }
                    catch
                    {
                        MessageBox.Show(rm.GetString("invalidFile"), null, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    return;
                }
                BinaryWriter export = new BinaryWriter(File.Create(ef.FileName));
                BinaryReader read = new BinaryReader(File.OpenRead(bldPath + "\\" + "model" + "\\" + listBox1.SelectedIndex.ToString("D4")));
                for (int i = 0; i < 0xe; i++)
                {
                    export.Write(read.ReadByte()); // Reads header bytes and writes them to file
                }
                export.Write((Int16)0x0002); // Writes texture flag
                export.Write((Int16)0x0018);
                export.Write((Int16)0x0);
                export.Write(0x0); // Writes blank BTX offset
                read.BaseStream.Position += 0x6;
                for (int i = 0; i < read.BaseStream.Length - 0x14; i++)
                {
                    export.Write(read.ReadByte()); // Writes model section
                }
                read.Close();
                string tex;
                if (!checkBox1.Checked)
                {
                    tex= Form1.workingFolder + @"data\a\1\7\bldtilesets" + "\\" + comboBox1.SelectedIndex.ToString("D4");
                }
                else
                {
                    tex = Form1.workingFolder + @"data\a\1\7\bld2tilesets" + "\\" + comboBox1.SelectedIndex.ToString("D4");
                }
                System.IO.BinaryReader readTex = new System.IO.BinaryReader(File.OpenRead(tex));
                long texLength = readTex.BaseStream.Length - 0x14;
                readTex.BaseStream.Position = 0x14;
                long texOffset = export.BaseStream.Position;
                for (int i = 0; i < texLength; i++)
                {
                    export.Write(readTex.ReadByte()); // Writes BTX section
                }
                export.BaseStream.Position = 0x8;
                export.Write((int)(new FileInfo(bldPath + "\\" + "model" + "\\" + listBox1.SelectedIndex.ToString("D4")).Length + 0x4 + texLength));
                export.BaseStream.Position = 0x14;
                export.Write((int)texOffset);
                export.Close();
                readTex.Close();
            }
            return;
        }

        private void importBtn_Click(object sender, EventArgs e) // Import model
        {
            OpenFileDialog ifModel = new OpenFileDialog();
            ifModel.Title = rm.GetString("selectModel");
            ifModel.Filter = rm.GetString("importModelFile");
            if (ifModel.ShowDialog() == DialogResult.OK)
            {
                System.IO.BinaryReader modelStream = new System.IO.BinaryReader(File.OpenRead(ifModel.FileName));
                string importModel = ifModel.FileName;
                long importnsbmdSize = new System.IO.FileInfo(ifModel.FileName).Length;
                int header;
                header = (int)modelStream.ReadUInt32();
                if (header == 809782594)
                {
                    modelStream.BaseStream.Position = 0x18;
                    header = (int)modelStream.ReadUInt32();
                    if (header == 810304589)
                    {
                        MessageBox.Show(rm.GetString("embeddedTextures"));
                        System.IO.BinaryWriter write = new System.IO.BinaryWriter(File.Create(bldPath + "\\" + "model" + "\\" + listBox1.SelectedIndex.ToString("D4")));
                        modelStream.BaseStream.Position = 0x14;
                        importnsbmdSize = (int)(modelStream.ReadUInt32() - 0x4);
                        write.Write((int)809782594);
                        write.Write((int)0x02FEFF);
                        write.Write((int)importnsbmdSize);
                        write.Write((int)0x010010);
                        write.Write((int)0x14);
                        for (int i = 0; i < importnsbmdSize - 0x14; i++)
                        {
                            write.Write(modelStream.ReadByte()); // Reads import file bytes and writes them to the main file
                        }
                        modelStream.Close();
                        write.Close();
                    }
                    else
                    {
                        modelStream.Close();
                        File.Copy(ifModel.FileName, bldPath + "\\" + "model" + "\\" + listBox1.SelectedIndex.ToString("D4"), true);
                    }
                    int index = listBox1.SelectedIndex;
                    button1_Click(null, null);
                    comboBox1_SelectedIndexChanged(null, null);
                    listBox1.SelectedIndex = index;
                }
                else
                {
                    MessageBox.Show(rm.GetString("invalidFile"), null, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    modelStream.Close();
                }
            }
        }

        public void CloseForm()
        {
            this.Close();
            if (Form1.isBW)
            {
                if (Directory.Exists(Form1.workingFolder + @"data\a\2\3\inBld\header"))
                {
                    Directory.Delete(Form1.workingFolder + @"data\a\2\3\inBld\header", true);
                    Directory.Delete(Form1.workingFolder + @"data\a\2\3\inBld\model", true);
                }
                Directory.Delete(Form1.workingFolder + @"data\a\2\2\exBld\header", true);
                Directory.Delete(Form1.workingFolder + @"data\a\2\2\exBld\model", true);
                Narc.FromFolder(Form1.workingFolder + @"data\a\2\2\exBld").Save(Form1.workingFolder + @"data\a\2\2\9");
                Narc.FromFolder(Form1.workingFolder + @"data\a\2\3\inBld").Save(Form1.workingFolder + @"data\a\2\3\0");
                Directory.Delete(Form1.workingFolder + @"data\a\2\2\exBld", true);
                Directory.Delete(Form1.workingFolder + @"data\a\2\3\inBld", true);
            }
            else
            {
                if (Directory.Exists(Form1.workingFolder + @"data\a\2\2\inBld\header"))
                {
                    Directory.Delete(Form1.workingFolder + @"data\a\2\2\inBld\header", true);
                    Directory.Delete(Form1.workingFolder + @"data\a\2\2\inBld\model", true);
                }
                Directory.Delete(Form1.workingFolder + @"data\a\2\2\exBld\header", true);
                Directory.Delete(Form1.workingFolder + @"data\a\2\2\exBld\model", true);
                Narc.FromFolder(Form1.workingFolder + @"data\a\2\2\exBld").Save(Form1.workingFolder + @"data\a\2\2\5");
                Narc.FromFolder(Form1.workingFolder + @"data\a\2\2\inBld").Save(Form1.workingFolder + @"data\a\2\2\6");
                Directory.Delete(Form1.workingFolder + @"data\a\2\2\exBld", true);
                Directory.Delete(Form1.workingFolder + @"data\a\2\2\inBld", true);
            }
        }

        #region 3D Viewer

        #region Properties (1)

        public Tao.Platform.Windows.SimpleOpenGlControl OpenGLControl
        {
            get { return this.simpleOpenGlControl1; }
        }

        #endregion Properties

        #region Delegates and Events (1)

        // Events (1) 

        public event Action RenderScene;

        #endregion Delegates and Events

        public int mouseX;
        public int mouseY;
        public int screenX = 0;
        public int screenY = 0;
        int[] aniframeS = new int[0];
        int[] aniframeT = new int[0];
        int[] aniframeR = new int[0];
        int[] aniScaleS = new int[0];
        int[] aniScaleT = new int[0];

        private void RenderBuilding(object sender, EventArgs e) // Render 3D Model
        {
            simpleOpenGlControl1.Invalidate();
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_RESCALE_NORMAL);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glDisable(Gl.GL_CULL_FACE);
            Gl.glFrontFace(Gl.GL_CW);
            Gl.glClearDepth(1);
            Gl.glEnable(Gl.GL_ALPHA_TEST);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glAlphaFunc(Gl.GL_GREATER, 0f);
            Gl.glClearColor(51f / 255f, 51f / 255f, 51f / 255f, 1f);
            Gl.glViewport(0, 0, simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);
            float aspect = (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height;//(vp[2] - vp[0]) / (vp[3] - vp[1]);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(45, aspect, 0.02f, 1000.0f);//0.02f, 32.0f);
            Gl.glTranslatef(0, 0, -dist);
            Gl.glRotatef(elev, 1, 0, 0);
            Gl.glRotatef(ang, 0, 1, 0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glTranslatef(0, 0, -dist);
            Gl.glRotatef(elev, 1, 0, 0);
            Gl.glRotatef(-ang, 0, 1, 0);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT2, Gl.GL_POSITION, new float[] { 1, 1, 1, 0 });
            Gl.glLightfv(Gl.GL_LIGHT3, Gl.GL_POSITION, new float[] { 1, 1, 1, 0 });
            Gl.glLoadIdentity();
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
            Gl.glColor3f(1.0f, 1.0f, 1.0f);
            Gl.glDepthMask(Gl.GL_TRUE);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            try
            {

                var mod = _nsbmd.models[0];
                if (mod == null)
                    return;
                renderer.Model = _nsbmd.models[0];
                renderer.RenderModel(file_2, ani, aniframeS, aniframeT, aniframeR, aniScaleS, aniScaleT, ca, false, -1, 0.0f, 0.0f, dist, elev, ang, true, tp, _nsbmd);
            }

            catch (Exception ex)
            {
            }
        }

        private void Render()
        {
            Gl.glViewport(0, 0, Width, Height);
            var vp = new[] { 0f, 0f, 0f, 0f };
            Gl.glGetFloatv(Gl.GL_VIEWPORT, vp);
            float aspect = (vp[2] - vp[0]) / (vp[3] - vp[1]);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(90.0f, aspect, 0.02f, 32.0f);
            Gl.glTranslatef(0, 0, -dist);
            Gl.glRotatef(elev, 1, 0, 0);
            Gl.glRotatef(ang, 0, 1, 0);

            if (RenderScene != null)
                RenderScene.Invoke();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        void simpleOpenGlControl1_MouseWheel(object sender, MouseEventArgs e) // Zoom In/Out
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                dist += (float)e.Delta / 200;
            }
            else
            {
                dist -= (float)e.Delta / 200;
            }
            simpleOpenGlControl1.Invalidate();
            RenderBuilding(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e) // Mouse Control
        {
            ang -= mouseX - Cursor.Position.X;
            elev -= mouseY - Cursor.Position.Y;
            simpleOpenGlControl1.Invalidate();
            RenderBuilding(null, null);
            Cursor.Position = new Point(mouseX, mouseY);
        }

        private void simpleOpenGlControl1_MouseDown(object sender, MouseEventArgs e) // Begin Mouse Control
        {
            timer1.Enabled = true;
            Cursor.Hide();
            screenX = Screen.PrimaryScreen.WorkingArea.Width;
            screenY = Screen.PrimaryScreen.WorkingArea.Height;
            mouseX = Cursor.Position.X;
            mouseY = Cursor.Position.Y;
            timer1.Start();
        }

        private void simpleOpenGlControl1_MouseUp(object sender, MouseEventArgs e) // End Mouse Control
        {
            timer1.Stop();
            Cursor.Show();
        }

        private void simpleOpenGlControl1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) // 3D Navigation
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    ang += 1;
                    simpleOpenGlControl1.Invalidate();
                    RenderBuilding(null, null);
                    break;
                case Keys.A:
                    ang -= 1;
                    simpleOpenGlControl1.Invalidate();
                    RenderBuilding(null, null);
                    break;
                case Keys.W:
                    elev += 1;
                    simpleOpenGlControl1.Invalidate();
                    RenderBuilding(null, null);
                    break;
                case Keys.S:
                    elev -= 1;
                    simpleOpenGlControl1.Invalidate();
                    RenderBuilding(null, null);
                    break;
                case Keys.Add:
                    dist -= 10.0f / 100;
                    simpleOpenGlControl1.Invalidate();
                    RenderBuilding(null, null);
                    break;
                case Keys.Subtract:
                    dist += 10.0f / 100;
                    simpleOpenGlControl1.Invalidate();
                    RenderBuilding(null, null);
                    break;
            }
        }

        #endregion

    }
}
