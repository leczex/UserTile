using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace UserTile {
    public partial class PopupWindow : Window {
        public PopupWindow() {
            this.InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

        }

        private void Window_Deactivated(object sender, EventArgs e) {
            base.Close();
        }

        private void Window_SourceInitialized(object sender, EventArgs e) {
            base.Top = (double)SystemInformation.WorkingArea.Bottom - base.Height - 5.0;
            base.Left = (double)SystemInformation.WorkingArea.Right - base.Width - 10.0;
            
            if (!string.IsNullOrEmpty(Program.AvatarPath) && File.Exists(Program.AvatarPath)) {
                if (Program.AvatarPath.EndsWith(".wmv")) {
                    this.Player.Source = new Uri(Program.AvatarPath, UriKind.Relative);
                    this.Player.Play();
                }
                else {
                    this.Avatar.Source = new BitmapImage(new Uri(Program.AvatarPath, UriKind.RelativeOrAbsolute));
                }
            }
            else if (File.Exists(Path.GetTempPath() + "\\" + Environment.UserName + ".bmp")) {
                this.Avatar.Source = new BitmapImage(new Uri(Path.GetTempPath() + "\\" + Environment.UserName + ".bmp"));
            }
            else {
                this.Avatar.Source = new BitmapImage(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Resources\\userpic.png"));
            }
            
            this.Username.Text = Environment.UserName;
        }

        private void LockButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Process.Start("c:\\windows\\system32\\rundll32.exe", "user32.dll,LockWorkStation");
        }

        private void LogOffButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Process.Start("shutdown.exe", "/l");
        }

        private void MySettingsButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Process.Start("control.exe");
        }

        private void MyLookButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            Process.Start("control.exe", "userpasswords");
        }

        private void SwitchUserButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            try {
                Process.Start("tsdiscon.exe");
            }
            catch (Exception e2) {
                Program.Log(e2);
            }
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e) {
            this.Player.Position = TimeSpan.FromMilliseconds(0.0);
            this.Player.Play();
        }

        private void Window_Closed(object sender, EventArgs e) {
            this.Player.Close();
        }
    }
}
