using FoundationLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace EEG
{
    /// <summary>
    /// Элемент управления для настройки цветами и видимостью каналов
    /// </summary>
    [Category("ЭЭГ"), Description("Элемент управления для настройки цветами и видимостью каналов")]
    public partial class ViewChannelsGrid : UserControl, IEventList<ViewChannel>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewChannelsGrid()
        {
            InitializeComponent();

            dgDrawChannels.AutoGenerateColumns = false;
            dgDrawChannels.Columns[0].ValueType = typeof(bool);

            m_BindingSource = new BindingSource();
            ViewChannels = new EventList<ViewChannel>();
            dgDrawChannels.DataSource = m_BindingSource;
        }

        private void ViewChannel_ValueChanged(object sender, EventArgs e)
        {
            int index = ViewChannels.IndexOf(sender as ViewChannel);
            (dgDrawChannels.DataSource as BindingSource).ResetItem(index);

            CellValueChanged?.Invoke(this, new DataGridViewCellEventArgs(0, index));
        }

        private void ViewChannel_ColorChanged(object sender, EventArgs e)
        {
            ViewChannel item = sender as ViewChannel;
            int index = ViewChannels.IndexOf(item);
            dgDrawChannels[1, index].Style.BackColor = item.Color;
            dgDrawChannels[1, index].Style.SelectionBackColor = item.Color;
            (dgDrawChannels.DataSource as BindingSource).ResetItem(index);
        }

        private BindingSource m_BindingSource;
        private IEventList<ViewChannel> m_ViewChannels;

        /// <summary>
        /// Управление видимостью и цветом каналов
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public IEventList<ViewChannel> ViewChannels
        {
            get
            {
                return m_ViewChannels;
            }
            set
            {
                if (m_ViewChannels == value)
                    return;

                if (value == null)
                    throw new ArgumentNullException();

                if (m_ViewChannels != null)
                {
                    m_ViewChannels.BeforeClear -= ViewChannels_BeforeClear;
                    m_ViewChannels.Cleared -= ViewChannels_Cleared;
                    m_ViewChannels.ItemAdded -= ViewChannels_ItemAdded;
                    m_ViewChannels.ItemRemoved -= ViewChannels_ItemRemoved;

                    foreach (ViewChannel item in m_ViewChannels)
                    {
                        item.ColorChanged -= ViewChannel_ColorChanged;
                        item.NameChanged -= ViewChannel_ValueChanged;
                        item.VisibleChanged -= ViewChannel_ValueChanged;
                    }
                }

                m_ViewChannels = value;
                m_BindingSource.DataSource = new BindingList<ViewChannel>(value);
                m_ViewChannels.BeforeClear += ViewChannels_BeforeClear;
                m_ViewChannels.Cleared += ViewChannels_Cleared;
                m_ViewChannels.ItemAdded += ViewChannels_ItemAdded;
                m_ViewChannels.ItemRemoved += ViewChannels_ItemRemoved;
                m_ViewChannels.ItemChanged += ViewChannels_ItemChanged;

                foreach (ViewChannel item in m_ViewChannels)
                {
                    item.ColorChanged += ViewChannel_ColorChanged;
                    item.NameChanged += ViewChannel_ValueChanged;
                    item.VisibleChanged += ViewChannel_ValueChanged;
                }

                ViewChannelsChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, возникающее когда ссылка на список ViewChannels изменилась
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, возникающее когда ссылка на список ViewChannels изменилась")]
        public event EventHandler ViewChannelsChanged;

        private void ViewChannels_ItemChanged(object sender, ListEventItemChangedArgs<ViewChannel> e)
        {
            e.OldItem.ColorChanged -= ViewChannel_ColorChanged;
            e.OldItem.NameChanged -= ViewChannel_ValueChanged;
            e.OldItem.VisibleChanged -= ViewChannel_ValueChanged;

            e.NewItem.ColorChanged += ViewChannel_ColorChanged;
            e.NewItem.NameChanged += ViewChannel_ValueChanged;
            e.NewItem.VisibleChanged += ViewChannel_ValueChanged;

            ViewChannel_ColorChanged(e.NewItem, new EventArgs());
            m_BindingSource.ResetItem(e.Index);

            ItemChanged?.Invoke(this, e);
        }

        private void ViewChannels_ItemRemoved(object sender, ListEventArgs<ViewChannel> e)
        {
            e.Item.ColorChanged -= ViewChannel_ColorChanged;
            e.Item.NameChanged -= ViewChannel_ValueChanged;
            e.Item.VisibleChanged -= ViewChannel_ValueChanged;

            m_BindingSource.ResetBindings(false);

            ItemRemoved?.Invoke(this, e);
        }

        private void ViewChannels_ItemAdded(object sender, ListEventArgs<ViewChannel> e)
        {
            e.Item.ColorChanged += ViewChannel_ColorChanged;
            e.Item.NameChanged += ViewChannel_ValueChanged;
            e.Item.VisibleChanged += ViewChannel_ValueChanged;

            m_BindingSource.ResetBindings(false);

            ItemAdded?.Invoke(this, e);
        }

        private void ViewChannels_Cleared(object sender, EventArgs e)
        {
            m_BindingSource.ResetBindings(false);

            Cleared?.Invoke(this, e);
        }

        private void ViewChannels_BeforeClear(object sender, EventArgs e)
        {
            foreach (ViewChannel item in m_ViewChannels)
            {
                item.ColorChanged -= ViewChannel_ColorChanged;
                item.NameChanged -= ViewChannel_ValueChanged;
                item.VisibleChanged -= ViewChannel_ValueChanged;
            }

            BeforeClear?.Invoke(this, e);
        }

        /// <summary>
        /// Событие, возникающее когда значение в таблице было изменено
        /// </summary>
        [Category("Свойство изменено"), Description("Событие, возникающее когда значение в таблице было изменено")]
        public event DataGridViewCellEventHandler CellValueChanged;

        #region IList<ViewChannel>

        int ICollection<ViewChannel>.Count => m_ViewChannels.Count;

        bool ICollection<ViewChannel>.IsReadOnly => m_ViewChannels.IsReadOnly;

        ViewChannel IList<ViewChannel>.this[int index]
        {
            get
            {
                return m_ViewChannels[index];
            }

            set
            {
                m_ViewChannels[index] = value;
            }
        }

        int IList<ViewChannel>.IndexOf(ViewChannel item) => m_ViewChannels.IndexOf(item);

        void IList<ViewChannel>.Insert(int index, ViewChannel item) => m_ViewChannels.Insert(index, item);

        void IList<ViewChannel>.RemoveAt(int index) => m_ViewChannels.RemoveAt(index);

        void ICollection<ViewChannel>.Add(ViewChannel item) => m_ViewChannels.Add(item);

        void ICollection<ViewChannel>.Clear() => m_ViewChannels.Clear();

        bool ICollection<ViewChannel>.Contains(ViewChannel item) => m_ViewChannels.Contains(item);

        void ICollection<ViewChannel>.CopyTo(ViewChannel[] array, int arrayIndex) => m_ViewChannels.CopyTo(array, arrayIndex);

        bool ICollection<ViewChannel>.Remove(ViewChannel item) => m_ViewChannels.Remove(item);

        IEnumerator<ViewChannel> IEnumerable<ViewChannel>.GetEnumerator() => m_ViewChannels.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => m_ViewChannels.GetEnumerator();

        #endregion

        private void dgDrawChannels_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dgDrawChannels.EndEdit();
        }

        private void dgDrawChannels_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                colorDialog.Color = dgDrawChannels[1, e.RowIndex].Style.BackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    ViewChannels[e.RowIndex].Color = colorDialog.Color;
                    CellValueChanged?.Invoke(this, new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
                }
            }
        }

        private void ViewChannelsGrid_SizeChanged(object sender, EventArgs e)
        {
            if (Width > 0 && Height > 0)
                dgDrawChannels.Columns[1].Width = Width - dgDrawChannels.Columns[0].Width;
        }

        /// <summary>
        /// Сохраняются настройки компонента
        /// </summary>
        /// <param name="node">Узел с настройками</param>
        public void SaveState(XmlElement node)
        {
            node.RemoveAll();
            for (int i = 0; i < ViewChannels.Count; i++)
            {
                XmlElement viewChannelNode = node.AppendChild(node.OwnerDocument.CreateElement("Channel")) as XmlElement;
                viewChannelNode.SetAttribute("Index", i.ToString());
                viewChannelNode.SetAttribute("Name", ViewChannels[i].Name);
                viewChannelNode.SetAttribute("Visible", ViewChannels[i].Visible.ToString());
                viewChannelNode.SetAttribute("Color", ViewChannels[i].Color.ToArgb().ToString());
            }
        }

        /// <summary>
        /// Загружаются настройки компонента
        /// </summary>
        /// <param name="node">Узел с настройками</param>
        public void LoadState(XmlElement node)
        {
            XmlNodeList nodes = node.GetElementsByTagName("Channel");
            ViewChannels.Clear();
            foreach (XmlElement value in nodes)
            {
                int index = int.Parse(value.GetAttribute("Index").Trim());
                ViewChannels.Insert(index, new ViewChannel(value.GetAttribute("Name"), Color.FromArgb(int.Parse(value.GetAttribute("Color").Trim())), bool.Parse(value.GetAttribute("Visible").Trim())));
            }
        }

        /// <summary>
        /// Событие, возникающее когда значение добавлено
        /// </summary>
        [Category("Действие"), Description("Событие, возникающее когда значение добавлено")]
        public event EventHandler<ListEventArgs<ViewChannel>> ItemAdded;

        /// <summary>
        /// Событие, возникающее когда значение удалено
        /// </summary>
        [Category("Действие"), Description("Событие, возникающее когда значение удалено")]
        public event EventHandler<ListEventArgs<ViewChannel>> ItemRemoved;

        /// <summary>
        /// Событие, возникающее перед очисткой списка
        /// </summary>
        [Category("Действие"), Description("Событие, возникающее перед очисткой списка")]
        public event EventHandler BeforeClear;

        /// <summary>
        /// Событие, возникающее когда список очищен
        /// </summary>
        [Category("Действие"), Description("Событие, возникающее когда список очищен")]
        public event EventHandler Cleared;

        /// <summary>
        /// Событие, возникающее когда значение элемента в списке было изменено
        /// </summary>
        [Category("Действие"), Description("Событие, возникающее когда значение элемента в списке было изменено")]
        public event EventHandler<ListEventItemChangedArgs<ViewChannel>> ItemChanged;

        private void dgDrawChannels_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < e.RowCount && i + e.RowIndex < ViewChannels.Count; i++)
                ViewChannel_ColorChanged(ViewChannels[i + e.RowIndex], new EventArgs());
        }
    }
}
