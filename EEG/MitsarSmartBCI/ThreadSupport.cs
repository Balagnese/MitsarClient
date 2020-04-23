using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mitsar.Hardware;
using Mitsar.Essentials;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace EEG.MitsarSmartBCI
{

    /// <summary>
    /// Input thread support
    /// </summary>
    class ThreadSupport
    {
        public const uint _AttrMask = 0xFC000000;
        public const uint _UIDMask = 0x03FF0000;
        public const uint _RMask = 0x00007FFF;

        /// <summary>
        /// Target input device
        /// </summary>
        DeviceInput device = null;

        /// <summary>
        /// Функция, используемая для преобразования сигнала перед его отправкой подписчикам на ReseiveData
        /// </summary>
        public Action<int[,]> ThreadSupportTransformData { get; set; }

        /// <summary>
        /// Target draw control
        /// </summary>
        //PlotView view = null;

        /// <summary>
        /// Received portion of data
        /// </summary>
        float[] inputdata = null;

        /// <summary>
        /// Number of channels
        /// </summary>
        int ChannelCount;

        /// <summary>
        /// Max number of ticks
        /// </summary>
        int TickCount;


        /// <summary>
        /// Thread stop flag 
        /// </summary>
        private volatile bool stop = false;
       
        public void Start()
        {
            var task = new Task(() =>
            {
                Run();
            });
            task.Start();
        }

        public void Stop()
        {
            //Signal thread to terminate
            //Can be replaced with ResetEvent
            stop = true;
        }
        
        volatile public bool Terminated = false;

        public void Wait()
        {
            //Waiting until thread terminated
            //Can be replaced with ResetEvent
            while (Terminated == false)
            {
                //Console.WriteLine("________________________Terminated " + Terminated);
                Thread.Sleep(1);
                Application.DoEvents();
                //Console.WriteLine("________________________Terminated " + Terminated);
            }
            
        }

        int[,] data = null;
        public ThreadSupport(DeviceInput device, int ChannelCount, int TickCount)
        {
            this.device = device;
            this.ChannelCount = ChannelCount;
            this.TickCount = TickCount;
            inputdata = new float[ChannelCount * TickCount];
        }

        /// <summary> 
        /// событие получения данных с устройства (index1 - номер канала, index2 - отсчет сигнала) 
        /// </summary> 
        public event EventHandler<EEGReseiveDataEventArgs> ThreadSupportReseiveData;

        public event EventHandler<string> ThreadSupportEmitterData;
        string newLine = Environment.NewLine;
        private void Run()
        {
            Terminated = false;
            while (stop == false)
            {
                string tmp = "";
                bool Result = device.Scan(); //Returns true if buffer contains valid number of tics
                if (device.HALError.Code != HALErrorCode.OK) break; //Error encountered
                if (Result == true)
                {
                    Resource_MultiChannel Resource = device.ActiveDeviceDescription.Resource as Resource_MultiChannel;

                    //Receiveing Data
                    if (device.Grab() == true)
                    {//
                        if (device.HALError.CoreCode == HALCoreErrorCode.TickMiss)
                        {
                            device.HALError.Assign(device.ActiveDeviceDescription, HALErrorCode.OK);// Code = HALErrorCode.OK;
                            device.HALError.AssignCoreCode(HALCoreErrorCode.OK);//TickMiss
                        }
                        //Remember number of ticks in input buffer, those was read from FIFO to DataOut
                        int TicksReceived = Resource.DataBuffer.TicksReceived;
                        int ChannelCount = Resource.InputHardwareChannelCountExternal;
                        int time_on_bus = 0;
                        data = new int[ChannelCount, TicksReceived];
                        //process each channel in input, linked with current datasource
                        foreach (EmitterDescription em in Resource.EmitterFactory)
                        {
                            //Getting index of emitter channel in Data Out buffer
                            int HardwareIndex = em.HardwareIndex;
                            //Console.WriteLine(em.UID.ToString() + ' ' + em.HardwareIndex);
                            tmp += em.UID.ToString() + ' ' + em.HardwareIndex + newLine;
                            ThreadSupportEmitterData?.Invoke(this, tmp);
                            tmp = "";
                            if (HardwareIndex < 0) continue;
                            for (long i = 0; i < TicksReceived; i++)
                            {
                                //Сырые данные полученные с прибора в единицах АЦП.
                                int value = Resource.DataBuffer.DataOut[i * ChannelCount + HardwareIndex];
                                
                                if (em.UID == UID.AD0)
                                {
                                    if (value > 0)
                                    {
                                        UID LogicalUID = (UID)((value & _UIDMask) >> 16);
                                        double Rvalue = (double)(value & _RMask) / 10.0;
                                        //Attributes
                                        bool Active = ((value & (uint)ImpedanceElementAttributes.Active) > 0) ? true : false;
                                        bool HardwareReferent = ((value & (uint)ImpedanceElementAttributes.Referent) > 0) ? true : false;
                                        Console.WriteLine(em.UID.ToString() + " UID " + LogicalUID.ToString() + " Rvalue " + Rvalue +
                                            " Active " + Active + " HardwareReferent " + HardwareReferent);
                                    }
                                }
                                else if (em.UID == UID.AD1)
                                {
                                    if (value > 0)
                                    {
                                        /* UID.AD1 - Канал акселерометра.Значения датчиков положения тела. byte XAxis; byte YAxis; 
                                            byte ZAxis;byte Reserved;  */
                                        byte[] ad1bytes = BitConverter.GetBytes(value);
                                        int x = ad1bytes[0];
                                        int y = ad1bytes[1];
                                        int z = ad1bytes[2];
                                         
                                        Console.WriteLine(em.UID.ToString() + " x " + x + " y " + y + " z " + z);
                                    }
                                }
                                else
                                {
                                    //Преобразуем значение в единицах АЦП в значение в микровольтах умножая на коэффициент квантования.
                                    float fValue = value * em.UnitsPerSample;
                                    int fValueInt = (int)fValue;
                                    //Преобразовываем в думерный массив

                                    data[em.HardwareIndex, i] = fValueInt;
                                }
                            }
                            time_on_bus += TicksReceived;
                        }
                        ThreadSupportTransformData?.Invoke(data);
                        
                        ThreadSupportReseiveData?.Invoke(this, new EEGReseiveDataEventArgs((int[,])data.Clone(), time_on_bus));
                    }
                }
                else
                    Thread.Sleep(1);
            }
            Terminated = true;
        }
    }

}


