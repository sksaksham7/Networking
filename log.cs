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
    public partial class log : Form
    {
        public log()
        {
            InitializeComponent();
            client.logBox = logBox;
        }
    }
}
