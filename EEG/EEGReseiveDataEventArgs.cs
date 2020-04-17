using System;

namespace EEG
{
    /// <summary>
    /// Аргументы события получения данные
    /// </summary>
    public class EEGReseiveDataEventArgs : EventArgs
    {
        /// <summary>
        /// Полученные данные (index1 - номер канала, index2 - отсчет сигнала)
        /// </summary>
        public int[,] Data
        {
            get;
        }

        /// <summary>
        /// счетчик шины USB
        /// </summary>
        public int Time_on_bus
        {
            get;
        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="data">полученные данные (index1 - номер канала, index2 - отсчет сигнала)</param>
        /// <param name="time_on_bus">счетчик шины</param>
        public EEGReseiveDataEventArgs(int[,] data, int time_on_bus)
            : base()
        {
            Data = data;
            Time_on_bus = time_on_bus;
        }
    }
}
