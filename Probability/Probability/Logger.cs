using System;
using System.Collections.Generic;
using System.Drawing;
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
        Dictionary<string, LoggerType> loggerTypesDictionary;



        StreamWriter w;
        public Logger(System.Windows.Forms.RichTextBox richTextLog, System.Windows.Forms.DataVisualization.Charting.Chart chartLog, string logFile, int debugLevel)
        {
            this.richTextLog = richTextLog;
            this.chartLog = chartLog;
            this.logFile = logFile;
            this.debugLevel = debugLevel;
            w = File.AppendText(logFile);
            loggerTypesDictionary = new Dictionary<string, LoggerType>();
        }

        public void log(string logMessage)
        {
            log(logMessage, 5, "");
        }

        LoggerType getAppendLoggerType(string name)
        {
            LoggerType loggerType;
            if (!loggerTypesDictionary.TryGetValue(name, out loggerType))
            {//LoggerType not found, creating new one with default values
                loggerType = new LoggerType();
                loggerType.color = Color.Black;
                loggerType.debugLevel = 5;
                loggerTypesDictionary.Add(name, loggerType);
            }
            return loggerType;
        }

        public void set(string name, int debugLevel, Color color)
        {
            LoggerType loggerType = getAppendLoggerType(name);
            loggerType.debugLevel = debugLevel;
            loggerType.color = color;
        }

        public void log(string logMessage, int debugLevel, string name = "")
        {
            LoggerType loggerType=getAppendLoggerType(name);
            if (debugLevel <= loggerType.debugLevel)
            {
                string ss = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff") + " "+name+"> " + logMessage;
                write(ss + "\n", loggerType.color);
            }
        }

        void write(string ss, Color color)
        {
            richTextLog.SelectionStart = richTextLog.TextLength;
            richTextLog.SelectionLength = 0;
            richTextLog.SelectionColor = color;
            richTextLog.AppendText(ss);
            richTextLog.SelectionColor = richTextLog.ForeColor;
            richTextLog.Refresh();
            w.Write(ss);
            richTextLog.ScrollToCaret();
            w.Flush();
        }

        public void logChart(double x)
        {
            chartLog.Series["Series1"].Points.AddY(x);
            chartLog.Update();
        }


    }

    class LoggerType
    {
        public int debugLevel;
        public Color color;
    }
}
