namespace EEG.MitsarSmartBCI
{
    partial class MitsarSmartBCIChanelsState
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
            this.emittersdgv = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BPS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HHP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HLP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UPS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.emittersdgv)).BeginInit();
            this.SuspendLayout();
            // 
            // emittersdgv
            // 
            this.emittersdgv.AllowUserToAddRows = false;
            this.emittersdgv.AllowUserToDeleteRows = false;
            this.emittersdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.emittersdgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.UID,
            this.BPS,
            this.HHP,
            this.HLP,
            this.UPS});
            this.emittersdgv.Location = new System.Drawing.Point(14, 29);
            this.emittersdgv.Name = "emittersdgv";
            this.emittersdgv.ReadOnly = true;
            this.emittersdgv.RowHeadersVisible = false;
            this.emittersdgv.RowTemplate.Height = 24;
            this.emittersdgv.Size = new System.Drawing.Size(335, 337);
            this.emittersdgv.TabIndex = 0;
            // 
            // Index
            // 
            this.Index.HeaderText = "ID";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Width = 30;
            // 
            // UID
            // 
            this.UID.HeaderText = "UID";
            this.UID.Name = "UID";
            this.UID.ReadOnly = true;
            this.UID.Width = 40;
            // 
            // BPS
            // 
            this.BPS.HeaderText = "BPS";
            this.BPS.Name = "BPS";
            this.BPS.ReadOnly = true;
            this.BPS.Width = 40;
            // 
            // HHP
            // 
            this.HHP.HeaderText = "HHP";
            this.HHP.Name = "HHP";
            this.HHP.ReadOnly = true;
            this.HHP.Width = 40;
            // 
            // HLP
            // 
            this.HLP.HeaderText = "HLP";
            this.HLP.Name = "HLP";
            this.HLP.ReadOnly = true;
            this.HLP.Width = 40;
            // 
            // UPS
            // 
            this.UPS.HeaderText = "UPS";
            this.UPS.Name = "UPS";
            this.UPS.ReadOnly = true;
            this.UPS.Width = 40;
            // 
            // MitsarSmartBCIChanelsState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.emittersdgv);
            this.Name = "MitsarSmartBCIChanelsState";
            this.Size = new System.Drawing.Size(368, 391);
            ((System.ComponentModel.ISupportInitialize)(this.emittersdgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView emittersdgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn UID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BPS;
        private System.Windows.Forms.DataGridViewTextBoxColumn HHP;
        private System.Windows.Forms.DataGridViewTextBoxColumn HLP;
        private System.Windows.Forms.DataGridViewTextBoxColumn UPS;
    }
}
