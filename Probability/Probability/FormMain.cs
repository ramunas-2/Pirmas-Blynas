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
        World world;
        public FormMain()
        {
            InitializeComponent();
        }

        void run()
        {
            world.mainScenario();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logger = new Logger(richTextBoxMainLogger, chartLog, "log.txt", 5);
            logger.log("Hello");
            world = new World(logger);
            //run();
        }


        private void buttonRun01_Click(object sender, EventArgs e)
        {
            run();




        }
    }
}
