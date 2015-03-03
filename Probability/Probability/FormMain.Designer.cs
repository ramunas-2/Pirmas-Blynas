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
            this.buttonRun2 = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.labelResult = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartLog)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxMainLogger
            // 
            this.richTextBoxMainLogger.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.richTextBoxMainLogger.Location = new System.Drawing.Point(12, 12);
            this.richTextBoxMainLogger.Name = "richTextBoxMainLogger";
            this.richTextBoxMainLogger.Size = new System.Drawing.Size(682, 173);
            this.richTextBoxMainLogger.TabIndex = 0;
            this.richTextBoxMainLogger.Text = "";
            // 
            // buttonRun01
            // 
            this.buttonRun01.Location = new System.Drawing.Point(700, 12);
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
            this.chartLog.Location = new System.Drawing.Point(700, 41);
            this.chartLog.Name = "chartLog";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            this.chartLog.Series.Add(series1);
            this.chartLog.Size = new System.Drawing.Size(444, 144);
            this.chartLog.TabIndex = 2;
            this.chartLog.Text = "chart1";
            // 
            // buttonRun2
            // 
            this.buttonRun2.Location = new System.Drawing.Point(1069, 12);
            this.buttonRun2.Name = "buttonRun2";
            this.buttonRun2.Size = new System.Drawing.Size(75, 23);
            this.buttonRun2.TabIndex = 3;
            this.buttonRun2.Text = "Tree";
            this.buttonRun2.UseVisualStyleBackColor = true;
            this.buttonRun2.Click += new System.EventHandler(this.buttonRun2_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(781, 12);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Stop!";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.button1_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(864, 17);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(10, 13);
            this.labelResult.TabIndex = 5;
            this.labelResult.Text = "-";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 195);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonRun2);
            this.Controls.Add(this.chartLog);
            this.Controls.Add(this.buttonRun01);
            this.Controls.Add(this.richTextBoxMainLogger);
            this.Name = "FormMain";
            this.Text = "Probabilities";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMainLogger;
        private System.Windows.Forms.Button buttonRun01;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLog;
        private System.Windows.Forms.Button buttonRun2;
        private System.Windows.Forms.Button buttonStop;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label labelResult;
    }
}

