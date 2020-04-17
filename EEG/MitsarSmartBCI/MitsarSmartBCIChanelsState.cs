using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEG.MitsarSmartBCI
{
    public partial class MitsarSmartBCIChanelsState : UserControl
    {
        public MitsarSmartBCIChanelsState()
        {
            InitializeComponent();
        }
        MitsarSmartBCI m_EEG;
        /// <summary>
        /// Устройство ЭЭГ
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public MitsarSmartBCI EEG
        {
            get
            {
                return m_EEG;
            }
            set
            {
                if (EEG == value)
                    return;

                if (value == null)
                    throw new ArgumentNullException();

                if ((EEG != null && EEG.IsRunning) || value.IsRunning)
                    throw new Exception("Нельзя задать генератор ЭЭГ когда текущий или новый работает");

                if (m_EEG != null)
                {
                    m_EEG.Started -= EEG_Started;
                    m_EEG.Stopped -= EEG_Stopped;
                }

                m_EEG = value;
                
                m_EEG.Started += EEG_Started;
                m_EEG.Stopped += EEG_Stopped;
            }
        }

        private void EEG_Started(object sender, EventArgs e)
        {
            Dispatcher.Invoke(this, () =>
            {
                string[,] emitters = m_EEG.GetEmittersDescription();
                for (int i = 0; i < emitters.GetLength(0); i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    for (int j = 0; j < emitters.GetLength(1); j++)
                    {
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = emitters[i, j] });
                    }
                    emittersdgv.Rows.Add(row);
                }
            });
            
        }
        private void EEG_Stopped(object sender, EventArgs e)
        {
            Dispatcher.Invoke(this, () =>
            {
                emittersdgv.Rows.Clear();
            });
        }
    }
}

