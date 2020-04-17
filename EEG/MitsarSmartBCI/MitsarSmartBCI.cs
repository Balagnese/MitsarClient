using Mitsar.Essentials;
using Mitsar.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEG.MitsarSmartBCI
{
    /// <summary>
    /// класс в котором реализована работа с прибором ЭЭГ Нейрон-спектр 4СП
    /// </summary>
    public class MitsarSmartBCI : IEEG
    {
        DeviceInput device = new DeviceInput();
        string ErrorMessage = null;
        InputDataType DataType = InputDataType.DataSignal; //Data/test signal input mode
                                                           //InputDataType DataType = InputDataType.ImpedanceSignal; //Impedance signal input mode

        //ProtocolClassID ProtocolClass = ProtocolClassID.FTDI; //Mitsar FTDI devicesC:\Mitsar HAL SDK 8.37\InputTest\Program.cs
        ProtocolClassID ProtocolClass = ProtocolClassID.SMART_LOWLEVEL_BT; //Mitsar SmartBCI devices


        public MitsarSmartBCI()
        {
            //No preffered serial number, trying to opne any device
            string PrefferedSerialNumber = null;
            //PrefferedSerialNumber = "3005";
            bool result = MitsarDeviceOperation.Launch(device,
                ProtocolClass,
                PrefferedSerialNumber,
                DataType,
                test,
                ref ErrorMessage);
            m_Sampling = SamplingMode.Frequency250;
            if (ErrorMessage != null)
            {
                MessageBox.Show(ErrorMessage);
            }
        }
        //Эта функция нужна для того, чтобы положить ее в MitsarDeviceOperation.Launch когда создается MitsarSmartBCI
        void test() { }

        /// <summary>
        /// Функция, используемая для преобразования сигнала перед его отправкой подписчикам на ReseiveData
        /// </summary>
        public Action<int[,]> TransformData { get; set; }

        /// <summary>
        /// Частотный режим ЭЭГ
        /// </summary>
        private SamplingMode m_Sampling;

        /// <summary>
        /// Определяет, будет ли запущена программа
        /// </summary>
        private int m_Started = 0;

        /// <summary>
        /// Возвращает значение заряда батареи в процентах
        /// </summary>
        public int Battery { get; private set; }

        /// <summary>
        /// Событие, возникающее когда заряд устройства изменен
        /// </summary>
        public event EventHandler BatteryChanged;

        /// <summary>
        /// true - процесс запущен
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return m_Started == 1;
            }
        }

        /// <summary>
        /// Частотный режим ЭЭГ
        /// </summary>
        public SamplingMode Sampling
        {
            get
            {
                return m_Sampling;
            }
            set
            {
                if (Sampling == value)
                    return;

                if (IsRunning)
                    throw new Exception("Нельзя изменить свойство, когда ЭЭГ запущен");

                m_Sampling = value;

                SamplingChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// событие обработки ошибки
        /// </summary>
        public event EventHandler<EEGErrorEventArgs> Error;

        /// <summary>
        /// событие получения данных с устройства (index1 - номер канала, index2 - отсчет сигнала)
        /// </summary>
        public event EventHandler<EEGReseiveDataEventArgs> ReseiveData;

        /// <summary>
        /// Событие, возникающее когда процесс запущен
        /// </summary>
        public event EventHandler Started;

        /// <summary>
		/// Вызывается, когда свойство Sampling изменилось
		/// </summary>
        public event EventHandler SamplingChanged;

        /// <summary>
        /// Событие, возникающее когда процесс остановлен
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Событие, когда получаются данные с одного канала
        /// </summary>
        public event EventHandler<string> EmitterData;

        /// <summary>
        /// Запуск процесса
        /// </summary>
        public bool Start()
        {
            Started?.Invoke(this, new EventArgs());
            m_Started = 1;
            //No preffered serial number, trying to opne any device
            string PrefferedSerialNumber = null;
            //PrefferedSerialNumber = "3005";
            bool result = MitsarDeviceOperation.Launch(device,
                ProtocolClass,
                PrefferedSerialNumber,
                DataType,
                InputTask,
                ref ErrorMessage);
            m_Sampling = SamplingMode.Frequency250;
            updateBatteryPercent();
            if (ErrorMessage != null)
            {
                MessageBox.Show(ErrorMessage);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Остановка процесса
        /// </summary>
        public void Stop()
        {
            Stopped?.Invoke(this, new EventArgs());
            device.Stop();
            m_Started = 0;
        }
        ThreadSupport InputSupport = null;
        private void InputTask()
        {
            if (device != null)
            {
                updateBatteryPercent();
                SynchronizationContext context = SynchronizationContext.Current;
                Resource_MultiChannel mresource = device.ActiveDeviceDescription.Resource as Resource_MultiChannel;
                int ChannelCount = mresource.EmitterFactory.EmitterCount;
                int TickCount = mresource.DataBuffer.TicksMax;
                InputSupport = new ThreadSupport(device, ChannelCount, TickCount);
                InputSupport.ThreadSupportTransformData += TransformData;
                InputSupport.ThreadSupportReseiveData += ReseiveData;
                InputSupport.ThreadSupportEmitterData += EmitterData;
                InputSupport.Start();
                //Waiting Task to complete
                InputSupport.Wait();
            }
        }

        private void EmitterData1(object sender, string e)
        {
            //Console.WriteLine(e);
        }

        private void updateBatteryPercent()
        {
            try
            {
                Battery = device.ActiveDeviceDescription.ParamStatus.BatteryPercent;
                BatteryChanged?.Invoke(this, new EventArgs());
            }
            catch { }
        }


       

        public string[,] GetEmittersDescription()
        {
            Resource_MultiChannel mresource = device.ActiveDeviceDescription.Resource as Resource_MultiChannel;
            string[,] emitters = new string[mresource.EmitterFactory.EmitterCount, 6];
            foreach (EmitterDescription emm in mresource.EmitterFactory)
            {
                emitters[emm.HardwareIndex, 0] = emm.HardwareIndex.ToString();
                emitters[emm.HardwareIndex, 1] = emm.UID.ToString();
                emitters[emm.HardwareIndex, 2] = emm.BytesPerSample.ToString();
                emitters[emm.HardwareIndex, 3] = emm.HardHighPass.ToString();
                emitters[emm.HardwareIndex, 4] = emm.HardLowPass.ToString();
                emitters[emm.HardwareIndex, 5] = emm.UnitsPerSample.ToString();
            }
            return emitters;
        }


    }
}
