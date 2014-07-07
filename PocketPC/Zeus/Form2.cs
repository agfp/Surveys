using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Zeus.Global;
using System.IO;

namespace Zeus
{
    public partial class Form2 : Form
    {
        Dictionary<string, string> _fileLocation;

        public Form2()
        {
            InitializeComponent();
            LoadSurveys();
            inputPanel1.Enabled = false;
        }

        private void LoadSurveys()
        {
            _fileLocation = new Dictionary<string, string>();

            if (Directory.Exists(Zeus.Properties.Resources.ZeusFolder))
            {
                var files = Directory.GetFiles(Zeus.Properties.Resources.ZeusFolder, "*.zeus");
                foreach (var file in files)
                {
                    var friendlyName = Path.GetFileNameWithoutExtension(file);
                    _fileLocation.Add(friendlyName, file);
                    cbbFiles.Items.Add(friendlyName);
                }
            }
            if (Directory.Exists(Zeus.Properties.Resources.StorageCard))
            {
                var files = Directory.GetFiles(Zeus.Properties.Resources.StorageCard, "*.zeus");
                foreach (var file in files)
                {
                    var friendlyName = "[SD] " + Path.GetFileNameWithoutExtension(file);
                    _fileLocation.Add(friendlyName, file);
                    cbbFiles.Items.Add(friendlyName);
                }
            }
        }

        private void cbbFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            var friendlyName = cbbFiles.SelectedItem.ToString();
            var filename = _fileLocation[friendlyName];
            Interview.OpenSurvey(filename);
            LoadFields();
        }

        private void LoadFields()
        {
            cbbPesquisadores.Items.Clear();
            foreach (DataRow dr in Interview.Survey.Interviewers.Rows)
            {
                cbbPesquisadores.Items.Add(dr["Name"]);
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (cbbFiles.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um questionário");
                return;
            }

            if (cbbPesquisadores.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um pesquisador");
                return;
            }

            var interviwerId = (from o in Interview.Survey.Interviewers.AsEnumerable()
                                where o.Field<string>("Name") == cbbPesquisadores.SelectedItem.ToString()
                                select o.Field<int>("Id")).First();

            Interview.NewInterview(interviwerId);
            Form3 frm3 = new Form3();
            frm3.Owner = this;
            frm3.Show();
        }
    }
}