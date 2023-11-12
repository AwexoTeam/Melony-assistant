using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;
using Telepathy;

namespace MelonyLovense
{
    public partial class Form1 : Form
    {
        public static Form1 instance;

        public bool hasConnected = false;
        public ulong userId = 0;

        public Form1()
        {

            InitializeComponent();
        }

        public void SetStatus(string message, Color color)
        {
            statusLabel.Text = message;
            statusPicture.BackColor = Color.Red;
        }

        public void SetError(string message) => SetStatus(message, Color.Red);


        private void tick_Tick(object sender, EventArgs e)
        {
            if (!hasConnected)
            {
                TryConnect();
                return;
            }

            ServerManager.Tick();
            LovenseManager.Tick();
        }

        private void TryConnect()
        {
            string str = uidTB.Text;
            if (!ulong.TryParse(str, out userId))
            {
                SetError("User Id is not a valid number!");
                return;
            }

            if (!ServerManager.ConnectToServer())
                return;

            Console.WriteLine("Connected!");

            if (!LovenseManager.TryConnect())
                return;

            statusLabel.Text = "Toys found!";
            statusPicture.BackColor = Color.Green;
            hasConnected = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            instance = this;
            string lastUid = File.ReadAllText("lastUid.txt");
            uidTB.Text = lastUid;

            ServerManager.Initialize();
            LovenseManager.Initialize();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText("lastUid.txt", uidTB.Text);
        }
    }
}