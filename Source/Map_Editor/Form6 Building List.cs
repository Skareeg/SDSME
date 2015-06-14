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

namespace WindowsFormsApplication1
{
    public partial class Form6_Building_List : Form
    {
        ResourceManager rm = new ResourceManager("WindowsFormsApplication1.WinFormStrings", Assembly.GetExecutingAssembly());

        public string editorTileset;
        public string animationID;
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

        public Form6_Building_List()
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
            if (Form1.gameID == 0x45414441 || Form1.gameID == 0x45415041 || Form1.gameID == 0x53414441 || Form1.gameID == 0x53415041 || Form1.gameID == 0x46414441 || Form1.gameID == 0x46415041 || Form1.gameID == 0x49414441 || Form1.gameID == 0x49415041 || Form1.gameID == 0x44414441 || Form1.gameID == 0x44415041 || Form1.gameID == 0x4A414441 || Form1.gameID == 0x4A415041 || Form1.gameID == 0x4B414441 || Form1.gameID == 0x4B415041 || Form1.gameID == 0x45555043 || Form1.gameID == 0x53555043 || Form1.gameID == 0x46555043 || Form1.gameID == 0x49555043 || Form1.gameID == 0x44555043 || Form1.gameID == 0x4A555043 || Form1.gameID == 0x4B555043)
            {
                bldPath = Form1.workingFolder + @"data\fielddata\build_model\build_model\";
                editorTileset = Form1.workingFolder + @"data\fielddata\areadata\area_build_model\areabm_texset";
            }
            else
            {
                checkBox1.Enabled = true;
                bldPath = Form1.workingFolder + @"data\a\0\4\building\";
                editorTileset = Form1.workingFolder + @"data\a\0\7\textureBld";
            }
            int bldCount = Directory.GetFiles(bldPath).Length;
            string bldName;
            for (int i = 0; i < bldCount; i++)
            {
                bldName = "";
                System.IO.BinaryReader read = new System.IO.BinaryReader(File.OpenRead(bldPath + i.ToString("D4")));
                read.BaseStream.Position = 0x14;
                if (read.ReadByte() + (read.ReadByte()<<8)+(read.ReadByte()<<16)+(read.ReadByte()<<24) == 0x304C444D)
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
                listBox1.Items.Add(i.ToString("D3") + ": " + bldName);
                read.Close();
            }
            comboBox1.Items.Clear();
            comboBox1.Items.Add(rm.GetString("internalTextures"));
            for (int i = 0; i < Form1.bldTexturesCount; i++)
            {
                comboBox1.Items.Add(rm.GetString("buildingPackList") + i);
            }
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fileStream = new FileStream(bldPath + "\\" + listBox1.SelectedIndex.ToString("D4"), FileMode.Open);
            file_2 = bldPath + "\\" + listBox1.SelectedIndex.ToString("D4");
            _nsbmd = NsbmdLoader.LoadNsbmd(fileStream);
            #region Create texture
            File.Create(Path.GetTempPath() + "BLDtexture.nsbtx").Close();
            BinaryWriter writeBLDtexture = new BinaryWriter(File.OpenWrite(Path.GetTempPath() + "BLDtexture.nsbtx"));
            writeBLDtexture.Write((UInt32)0x30585442);
            writeBLDtexture.Write((UInt32)0x0001FEFF);
            fileStream.Position = 0x8;
            int nsbmdSize = fileStream.ReadByte() + (fileStream.ReadByte() << 8) + (fileStream.ReadByte() << 16) + (fileStream.ReadByte() << 24);
            fileStream.Position = 0x14;
            int nsbmdTexOffset = fileStream.ReadByte() + (fileStream.ReadByte() << 8) + (fileStream.ReadByte() << 16) + (fileStream.ReadByte() << 24);
            int texSize = nsbmdSize - nsbmdTexOffset + 0x14;
            writeBLDtexture.Write((UInt32)texSize);
            writeBLDtexture.Write((UInt32)0x00010010);
            writeBLDtexture.Write((UInt32)0x00000014);
            fileStream.Position = nsbmdTexOffset;
            for (int i = 0; i < texSize - 0x14; i++)
            {
                writeBLDtexture.Write((byte)fileStream.ReadByte());
            }
            fileStream.Close();
            writeBLDtexture.Close();
            _nsbmd.materials = LibNDSFormats.NSBTX.NsbtxLoader.LoadNsbtx(new MemoryStream(File.ReadAllBytes(Path.GetTempPath() + "BLDtexture.nsbtx")), out _nsbmd.Textures, out _nsbmd.Palettes);
            #endregion
            try
            {
                _nsbmd.MatchTextures();
            }
            catch { }
            RenderBuilding(null, null);
            comboBox1.SelectedIndex = 0;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (checkBox1.Checked == true)
            {
                NarcAPI.Narc.Open(Form1.workingFolder + @"data\a\1\4\8").ExtractToFolder(Form1.workingFolder + @"data\a\1\4\building\");
                bldPath = Form1.workingFolder + @"data\a\1\4\building\";
            }
            else
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\a\1\4\building\").Save(Form1.workingFolder + @"data\a\1\4\8");
                bldPath = Form1.workingFolder + @"data\a\0\4\building\";
            }
            int bldCount = Directory.GetFiles(bldPath).Length;
            string bldName;
            for (int i = 0; i < bldCount; i++)
            {
                bldName = "";
                System.IO.BinaryReader read = new System.IO.BinaryReader(File.OpenRead(bldPath + i.ToString("D4")));
                read.BaseStream.Position = 0x14;
                if (read.ReadByte() + (read.ReadByte() << 8) + (read.ReadByte() << 16) + (read.ReadByte() << 24) == 0x304C444D)
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
                listBox1.Items.Add(i.ToString("D3") + ": " + bldName);
                read.Close();
            }
            listBox1.SelectedIndex = 0;
        }

        private void Form6_Building_List_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Directory.Exists(Form1.workingFolder + @"data\a\1\4\building\"))
            {
                NarcAPI.Narc.FromFolder(Form1.workingFolder + @"data\a\1\4\building\").Save(Form1.workingFolder + @"data\a\1\4\8");
                Directory.Delete(Form1.workingFolder + @"data\a\1\4\building\", true);
            }
        }

        private void exportBtn_Click(object sender, EventArgs e)
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
                File.Copy(bldPath + "\\" + listBox1.SelectedIndex.ToString("D4"),ef.FileName, true);
            }
            return;
        }

        private void importBtn_Click(object sender, EventArgs e)
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
                    modelStream.Close();
                    File.Copy(ifModel.FileName, bldPath + "\\" + listBox1.SelectedIndex.ToString("D4"), true);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            renderer.ClearOBJ();
            if (comboBox1.SelectedIndex == 0)
            {
                var fileStream = new FileStream(bldPath + "\\" + listBox1.SelectedIndex.ToString("D4"), FileMode.Open);
                file_2 = bldPath + "\\" + listBox1.SelectedIndex.ToString("D4");
                _nsbmd = NsbmdLoader.LoadNsbmd(fileStream);
                fileStream.Close();
                _nsbmd.materials = LibNDSFormats.NSBTX.NsbtxLoader.LoadNsbtx(new MemoryStream(File.ReadAllBytes(Path.GetTempPath() + "BLDtexture.nsbtx")), out _nsbmd.Textures, out _nsbmd.Palettes);
                try
                {
                    _nsbmd.MatchTextures();
                }
                catch { }
                simpleOpenGlControl1.Invalidate();
                RenderBuilding(null, null);
            }
            else
            {
                _nsbmd.models[0].Palettes.Clear();
                _nsbmd.models[0].Textures.Clear();
                var fileStream = new FileStream(bldPath + "\\" + listBox1.SelectedIndex.ToString("D4"), FileMode.Open);
                file_2 = bldPath + "\\" + listBox1.SelectedIndex.ToString("D4");
                _nsbmd = NsbmdLoader.LoadNsbmd(fileStream);
                fileStream.Close();
                _nsbmd.materials = LibNDSFormats.NSBTX.NsbtxLoader.LoadNsbtx(new MemoryStream(File.ReadAllBytes(editorTileset + "\\" + (comboBox1.SelectedIndex - 1).ToString("D4"))), out _nsbmd.Textures, out _nsbmd.Palettes);
                try
                {
                    _nsbmd.MatchTextures();
                }
                catch { }
                simpleOpenGlControl1.Invalidate();
                RenderBuilding(null, null);
            }
        }
    }
}
