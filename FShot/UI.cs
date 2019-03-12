using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FShot
{
    class UI : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public UI()
        {
            MenuItem runOnStartup = new MenuItem("Run on startup", runOnStartupClick);
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            RegistryKey rkApp2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run", true);
            if (rkApp.GetValue("FShot") == null || rkApp2.GetValue("FShot") == null)
            {
                runOnStartup.Checked = false;
            }
            else
            {
                runOnStartup.Checked = true;

                if (rkApp.GetValue("FShot").ToString() != Application.ExecutablePath)
                    rkApp.SetValue("FShot", Application.ExecutablePath);

                if ((byte[])rkApp2.GetValue("FShot") != new byte[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })
                    rkApp2.SetValue("FShot", new byte[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, Microsoft.Win32.RegistryValueKind.Binary);
            }

            trayIcon = new NotifyIcon()
            {
                Icon = Properties.Resources.Placeholder,
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit),
                runOnStartup
            }),
                Visible = true
            };
            new Thread(startBot).Start();
            KeyboardHook.HookKeyboard();
        }


        void runOnStartupClick(object sender, EventArgs e) {

            ((MenuItem)sender).Checked = !((MenuItem)sender).Checked;
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            RegistryKey rkApp2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\StartupApproved\\Run", true);
            if (((MenuItem)sender).Checked)
            {
                rkApp.SetValue("FShot", Application.ExecutablePath);
                //byte[] data = new byte[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                rkApp2.SetValue("FShot", new byte[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, Microsoft.Win32.RegistryValueKind.Binary);
            }
            else
            {
                rkApp.DeleteValue("FShot", false);
                rkApp2.DeleteValue("FShot", false);
            }
        }

        void startBot() {
                DiscordBot.initialize("NTUzNzc2NjY3NTU4OTM2NTc2.D2THiw.kHiu96iirilxaVf8lQk7AUkMXNc").ConfigureAwait(false).GetAwaiter().GetResult();
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            Environment.Exit(0);
            Application.Exit();
        }
    }
}
