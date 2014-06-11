using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Apolo
{
    class Utils
    {
        public static void SwitchForm(Form currentForm, Form newForm)
        {
            Form oldForm = currentForm;
            newForm.Show();
            currentForm.Hide();
            currentForm = newForm;
            oldForm.Dispose();
        }
    }
}
