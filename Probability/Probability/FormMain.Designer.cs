namespace Probability
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.richTextBoxMainLogger = new System.Windows.Forms.RichTextBox();
            this.buttonRun01 = new System.Windows.Forms.Button();
            this.chartLog = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartLog)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxMainLogger
            // 
            this.richTextBoxMainLogger.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxMainLogger.Name = "richTextBoxMainLogger";
            this.richTextBoxMainLogger.Size = new System.Drawing.Size(987, 509);
            this.richTextBoxMainLogger.TabIndex = 0;
            this.richTextBoxMainLogger.Text = "";
            // 
            // buttonRun01
            // 
            this.buttonRun01.Location = new System.Drawing.Point(1020, 12);
            this.buttonRun01.Name = "buttonRun01";
            this.buttonRun01.Size = new System.Drawing.Size(75, 23);
            this.buttonRun01.TabIndex = 1;
            this.buttonRun01.Text = "Run!";
            this.buttonRun01.UseVisualStyleBackColor = true;
            this.buttonRun01.Click += new System.EventHandler(this.buttonRun01_Click);
            // 
            // chartLog
            // 
            chartArea1.Name = "ChartArea1";
            this.chartLog.ChartAreas.Add(chartArea1);
            this.chartLog.Location = new System.Drawing.Point(1020, 221);
            this.chartLog.Name = "chartLog";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            this.chartLog.Series.Add(series1);
            this.chartLog.Size = new System.Drawing.Size(444, 300);
            this.chartLog.TabIndex = 2;
            this.chartLog.Text = "chart1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 533);
            this.Controls.Add(this.chartLog);
            this.Controls.Add(this.buttonRun01);
            this.Controls.Add(this.richTextBoxMainLogger);
            this.Name = "FormMain";
            this.Text = "Probabilities";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMainLogger;
        private System.Windows.Forms.Button buttonRun01;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLog;
    }
}

