using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AxWMPLib;
using WMPLib;

namespace UserTile {
    public class UserPic : UserControl {
        protected override void Dispose(bool disposing) {
            if (disposing && this.components != null) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UserPic));
            this.contextMenu = new ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new ToolStripMenuItem();
            this.player = new AxWindowsMediaPlayer();
            this.contextMenu.SuspendLayout();
            ((ISupportInitialize)this.player).BeginInit();
            base.SuspendLayout();
            this.contextMenu.Items.AddRange(new ToolStripItem[] { this.toolStripMenuItem1 });
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new Size(104, 26);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(103, 22);
            this.toolStripMenuItem1.Text = "Close";
            this.toolStripMenuItem1.Click += this.toolStripMenuItem1_Click;
            this.player.Enabled = true;
            this.player.Location = new Point(0, 0);
            this.player.Name = "player";
            //this.player.OcxState = (AxHost.State)componentResourceManager.GetObject("player.OcxState");
            this.player.Size = new Size(37, 36);
            this.player.TabIndex = 1;
            this.player.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.Controls.Add(this.player);
            base.Name = "UserPic";
            base.SizeChanged += this.UserPicSizeChanged;
            this.contextMenu.ResumeLayout(false);
            ((ISupportInitialize)this.player).EndInit();
            base.ResumeLayout(false);
        }

        public UserPic() {
            this.InitializeComponent();
            this.picture = new PictureBox();
            this.picture.Width = base.Width - 2;
            this.picture.Height = base.Height - 2;
            this.picture.Left = 1;
            this.picture.Top = 1;
            this.picture.BringToFront();
            this.picture.SizeMode = PictureBoxSizeMode.StretchImage;

            if (string.IsNullOrEmpty(Program.AvatarPath)) {
                if (File.Exists(Path.GetTempPath() + "\\" + Environment.UserName + ".bmp")) {
                    this.picture.Load(Path.GetTempPath() + "\\" + Environment.UserName + ".bmp");
                }
                else {
                    this.picture.Load(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Resources\\userpic.png");
                }

                this.picture.Parent = this;
            }
            else if (Program.AvatarPath.EndsWith(".wmv") && File.Exists(Program.AvatarPath)) {
                this.player.Width = base.Width - 2;
                this.player.Height = base.Height - 2;
                this.player.Left = 1;
                this.player.Top = 1;
                this.player.uiMode = "none";
                this.player.enableContextMenu = false;
                this.player.URL = Program.AvatarPath;
                this.player.Visible = true;
                this.player.ClickEvent += this.PlayerClickEvent;
                this.t = new Timer();
                this.t.Interval = 1000;
                this.t.Tick += this.Tick;
                this.t.Start();
            }
            else {
                this.picture.Load(Program.AvatarPath);
                this.picture.Parent = this;
            }

            this.picture.MouseClick += this.PictureMouseClick;
        }

        private void Tick(object sender, EventArgs e) {
            if (this.player != null && this.player.currentMedia != null) {
                if (this.player.Ctlcontrols.currentPosition >= this.player.currentMedia.duration - 0.5) {
                    this.player.Ctlcontrols.currentPosition = 0.0;
                }

                if (this.player.playState != WMPPlayState.wmppsMediaEnded) {
                    this.player.Width = base.Width - 2;
                    this.player.Height = base.Height - 2;
                    this.player.Ctlcontrols.play();
                }
            }
        }

        private void PictureMouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                PopupWindow popupWindow = new PopupWindow();
                popupWindow.Show();
            }
        }

        protected override void WndProc(ref Message m) {
            if (m.Msg == 123) {
                this.contextMenu.Left = Control.MousePosition.X;
                this.contextMenu.Show(this, Control.MousePosition);
            }
            else {
                base.WndProc(ref m);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (this.t != null) {
                this.t.Stop();
                this.t.Dispose();
            }

            Program.taskbarManager.Dispose();
            Application.Exit();
        }

        private void UserPicSizeChanged(object sender, EventArgs e) {
            this.picture.Width = base.Width - 2;
            this.picture.Height = base.Height - 2;
            this.player.Width = base.Width - 2;
            this.player.Height = base.Height - 2;
        }

        private void PlayerClickEvent(object sender, _WMPOCXEvents_ClickEvent e) {
            if (e.nButton == 1) {
                PopupWindow popupWindow = new PopupWindow();
                popupWindow.Show();
            }
            else {
                this.contextMenu.Left = Control.MousePosition.X;
                this.contextMenu.Show(this, Control.MousePosition);
            }
        }

        private IContainer components = null;
        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem toolStripMenuItem1;
        private AxWindowsMediaPlayer player;
        private readonly PictureBox picture;
        private readonly Timer t;
    }
}
