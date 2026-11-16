using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HardOptimizer
{
    public enum ButtonKind { Standard, Primary, Subtle }
    public interface IThemeable { void ApplyTheme(Palette palette); }

    public class FluentButton : Button, IThemeable
    {
        private ButtonKind _kind = ButtonKind.Standard;
        private bool _hover, _pressed;

        public FluentButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
        }

        [DefaultValue(ButtonKind.Standard), Category("Appearance")]
        public ButtonKind Kind { get => _kind; set { _kind = value; Invalidate(); } }

        public void ApplyTheme(Palette p)
        {
            if (Parent != null) BackColor = Parent.BackColor;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs mevent) { if (mevent.Button == MouseButtons.Left) { _pressed = true; Invalidate(); } base.OnMouseDown(mevent); }
        protected override void OnMouseUp(MouseEventArgs mevent) { _pressed = false; Invalidate(); base.OnMouseUp(mevent); }
        protected override void OnParentBackColorChanged(EventArgs e) { if (Parent != null) BackColor = Parent.BackColor; base.OnParentBackColorChanged(e); }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var p = ThemeManager.Current;
            var g = pevent.Graphics;
            var surface = Parent != null ? Parent.BackColor : p.WindowBg;
            g.Clear(surface);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color fill, border, text;
            if (!Enabled) { fill = _kind == ButtonKind.Subtle ? Color.Transparent : Color.FromArgb(100, p.ControlFill); border = p.Stroke; text = p.TextDisabled; }
            else
            {
                switch (_kind)
                {
                    case ButtonKind.Primary: fill = _pressed ? p.AccentPressed : (_hover ? p.AccentHover : p.Accent); border = fill; text = Color.White; break;
                    case ButtonKind.Subtle: fill = _pressed ? Color.FromArgb(60, p.Accent) : Color.Transparent; border = Color.Transparent; text = p.TextPrimary; break;
                    default: fill = _pressed ? Color.FromArgb(60, p.ControlFill) : p.ControlFill; border = p.Stroke; text = p.TextPrimary; break;
                }
            }

            var r = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            using (var path = Draw.RoundedRect(r, Draw.S(4)))
            {
                if (fill.A > 0) using (var b = new SolidBrush(fill)) g.FillPath(b, path);
                if (border.A > 0) using (var pen = new Pen(border)) g.DrawPath(pen, path);
            }

            var content = new Rectangle(Padding.Left + 6, Padding.Top, Width - Padding.Horizontal - 12, Height - Padding.Vertical);
            TextRenderer.DrawText(g, Text, Font, content, text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
        }

        private Color Stroke => ThemeManager.Current.Stroke;
    }

    public class MoonToggle : CheckBox, IThemeable
    {
        private bool _hover, _pressed;

        public MoonToggle()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            MinimumSize = new Size(46, 22);
            Cursor = Cursors.Hand;
        }

        public new bool Checked { get => base.Checked; set { base.Checked = value; Invalidate(); } }

        public void ApplyTheme(Palette p)
        {
            if (Parent != null) BackColor = Parent.BackColor;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { if (e.Button == MouseButtons.Left) { _pressed = true; Invalidate(); } base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var p = ThemeManager.Current;
            var g = pevent.Graphics;
            g.Clear(Parent != null ? Parent.BackColor : p.WindowBg);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float trackW = Math.Min(Draw.S(40), Width - 3);
            float trackH = Math.Min(Draw.S(20), Height - 3);
            float x = Width - trackW - 1.5f;
            float y = (Height - trackH) / 2f;
            var track = new RectangleF(x, y, trackW, trackH);
            float radius = trackH / 2f;
            float knob = trackH * (_pressed ? 0.70f : (_hover ? 0.62f : 0.54f));
            float knobY = y + (trackH - knob) / 2f;
            float inset = (trackH - knob) / 2f;

            using (var path = Draw.RoundedRect(track, radius))
            {
                if (Checked)
                {
                    Color fill = !Enabled ? p.TextSecondary : (_pressed ? p.AccentPressed : (_hover ? p.AccentHover : p.Accent));
                    using (var b = new SolidBrush(fill)) g.FillPath(b, path);
                    using (var b = new SolidBrush(Color.White)) g.FillEllipse(b, track.Right - inset - knob, knobY, knob, knob);
                }
                else
                {
                    Color surface = Parent != null ? Parent.BackColor : p.WindowBg;
                    Color fill = _hover ? Color.FromArgb(100, p.ControlHover) : Color.FromArgb(80, p.ControlFill);
                    Color stroke = Enabled ? p.TextSecondary : p.TextDisabled;
                    using (var b = new SolidBrush(fill)) g.FillPath(b, path);
                    using (var pen = new Pen(stroke)) g.DrawPath(pen, path);
                    using (var b = new SolidBrush(stroke)) g.FillEllipse(b, track.X + inset, knobY, knob, knob);
                }
            }
        }
    }

    public static class Draw
    {
        private static float _dpiScale = -1f;
        public static float DpiScale
        {
            get
            {
                if (_dpiScale < 0)
                {
                    try { using (var g = Graphics.FromHwnd(IntPtr.Zero)) _dpiScale = g.DpiX / 96f; }
                    catch { _dpiScale = 1f; }
                }
                return _dpiScale;
            }
        }

        public static float S(float value) => value * DpiScale;
        public static int SI(int value) => (int)Math.Round(value * DpiScale);

        public static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            var path = new GraphicsPath();
            float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d <= 0.5f) { path.AddRectangle(r); return path; }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
