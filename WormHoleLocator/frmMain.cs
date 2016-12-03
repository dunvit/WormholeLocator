using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using WHLocator.Infrastructure;

namespace WHLocator
{
    public partial class frmMain : Form
    {
        public delegate void DelegateStartProcess(string value);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        private Wormholes _wormholes = new Wormholes();

        private CcpXmlApi _ccpXmlApi = new CcpXmlApi();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private Pilot _currentPilot;

        private static readonly ILog Log = LogManager.GetLogger(typeof(frmMain));

        private Size _sizeCompact = new Size(98, 43);

        private Size _sizeOpen = new Size(585, 332);

        private Size _sizeWithZkillboard = new Size(1467, 662);

        public frmMain()
        {
            InitializeComponent();

            DelegateStartProcess startProcessFunction = StartPilotAuthorizeFlow;

            new Thread(() => new CrestApiListener().ListenLocalhost(startProcessFunction)) { IsBackground = true }.Start();

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Size = _sizeOpen;
            cmdResize.Text = "Hide";
        }

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendTextBox), value);
                return;
            }
            Log.Debug(value);
        }

        public void StartPilotAuthorizeFlow(string value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(StartPilotAuthorizeFlow), value);
                return;
            }

            Log.Debug(value);

            PilotAuthorizeFlow(value);

            BringApplicationToFront();
        }

        private void PilotAuthorizeFlow(string code)
        {
            Log.DebugFormat("Start PilotAuthorizeFlow for token = {0}", code);

            _currentPilot = new Pilot();

            _currentPilot.Initialization(code);

            crlPilotPortrait.Image = _currentPilot.Portrait;

            crlPilotPortrait.Refresh();

            crlPilotName.Text = _currentPilot.Name;

            UpdateLocationInfo(_currentPilot.Name);

            if (_currentPilot.Location.Id > 0)
            {
                cmdShowCurrentLocation.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
            }
            

            panelCurrentPilot.Visible = true;
        }

        private void UpdateLocationInfo(string pilotName)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateLocationInfo), pilotName);
                return;
            }

            crlPilotLocation.Text = _currentPilot.Location.Name;

            label11.Text = _currentPilot.Location.Class;
            label11.Visible = true;

            label12.Text = _currentPilot.Location.Effect;
            label12.Visible = true;

            

            switch (_currentPilot.Location.StaticSystems.Count)
            {
                case 0:
                    label13.Visible = false;
                    label14.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    break;
                case 1:
                    label14.Visible = false;
                    label3.Visible = false;
                    label13.Text = _currentPilot.Location.StaticSystems[0];
                    label13.Visible = true;
                    label2.Text = _wormholes.GetWormhole(_currentPilot.Location.StaticSystems[0]).Name;
                    label2.Visible = true;
                    break;
                case 2:
                    label13.Text = _currentPilot.Location.StaticSystems[0];
                    label13.Visible = true;
                    label14.Text = _currentPilot.Location.StaticSystems[1];
                    label14.Visible = true;
                    label2.Text = _wormholes.GetWormhole(_currentPilot.Location.StaticSystems[0]).Name;
                    label2.Visible = true;
                    label3.Text = _wormholes.GetWormhole(_currentPilot.Location.StaticSystems[1]).Name;
                    label3.Visible = true;
                    break;
            }
        }



        public void BringApplicationToFront()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(BringApplicationToFront));
            }
            else
            {
                var topMost = TopMost;

                TopMost = true;
                Focus();
                BringToFront();
                System.Media.SystemSounds.Beep.Play();
                TopMost = topMost;
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var data = WebUtility.UrlEncode( @"http://localhost:8080/WormholeLocator");
            Process.Start("https://login.eveonline.com/oauth/authorize?response_type=code&redirect_uri=" + data + "&client_id=8f1e2ac9d4aa467c88b12674926dc5e6&scope=characterLocationRead&state=75c68f04aec80589a157fd13");
        }

        #region GUI


        #endregion

        private void crlTimer_Tick(object sender, EventArgs e)
        {
            if (_currentPilot != null)
            {
                Task.Run(() =>
                {
                    AppendTextBox("Start get location info.");
                    _currentPilot.RefreshInfo();
                    AppendTextBox("End get location info.");

                    UpdateLocationInfo(_currentPilot.Name);
                });
            }
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }



        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void cmdResize_Click(object sender, EventArgs e)
        {
            if (Size == _sizeCompact)
            {
                Size = _sizeOpen;
                cmdResize.Text = "Hide";
                cmdShowZkillboard.Text = "Show Zkillboard";
            }
            else if(Size == _sizeOpen)
            {
                Size = _sizeCompact;
                cmdResize.Text = "Show";
                cmdShowZkillboard.Text = "Show Zkillboard";
            } if (Size == _sizeWithZkillboard)
            {
                Size = _sizeOpen;
                cmdResize.Text = "Hide";
            }
        }

        private void cmdShowZkillboard_Click(object sender, EventArgs e)
        {
            

            if (Size == _sizeCompact || Size == _sizeOpen)
            {
                cmdShowZkillboard.Text = "Zkillboard";
                Size = _sizeWithZkillboard;
            }
            else
            {
                cmdShowZkillboard.Text = "Zkillboard";
                Size = _sizeOpen;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cmdPin.Text == "Pin")
            {
                cmdPin.Text = "UnPin";
                TopMost = true;
            }
            else
            {
                cmdPin.Text = "Pin";
                TopMost = false;
            }
        }

        private void cmdShowCurrentLocation_Click(object sender, EventArgs e)
        {
            if (_currentPilot != null && _currentPilot.Location != null && _currentPilot.Location.Id > 0)
            {
                webBrowser1.Url = new Uri("https://zkillboard.com/system/" + _currentPilot.Location.Id + "/");

                Size = _sizeWithZkillboard;

                return;
            }

            MessageBox.Show("First log in with Eve Online");
        }

        private void cmdPastePilots_Click(object sender, EventArgs e)
        {
            cmdPilotsInLocal.DisplayMember = "Text";
            cmdPilotsInLocal.ValueMember = "Value";

            cmdPilotsInLocal.Items.Clear();


            var txtInClip = Clipboard.GetText();

            if(string.IsNullOrEmpty(txtInClip))
            {
                return;
            }

            string [] pilots;
            
            //= txtInClip.Split(new[] { "\r\n" }, StringSplitOptions.None);

            pilots = txtInClip.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var pilot in pilots)
            {
                cmdPilotsInLocal.Items.Add(new { Text = pilot, Value = pilot });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmdPilotsInLocal.Text))
            {
                return;
            }

            var characterId = _ccpXmlApi.GetPilotIdByName(cmdPilotsInLocal.Text.Trim());

            webBrowser1.Url = new Uri("https://zkillboard.com/character/" + characterId + "/");

            Size = _sizeWithZkillboard;

            //http://eve-hunt.net/hunt/Ksanta%20Eddord/
        }

        private void cmdWhList_Click(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri("http://www.ellatha.com/eve/wormholelist.asp");

            Size = _sizeWithZkillboard;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmdPilotsInLocal.Text))
            {
                return;
            }

            webBrowser1.Url = new Uri("http://eve-hunt.net/hunt/" + cmdPilotsInLocal.Text + "/");

            Size = _sizeWithZkillboard;

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (_currentPilot != null && _currentPilot.Location != null && _currentPilot.Location.Id > 0)
            {
                webBrowser1.Url = new Uri("http://superpute.com/system/" + _currentPilot.Location.Name + "");

                Size = _sizeWithZkillboard;

                return;
            }

            MessageBox.Show("First log in with Eve Online");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_currentPilot != null && _currentPilot.Location != null && _currentPilot.Location.Id > 0)
            {

                if (_currentPilot.Location.Name.Contains("J") == false)
                {
                    MessageBox.Show("Ellatha only for W-Space systems");
                    return;
                }

                webBrowser1.Url = new Uri("http://www.ellatha.com/eve/WormholeSystemview.asp?key=" + _currentPilot.Location.Name.Replace("J","") + "");

                Size = _sizeWithZkillboard;

                return;
            }

            MessageBox.Show("First log in with Eve Online");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_currentPilot != null && _currentPilot.Location != null && _currentPilot.Location.Id > 0)
            {

                webBrowser1.Url = new Uri("http://evemaps.dotlan.net/system/" + _currentPilot.Location.Name + "");

                Size = _sizeWithZkillboard;

                return;
            }

            MessageBox.Show("First log in with Eve Online");
        }
    }
}
