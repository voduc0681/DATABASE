namespace DATABASE.View
{
    partial class Statistic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Statistic));
            this.label1 = new System.Windows.Forms.Label();
            this.dgvStatistic = new System.Windows.Forms.DataGridView();
            this.OrderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dtmStart = new System.Windows.Forms.DateTimePicker();
            this.dtmEnd = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistic)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(261, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(277, 80);
            this.label1.TabIndex = 15;
            this.label1.Text = "Statistic";
            // 
            // dgvStatistic
            // 
            this.dgvStatistic.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStatistic.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatistic.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderCode,
            this.OrderDate,
            this.TotalPrice});
            this.dgvStatistic.Location = new System.Drawing.Point(34, 241);
            this.dgvStatistic.Name = "dgvStatistic";
            this.dgvStatistic.RowHeadersWidth = 51;
            this.dgvStatistic.RowTemplate.Height = 24;
            this.dgvStatistic.Size = new System.Drawing.Size(736, 197);
            this.dgvStatistic.TabIndex = 16;
            // 
            // OrderCode
            // 
            this.OrderCode.DataPropertyName = "OrderCode";
            this.OrderCode.HeaderText = "Order Code";
            this.OrderCode.MinimumWidth = 6;
            this.OrderCode.Name = "OrderCode";
            // 
            // OrderDate
            // 
            this.OrderDate.DataPropertyName = "OrderDate";
            this.OrderDate.HeaderText = "Order Date";
            this.OrderDate.MinimumWidth = 6;
            this.OrderDate.Name = "OrderDate";
            // 
            // TotalPrice
            // 
            this.TotalPrice.DataPropertyName = "TotalPrice";
            this.TotalPrice.HeaderText = "Total Price";
            this.TotalPrice.MinimumWidth = 6;
            this.TotalPrice.Name = "TotalPrice";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(581, 206);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(189, 22);
            this.textBox1.TabIndex = 17;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // dtmStart
            // 
            this.dtmStart.Location = new System.Drawing.Point(523, 143);
            this.dtmStart.Name = "dtmStart";
            this.dtmStart.Size = new System.Drawing.Size(247, 22);
            this.dtmStart.TabIndex = 18;
            this.dtmStart.ValueChanged += new System.EventHandler(this.dtmStart_ValueChanged);
            // 
            // dtmEnd
            // 
            this.dtmEnd.Location = new System.Drawing.Point(523, 171);
            this.dtmEnd.Name = "dtmEnd";
            this.dtmEnd.Size = new System.Drawing.Size(247, 22);
            this.dtmEnd.TabIndex = 18;
            this.dtmEnd.ValueChanged += new System.EventHandler(this.dtmEnd_ValueChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(34, 200);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 34);
            this.btnRefresh.TabIndex = 19;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(115, 200);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 34);
            this.btnExit.TabIndex = 19;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(485, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 20;
            this.label2.Text = "Search Code";
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(420, 171);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(63, 16);
            this.lblEndDate.TabIndex = 21;
            this.lblEndDate.Text = "End Date";
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(420, 143);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(66, 16);
            this.lblStartDate.TabIndex = 22;
            this.lblStartDate.Text = "Start Date";
            // 
            // Statistic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dtmEnd);
            this.Controls.Add(this.dtmStart);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dgvStatistic);
            this.Controls.Add(this.label1);
            this.Name = "Statistic";
            this.Text = "Statistic";
            this.Load += new System.EventHandler(this.Statistic_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvStatistic;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DateTimePicker dtmStart;
        private System.Windows.Forms.DateTimePicker dtmEnd;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalPrice;
    }
}