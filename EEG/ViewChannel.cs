using System;
using System.Drawing;

namespace EEG
{
    /// <summary>
    /// Определяет видимость и цвет канала
    /// </summary>
    public class ViewChannel
    {
        private bool m_Visible = true;

        /// <summary>
        /// true - канал рисуется
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                if (m_Visible == value)
                    return;

                m_Visible = value;

                VisibleChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, вызываемое когда свойство Visible изменилось 
        /// </summary>
        public event EventHandler VisibleChanged;

        private Color m_Color = Color.Black;

        /// <summary>
        /// Цвет отображаемого канала
        /// </summary>
        public Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                if (m_Color == value)
                    return;

                m_Color = value;

                ColorChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, вызываемое когда свойство Color изменилось
        /// </summary>
        public event EventHandler ColorChanged;

        private string m_Name = "";

        /// <summary>
        /// Название канала
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                if (m_Name == value)
                    return;

                m_Name = value;

                NameChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, вызываемое когда свойство Name изменилось
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewChannel()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Название канала</param>
        /// <param name="color">Цвет для канала</param>
        public ViewChannel(string name, Color color)
        {
            m_Name = name;
            m_Color = color;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Название канала</param>
        /// <param name="color">Цвет для канала</param>
        /// <param name="visible">Определяет видимость канала</param>
        public ViewChannel(string name, Color color, bool visible) : this(name, color)
        {
            m_Visible = visible;
        }
    }
}
