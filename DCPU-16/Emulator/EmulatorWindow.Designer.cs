namespace DCPU_16.Emulator
{
    partial class EmulatorWindow
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
            this.registerALabel = new System.Windows.Forms.Label();
            this.registerBLable = new System.Windows.Forms.Label();
            this.registerCLabel = new System.Windows.Forms.Label();
            this.registerILabel = new System.Windows.Forms.Label();
            this.registerJLabel = new System.Windows.Forms.Label();
            this.registerXLabel = new System.Windows.Forms.Label();
            this.registerYLabel = new System.Windows.Forms.Label();
            this.registerZLabel = new System.Windows.Forms.Label();
            this.registerOLabel = new System.Windows.Forms.Label();
            this.registerSPLabel = new System.Windows.Forms.Label();
            this.registerPCLabel = new System.Windows.Forms.Label();
            this.stackTextBox = new System.Windows.Forms.TextBox();
            this.labelStackWindow = new System.Windows.Forms.Label();
            this.cpuMemSize = new System.Windows.Forms.TextBox();
            this.numericRegisterA = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterB = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterC = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterI = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterJ = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterX = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterY = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterZ = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterPC = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterSP = new System.Windows.Forms.NumericUpDown();
            this.numericRegisterO = new System.Windows.Forms.NumericUpDown();
            this.cpuMemLabel = new System.Windows.Forms.Label();
            this.setpsToRunLabel = new System.Windows.Forms.Label();
            this.stepsToRunBox = new System.Windows.Forms.TextBox();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnRunSteps = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.disassemblerTip = new System.Windows.Forms.ToolTip(this.components);
            this.memDump = new FastColoredTextBoxNS.FastColoredTextBox();
            this.memoryTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterJ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterPC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterSP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterO)).BeginInit();
            this.SuspendLayout();
            // 
            // registerALabel
            // 
            this.registerALabel.AutoSize = true;
            this.registerALabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerALabel.Location = new System.Drawing.Point(20, 40);
            this.registerALabel.Name = "registerALabel";
            this.registerALabel.Size = new System.Drawing.Size(20, 20);
            this.registerALabel.TabIndex = 9;
            this.registerALabel.Text = "A";
            // 
            // registerBLable
            // 
            this.registerBLable.AutoSize = true;
            this.registerBLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerBLable.Location = new System.Drawing.Point(20, 80);
            this.registerBLable.Name = "registerBLable";
            this.registerBLable.Size = new System.Drawing.Size(20, 20);
            this.registerBLable.TabIndex = 10;
            this.registerBLable.Text = "B";
            // 
            // registerCLabel
            // 
            this.registerCLabel.AutoSize = true;
            this.registerCLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerCLabel.Location = new System.Drawing.Point(20, 120);
            this.registerCLabel.Name = "registerCLabel";
            this.registerCLabel.Size = new System.Drawing.Size(20, 20);
            this.registerCLabel.TabIndex = 11;
            this.registerCLabel.Text = "C";
            // 
            // registerILabel
            // 
            this.registerILabel.AutoSize = true;
            this.registerILabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerILabel.Location = new System.Drawing.Point(20, 160);
            this.registerILabel.Name = "registerILabel";
            this.registerILabel.Size = new System.Drawing.Size(14, 20);
            this.registerILabel.TabIndex = 12;
            this.registerILabel.Text = "I";
            // 
            // registerJLabel
            // 
            this.registerJLabel.AutoSize = true;
            this.registerJLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerJLabel.Location = new System.Drawing.Point(20, 200);
            this.registerJLabel.Name = "registerJLabel";
            this.registerJLabel.Size = new System.Drawing.Size(17, 20);
            this.registerJLabel.TabIndex = 13;
            this.registerJLabel.Text = "J";
            // 
            // registerXLabel
            // 
            this.registerXLabel.AutoSize = true;
            this.registerXLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerXLabel.Location = new System.Drawing.Point(20, 240);
            this.registerXLabel.Name = "registerXLabel";
            this.registerXLabel.Size = new System.Drawing.Size(20, 20);
            this.registerXLabel.TabIndex = 14;
            this.registerXLabel.Text = "X";
            // 
            // registerYLabel
            // 
            this.registerYLabel.AutoSize = true;
            this.registerYLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerYLabel.Location = new System.Drawing.Point(20, 280);
            this.registerYLabel.Name = "registerYLabel";
            this.registerYLabel.Size = new System.Drawing.Size(20, 20);
            this.registerYLabel.TabIndex = 15;
            this.registerYLabel.Text = "Y";
            // 
            // registerZLabel
            // 
            this.registerZLabel.AutoSize = true;
            this.registerZLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerZLabel.Location = new System.Drawing.Point(20, 320);
            this.registerZLabel.Name = "registerZLabel";
            this.registerZLabel.Size = new System.Drawing.Size(19, 20);
            this.registerZLabel.TabIndex = 16;
            this.registerZLabel.Text = "Z";
            // 
            // registerOLabel
            // 
            this.registerOLabel.AutoSize = true;
            this.registerOLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerOLabel.Location = new System.Drawing.Point(162, 121);
            this.registerOLabel.Name = "registerOLabel";
            this.registerOLabel.Size = new System.Drawing.Size(21, 20);
            this.registerOLabel.TabIndex = 22;
            this.registerOLabel.Text = "O";
            // 
            // registerSPLabel
            // 
            this.registerSPLabel.AutoSize = true;
            this.registerSPLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerSPLabel.Location = new System.Drawing.Point(162, 81);
            this.registerSPLabel.Name = "registerSPLabel";
            this.registerSPLabel.Size = new System.Drawing.Size(30, 20);
            this.registerSPLabel.TabIndex = 21;
            this.registerSPLabel.Text = "SP";
            // 
            // registerPCLabel
            // 
            this.registerPCLabel.AutoSize = true;
            this.registerPCLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.registerPCLabel.Location = new System.Drawing.Point(162, 41);
            this.registerPCLabel.Name = "registerPCLabel";
            this.registerPCLabel.Size = new System.Drawing.Size(30, 20);
            this.registerPCLabel.TabIndex = 20;
            this.registerPCLabel.Text = "PC";
            // 
            // stackTextBox
            // 
            this.stackTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stackTextBox.Location = new System.Drawing.Point(160, 200);
            this.stackTextBox.Multiline = true;
            this.stackTextBox.Name = "stackTextBox";
            this.stackTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.stackTextBox.Size = new System.Drawing.Size(60, 140);
            this.stackTextBox.TabIndex = 23;
            this.stackTextBox.Text = "0000";
            // 
            // labelStackWindow
            // 
            this.labelStackWindow.AutoSize = true;
            this.labelStackWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.labelStackWindow.Location = new System.Drawing.Point(162, 177);
            this.labelStackWindow.Name = "labelStackWindow";
            this.labelStackWindow.Size = new System.Drawing.Size(50, 20);
            this.labelStackWindow.TabIndex = 24;
            this.labelStackWindow.Text = "Stack";
            // 
            // cpuMemSize
            // 
            this.cpuMemSize.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.cpuMemSize.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpuMemSize.Location = new System.Drawing.Point(60, 380);
            this.cpuMemSize.MaxLength = 7;
            this.cpuMemSize.Name = "cpuMemSize";
            this.cpuMemSize.Size = new System.Drawing.Size(60, 22);
            this.cpuMemSize.TabIndex = 25;
            this.cpuMemSize.Text = "100";
            this.cpuMemSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cpuMemSize.WordWrap = false;
            // 
            // numericRegisterA
            // 
            this.numericRegisterA.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterA.Hexadecimal = true;
            this.numericRegisterA.Location = new System.Drawing.Point(60, 40);
            this.numericRegisterA.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterA.Name = "numericRegisterA";
            this.numericRegisterA.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterA.TabIndex = 1;
            this.numericRegisterA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericRegisterB
            // 
            this.numericRegisterB.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterB.Hexadecimal = true;
            this.numericRegisterB.Location = new System.Drawing.Point(60, 80);
            this.numericRegisterB.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterB.Name = "numericRegisterB";
            this.numericRegisterB.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterB.TabIndex = 2;
            this.numericRegisterB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericRegisterC
            // 
            this.numericRegisterC.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterC.Hexadecimal = true;
            this.numericRegisterC.Location = new System.Drawing.Point(60, 120);
            this.numericRegisterC.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterC.Name = "numericRegisterC";
            this.numericRegisterC.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterC.TabIndex = 3;
            this.numericRegisterC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericRegisterI
            // 
            this.numericRegisterI.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterI.Hexadecimal = true;
            this.numericRegisterI.Location = new System.Drawing.Point(60, 160);
            this.numericRegisterI.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterI.Name = "numericRegisterI";
            this.numericRegisterI.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterI.TabIndex = 4;
            this.numericRegisterI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericRegisterJ
            // 
            this.numericRegisterJ.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterJ.Hexadecimal = true;
            this.numericRegisterJ.Location = new System.Drawing.Point(60, 200);
            this.numericRegisterJ.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterJ.Name = "numericRegisterJ";
            this.numericRegisterJ.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterJ.TabIndex = 5;
            this.numericRegisterJ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericRegisterX
            // 
            this.numericRegisterX.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterX.Hexadecimal = true;
            this.numericRegisterX.Location = new System.Drawing.Point(60, 240);
            this.numericRegisterX.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterX.Name = "numericRegisterX";
            this.numericRegisterX.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterX.TabIndex = 6;
            this.numericRegisterX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericRegisterY
            // 
            this.numericRegisterY.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterY.Hexadecimal = true;
            this.numericRegisterY.Location = new System.Drawing.Point(60, 280);
            this.numericRegisterY.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterY.Name = "numericRegisterY";
            this.numericRegisterY.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterY.TabIndex = 7;
            this.numericRegisterY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericRegisterZ
            // 
            this.numericRegisterZ.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterZ.Hexadecimal = true;
            this.numericRegisterZ.Location = new System.Drawing.Point(60, 320);
            this.numericRegisterZ.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterZ.Name = "numericRegisterZ";
            this.numericRegisterZ.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterZ.TabIndex = 8;
            this.numericRegisterZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericRegisterPC
            // 
            this.numericRegisterPC.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterPC.Hexadecimal = true;
            this.numericRegisterPC.Location = new System.Drawing.Point(200, 40);
            this.numericRegisterPC.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterPC.Name = "numericRegisterPC";
            this.numericRegisterPC.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterPC.TabIndex = 31;
            this.numericRegisterPC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericRegisterPC.ValueChanged += new System.EventHandler(this.numericRegisterPC_ValueChanged);
            // 
            // numericRegisterSP
            // 
            this.numericRegisterSP.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterSP.Hexadecimal = true;
            this.numericRegisterSP.Location = new System.Drawing.Point(200, 80);
            this.numericRegisterSP.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterSP.Name = "numericRegisterSP";
            this.numericRegisterSP.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterSP.TabIndex = 32;
            this.numericRegisterSP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericRegisterO
            // 
            this.numericRegisterO.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericRegisterO.Hexadecimal = true;
            this.numericRegisterO.Location = new System.Drawing.Point(200, 120);
            this.numericRegisterO.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericRegisterO.Name = "numericRegisterO";
            this.numericRegisterO.Size = new System.Drawing.Size(60, 22);
            this.numericRegisterO.TabIndex = 33;
            this.numericRegisterO.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cpuMemLabel
            // 
            this.cpuMemLabel.AutoSize = true;
            this.cpuMemLabel.Location = new System.Drawing.Point(12, 384);
            this.cpuMemLabel.Name = "cpuMemLabel";
            this.cpuMemLabel.Size = new System.Drawing.Size(48, 13);
            this.cpuMemLabel.TabIndex = 34;
            this.cpuMemLabel.Text = "Memory*";
            this.cpuMemLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // setpsToRunLabel
            // 
            this.setpsToRunLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setpsToRunLabel.AutoSize = true;
            this.setpsToRunLabel.Location = new System.Drawing.Point(20, 414);
            this.setpsToRunLabel.Name = "setpsToRunLabel";
            this.setpsToRunLabel.Size = new System.Drawing.Size(34, 13);
            this.setpsToRunLabel.TabIndex = 36;
            this.setpsToRunLabel.Text = "Steps";
            this.setpsToRunLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // stepsToRunBox
            // 
            this.stepsToRunBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.stepsToRunBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepsToRunBox.Location = new System.Drawing.Point(60, 410);
            this.stepsToRunBox.MaxLength = 7;
            this.stepsToRunBox.Name = "stepsToRunBox";
            this.stepsToRunBox.Size = new System.Drawing.Size(60, 22);
            this.stepsToRunBox.TabIndex = 35;
            this.stepsToRunBox.Text = "10000";
            this.stepsToRunBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.stepsToRunBox.WordWrap = false;
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(140, 380);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(60, 22);
            this.btnStep.TabIndex = 37;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnRunSteps
            // 
            this.btnRunSteps.Location = new System.Drawing.Point(140, 410);
            this.btnRunSteps.Name = "btnRunSteps";
            this.btnRunSteps.Size = new System.Drawing.Size(60, 22);
            this.btnRunSteps.TabIndex = 38;
            this.btnRunSteps.Text = "Run...";
            this.btnRunSteps.UseVisualStyleBackColor = true;
            this.btnRunSteps.Click += new System.EventHandler(this.btnRunSteps_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(220, 380);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(60, 22);
            this.btnReset.TabIndex = 39;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(220, 410);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(60, 22);
            this.btnLoad.TabIndex = 40;
            this.btnLoad.Text = "Load...";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // disassemblerTip
            // 
            this.disassemblerTip.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.disassemblerTip.IsBalloon = true;
            this.disassemblerTip.ShowAlways = true;
            this.disassemblerTip.ToolTipTitle = "Disassembled:";
            this.disassemblerTip.Popup += new System.Windows.Forms.PopupEventHandler(this.disassemblerTip_Popup);
            // 
            // memDump
            // 
            this.memDump.AutoScrollMinSize = new System.Drawing.Size(0, 12);
            this.memDump.BackBrush = null;
            this.memDump.BackColor = System.Drawing.SystemColors.Window;
            this.memDump.CommentPrefix = "//";
            this.memDump.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.memDump.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.memDump.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memDump.IndentBackColor = System.Drawing.Color.WhiteSmoke;
            this.memDump.LeftPadding = 20;
            this.memDump.LineNumberColor = System.Drawing.Color.DimGray;
            this.memDump.LineNumberFormat = "{0:X4}";
            this.memDump.LineNumberMultiplier = ((uint)(8u));
            this.memDump.LineNumberStartValue = ((uint)(0u));
            this.memDump.Location = new System.Drawing.Point(280, 40);
            this.memDump.Name = "memDump";
            this.memDump.Paddings = new System.Windows.Forms.Padding(0);
            this.memDump.ReadOnly = true;
            this.memDump.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.memDump.Size = new System.Drawing.Size(340, 400);
            this.memDump.TabIndex = 0;
            this.memDump.WordWrap = true;
            // 
            // EmulatorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnRunSteps);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.setpsToRunLabel);
            this.Controls.Add(this.stepsToRunBox);
            this.Controls.Add(this.cpuMemLabel);
            this.Controls.Add(this.numericRegisterO);
            this.Controls.Add(this.numericRegisterSP);
            this.Controls.Add(this.numericRegisterPC);
            this.Controls.Add(this.numericRegisterZ);
            this.Controls.Add(this.numericRegisterY);
            this.Controls.Add(this.numericRegisterX);
            this.Controls.Add(this.numericRegisterJ);
            this.Controls.Add(this.numericRegisterI);
            this.Controls.Add(this.numericRegisterC);
            this.Controls.Add(this.numericRegisterB);
            this.Controls.Add(this.numericRegisterA);
            this.Controls.Add(this.cpuMemSize);
            this.Controls.Add(this.labelStackWindow);
            this.Controls.Add(this.stackTextBox);
            this.Controls.Add(this.registerOLabel);
            this.Controls.Add(this.registerSPLabel);
            this.Controls.Add(this.registerPCLabel);
            this.Controls.Add(this.registerZLabel);
            this.Controls.Add(this.registerYLabel);
            this.Controls.Add(this.registerXLabel);
            this.Controls.Add(this.registerJLabel);
            this.Controls.Add(this.registerILabel);
            this.Controls.Add(this.registerCLabel);
            this.Controls.Add(this.registerBLable);
            this.Controls.Add(this.registerALabel);
            this.Controls.Add(this.memDump);
            this.Name = "EmulatorWindow";
            this.Text = "EmulatorWindow";
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterJ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterPC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterSP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericRegisterO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox memDump;
        private System.Windows.Forms.Label registerALabel;
        private System.Windows.Forms.Label registerBLable;
        private System.Windows.Forms.Label registerCLabel;
        private System.Windows.Forms.Label registerILabel;
        private System.Windows.Forms.Label registerJLabel;
        private System.Windows.Forms.Label registerXLabel;
        private System.Windows.Forms.Label registerYLabel;
        private System.Windows.Forms.Label registerZLabel;
        private System.Windows.Forms.Label registerOLabel;
        private System.Windows.Forms.Label registerSPLabel;
        private System.Windows.Forms.Label registerPCLabel;
        private System.Windows.Forms.TextBox stackTextBox;
        private System.Windows.Forms.Label labelStackWindow;
        private System.Windows.Forms.TextBox cpuMemSize;
        private System.Windows.Forms.NumericUpDown numericRegisterA;
        private System.Windows.Forms.NumericUpDown numericRegisterB;
        private System.Windows.Forms.NumericUpDown numericRegisterC;
        private System.Windows.Forms.NumericUpDown numericRegisterI;
        private System.Windows.Forms.NumericUpDown numericRegisterJ;
        private System.Windows.Forms.NumericUpDown numericRegisterX;
        private System.Windows.Forms.NumericUpDown numericRegisterY;
        private System.Windows.Forms.NumericUpDown numericRegisterZ;
        private System.Windows.Forms.NumericUpDown numericRegisterPC;
        private System.Windows.Forms.NumericUpDown numericRegisterSP;
        private System.Windows.Forms.NumericUpDown numericRegisterO;
        private System.Windows.Forms.Label cpuMemLabel;
        private System.Windows.Forms.Label setpsToRunLabel;
        private System.Windows.Forms.TextBox stepsToRunBox;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnRunSteps;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolTip disassemblerTip;
        private System.Windows.Forms.ToolTip memoryTip;
    }
}