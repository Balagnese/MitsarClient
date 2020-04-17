namespace EEG
{
    partial class ViewChannelsGrid
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgDrawChannels = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgDrawChannels)).BeginInit();
            this.SuspendLayout();
            // 
            // dgDrawChannels
            // 
            this.dgDrawChannels.AllowUserToAddRows = false;
            this.dgDrawChannels.AllowUserToDeleteRows = false;
            this.dgDrawChannels.AllowUserToResizeColumns = false;
            this.dgDrawChannels.AllowUserToResizeRows = false;
            this.dgDrawChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDrawChannels.ColumnHeadersVisible = false;
            this.dgDrawChannels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column6});
            this.dgDrawChannels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDrawChannels.Location = new System.Drawing.Point(0, 0);
            this.dgDrawChannels.Name = "dgDrawChannels";
            this.dgDrawChannels.RowHeadersVisible = false;
            this.dgDrawChannels.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgDrawChannels.Size = new System.Drawing.Size(185, 303);
            this.dgDrawChannels.TabIndex = 4;
            this.dgDrawChannels.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDrawChannels_CellDoubleClick);
            this.dgDrawChannels.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgDrawChannels_CurrentCellDirtyStateChanged);
            this.dgDrawChannels.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgDrawChannels_RowsAdded);
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "Visible";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.NullValue = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column5.FalseValue = "false";
            this.Column5.HeaderText = "";
            this.Column5.Name = "Column5";
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.TrueValue = "true";
            this.Column5.Width = 20;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "Name";
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column6.HeaderText = "";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 120;
            // 
            // ViewChannelsGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgDrawChannels);
            this.Name = "ViewChannelsGrid";
            this.Size = new System.Drawing.Size(185, 303);
            this.SizeChanged += new System.EventHandler(this.ViewChannelsGrid_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgDrawChannels)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgDrawChannels;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}
