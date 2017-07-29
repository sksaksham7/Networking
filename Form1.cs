using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Le_Chat_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            client.bps = bps;
            client.time = time;
            (new graph()).Show();
            (new log()).Show();
            client.Connect();
        }

        private void sendTextMessage_Click(object sender, EventArgs e)
        {
            string text = messageBox.Text;
            client.send(client.Command.txtMesg, text);
        }

        private void sendMP4_Click(object sender, EventArgs e)
        {
            if (!client.shouldZip)
            {
                client.bps = bps;
                client.time = time;
                client.currentFileType = client.FileTypes.mp4;
            }
            else
            {
                (new compare_compressed()).Show();
            }
            client.startSendFile(@"C:\Users\Sid\Desktop\dummy.mp4", 0, client.FileTypes.mp4);
        }

        private void senMp3_Click(object sender, EventArgs e)
        {
            client.bps = bpsMp3;
            client.time = timeMp3;
            client.startSendFile(@"C:\Users\Sid\Desktop\dummy.mp3",0,client.FileTypes.mp3);
            client.currentFileType = client.FileTypes.mp3;
        }

        private void sendTxt_Click(object sender, EventArgs e)
        {
            client.bps = bpsTxt;
            client.time = timeTxt;
            client.startSendFile(@"C:\Users\Sid\Desktop\dummy.txt",0,client.FileTypes.text);
            client.currentFileType = client.FileTypes.text;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) client.shouldZip = true;
            else client.shouldZip = false;
        }
    }
}
