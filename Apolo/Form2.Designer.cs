namespace Apolo
{
    partial class Form2
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNomeQuestionario = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbPesquisadores = new System.Windows.Forms.ComboBox();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(0, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 50);
            this.label1.Text = "Apolo 1.0\r\nPesquisa de Campo";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblNomeQuestionario
            // 
            this.lblNomeQuestionario.Location = new System.Drawing.Point(3, 67);
            this.lblNomeQuestionario.Name = "lblNomeQuestionario";
            this.lblNomeQuestionario.Size = new System.Drawing.Size(234, 44);
            this.lblNomeQuestionario.Text = "Questionário de Satisfação no Trabalho";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.Text = "Pesquisador:";
            // 
            // cbbPesquisadores
            // 
            this.cbbPesquisadores.Location = new System.Drawing.Point(4, 135);
            this.cbbPesquisadores.Name = "cbbPesquisadores";
            this.cbbPesquisadores.Size = new System.Drawing.Size(233, 22);
            this.cbbPesquisadores.TabIndex = 3;
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(165, 245);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(72, 20);
            this.btnIniciar.TabIndex = 4;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.cbbPesquisadores);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblNomeQuestionario);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNomeQuestionario;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbbPesquisadores;
        private System.Windows.Forms.Button btnIniciar;
    }
}