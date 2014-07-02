using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace MeasureStringSample
{
    public class CFMeasureString
    {
        public struct Rect
        {
            public int Left, Top, Right, Bottom;
            public Rect(Rectangle r)
            {
                this.Left = r.Left;
                this.Top = r.Top;
                this.Bottom = r.Bottom;
                this.Right = r.Right;
            }
        }

        [DllImport("coredll.dll")]
        static extern int DrawText(IntPtr hdc, string lpStr, int nCount, ref Rect lpRect, int wFormat);
        [DllImport("coredll.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("coredll.dll")]
        static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        private const int DT_CALCRECT = 0x00000400;
        private const int DT_WORDBREAK = 0x00000010;
        private const int DT_EDITCONTROL = 0x00002000;

        static public Size MeasureString(Graphics gr, string text, Rectangle rect, bool textboxControl)
        {
            Rect bounds = new Rect(rect);
            IntPtr hdc = gr.GetHdc();
            int flags = DT_CALCRECT | DT_WORDBREAK;
            if (textboxControl) flags |= DT_EDITCONTROL;
            DrawText(hdc, text, text.Length, ref bounds, flags);
            gr.ReleaseHdc(hdc);
            return new Size(bounds.Right - bounds.Left, bounds.Bottom - bounds.Top + (textboxControl ? 6 : 0));
        }

        static public Size MeasureString(Control control, string text, Rectangle rect)
        {
            Size result = Size.Empty;
            IntPtr controlFont = control.Font.ToHfont();
            IntPtr hDC = GetDC(control.Handle);
            using (Graphics gr = Graphics.FromHdc(hDC))
            {
                IntPtr originalObject = SelectObject(hDC, controlFont);
                result = MeasureString(gr, text, rect, control is TextBox);
                SelectObject(hDC, originalObject); //Release resources
            }
            return result;
        }
    }
}