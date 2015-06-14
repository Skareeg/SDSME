namespace WindowsFormsApplication1
{
    partial class Form6_Building_List
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form6_Building_List));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.simpleOpenGlControl1 = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.exportBtn = new System.Windows.Forms.Button();
            this.importBtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // simpleOpenGlControl1
            // 
            this.simpleOpenGlControl1.AccumBits = ((byte)(0));
            this.simpleOpenGlControl1.AutoCheckErrors = false;
            this.simpleOpenGlControl1.AutoFinish = false;
            this.simpleOpenGlControl1.AutoMakeCurrent = true;
            this.simpleOpenGlControl1.AutoSwapBuffers = true;
            this.simpleOpenGlControl1.BackColor = System.Drawing.Color.Black;
            this.simpleOpenGlControl1.ColorBits = ((byte)(32));
            this.simpleOpenGlControl1.DepthBits = ((byte)(24));
            resources.ApplyResources(this.simpleOpenGlControl1, "simpleOpenGlControl1");
            this.simpleOpenGlControl1.Name = "simpleOpenGlControl1";
            this.simpleOpenGlControl1.StencilBits = ((byte)(0));
            this.simpleOpenGlControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.simpleOpenGlControl1_Paint);
            this.simpleOpenGlControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.simpleOpenGlControl1_MouseDown);
            this.simpleOpenGlControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.simpleOpenGlControl1_MouseUp);
            this.simpleOpenGlControl1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.simpleOpenGlControl1_PreviewKeyDown);
            // 
            // exportBtn
            // 
            resources.ApplyResources(this.exportBtn, "exportBtn");
            this.exportBtn.Name = "exportBtn";
            this.exportBtn.UseVisualStyleBackColor = true;
            this.exportBtn.Click += new System.EventHandler(this.exportBtn_Click);
            // 
            // importBtn
            // 
            resources.ApplyResources(this.importBtn, "importBtn");
            this.importBtn.Name = "importBtn";
            this.importBtn.UseVisualStyleBackColor = true;
            this.importBtn.Click += new System.EventHandler(this.importBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Form6_Building_List
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.importBtn);
            this.Controls.Add(this.exportBtn);
            this.Controls.Add(this.simpleOpenGlControl1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form6_Building_List";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form6_Building_List_FormClosed);
            this.Load += new System.EventHandler(this.Form6_Building_List_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private Tao.Platform.Windows.SimpleOpenGlControl simpleOpenGlControl1;
        private System.Windows.Forms.Button exportBtn;
        private System.Windows.Forms.Button importBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}