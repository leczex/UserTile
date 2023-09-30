using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Nini.Config;
using UserTileLib;

namespace UserTile {
    internal class Program {
        [STAThread]
        private static void Main(string[] args) {
            Application.SetCompatibleTextRenderingDefault(false);

            string photo = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\UserTile\\avatar.png";

            if (File.Exists("config.ini")) {
                Program.config = new IniConfigSource("config.ini");
                //Program.AvatarPath = Program.config.Configs["Main"].GetString("AvatarPath");
                Program.AvatarPath = photo;
            }
            
            Program.taskbarManager = new TaskbarManager();
            Program.userPic = new UserPic();
            
            if (!Program.taskbarManager.IsTaskbarSmall()) {
                Program.userPic.Width = 37;
                Program.userPic.Height = 36;
                Program.taskbarManager.ReserveSpace(37);
            }
            else {
                Program.userPic.Width = 27;
                Program.userPic.Height = 26;
                Program.taskbarManager.ReserveSpace(27);
            }
            
            Program.userPic.Top = 3;
            Program.taskbarManager.AddControl(Program.userPic);
            Program.timer = new System.Windows.Forms.Timer();
            Program.timer.Interval = 10;
            Program.timer.Tick += Program.TimerTick;
            Program.timer.Enabled = true;
            
            Application.EnableVisualStyles();
            Application.ThreadException += Program.Application_ThreadException;
            Application.Run();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            Program.Log(e.Exception);
        }

        public static void Log(Exception e) {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            if (!File.Exists(directoryName + "\\log.txt")) {
                File.WriteAllText(directoryName + "\\log.txt", string.Empty);
            }
            
            try {
                File.AppendAllText(directoryName + "\\log.txt", string.Concat(new object[] {
                    DateTime.Now,
                    " -------------- ",
                    '\r',
                    '\n',
                    "OS: ",
                    Environment.OSVersion.VersionString,
                    '\r',
                    '\n',
                    e.ToString(),
                    '\r',
                    '\n'
                }));
            }
            catch (Exception ex) {
                MessageBox.Show("Can't write log. " + ex.Message);
            }
        }

        private static void TimerTick(object sender, EventArgs e) {
            Program.taskbarManager.CheckTaskbar();
        }

        public static IntPtr taskbarHwnd;
        public static IntPtr trayHwnd;
        private static IntPtr langbarHwnd;
        private static IntPtr showDesktopButtonHwnd;
        private static UserPic userPic;
        private static System.Windows.Forms.Timer timer;
        public static WinAPI.RECT defaultTrayRect;
        public static WinAPI.WINDOWPLACEMENT showDesktopDefaultPlacement;
        public static TaskbarManager taskbarManager;
        private static IConfigSource config;
        public static string AvatarPath;
    }
}
