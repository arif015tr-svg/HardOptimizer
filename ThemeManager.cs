using System;
using System.Drawing;
using System.Windows.Forms;

namespace HardOptimizer
{
    public static class ThemeManager
    {
        public static Palette Current { get; private set; } = Palette.Create(ThemeMode.Dark, Palette.DefaultAccent);

        public static void SetTheme(ThemeMode mode, Color accent)
        {
            Current = Palette.Create(mode, accent);
        }

        public static void Apply(Control root)
        {
            if (root == null) return;
            var p = Current;
            root.BackColor = p.WindowBg;
            root.ForeColor = p.TextPrimary;

            ApplyRecursive(root, p, p.WindowBg);
            root.Invalidate(true);
        }

        private static void ApplyRecursive(Control control, Palette p, Color surface)
        {
            foreach (Control child in control.Controls)
            {
                child.BackColor = surface;
                child.ForeColor = p.TextPrimary;
                ApplyRecursive(child, p, surface);
            }
        }
    }

    public sealed class Palette
    {
        public static readonly Color DefaultAccent = Color.FromArgb(153, 102, 204);

        public ThemeMode Mode { get; private set; }
        public Color WindowBg { get; private set; }
        public Color TextPrimary { get; private set; }
        public Color TextSecondary { get; private set; }
        public Color Accent { get; private set; }
        public Color AccentHover { get; private set; }
        public Color AccentPressed { get; private set; }
        public Color Layer1 { get; private set; }
        public Color ControlFill { get; private set; }
        public Color Stroke { get; private set; }
        public Color Danger { get; private set; }
        public Color TextDisabled { get; private set; }
        public Color ControlHover { get; private set; }
        public bool IsDark => Mode == ThemeMode.Dark;

        internal static Palette Create(ThemeMode mode, Color accent)
        {
            if (accent.IsEmpty || accent.A == 0) accent = DefaultAccent;
            accent = Color.FromArgb(255, accent);
            var p = new Palette { Mode = mode, Accent = accent };

            if (mode == ThemeMode.Light)
            {
                p.WindowBg = Color.FromArgb(243, 243, 243);
                p.TextPrimary = Color.FromArgb(27, 27, 27);
                p.TextSecondary = Color.FromArgb(96, 96, 96);
                p.Layer1 = Color.FromArgb(251, 251, 251);
                p.ControlFill = Color.FromArgb(253, 253, 253);
                p.Stroke = Color.FromArgb(225, 225, 225);
                p.Danger = Color.FromArgb(196, 43, 28);
                p.TextDisabled = Color.FromArgb(160, 160, 160);
                p.ControlHover = Color.FromArgb(230, 230, 230);
                p.AccentHover = Blend(accent, Color.Black, 0.10);
                p.AccentPressed = Blend(accent, Color.Black, 0.20);
            }
            else
            {
                p.WindowBg = Color.FromArgb(32, 32, 32);
                p.TextPrimary = Color.FromArgb(255, 255, 255);
                p.TextSecondary = Color.FromArgb(200, 200, 200);
                p.Layer1 = Color.FromArgb(43, 43, 43);
                p.ControlFill = Color.FromArgb(55, 55, 55);
                p.Stroke = Color.FromArgb(58, 58, 58);
                p.Danger = Color.FromArgb(255, 153, 164);
                p.TextDisabled = Color.FromArgb(120, 120, 120);
                p.ControlHover = Color.FromArgb(70, 70, 70);
                p.AccentHover = Blend(accent, Color.White, 0.10);
                p.AccentPressed = Blend(accent, Color.Black, 0.15);
            }
            return p;
        }

        private static Color Blend(Color from, Color to, double amount)
        {
            amount = Math.Max(0, Math.Min(1, amount));
            return Color.FromArgb(
                (int)Math.Round(from.R + (to.R - from.R) * amount),
                (int)Math.Round(from.G + (to.G - from.G) * amount),
                (int)Math.Round(from.B + (to.B - from.B) * amount));
        }
    }
}
