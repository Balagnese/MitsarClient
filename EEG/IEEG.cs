using System;

namespace EEG
{
    /// <summary>
    /// Интерфейс для устройств типа ЭЭГ 
    /// </summary>
    public interface IEEG
    {
        /// <summary>
        /// событие обработки ошибки
        /// </summary>
        event EventHandler<EEGErrorEventArgs> Error;

        /// <summary>
        /// событие получения данных с устройства (index1 - номер канала, index2 - отсчет сигнала)
        /// </summary>
        event EventHandler<EEGReseiveDataEventArgs> ReseiveData;

        /// <summary>
        /// Функция, используемая для преобразования сигнала перед его отправкой подписчикам на ReseiveData
        /// </summary>
        Action<int[,]> TransformData
        {
            get;
            set;
        }

        /// <summary>
        /// true - процесс запущен
        /// </summary>
        bool IsRunning
        {
            get;
        }

        /// <summary>
        /// Частотный режим ЭЭГ
        /// </summary>
        SamplingMode Sampling
        {
            get;
            set;
        }

        /// <summary>
        /// Запуск процесса
        /// </summary>
        bool Start();

        /// <summary>
        /// Событие, возникающее когда процесс запущен
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// Вызывается, когда свойство Sampling изменилось
        /// </summary>
        event EventHandler SamplingChanged;

        /// <summary>
        /// Остановка процесса
        /// </summary>
        void Stop();

        /// <summary>
        /// Событие, возникающее когда процесс остановлен
        /// </summary>
        event EventHandler Stopped;
    }
}
