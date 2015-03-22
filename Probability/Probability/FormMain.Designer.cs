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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.richTextBoxMainLogger0 = new System.Windows.Forms.RichTextBox();
            this.buttonRun01 = new System.Windows.Forms.Button();
            this.chartLog0 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonRun2 = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.backgroundWorker0 = new System.ComponentModel.BackgroundWorker();
            this.labelResult0 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.chartLog1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.richTextBoxMainLogger1 = new System.Windows.Forms.RichTextBox();
            this.chartLog2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.richTextBoxMainLogger2 = new System.Windows.Forms.RichTextBox();
            this.chartLog3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.richTextBoxMainLogger3 = new System.Windows.Forms.RichTextBox();
            this.labelResult1 = new System.Windows.Forms.Label();
            this.labelResult2 = new System.Windows.Forms.Label();
            this.labelResult3 = new System.Windows.Forms.Label();
            this.labelMain = new System.Windows.Forms.Label();
            this.richTextBoxMain = new System.Windows.Forms.RichTextBox();
            this.chartMain = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartLog0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLog1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLog2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLog3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBoxMainLogger0
            // 
            this.richTextBoxMainLogger0.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.richTextBoxMainLogger0.Location = new System.Drawing.Point(12, 172);
            this.richTextBoxMainLogger0.Name = "richTextBoxMainLogger0";
            this.richTextBoxMainLogger0.Size = new System.Drawing.Size(682, 140);
            this.richTextBoxMainLogger0.TabIndex = 0;
            this.richTextBoxMainLogger0.Text = "";
            // 
            // buttonRun01
            // 
            this.buttonRun01.Location = new System.Drawing.Point(1130, 8);
            this.buttonRun01.Name = "buttonRun01";
            this.buttonRun01.Size = new System.Drawing.Size(75, 23);
            this.buttonRun01.TabIndex = 1;
            this.buttonRun01.Text = "Run!";
            this.buttonRun01.UseVisualStyleBackColor = true;
            this.buttonRun01.Click += new System.EventHandler(this.buttonRun01_Click);
            // 
            // chartLog0
            // 
            chartArea6.AxisX.LabelStyle.Format = "{0:0.00}";
            chartArea6.Name = "ChartArea1";
            this.chartLog0.ChartAreas.Add(chartArea6);
            this.chartLog0.Location = new System.Drawing.Point(700, 192);
            this.chartLog0.Name = "chartLog0";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Name = "Series1";
            this.chartLog0.Series.Add(series6);
            this.chartLog0.Size = new System.Drawing.Size(690, 120);
            this.chartLog0.TabIndex = 2;
            this.chartLog0.Text = "chart1";
            // 
            // buttonRun2
            // 
            this.buttonRun2.Location = new System.Drawing.Point(1315, 8);
            this.buttonRun2.Name = "buttonRun2";
            this.buttonRun2.Size = new System.Drawing.Size(75, 23);
            this.buttonRun2.TabIndex = 3;
            this.buttonRun2.Text = "Test";
            this.buttonRun2.UseVisualStyleBackColor = true;
            this.buttonRun2.Click += new System.EventHandler(this.buttonRun2_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(1211, 8);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Stop!";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.button1_Click);
            // 
            // backgroundWorker0
            // 
            this.backgroundWorker0.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // labelResult0
            // 
            this.labelResult0.AutoSize = true;
            this.labelResult0.Location = new System.Drawing.Point(700, 176);
            this.labelResult0.Name = "labelResult0";
            this.labelResult0.Size = new System.Drawing.Size(10, 13);
            this.labelResult0.TabIndex = 5;
            this.labelResult0.Text = "-";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted_1);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker3_RunWorkerCompleted);
            // 
            // chartLog1
            // 
            chartArea7.AxisX.LabelStyle.Format = "{0:0.00}";
            chartArea7.Name = "ChartArea1";
            this.chartLog1.ChartAreas.Add(chartArea7);
            this.chartLog1.Location = new System.Drawing.Point(700, 345);
            this.chartLog1.Name = "chartLog1";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Name = "Series1";
            this.chartLog1.Series.Add(series7);
            this.chartLog1.Size = new System.Drawing.Size(690, 120);
            this.chartLog1.TabIndex = 7;
            this.chartLog1.Text = "chart1";
            // 
            // richTextBoxMainLogger1
            // 
            this.richTextBoxMainLogger1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.richTextBoxMainLogger1.Location = new System.Drawing.Point(12, 325);
            this.richTextBoxMainLogger1.Name = "richTextBoxMainLogger1";
            this.richTextBoxMainLogger1.Size = new System.Drawing.Size(682, 140);
            this.richTextBoxMainLogger1.TabIndex = 6;
            this.richTextBoxMainLogger1.Text = "";
            // 
            // chartLog2
            // 
            chartArea8.AxisX.LabelStyle.Format = "{0:0.00}";
            chartArea8.Name = "ChartArea1";
            this.chartLog2.ChartAreas.Add(chartArea8);
            this.chartLog2.Location = new System.Drawing.Point(700, 498);
            this.chartLog2.Name = "chartLog2";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Name = "Series1";
            this.chartLog2.Series.Add(series8);
            this.chartLog2.Size = new System.Drawing.Size(690, 120);
            this.chartLog2.TabIndex = 9;
            this.chartLog2.Text = "chart1";
            // 
            // richTextBoxMainLogger2
            // 
            this.richTextBoxMainLogger2.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.richTextBoxMainLogger2.Location = new System.Drawing.Point(12, 478);
            this.richTextBoxMainLogger2.Name = "richTextBoxMainLogger2";
            this.richTextBoxMainLogger2.Size = new System.Drawing.Size(682, 140);
            this.richTextBoxMainLogger2.TabIndex = 8;
            this.richTextBoxMainLogger2.Text = "";
            // 
            // chartLog3
            // 
            chartArea9.AxisX.LabelStyle.Format = "{0:0.00}";
            chartArea9.Name = "ChartArea1";
            this.chartLog3.ChartAreas.Add(chartArea9);
            this.chartLog3.Location = new System.Drawing.Point(700, 655);
            this.chartLog3.Name = "chartLog3";
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Name = "Series1";
            this.chartLog3.Series.Add(series9);
            this.chartLog3.Size = new System.Drawing.Size(690, 120);
            this.chartLog3.TabIndex = 11;
            this.chartLog3.Text = "chart1";
            // 
            // richTextBoxMainLogger3
            // 
            this.richTextBoxMainLogger3.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.richTextBoxMainLogger3.Location = new System.Drawing.Point(12, 635);
            this.richTextBoxMainLogger3.Name = "richTextBoxMainLogger3";
            this.richTextBoxMainLogger3.Size = new System.Drawing.Size(682, 140);
            this.richTextBoxMainLogger3.TabIndex = 10;
            this.richTextBoxMainLogger3.Text = "";
            // 
            // labelResult1
            // 
            this.labelResult1.AutoSize = true;
            this.labelResult1.Location = new System.Drawing.Point(700, 329);
            this.labelResult1.Name = "labelResult1";
            this.labelResult1.Size = new System.Drawing.Size(10, 13);
            this.labelResult1.TabIndex = 12;
            this.labelResult1.Text = "-";
            // 
            // labelResult2
            // 
            this.labelResult2.AutoSize = true;
            this.labelResult2.Location = new System.Drawing.Point(700, 482);
            this.labelResult2.Name = "labelResult2";
            this.labelResult2.Size = new System.Drawing.Size(10, 13);
            this.labelResult2.TabIndex = 13;
            this.labelResult2.Text = "-";
            // 
            // labelResult3
            // 
            this.labelResult3.AutoSize = true;
            this.labelResult3.Location = new System.Drawing.Point(700, 639);
            this.labelResult3.Name = "labelResult3";
            this.labelResult3.Size = new System.Drawing.Size(10, 13);
            this.labelResult3.TabIndex = 14;
            this.labelResult3.Text = "-";
            // 
            // labelMain
            // 
            this.labelMain.AutoSize = true;
            this.labelMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.labelMain.ForeColor = System.Drawing.Color.Maroon;
            this.labelMain.Location = new System.Drawing.Point(700, 14);
            this.labelMain.Name = "labelMain";
            this.labelMain.Size = new System.Drawing.Size(15, 20);
            this.labelMain.TabIndex = 15;
            this.labelMain.Text = "-";
            // 
            // richTextBoxMain
            // 
            this.richTextBoxMain.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.richTextBoxMain.Location = new System.Drawing.Point(12, 17);
            this.richTextBoxMain.Name = "richTextBoxMain";
            this.richTextBoxMain.Size = new System.Drawing.Size(682, 140);
            this.richTextBoxMain.TabIndex = 16;
            this.richTextBoxMain.Text = "";
            // 
            // chartMain
            // 
            chartArea10.AxisX.LabelStyle.Format = "{0:0.00}";
            chartArea10.Name = "ChartArea1";
            this.chartMain.ChartAreas.Add(chartArea10);
            this.chartMain.Location = new System.Drawing.Point(700, 37);
            this.chartMain.Name = "chartMain";
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Name = "Series1";
            this.chartMain.Series.Add(series10);
            this.chartMain.Size = new System.Drawing.Size(690, 120);
            this.chartMain.TabIndex = 17;
            this.chartMain.Text = "chart1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1407, 781);
            this.Controls.Add(this.chartMain);
            this.Controls.Add(this.richTextBoxMain);
            this.Controls.Add(this.labelMain);
            this.Controls.Add(this.labelResult3);
            this.Controls.Add(this.labelResult2);
            this.Controls.Add(this.labelResult1);
            this.Controls.Add(this.chartLog3);
            this.Controls.Add(this.richTextBoxMainLogger3);
            this.Controls.Add(this.chartLog2);
            this.Controls.Add(this.richTextBoxMainLogger2);
            this.Controls.Add(this.chartLog1);
            this.Controls.Add(this.richTextBoxMainLogger1);
            this.Controls.Add(this.labelResult0);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonRun2);
            this.Controls.Add(this.chartLog0);
            this.Controls.Add(this.buttonRun01);
            this.Controls.Add(this.richTextBoxMainLogger0);
            this.Name = "FormMain";
            this.Text = "Probabilities";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartLog0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLog1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLog2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartLog3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMainLogger0;
        private System.Windows.Forms.Button buttonRun01;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLog0;
        private System.Windows.Forms.Button buttonRun2;
        private System.Windows.Forms.Button buttonStop;
        private System.ComponentModel.BackgroundWorker backgroundWorker0;
        private System.Windows.Forms.Label labelResult0;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLog1;
        private System.Windows.Forms.RichTextBox richTextBoxMainLogger1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLog2;
        private System.Windows.Forms.RichTextBox richTextBoxMainLogger2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartLog3;
        private System.Windows.Forms.RichTextBox richTextBoxMainLogger3;
        private System.Windows.Forms.Label labelResult1;
        private System.Windows.Forms.Label labelResult2;
        private System.Windows.Forms.Label labelResult3;
        private System.Windows.Forms.Label labelMain;
        private System.Windows.Forms.RichTextBox richTextBoxMain;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMain;
    }
}

