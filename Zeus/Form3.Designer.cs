namespace Zeus
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemGoTo = new System.Windows.Forms.MenuItem();
            this.menuItemPreviousQuestion = new System.Windows.Forms.MenuItem();
            this.menuItemAbort = new System.Windows.Forms.MenuItem();
            this.menuItemNextQuestion = new System.Windows.Forms.MenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblDbInstruction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItemNextQuestion);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItemGoTo);
            this.menuItem1.MenuItems.Add(this.menuItemPreviousQuestion);
            this.menuItem1.MenuItems.Add(this.menuItemAbort);
            this.menuItem1.Text = "Menu";
            // 
            // menuItemGoTo
            // 
            this.menuItemGoTo.Text = "Ir para...";
            this.menuItemGoTo.Click += new System.EventHandler(this.menuItemGoTo_Click);
            // 
            // menuItemPreviousQuestion
            // 
            this.menuItemPreviousQuestion.Text = "Anterior";
            this.menuItemPreviousQuestion.Click += new System.EventHandler(this.menuItemPreviousQuestion_Click);
            // 
            // menuItemAbort
            // 
            this.menuItemAbort.Text = "Abortar";
            this.menuItemAbort.Click += new System.EventHandler(this.menuItemAbort_Click);
            // 
            // menuItemNextQuestion
            // 
            this.menuItemNextQuestion.Text = "Próximo";
            this.menuItemNextQuestion.Click += new System.EventHandler(this.menuItemNextQuestion_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(221, 20);
            // 
            // lblHeader
            // 
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblHeader.Location = new System.Drawing.Point(3, 26);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(191, 20);
            this.lblHeader.Text = "Pergunta N de M";
            // 
            // lblQuestion
            // 
            this.lblQuestion.Location = new System.Drawing.Point(3, 46);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(221, 34);
            this.lblQuestion.Text = "Qual a cor do cavalo branco de Napoleão?";
            // 
            // lblInstructions
            // 
            this.lblInstructions.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblInstructions.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblInstructions.Location = new System.Drawing.Point(3, 90);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(221, 20);
            this.lblInstructions.Text = "Instrução padrão";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(144, 50);
            // 
            // lblDbInstruction
            // 
            this.lblDbInstruction.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDbInstruction.ForeColor = System.Drawing.Color.DarkRed;
            this.lblDbInstruction.Location = new System.Drawing.Point(3, 110);
            this.lblDbInstruction.Name = "lblDbInstruction";
            this.lblDbInstruction.Size = new System.Drawing.Size(221, 20);
            this.lblDbInstruction.Text = "Instrução personalizada";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.lblDbInstruction);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Menu = this.mainMenu1;
            this.Name = "Form3";
            this.Text = "Entrevista";
            this.GotFocus += new System.EventHandler(this.Form3_GotFocus);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemPreviousQuestion;
        private System.Windows.Forms.MenuItem menuItemNextQuestion;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.MenuItem menuItemAbort;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblDbInstruction;
        private System.Windows.Forms.MenuItem menuItemGoTo;
    }
}