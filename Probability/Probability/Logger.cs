﻿using System;
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
        System.Windows.Forms.Label labelResult;
        string logFile;

        delegate void SetTextCallback(string ss, Color color);
        delegate void SetChartCallback(double x);
        delegate void SetLabelCallback(string s);


        int debugLevel;
        Dictionary<string, LoggerType> loggerTypesDictionary;



        StreamWriter w;
        public Logger(System.Windows.Forms.RichTextBox richTextLog, System.Windows.Forms.DataVisualization.Charting.Chart chartLog, System.Windows.Forms.Label labelResult, string logFile, int debugLevel)
        {
            this.richTextLog = richTextLog;
            this.chartLog = chartLog;
            this.labelResult = labelResult;
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
            LoggerType loggerType = getAppendLoggerType(name);
            if (debugLevel <= loggerType.debugLevel)
            {
                string ss = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff") + " " + name + "> " + logMessage;
                //string ss = logMessage;
                write(ss + "\n", loggerType.color);
            }
        }

        //delegate void write(string ss, Color color);

        void write(string ss, Color color)
        {


            if (richTextLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(write);
                richTextLog.Invoke(d, new object[] { ss, color });
            }
            else
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
        }

        public void logChart(double x)
        {
            if (chartLog.InvokeRequired)
            {
                SetChartCallback d = new SetChartCallback(logChart);
                richTextLog.Invoke(d, new object[] { x });
            }
            else
            {
                chartLog.Series["Series1"].Points.AddY(x);
                chartLog.Update();
            }
        }

        public void logLabel(string s)
        {
            if (labelResult.InvokeRequired)
            {
                SetLabelCallback d = new SetLabelCallback(logLabel);
                labelResult.Invoke(d, new object[] { s });
            }
            else
            {
                labelResult.Text = s;
                labelResult.Update();
            }
        }

        public void logChartReset()
        {
            chartLog.Series["Series1"].Points.Clear();
            chartLog.Update();
        }


    }

    class LoggerType
    {
        public int debugLevel;
        public Color color;
    }
}
