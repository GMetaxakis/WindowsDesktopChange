using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using Microsoft.Win32;

namespace WindowsDesktopChange
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string AppName = "WindowsDesktopChange";
        InputSimulator inputSimulator;
        NotifyIcon rightIcon = new NotifyIcon();
        NotifyIcon leftIcon = new NotifyIcon();
        ContextMenu contextMenu;

        public MainWindow()
        {
            InitializeComponent();
            inputSimulator = new InputSimulator();
            try
            {
                rightIcon.Icon = new Icon(@"right.ico");
                rightIcon.Visible = true;
                rightIcon.Click += rightIcon_Click;

                leftIcon.Icon = new Icon(@"left.ico");
                leftIcon.Visible = true;
                leftIcon.Click += leftIcon_Click;

                contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add("Go to left", leftIcon_Click);
                contextMenu.MenuItems.Add("Go to right", rightIcon_Click);
                contextMenu.MenuItems.Add("Run on startup", startup_Click);
                contextMenu.MenuItems.Add("Exit", (s, e) => Close());

                rightIcon.ContextMenu = contextMenu;
                leftIcon.ContextMenu = contextMenu;
            }
            catch (Exception)
            {

            }
        }

        private void rightIcon_Click(object sender, EventArgs e)
        {
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LWIN);
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RIGHT);

            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.LWIN);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.RIGHT);
        }

        private void leftIcon_Click(object sender, EventArgs e)
        {
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LWIN);
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LEFT);

            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.LWIN);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.LEFT);
        }

        private void startup_Click(object sender, EventArgs e)
        {
            if (sender is MenuItem)
            {
                var item = sender as MenuItem;
                item.Checked = !item.Checked;

                try
                {
                    RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                    if (item.Checked)
                        rk.SetValue(AppName, System.Windows.Forms.Application.ExecutablePath);
                    else
                        rk.DeleteValue(AppName, false);
                }
                catch (Exception) { }
            }

        }

    }
}
