namespace EEG
{
    partial class EEGViewControl
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
            FoundationLibrary.EventList<EEG.ViewChannel> eventList_11 = new FoundationLibrary.EventList<EEG.ViewChannel>();
            this.pLeft = new System.Windows.Forms.Panel();
            this.viewChannelsGrid = new EEG.ViewChannelsGrid();
            this.gbScale = new System.Windows.Forms.GroupBox();
            this.lScaleY = new System.Windows.Forms.Label();
            this.nScaleY = new System.Windows.Forms.NumericUpDown();
            this.lScaleX = new System.Windows.Forms.Label();
            this.nScaleX = new System.Windows.Forms.NumericUpDown();
            this.panelDraw = new System.Windows.Forms.PictureBox();
            this.pLeft.SuspendLayout();
            this.gbScale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelDraw)).BeginInit();
            this.SuspendLayout();
            // 
            // pLeft
            // 
            this.pLeft.Controls.Add(this.viewChannelsGrid);
            this.pLeft.Controls.Add(this.gbScale);
            this.pLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLeft.Location = new System.Drawing.Point(0, 0);
            this.pLeft.Name = "pLeft";
            this.pLeft.Size = new System.Drawing.Size(164, 365);
            this.pLeft.TabIndex = 4;
            // 
            // viewChannelsGrid
            // 
            this.viewChannelsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewChannelsGrid.Location = new System.Drawing.Point(0, 64);
            this.viewChannelsGrid.Name = "viewChannelsGrid";
            this.viewChannelsGrid.Size = new System.Drawing.Size(164, 301);
            this.viewChannelsGrid.TabIndex = 3;
            this.viewChannelsGrid.ViewChannels = eventList_11;
            this.viewChannelsGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.viewChannelsGrid_CellValueChanged);
            this.viewChannelsGrid.ItemAdded += new System.EventHandler<FoundationLibrary.ListEventArgs<EEG.ViewChannel>>(this.ViewChannelsGrid_ItemAddedChanged);
            this.viewChannelsGrid.ItemRemoved += new System.EventHandler<FoundationLibrary.ListEventArgs<EEG.ViewChannel>>(this.ViewChannelsGrid_ItemRemovedChanged);
            this.viewChannelsGrid.Cleared += new System.EventHandler(this.ViewChannelsGrid_Cleared);
            this.viewChannelsGrid.ItemChanged += new System.EventHandler<FoundationLibrary.ListEventItemChangedArgs<EEG.ViewChannel>>(this.ViewChannelsGrid_ItemChanged);
            // 
            // gbScale
            // 
            this.gbScale.Controls.Add(this.lScaleY);
            this.gbScale.Controls.Add(this.nScaleY);
            this.gbScale.Controls.Add(this.lScaleX);
            this.gbScale.Controls.Add(this.nScaleX);
            this.gbScale.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbScale.Location = new System.Drawing.Point(0, 0);
            this.gbScale.Name = "gbScale";
            this.gbScale.Size = new System.Drawing.Size(164, 64);
            this.gbScale.TabIndex = 2;
            this.gbScale.TabStop = false;
            this.gbScale.Text = "Масштаб";
            // 
            // lScaleY
            // 
            this.lScaleY.AutoSize = true;
            this.lScaleY.Location = new System.Drawing.Point(83, 16);
            this.lScaleY.Name = "lScaleY";
            this.lScaleY.Size = new System.Drawing.Size(14, 13);
            this.lScaleY.TabIndex = 2;
            this.lScaleY.Text = "Y";
            // 
            // nScaleY
            // 
            this.nScaleY.DecimalPlaces = 5;
            this.nScaleY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.nScaleY.Location = new System.Drawing.Point(86, 32);
            this.nScaleY.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nScaleY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            327680});
            this.nScaleY.Name = "nScaleY";
            this.nScaleY.Size = new System.Drawing.Size(70, 20);
            this.nScaleY.TabIndex = 3;
            this.nScaleY.Value = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            this.nScaleY.ValueChanged += new System.EventHandler(this.nScaleY_ValueChanged);
            // 
            // lScaleX
            // 
            this.lScaleX.AutoSize = true;
            this.lScaleX.Location = new System.Drawing.Point(5, 16);
            this.lScaleX.Name = "lScaleX";
            this.lScaleX.Size = new System.Drawing.Size(14, 13);
            this.lScaleX.TabIndex = 0;
            this.lScaleX.Text = "X";
            // 
            // nScaleX
            // 
            this.nScaleX.DecimalPlaces = 2;
            this.nScaleX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nScaleX.Location = new System.Drawing.Point(8, 32);
            this.nScaleX.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nScaleX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nScaleX.Name = "nScaleX";
            this.nScaleX.Size = new System.Drawing.Size(70, 20);
            this.nScaleX.TabIndex = 1;
            this.nScaleX.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nScaleX.ValueChanged += new System.EventHandler(this.nScaleX_ValueChanged);
            // 
            // panelDraw
            // 
            this.panelDraw.BackColor = System.Drawing.Color.White;
            this.panelDraw.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDraw.Location = new System.Drawing.Point(164, 0);
            this.panelDraw.Name = "panelDraw";
            this.panelDraw.Size = new System.Drawing.Size(516, 365);
            this.panelDraw.TabIndex = 5;
            this.panelDraw.TabStop = false;
            this.panelDraw.SizeChanged += new System.EventHandler(this.panelDraw_SizeChanged);
            this.panelDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDraw_Paint);
            // 
            // EEGViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDraw);
            this.Controls.Add(this.pLeft);
            this.Name = "EEGViewControl";
            this.Size = new System.Drawing.Size(680, 365);
            this.VisibleChanged += new System.EventHandler(this.EEGViewControl_VisibleChanged);
            this.pLeft.ResumeLayout(false);
            this.gbScale.ResumeLayout(false);
            this.gbScale.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelDraw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
        public System.Windows.Forms.Panel pLeft;
        public System.Windows.Forms.GroupBox gbScale;
        public System.Windows.Forms.Label lScaleY;
        public System.Windows.Forms.NumericUpDown nScaleY;
        public System.Windows.Forms.Label lScaleX;
        public System.Windows.Forms.NumericUpDown nScaleX;
        public System.Windows.Forms.PictureBox panelDraw;
        public ViewChannelsGrid viewChannelsGrid;
#pragma warning restore CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    }
}
