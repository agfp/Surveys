//-----------------------------------------------------------------------
// 
//  Copyright (C) MOBILE PRACTICES.  All rights reserved.
//  http://www.mobilepractices.com/
//
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//-----------------------------------------------------------------------

namespace MobilePractices.OpenFileDialogEx
{
    partial class OpenFileDialogEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenFileDialogEx));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.softKey2Menu = new System.Windows.Forms.MenuItem();
            this.menuItemCancel = new System.Windows.Forms.MenuItem();
            this.fileListView = new System.Windows.Forms.ListView();
            this.FileSystemIcons = new System.Windows.Forms.ImageList();
            this.PathSelectorComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.softKey2Menu);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Acima";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // softKey2Menu
            // 
            this.softKey2Menu.MenuItems.Add(this.menuItemCancel);
            this.softKey2Menu.Text = "Menu";
            // 
            // menuItemCancel
            // 
            this.menuItemCancel.Text = "Cancelar";
            this.menuItemCancel.Click += new System.EventHandler(this.menuItemCancel_Click);
            // 
            // fileListView
            // 
            this.fileListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileListView.Location = new System.Drawing.Point(0, 22);
            this.fileListView.Name = "fileListView";
            this.fileListView.Size = new System.Drawing.Size(240, 246);
            this.fileListView.SmallImageList = this.FileSystemIcons;
            this.fileListView.TabIndex = 4;
            this.fileListView.View = System.Windows.Forms.View.SmallIcon;
            this.fileListView.ItemActivate += new System.EventHandler(this.fileListView_ItemActivate);
            // 
            // FileSystemIcons
            // 
            this.FileSystemIcons.ImageSize = new System.Drawing.Size(32, 32);
            this.FileSystemIcons.Images.Clear();
            this.FileSystemIcons.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
            // 
            // PathSelectorComboBox
            // 
            this.PathSelectorComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.PathSelectorComboBox.Location = new System.Drawing.Point(0, 0);
            this.PathSelectorComboBox.Name = "PathSelectorComboBox";
            this.PathSelectorComboBox.Size = new System.Drawing.Size(240, 22);
            this.PathSelectorComboBox.TabIndex = 5;
            this.PathSelectorComboBox.SelectedIndexChanged += new System.EventHandler(this.PathSelectorComboBox_SelectedIndexChanged);
            // 
            // OpenFileDialogEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.fileListView);
            this.Controls.Add(this.PathSelectorComboBox);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "OpenFileDialogEx";
            this.Text = "Abrir questionário";
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.ListView fileListView;
		private System.Windows.Forms.ImageList FileSystemIcons;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem softKey2Menu;
		private System.Windows.Forms.MenuItem menuItemCancel;
        private System.Windows.Forms.ComboBox PathSelectorComboBox;
    }
}