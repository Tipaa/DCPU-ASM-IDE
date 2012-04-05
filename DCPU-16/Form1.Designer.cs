namespace DCPU_16
{
    partial class Window
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
            this.menustrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceFiledcpuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compiledProgram0x10cToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compiledProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compilerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualBasicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assemblerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pseudocodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decompileToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assemblerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.macrosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redditOnlineHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menustrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menustrip
            // 
            this.menustrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.commandsToolStripMenuItem,
            this.compilerToolStripMenuItem,
            this.macrosToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menustrip.Location = new System.Drawing.Point(0, 0);
            this.menustrip.Name = "menustrip";
            this.menustrip.Size = new System.Drawing.Size(584, 24);
            this.menustrip.TabIndex = 0;
            this.menustrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceFiledcpuToolStripMenuItem,
            this.compiledProgram0x10cToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New...";
            // 
            // sourceFiledcpuToolStripMenuItem
            // 
            this.sourceFiledcpuToolStripMenuItem.Name = "sourceFiledcpuToolStripMenuItem";
            this.sourceFiledcpuToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.sourceFiledcpuToolStripMenuItem.Text = "Source File (.dcpu)";
            this.sourceFiledcpuToolStripMenuItem.Click += new System.EventHandler(this.sourceFiledcpuToolStripMenuItem_Click);
            // 
            // compiledProgram0x10cToolStripMenuItem
            // 
            this.compiledProgram0x10cToolStripMenuItem.Name = "compiledProgram0x10cToolStripMenuItem";
            this.compiledProgram0x10cToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.compiledProgram0x10cToolStripMenuItem.Text = "Compiled Program (.0x10c)";
            this.compiledProgram0x10cToolStripMenuItem.Click += new System.EventHandler(this.compiledProgram0x10cToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceFileToolStripMenuItem,
            this.compiledProgramToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open...";
            // 
            // sourceFileToolStripMenuItem
            // 
            this.sourceFileToolStripMenuItem.Name = "sourceFileToolStripMenuItem";
            this.sourceFileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.sourceFileToolStripMenuItem.Text = "Source File";
            this.sourceFileToolStripMenuItem.Click += new System.EventHandler(this.sourceFileToolStripMenuItem_Click);
            // 
            // compiledProgramToolStripMenuItem
            // 
            this.compiledProgramToolStripMenuItem.Name = "compiledProgramToolStripMenuItem";
            this.compiledProgramToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.compiledProgramToolStripMenuItem.Text = "Compiled Program";
            this.compiledProgramToolStripMenuItem.Click += new System.EventHandler(this.compiledProgramToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // commandsToolStripMenuItem
            // 
            this.commandsToolStripMenuItem.Name = "commandsToolStripMenuItem";
            this.commandsToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.commandsToolStripMenuItem.Text = "Commands";
            // 
            // compilerToolStripMenuItem
            // 
            this.compilerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileFromToolStripMenuItem,
            this.decompileToToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.compilerToolStripMenuItem.Name = "compilerToolStripMenuItem";
            this.compilerToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.compilerToolStripMenuItem.Text = "Compiler";
            // 
            // compileFromToolStripMenuItem
            // 
            this.compileFromToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cToolStripMenuItem,
            this.visualBasicToolStripMenuItem,
            this.assemblerToolStripMenuItem,
            this.pseudocodeToolStripMenuItem});
            this.compileFromToolStripMenuItem.Name = "compileFromToolStripMenuItem";
            this.compileFromToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.compileFromToolStripMenuItem.Text = "Compile From";
            // 
            // cToolStripMenuItem
            // 
            this.cToolStripMenuItem.Name = "cToolStripMenuItem";
            this.cToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.cToolStripMenuItem.Text = "C";
            // 
            // visualBasicToolStripMenuItem
            // 
            this.visualBasicToolStripMenuItem.Name = "visualBasicToolStripMenuItem";
            this.visualBasicToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.visualBasicToolStripMenuItem.Text = "Visual Basic";
            // 
            // assemblerToolStripMenuItem
            // 
            this.assemblerToolStripMenuItem.Name = "assemblerToolStripMenuItem";
            this.assemblerToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.assemblerToolStripMenuItem.Text = "Assembler";
            // 
            // pseudocodeToolStripMenuItem
            // 
            this.pseudocodeToolStripMenuItem.Name = "pseudocodeToolStripMenuItem";
            this.pseudocodeToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.pseudocodeToolStripMenuItem.Text = "Pseudocode";
            // 
            // decompileToToolStripMenuItem
            // 
            this.decompileToToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assemblerToolStripMenuItem1});
            this.decompileToToolStripMenuItem.Name = "decompileToToolStripMenuItem";
            this.decompileToToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.decompileToToolStripMenuItem.Text = "Decompile To";
            // 
            // assemblerToolStripMenuItem1
            // 
            this.assemblerToolStripMenuItem1.Name = "assemblerToolStripMenuItem1";
            this.assemblerToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
            this.assemblerToolStripMenuItem1.Text = "Assembler";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // macrosToolStripMenuItem
            // 
            this.macrosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1});
            this.macrosToolStripMenuItem.Name = "macrosToolStripMenuItem";
            this.macrosToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.macrosToolStripMenuItem.Text = "Macros";
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.newToolStripMenuItem1.Text = "New...";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.redditOnlineHelpToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // redditOnlineHelpToolStripMenuItem
            // 
            this.redditOnlineHelpToolStripMenuItem.Name = "redditOnlineHelpToolStripMenuItem";
            this.redditOnlineHelpToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.redditOnlineHelpToolStripMenuItem.Text = "Reddit Online Help";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveFileDialog_FileOk);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog_FileOk);
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 442);
            this.Controls.Add(this.menustrip);
            this.MainMenuStrip = this.menustrip;
            this.Name = "Window";
            this.Text = "DCPU-16";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menustrip.ResumeLayout(false);
            this.menustrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menustrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sourceFiledcpuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compiledProgram0x10cToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commandsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compilerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualBasicToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assemblerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pseudocodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decompileToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assemblerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem macrosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redditOnlineHelpToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem sourceFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compiledProgramToolStripMenuItem;
    }
}

