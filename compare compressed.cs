﻿using System;
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
    public partial class compare_compressed : Form
    {
        public compare_compressed()
        {
            InitializeComponent();
            client.comp = chart1;
            chart1.ChartAreas[0].AxisY.Title = "Time->";
            chart1.ChartAreas[0].AxisX.Title = "Number of bytes->";
        }
    }
}
