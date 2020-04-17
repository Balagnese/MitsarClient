using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FoundationLibrary;
using System.Xml;

namespace EEG
{
    /// <summary>
    /// Элемент управления для визуализации ЭЭГ
    /// </summary>
    [Category("ЭЭГ"), Description("Элемент управления для визуализации ЭЭГ")]
    public partial class EEGViewControl : UserControl
    {
        delegate void Action();

        /// <summary>
        /// смещение по X при отрисовке
        /// </summary>
        private int m_ShiftX = 130;

        /// <summary>
        /// смещение по X при отрисовке
        /// </summary>
        [Category("Макет"), Description("Cмещение по X при отрисовке")]
        public int ShiftX
        {
            get
            {
                return m_ShiftX;
            }
            set
            {
                if (ShiftX == value)
                    return;

                m_ShiftX = value;
                SetScaleX();

                ShiftXChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, вызываемое, когда свойство ShiftX изменилось
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, вызываемое, когда свойство ShiftX изменилось")]
        public event EventHandler ShiftXChanged;

        /// <summary>
        /// Устройство ЭЭГ
        /// </summary>
        private IEEG m_EEG;

        /// <summary>
        /// Устройство ЭЭГ
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public IEEG EEG
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
                    throw new Exception("Нельзя присвоить устройство элементу управления, когда старое или новое устройство работает");

                if (EEG != null)
                {
                    EEG.Started -= EEG_Started;
                    EEG.Stopped -= EEG_Stopped;
                    EEG.ReseiveData -= EEG_ReseiveData;
                }

                m_EEG = value;

                m_EEG.Started += EEG_Started;
                m_EEG.Stopped += EEG_Stopped;
                m_EEG.ReseiveData += EEG_ReseiveData;

                EEGChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, вызываемое, когда свойство EEG изменилось
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, вызываемое, когда свойство EEG изменилось")]
        public event EventHandler EEGChanged;

        /// <summary>
        /// На всякий случай, вдруг кто ссылку обнулит
        /// </summary>
        private ViewChannelsGrid m_ViewChannelGrid;

        /// <summary>
        /// Управление видимостью и цветом каналов
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public IEventList<ViewChannel> ViewChannels
        {
            get
            {
                if (viewChannelsGrid != null)
                {
                    return viewChannelsGrid.ViewChannels;
                }
                else
                    return m_ViewChannelGrid;
            }
            set
            {
                if (viewChannelsGrid != null)
                {
                    viewChannelsGrid.ViewChannels = value;
                }
                else
                    m_ViewChannelGrid.ViewChannels = value;

                ViewChannelsChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, вызываемое, когда ссылка на список ViewChannels изменилась
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, вызываемое, когда ссылка на список ViewChannels изменилась")]
        public event EventHandler ViewChannelsChanged;

        /// <summary>
        /// Событие, вызываемое, когда один из элементов ViewChannels изменился
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, вызываемое, когда один из элементов ViewChannels изменился")]
        public event EventHandler<ChannelEventArgs> ViewChannelChanged;

        /// <summary>
        /// Масштаб по горизонтали
        /// </summary>
        [Category("Макет"), Description("Масштаб по горизонтали")]
        public float ScaleX
        {
            get
            {
                return nScaleX != null ? (float)nScaleX.Value : m_ScaleX;
            }
            set
            {
                if (ScaleX == value)
                    return;

                if (nScaleX != null)
                {
                    nScaleX.Value = new decimal(value);
                }
                else
                {
                    m_ScaleX = value;
                    ScaleXChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Событие, вызываемое, когда свойство ScaleX изменилось
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, вызываемое, когда свойство ScaleX изменилось")]
        public event EventHandler ScaleXChanged;

        /// <summary>
        /// Масштаб по вертикали
        /// </summary>
        [Category("Макет"), Description("Масштаб по вертикали")]
        public float ScaleY
        {
            get
            {
                return nScaleY != null ? (float)nScaleY.Value : m_ScaleY;
            }
            set
            {
                if (ScaleY == value)
                    return;

                if (nScaleY != null)
                {
                    nScaleY.Value = new decimal(value);
                }
                else
                {
                    m_ScaleY = value;
                    ScaleYChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Событие, вызываемое, когда свойство ScaleY изменилось
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, вызываемое, когда свойство ScaleY изменилось")]
        public event EventHandler ScaleYChanged;

        /// <summary>
        /// Время задержки между отправкой команды на перерисовку (в мс)
        /// </summary>
        private int m_DrawTime = 100;

        /// <summary>
        /// Время задержки между отправкой команды на перерисовку (в мс)
        /// </summary>
        [Category("Макет"), Description("Время задержки между отправкой команды на перерисовку (в мс)")]
        public int DrawTime
        {
            get
            {
                return m_DrawTime;
            }
            set
            {
                if (DrawTime == value)
                    return;

                if (value < 0)
                    throw new ArgumentException();

                m_DrawTime = value;

                DrawTimeChanged.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, вызываемое, когда свойство DrawTime изменилось
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, вызываемое, когда свойство DrawTime изменилось")]
        public event EventHandler DrawTimeChanged;

        /// <summary>
        /// Задает видимость левой панели настроек 
        /// </summary>
        [Category("Макет"), Description("Задает видимость левой панели настроек")]
        public bool VisibleTools
        {
            get
            {
                return pLeft != null && pLeft.Visible;
            }
            set
            {
                if (pLeft == null || VisibleTools == value)
                    return;

                if (pLeft != null)
                    pLeft.Visible = value;

                VisibleToolsChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, вызываемое, когда свойство VisibleTools изменилось
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, вызываемое, когда свойство VisibleTools изменилось")]
        public event EventHandler VisibleToolsChanged;

        private FixedList<int> GetDrawList()
        {
            return new FixedList<int>((int)Math.Ceiling((panelDraw.ClientRectangle.Width - ShiftX - 10) / m_ScaleX));
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public EEGViewControl()
        {
            InitializeComponent();

            m_ViewChannelGrid = viewChannelsGrid;

            m_ScaleX = (float)nScaleX.Value;
            m_ScaleY = (float)nScaleY.Value;
            m_Datas = Queue.Synchronized(new Queue());

            m_DrawBuf = new List<FixedList<int>>(ViewChannels.Count);
            for (int i = 0; i < ViewChannels.Count; i++)
                m_DrawBuf.Add(GetDrawList());
            SetScaleX();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="eeg">Устройство ЭЭГ</param>
        public EEGViewControl(IEEG eeg)
        {
            EEG = eeg;

            InitializeComponent();

            m_ViewChannelGrid = viewChannelsGrid;

            m_ScaleX = (float)nScaleX.Value;
            m_ScaleY = (float)nScaleY.Value;
            m_Datas = Queue.Synchronized(new Queue());

            m_DrawBuf = new List<FixedList<int>>(ViewChannels.Count);
            for (int i = 0; i < ViewChannels.Count; i++)
                m_DrawBuf.Add(GetDrawList());
            SetScaleX();
        }

        /// <summary>
        /// Событие, вызываемое когда ЭЭГ запущен
        /// </summary>
        [Category("Поведение"), Description("Событие, вызываемое когда ЭЭГ запущен")]
        public event EventHandler Started;

        /// <summary>
        /// Событие, вызываемое когда ЭЭГ остановлен
        /// </summary>
        [Category("Поведение"), Description("Событие, вызываемое когда ЭЭГ остановлен")]
        public event EventHandler Stopped;

        /// <summary>
        /// Полученные данные
        /// </summary>
        private Queue m_Datas;

        /// <summary>
        /// Число необработанных пакетов в очереди
        /// </summary>
        [Browsable(false)]
        public int QueueCount
        {
            get
            {
                return m_Datas.Count;
            }
        }

        private void EEG_ReseiveData(object sender, EEGReseiveDataEventArgs e)
        {
            if (Visible)
            {
                m_Datas.Enqueue(e.Data.Clone());
            }
        }

        private void EEG_Stopped(object sender, EventArgs e)
        {
            Action stop = new Action(delegate
            {
                m_Datas.Clear();
                for (int i = 0; i < m_DrawBuf.Count; i++)
                    lock (m_DrawBuf[i])
                    {
                        m_DrawBuf[i].Clear();
                    }

                if (panelDraw != null)
                    panelDraw.Refresh();

                Stopped?.Invoke(this, e);
            });

            if (InvokeRequired)
                Invoke(stop);
            else
                stop();
        }

        private void EEG_Started(object sender, EventArgs e)
        {
            StartProcessing();

            Started?.Invoke(this, e);
        }

        /// <summary>
        /// данные для отображения на графике
        /// </summary>
        private List<FixedList<int>> m_DrawBuf;

        /// <summary>
        /// Параметр децимации
        /// </summary>
        private int m_Decimal = 1;

        /// <summary>
        /// Масштаб по X
        /// </summary>
        private float m_ScaleX;

        private void SetScaleX()
        {
            if (panelDraw == null)
                return;

            int n = (int)Math.Ceiling((panelDraw.ClientRectangle.Width - ShiftX - 10) / Math.Max(m_ScaleX, 1));
            for (int i = 0; i < m_DrawBuf.Count; i++)
                lock (m_DrawBuf[i])
                {
                    m_DrawBuf[i].Size = n;
                }
            m_Decimal = (int)Math.Max(1 / m_ScaleX, 1);
        }

        private void nScaleX_ValueChanged(object sender, EventArgs e)
        {
            m_ScaleX = (float)nScaleX.Value;
            SetScaleX();

            ScaleXChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Масштаб по Y
        /// </summary>
        private float m_ScaleY;

        private void nScaleY_ValueChanged(object sender, EventArgs e)
        {
            m_ScaleY = (float)nScaleY.Value;

            ScaleYChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Обработка полученных данных
        /// </summary>
        private void StartProcessing()
        {
            Console.WriteLine("__________________________________EEGVIEW StartProcessing");
            //Запуск обработки полученных данных
            Thread thr = new Thread(new ThreadStart(delegate
            {
                Console.WriteLine("__________________________________EEGVIEW StartProcessing");
                int curr = 0;
                while (EEG != null && EEG.IsRunning && Visible)
                {
                    if (m_Datas.Count > 0)
                    {
                        int[,] data = (int[,])m_Datas.Dequeue();
                        for (int i = curr; i < data.GetLength(1); i += m_Decimal)
                        {
                            for (int j = 0; j < data.GetLength(0) && j < m_DrawBuf.Count; j++)
                                lock (m_DrawBuf[j])
                                {
                                    m_DrawBuf[j].AddLast(data[j, i]);
                                }
                        }
                        curr = (m_Decimal - (data.GetLength(0) - curr) % m_Decimal) % m_Decimal;
                    }
                    else
                        Thread.Sleep(10);
                }
                //Очистка очереди
                m_Datas.Clear();
            }));
            thr.IsBackground = true;
            thr.Start();

            //Запуск отрисовки
            thr = new Thread(new ThreadStart(delegate
            {
                while (EEG != null && EEG.IsRunning && Visible)
                {
                    try
                    {
                        if (panelDraw != null)
                            if (InvokeRequired)
                                Invoke(new Action(panelDraw.Refresh));
                            else panelDraw.Refresh();
                    }
                    catch
                    {

                    }

                    Thread.Sleep(m_DrawTime);
                }

                //Очистка данных о сигналах
                for (int i = 0; i < m_DrawBuf.Count; i++)
                    lock (m_DrawBuf[i])
                    {
                        m_DrawBuf[i].Clear();
                    }
            }));
            thr.IsBackground = true;
            thr.Start();
        }

        private void panelDraw_Paint(object sender, PaintEventArgs e)
        {
            if (panelDraw == null)
                return;

            ThreadPriority pr = Thread.CurrentThread.Priority;
            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                int count = 0;
                for (int i = 0; i < ViewChannels.Count; i++)
                    if (ViewChannels[i].Visible)
                        count++;
                if (count > 0)
                {
                    float height = (float)(panelDraw.ClientRectangle.Height - 20) / count;
                    float y = e.ClipRectangle.Top + 10;
                    float x = e.ClipRectangle.Left + ShiftX;
                    float scaleX = m_ScaleX * m_Decimal;
                    float scaleY = m_ScaleY;
                    for (int i = 0; i < ViewChannels.Count; i++)
                        if (ViewChannels[i].Visible)
                        {
                            string str = ViewChannels[i].Name;
                            SizeF F = e.Graphics.MeasureString(str, panelDraw.Font);
                            Color color = ViewChannels[i].Color;
                            e.Graphics.DrawLine(Pens.Black, x, y + height / 2, panelDraw.ClientRectangle.Right - 10, y + height / 2);
                            e.Graphics.DrawString(str, panelDraw.Font, new SolidBrush(color), x - 5 - F.Width, y + (height - F.Height) / 2);
                            Pen p = new Pen(color);
                            lock (m_DrawBuf[i])
                            {
                                for (int j = 1; j < m_DrawBuf[i].Count; j++)
                                    e.Graphics.DrawLine(p, x + (j - 1) * scaleX, y + height / 2 - m_DrawBuf[i][j - 1] * scaleY, ShiftX + j * scaleX, y + height / 2 - m_DrawBuf[i][j] * scaleY);
                            }
                            y += height;
                        }
                }
            }
            finally
            {
                Thread.CurrentThread.Priority = pr;
            }
        }

        private void panelDraw_SizeChanged(object sender, EventArgs e)
        {
            SetScaleX();
        }

        private void viewChannelsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (panelDraw != null)
                panelDraw.Refresh();

            ViewChannelChanged?.Invoke(this, new ChannelEventArgs(e.RowIndex));
        }

        private void ViewChannelsGrid_ItemAddedChanged(object sender, ListEventArgs<ViewChannel> e)
        {
            if (e.Index < m_DrawBuf.Count)
                m_DrawBuf.Insert(e.Index, GetDrawList());
            else
                m_DrawBuf.Add(GetDrawList());

            if (panelDraw != null)
                panelDraw.Refresh();
        }

        private void ViewChannelsGrid_ItemRemovedChanged(object sender, ListEventArgs<ViewChannel> e)
        {
            m_DrawBuf.RemoveAt(e.Index);

            if (panelDraw != null)
                panelDraw.Refresh();
        }


        private void ViewChannelsGrid_ItemChanged(object sender, ListEventItemChangedArgs<ViewChannel> e)
        {
            if (panelDraw != null)
                panelDraw.Refresh();
        }

        private void ViewChannelsGrid_Cleared(object sender, EventArgs e)
        {
            m_DrawBuf.Clear();

            if (panelDraw != null)
                panelDraw.Refresh();
        }

        private void EEGViewControl_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
                StartProcessing();
        }

        /// <summary>
        /// Сохраняются настройки компонента
        /// </summary>
        /// <param name="node">Узел с настройками</param>
        public void SaveState(XmlElement node)
        {
            node.RemoveAll();
            node.SetAttribute("ScaleX", ScaleX.ToString());
            node.SetAttribute("ScaleY", ScaleY.ToString());
            if (viewChannelsGrid != null)
                viewChannelsGrid.SaveState(node.AppendChild(node.OwnerDocument.CreateElement("Channels")) as XmlElement);
        }

        /// <summary>
        /// Загружаются настройки компонента
        /// </summary>
        /// <param name="node">Узел с настройками</param>
        public void LoadState(XmlElement node)
        {
            ScaleX = float.Parse(node.GetAttribute("ScaleX").Trim());
            ScaleY = float.Parse(node.GetAttribute("ScaleY").Trim());
            if (viewChannelsGrid != null)
                viewChannelsGrid.LoadState(node["Channels"]);
        }
    }
}
