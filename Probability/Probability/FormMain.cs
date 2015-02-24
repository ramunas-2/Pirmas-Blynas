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
        WorldAugmented worldAugmented;
        public FormMain()
        {
            InitializeComponent();
        }

        void run()
        {
            worldAugmented.mainScenario();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logger = new Logger(richTextBoxMainLogger, chartLog, "log.txt", 5);
            logger.log("Hello");
            logger.set("Error",10,Color.Red);
            worldAugmented = new WorldAugmented(logger);
            //run();
            //change3
        }


        private void buttonRun01_Click(object sender, EventArgs e)
        {
            run();




        }

        private void buttonRun2_Click(object sender, EventArgs e)
        {
            worldAugmented.mainScenario2();
        }
    }
}
