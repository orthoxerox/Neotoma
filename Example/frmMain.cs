using Neotoma;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Neotoma.Helpers;

namespace Example
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            var g = new Grammar();

            var letters = r('a', 'z') / r('A', 'Z');
            var digits = r('0', '9');
            var _ = p("_");
            var number = "-" + digits[1] 
                + ~("." + digits[1]) 
                + ~(s("eE") + ~p("-") + digits[1]);

        }
    }
}
