using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Probability
{
    public partial class FormMain : Form
    {
        Logger logger;
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logger = new Logger(richTextBoxMainLogger, chartLog, "log.txt", 5);
            logger.log("Hello");

            /*
            Random rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int r1 = rand.Next(200);
                logger.logChart(r1);
                logger.log(r1.ToString());
            }
            */

            World world = new World(logger);

        }


        private void buttonRun01_Click(object sender, EventArgs e)
        {





        }
    }
}
