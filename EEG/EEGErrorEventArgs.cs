using System;

namespace EEG
{
    /// <summary>
    /// Параметры возвращаемые событием генерации ошибки
    /// </summary>
    public class EEGErrorEventArgs : EventArgs
    {
        /// <summary>
        /// тип ошибки 0 - перезагрузка не требуется, 1 - требуется перезагрузка
        /// </summary>
        public uint Error_type
        {
            get;
        }

        /// <summary>
        /// код ошибки
        /// </summary>
        public uint Error_code
        {
            get;
        }

        /// <summary>
        /// текст ошибки
        /// </summary>
        public string Error_Str
        {
            get;
        }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="error_type">тип ошибки 0 - перезагрузка не требуется, 1 - требуется перезагрузка</param>
        /// <param name="error_code">код ошибки</param>
        /// <param name="error_Str">текст ошибки</param>
        public EEGErrorEventArgs(uint error_type, uint error_code, string error_Str)
            : base()
        {
            Error_type = error_type;
            Error_code = error_code;
            Error_Str = error_Str;
        }
    }
}
