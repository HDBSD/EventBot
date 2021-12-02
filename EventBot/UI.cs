using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EventBot
{
    public class RoundButton : Button
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            var graphicsPath = new GraphicsPath();
            graphicsPath.AddEllipse(4, 4, ClientSize.Width-8, ClientSize.Height-8);
            this.Region = new Region(graphicsPath);
            base.OnPaint(e);
        }
    }

    public class solidBtn : Button
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            var graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0,0, ClientSize.Width, ClientSize.Height));
            this.Region = new Region(graphicsPath);
            base.OnPaint(e);
        }
    }
}
