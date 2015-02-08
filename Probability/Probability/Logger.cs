using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
    class Logger
    {
        System.Windows.Forms.RichTextBox richTextLog;
        System.Windows.Forms.DataVisualization.Charting.Chart chartLog;
        string logFile;
        int debugLevel;
        StreamWriter w;
        public Logger(System.Windows.Forms.RichTextBox richTextLog, System.Windows.Forms.DataVisualization.Charting.Chart chartLog, string logFile, int debugLevel)
        {
            this.richTextLog = richTextLog;
            this.chartLog = chartLog;
            this.logFile = logFile;
            this.debugLevel = debugLevel;
            w = File.AppendText(logFile);
        }

        public void log(string logMessage)
        {
            string ss = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")+" > "+logMessage;
            richTextLog.AppendText(ss + "\n");
            richTextLog.ScrollToCaret();
            richTextLog.Refresh();
            w.WriteLine(ss);
            w.Flush();
        }

        public void logChart(double x)
        {
            chartLog.Series["Series1"].Points.AddY(x);
        }

       
    }
}
