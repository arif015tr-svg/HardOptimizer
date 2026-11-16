using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HardOptimizer
{
    public partial class MainForm : Form
    {
        private readonly bool _silentMode;
        private readonly Panel _sidebar;
        private readonly FlowLayoutPanel _navButtons;
        private Panel _contentPanel;
        private Dictionary<string, Panel> _pages;

        public MainForm(bool silentMode = false)
        {
            _silentMode = silentMode;
            Text = $"Optimizer v{Program.VersionString}";
            Size = new Size(1100, 700);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ThemeManager.Current.WindowBg;

            InitializeUI();
            ApplyTheme();

            if (_silentMode)
            {
                SilentOps.ProcessAllActions();
                Application.Exit();
            }
        }

        private void InitializeUI()
        {
            _sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(26, 26, 26)
            };

            var logoLabel = new Label
            {
                Text = "OPTIMIZER",
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold),
                ForeColor = ThemeManager.Current.Accent,
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            _navButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            _sidebar.Controls.Add(_navButtons);
            _sidebar.Controls.Add(logoLabel);

            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.Current.WindowBg
            };

            _pages = new Dictionary<string, Panel>();

            BuildPages();
            CreateNavigation();

            Controls.Add(_contentPanel);
            Controls.Add(_sidebar);
        }

        private void BuildPages()
        {
            AddPage("Tweaks", BuildTweaksPage());
            AddPage("Cleaner", BuildCleanerPage());
            AddPage("Apps", BuildAppsPage());
            AddPage("Startup", BuildStartupPage());
            AddPage("Hosts", BuildHostsPage());
            AddPage("DNS", BuildDnsPage());
            AddPage("Hardware", BuildHardwarePage());
            AddPage("Services", BuildServicesPage());
            AddPage("Gaming", BuildGamingPage());
            AddPage("Options", BuildOptionsPage());
        }

        private void AddPage(string name, Panel panel)
        {
            panel.Dock = DockStyle.Fill;
            panel.Visible = false;
            _contentPanel.Controls.Add(panel);
            _pages[name] = panel;
        }

        private void ShowPage(string name)
        {
            foreach (var kvp in _pages) kvp.Value.Visible = false;
            if (_pages.TryGetValue(name, out var page)) page.Visible = true;
            _contentPanel.Invalidate();
        }

        private void CreateNavigation()
        {
            var pages = new[] { "Tweaks", "Cleaner", "Apps", "Startup", "Hosts", "DNS", "Hardware", "Services", "Gaming", "Options" };
            foreach (var pageName in pages)
            {
                var btn = new Button
                {
                    Text = pageName,
                    Height = 40,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    BackColor = Color.Transparent,
                    Width = 180,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(15, 0, 0, 0),
                    FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(60, 60, 60), MouseDownBackColor = Color.FromArgb(80, 80, 80) }
                };
                var name = pageName;
                btn.Click += (s, e) => ShowPage(name);
                _navButtons.Controls.Add(btn);
            }
            ShowPage("Tweaks");
        }

        private void ApplyTheme()
        {
            BackColor = ThemeManager.Current.WindowBg;
            ForeColor = ThemeManager.Current.TextPrimary;
            _sidebar.BackColor = Color.FromArgb(26, 26, 26);
            _contentPanel.BackColor = ThemeManager.Current.WindowBg;
        }

        private void AddToggle(FlowLayoutPanel panel, string labelText, Func<bool> getter, Action<bool> setter, Action action = null)
        {
            var row = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Margin = new Padding(0, 4, 0, 4) };
            var lbl = new Label { Text = labelText, AutoSize = false, Width = 220, Height = 30, TextAlign = ContentAlignment.MiddleLeft, ForeColor = ThemeManager.Current.TextPrimary };
            var toggle = new MoonToggle { AutoSize = false, Width = 50, Height = 28 };
            try { toggle.Checked = getter(); } catch { toggle.Checked = false; }
            toggle.CheckedChanged += (s, e) => { try { setter(toggle.Checked); action?.Invoke(); } catch { } };
            row.Controls.Add(lbl);
            row.Controls.Add(toggle);
            panel.Controls.Add(row);
        }

        private Panel BuildTweaksPage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var title = new Label { Text = "System Tweaks", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(0, 10, 0, 0) };

            AddToggle(layout, "Performance Tweaks", () => OptionsHelper.CurrentOptions.EnablePerformanceTweaks, v => OptionsHelper.CurrentOptions.EnablePerformanceTweaks = v, OptimizeHelper.EnablePerformanceTweaks);
            AddToggle(layout, "Disable Superfetch", () => OptionsHelper.CurrentOptions.DisableSuperfetch, v => OptionsHelper.CurrentOptions.DisableSuperfetch = v, OptimizeHelper.DisableSuperfetch);
            AddToggle(layout, "Disable Error Reporting", () => OptionsHelper.CurrentOptions.DisableErrorReporting, v => OptionsHelper.CurrentOptions.DisableErrorReporting = v, OptimizeHelper.DisableErrorReporting);
            AddToggle(layout, "Disable SmartScreen", () => OptionsHelper.CurrentOptions.DisableSmartScreen, v => OptionsHelper.CurrentOptions.DisableSmartScreen = v, OptimizeHelper.DisableSmartScreen);
            AddToggle(layout, "Disable Hibernation", () => OptionsHelper.CurrentOptions.DisableHibernation, v => OptionsHelper.CurrentOptions.DisableHibernation = v, OptimizeHelper.DisableHibernation);
            AddToggle(layout, "Disable SMB1", () => OptionsHelper.CurrentOptions.DisableSMB1, v => OptionsHelper.CurrentOptions.DisableSMB1 = v, OptimizeHelper.DisableSMB1);
            AddToggle(layout, "Enable UTC Time", () => OptionsHelper.CurrentOptions.EnableUtcTime, v => OptionsHelper.CurrentOptions.EnableUtcTime = v, OptimizeHelper.EnableUtcTime);
            AddToggle(layout, "Remove Menu Delay", () => OptionsHelper.CurrentOptions.RemoveMenusDelay, v => OptionsHelper.CurrentOptions.RemoveMenusDelay = v, OptimizeHelper.RemoveMenusDelay);
            AddToggle(layout, "Disable Telemetry Tasks", () => OptionsHelper.CurrentOptions.DisableTelemetryTasks, v => OptionsHelper.CurrentOptions.DisableTelemetryTasks = v, OptimizeHelper.DisableTelemetryTasks);
            AddToggle(layout, "Disable Cortana", () => OptionsHelper.CurrentOptions.DisableCortana, v => OptionsHelper.CurrentOptions.DisableCortana = v, OptimizeHelper.DisableCortana);
            AddToggle(layout, "Disable Xbox Live", () => OptionsHelper.CurrentOptions.DisableXboxLive, v => OptionsHelper.CurrentOptions.DisableXboxLive = v, OptimizeHelper.DisableXboxLive);
            AddToggle(layout, "Disable Game Bar", () => OptionsHelper.CurrentOptions.DisableGameBar, v => OptionsHelper.CurrentOptions.DisableGameBar = v, OptimizeHelper.DisableGameBar);
            AddToggle(layout, "Disable Widgets (Win11)", () => OptionsHelper.CurrentOptions.DisableWidgets, v => OptionsHelper.CurrentOptions.DisableWidgets = v, OptimizeHelper.DisableWidgets);
            AddToggle(layout, "Disable Copilot AI", () => OptionsHelper.CurrentOptions.DisableCoPilotAI, v => OptionsHelper.CurrentOptions.DisableCoPilotAI = v, OptimizeHelper.DisableCoPilotAI);

            var btnApply = new FluentButton { Text = "Apply Selected Tweaks", Kind = ButtonKind.Primary, Width = 200, Height = 36, Margin = new Padding(0, 15, 0, 0) };
            btnApply.Click += (s, e) =>
            {
                try
                {
                    if (OptionsHelper.CurrentOptions.EnablePerformanceTweaks) OptimizeHelper.EnablePerformanceTweaks();
                    if (OptionsHelper.CurrentOptions.DisableSuperfetch) OptimizeHelper.DisableSuperfetch();
                    if (OptionsHelper.CurrentOptions.DisableErrorReporting) OptimizeHelper.DisableErrorReporting();
                    if (OptionsHelper.CurrentOptions.DisableSmartScreen) OptimizeHelper.DisableSmartScreen();
                    if (OptionsHelper.CurrentOptions.DisableHibernation) OptimizeHelper.DisableHibernation();
                    if (OptionsHelper.CurrentOptions.DisableSMB1) OptimizeHelper.DisableSMB1();
                    if (OptionsHelper.CurrentOptions.EnableUtcTime) OptimizeHelper.EnableUtcTime();
                    if (OptionsHelper.CurrentOptions.RemoveMenusDelay) OptimizeHelper.RemoveMenusDelay();
                    if (OptionsHelper.CurrentOptions.DisableTelemetryTasks) OptimizeHelper.DisableTelemetryTasks();
                    if (OptionsHelper.CurrentOptions.DisableCortana) OptimizeHelper.DisableCortana();
                    if (OptionsHelper.CurrentOptions.DisableXboxLive) OptimizeHelper.DisableXboxLive();
                    if (OptionsHelper.CurrentOptions.DisableGameBar) OptimizeHelper.DisableGameBar();
                    if (OptionsHelper.CurrentOptions.DisableWidgets) OptimizeHelper.DisableWidgets();
                    if (OptionsHelper.CurrentOptions.DisableCoPilotAI) OptimizeHelper.DisableCoPilotAI();
                    OptionsHelper.SaveSettings();
                    MessageBox.Show("Tweaks applied!", "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Logger.Log("MainForm.BuildTweaksPage", ex.Message, ex.StackTrace);
                    MessageBox.Show("Error: " + ex.Message, "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            layout.Controls.Add(btnApply);
            panel.Controls.Add(layout);
            panel.Controls.Add(title);
            return panel;
        }

        private Panel BuildCleanerPage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var title = new Label { Text = "System & Browser Cleaner", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(0, 10, 0, 0) };

            void AddCleanRow(string name, Func<long> preview, Action clean)
            {
                var row = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Margin = new Padding(0, 4, 0, 4) };
                var lbl = new Label { Text = name, Width = 160, Height = 32, TextAlign = ContentAlignment.MiddleLeft };
                var sizeLbl = new Label { Text = "-", Width = 100, Height = 32, TextAlign = ContentAlignment.MiddleLeft, ForeColor = ThemeManager.Current.TextSecondary };
                var btn = new FluentButton { Text = "Clean", Kind = ButtonKind.Standard, Width = 90, Height = 30 };
                btn.Click += (s, e) =>
                {
                    try { clean(); sizeLbl.Text = Utilities.ByteSizeToString(preview()); MessageBox.Show(name + " cleaned.", "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };
                try { sizeLbl.Text = Utilities.ByteSizeToString(preview()); } catch { }
                row.Controls.Add(lbl); row.Controls.Add(sizeLbl); row.Controls.Add(btn);
                layout.Controls.Add(row);
            }

            AddCleanRow("Temp Files", CleanHelper.PreviewTemp, CleanHelper.CleanTemp);
            AddCleanRow("Minidumps", CleanHelper.PreviewMinidumps, CleanHelper.CleanMinidumps);
            AddCleanRow("Recycle Bin", CleanHelper.PreviewRecycleBin, CleanHelper.CleanRecycleBin);
            AddCleanRow("Chrome Cache", () => CleanHelper.PreviewBrowserCache("Chrome"), () => CleanHelper.CleanBrowserCache("Chrome"));
            AddCleanRow("Edge Cache", () => CleanHelper.PreviewBrowserCache("Edge"), () => CleanHelper.CleanBrowserCache("Edge"));
            AddCleanRow("Firefox Cache", () => CleanHelper.PreviewBrowserCache("Firefox"), () => CleanHelper.CleanBrowserCache("Firefox"));
            AddCleanRow("Brave Cache", () => CleanHelper.PreviewBrowserCache("Brave"), () => CleanHelper.CleanBrowserCache("Brave"));

            var btnAll = new FluentButton { Text = "Clean All", Kind = ButtonKind.Primary, Width = 140, Height = 36, Margin = new Padding(0, 12, 0, 0) };
            btnAll.Click += (s, e) =>
            {
                try
                {
                    CleanHelper.CleanTemp(); CleanHelper.CleanMinidumps(); CleanHelper.CleanRecycleBin();
                    CleanHelper.CleanBrowserCache("Chrome"); CleanHelper.CleanBrowserCache("Edge");
                    CleanHelper.CleanBrowserCache("Firefox"); CleanHelper.CleanBrowserCache("Brave");
                    MessageBox.Show("All cleaning done.", "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            };
            layout.Controls.Add(btnAll);
            panel.Controls.Add(layout);
            panel.Controls.Add(title);
            return panel;
        }

        private Panel BuildAppsPage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var label = new Label { Text = "UWP / Store Apps", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            var list = new ListBox { Dock = DockStyle.Fill };
            try { foreach (var app in UWPHelper.GetUWPApps(true)) list.Items.Add(app.Title); }
            catch { list.Items.Add("Unable to load UWP apps"); }
            panel.Controls.Add(list);
            panel.Controls.Add(label);
            return panel;
        }

        private Panel BuildStartupPage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var label = new Label { Text = "Startup Items", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            var list = new ListBox { Dock = DockStyle.Fill };
            try { foreach (var item in StartupHelper.GetStartupItems()) list.Items.Add($"{item.Name} [{item.RegistryLocation}]"); }
            catch { list.Items.Add("Unable to load startup items"); }
            panel.Controls.Add(list);
            panel.Controls.Add(label);
            return panel;
        }

        private Panel BuildHostsPage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var label = new Label { Text = "Hosts File Editor", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            var textBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Both, Font = new Font("Consolas", 10) };
            try { textBox.Lines = HostsHelper.ReadHosts(); } catch { textBox.Text = "Unable to read hosts file"; }
            var btnSave = new FluentButton { Text = "Save Hosts", Kind = ButtonKind.Primary, Dock = DockStyle.Bottom, Height = 35, Margin = new Padding(0, 10, 0, 0) };
            btnSave.Click += (s, e) => { try { HostsHelper.SaveHosts(textBox.Lines); MessageBox.Show("Hosts saved!", "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Information); } catch (Exception ex) { MessageBox.Show(ex.Message, "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Error); } };
            panel.Controls.Add(textBox);
            panel.Controls.Add(label);
            panel.Controls.Add(btnSave);
            return panel;
        }

        private Panel BuildDnsPage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown };
            var dnsOptions = new[] { ("Cloudflare", PingerHelper.CloudflareDNSv4), ("Google", PingerHelper.GoogleDNSv4), ("OpenDNS", PingerHelper.OpenDNSv4), ("Quad9", PingerHelper.Quad9DNSv4) };
            foreach (var (name, ips) in dnsOptions)
            {
                var btn = new FluentButton { Text = $"Set {name} DNS ({ips[0]})", Width = 220, Margin = new Padding(5) };
                var ipsCopy = ips;
                btn.Click += (s, e) => { foreach (var nic in PingerHelper.GetActiveNetworkAdapters()) PingerHelper.SetDNS(nic.Name, ipsCopy, null); MessageBox.Show($"DNS set to {name}", "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Information); };
                layout.Controls.Add(btn);
            }
            var btnReset = new FluentButton { Text = "Reset DNS to DHCP", Kind = ButtonKind.Subtle, Width = 220, Margin = new Padding(5) };
            btnReset.Click += (s, e) => { PingerHelper.ResetDefaultDNSForAllNICs(); MessageBox.Show("DNS reset!", "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Information); };
            layout.Controls.Add(btnReset);
            var btnFlush = new FluentButton { Text = "Flush DNS Cache", Width = 220, Margin = new Padding(5) };
            btnFlush.Click += (s, e) => { PingerHelper.FlushDNSCache(); MessageBox.Show("DNS cache flushed!", "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Information); };
            layout.Controls.Add(btnFlush);
            panel.Controls.Add(layout);
            return panel;
        }

        private Panel BuildHardwarePage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var label = new Label { Text = "Hardware Information", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            var info = new Label { Dock = DockStyle.Fill, AutoSize = false };
            try
            {
                var hw = HardwareHelper.GetHardwareInfo();
                info.Text = $"CPU: {hw.CpuName}\nRAM: {hw.TotalRam}\nGPU: {hw.GpuName}\nDisk: {hw.DiskModel}\nMotherboard: {hw.MotherboardModel}\nBIOS: {hw.BiosVersion}";
            }
            catch { info.Text = "Unable to read hardware info"; }
            panel.Controls.Add(info);
            panel.Controls.Add(label);
            return panel;
        }

        private Panel BuildOptionsPage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown };
            AddToggle(layout, "Check Updates on Launch", () => OptionsHelper.CurrentOptions.UpdateOnLaunch, v => OptionsHelper.CurrentOptions.UpdateOnLaunch = v);
            AddToggle(layout, "Auto Start with Windows", () => OptionsHelper.CurrentOptions.AutoStart, v => OptionsHelper.CurrentOptions.AutoStart = v);
            var btnSave = new FluentButton { Text = "Save Settings", Kind = ButtonKind.Primary, Width = 150, Margin = new Padding(0, 10, 0, 0) };
            btnSave.Click += (s, e) => { OptionsHelper.SaveSettings(); MessageBox.Show("Settings saved!", "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Information); };
            layout.Controls.Add(btnSave);
            panel.Controls.Add(layout);
            return panel;
        }

        private Panel BuildServicesPage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var title = new Label { Text = "Non-Microsoft Service Manager", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            var listView = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true };
            listView.Columns.Add("Service", 200);
            listView.Columns.Add("Display Name", 250);
            listView.Columns.Add("Status", 100);
            listView.Columns.Add("Start Type", 100);
            var btnRefresh = new FluentButton { Text = "Refresh", Kind = ButtonKind.Standard, Width = 100, Height = 35 };
            var btnStop = new FluentButton { Text = "Stop Selected", Kind = ButtonKind.Primary, Width = 140, Height = 35 };
            var lblResult = new Label { Text = "", Height = 30, AutoSize = false, ForeColor = ThemeManager.Current.TextSecondary };
            btnRefresh.Click += (s, e) =>
            {
                listView.Items.Clear();
                try
                {
                    foreach (var svc in ServiceHelper.GetNonMicrosoftServices())
                        if (!svc.IsMicrosoft) listView.Items.Add(new ListViewItem(new[] { svc.Name, svc.DisplayName, svc.Status, svc.StartType }));
                    lblResult.Text = listView.Items.Count + " non-Microsoft services found.";
                }
                catch (Exception ex) { lblResult.Text = "Error: " + ex.Message; }
            };
            btnStop.Click += (s, e) =>
            {
                if (listView.SelectedItems.Count == 0) { lblResult.Text = "Select a service."; return; }
                var selected = listView.SelectedItems[0].Text;
                try
                {
                    if (ServiceHelper.StopService(selected)) { lblResult.Text = "Stopped: " + selected; btnRefresh.PerformClick(); }
                    else lblResult.Text = "Could not stop: " + selected;
                }
                catch (Exception ex) { lblResult.Text = "Error: " + ex.Message; }
            };
            var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(10) };
            btnPanel.Controls.Add(btnRefresh); btnPanel.Controls.Add(btnStop); btnPanel.Controls.Add(lblResult);
            panel.Controls.Add(title); panel.Controls.Add(listView); panel.Controls.Add(btnPanel);
            btnRefresh.PerformClick();
            return panel;
        }

        private Panel BuildGamingPage()
        {
            var panel = new Panel { AutoScroll = true, Padding = new Padding(20) };
            var title = new Label { Text = "Gaming / FPS Optimizations", Dock = DockStyle.Top, Height = 30, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            var desc = new Label { Text = "Apply registry and system tweaks for better game performance.", Dock = DockStyle.Top, Height = 24, ForeColor = ThemeManager.Current.TextSecondary };
            var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(0, 10, 0, 0) };
            AddToggle(layout, "Disable Network Throttling", () => OptionsHelper.CurrentOptions.DisableNetworkThrottling, v => OptionsHelper.CurrentOptions.DisableNetworkThrottling = v, OptimizeHelper.OptimizeNetworkForGaming);
            AddToggle(layout, "Disable Game Bar / DVR", () => OptionsHelper.CurrentOptions.DisableGameBar, v => OptionsHelper.CurrentOptions.DisableGameBar = v, OptimizeHelper.DisableGameBar);
            AddToggle(layout, "Disable Superfetch (SysMain)", () => OptionsHelper.CurrentOptions.DisableSuperfetch, v => OptionsHelper.CurrentOptions.DisableSuperfetch = v, OptimizeHelper.DisableSuperfetch);
            AddToggle(layout, "Disable Visual Effects", () => OptionsHelper.CurrentOptions.EnablePerformanceTweaks, v => OptionsHelper.CurrentOptions.EnablePerformanceTweaks = v, OptimizeHelper.DisableVisualEffects);
            var btnApply = new FluentButton { Text = "Apply All Gaming Optimizations", Kind = ButtonKind.Primary, Width = 260, Height = 40, Margin = new Padding(0, 16, 0, 0) };
            btnApply.Click += (s, e) =>
            {
                try
                {
                    OptimizeHelper.ApplyGamingOptimizations();
                    OptimizeHelper.DisableVisualEffects();
                    OptimizeHelper.OptimizeNetworkForGaming();
                    OptimizeHelper.DisableGameBar();
                    OptionsHelper.SaveSettings();
                    MessageBox.Show("Gaming optimizations applied!", "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Logger.Log("MainForm.BuildGamingPage", ex.Message, ex.StackTrace);
                    MessageBox.Show("Error: " + ex.Message, "HardOptimizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            layout.Controls.Add(btnApply);
            panel.Controls.Add(layout);
            panel.Controls.Add(desc);
            panel.Controls.Add(title);
            return panel;
        }
    }
}
