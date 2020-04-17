using Mitsar.Essentials;
using Mitsar.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EEG.MitsarSmartBCI
{
    class MitsarDeviceOperation
    {
        public delegate void DeviceInputTask();
        private static bool Running = false;

        private static string GetAssemblyDescription(string AssemblyName_)
        {
            AssemblyName assemblyname = new AssemblyName(AssemblyName_);
            Assembly assembly = Assembly.Load(assemblyname.FullName);
            string fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(AssemblyName_);
            sb.Append("Version: ");
            sb.AppendLine(fileVersion);
            sb.Append("Path: ");
            sb.Append(assembly.Location);
            return sb.ToString();
        }


        /// <summary>
        /// Starts input
        /// </summary>
        /// <param name="device">Device object</param>
        /// <param name="ProtocolClass">Class of device to operate with</param>
        /// <param name="DataType">Type of data to input</param>
        /// <param name="InputTask">Data processing task</param>
        /// <param name="ErrorMessage">Error</param>
        /// <returns>Returns true if device was succesfully started and stopped </returns>
        public static bool Launch(DeviceInput device,
            ProtocolClassID ProtocolClass,
            string PrefferedSerialNumber,
            InputDataType DataType,
            DeviceInputTask InputTask,
            ref string ErrorMessage)
        {
            if (Running == true) return false;

            try
            {
                Running = true;
                if (device == null)
                {
                    ErrorMessage = "Device is null";
                    return false;
                }


                #region Getting library versions
                //Getting HAL library Version information
                int Version = 0, Release = 0;
                device.GetHALVersion(ref Version, ref Release);
                Console.WriteLine("Mitsar.HAL version: {0}.{1}", Version, Release);
                Console.WriteLine();
                Console.WriteLine("Assemblies_____________");
                Console.WriteLine(GetAssemblyDescription("Mitsar.Essentials"));
                Console.WriteLine(GetAssemblyDescription("Mitsar.HAL"));
                Console.WriteLine();
                #endregion


                #region Getting available hardware and selecting device
                DeviceDescription SelectedDescription = null;

                //Detecting Hardware
                List<DeviceDescription> descriptions = new List<DeviceDescription>();

                Console.WriteLine("Enumerating devices for protocol " + ProtocolClass);
                device.Enumerate(HardwareClassID.EEG, ProtocolClass, ref descriptions);

                //Showing all discovered devices

                if (descriptions != null)
                {
                    Console.WriteLine("Devices found ______________");

                    foreach (DeviceDescription desc in descriptions)
                    {
                        Console.WriteLine(desc.ToString());
                        Console.WriteLine("Driver Name: " + desc.GetDeviceDriverName());
                        Console.WriteLine("Driver SerialNumber: " + desc.DriverSerialNumber);
                        Console.WriteLine("State: " + desc.State.ToString());
                        Console.WriteLine();
                    }
                }
                else
                {
                    ErrorMessage = "No devices found";
                    return false;
                }

                Console.WriteLine("________________________");
                //Selecting device description
                if (descriptions.Count == 0)
                {
                    ErrorMessage = "Error: No devices found";
                    return false;
                }
                else
                {
                    if (string.IsNullOrEmpty(PrefferedSerialNumber) == false)
                    {
                        Console.Write("Searching for device with serial number " + PrefferedSerialNumber + "...");

                        //Choosing EEG Device
                        foreach (DeviceDescription desc in descriptions)
                        {
                            if (desc.DriverSerialNumber == PrefferedSerialNumber)
                            {
                                SelectedDescription = desc;
                                break;
                            }
                        }

                        if (SelectedDescription != null)
                            Console.WriteLine(" found " + SelectedDescription.ToString());
                        else Console.WriteLine(" not found");
                    }


                    if (SelectedDescription == null)
                    {
                        Console.Write("Searching for any device...");
                        //Choosing EEG Device
                        foreach (DeviceDescription desc in descriptions)
                        {
                            if (desc.HardwareClass == HardwareClassID.EEG)
                            {
                                SelectedDescription = desc;
                                break;
                            }
                        }

                        if (SelectedDescription != null)
                            Console.WriteLine(" found " + SelectedDescription.ToString());
                        else Console.WriteLine(" not found");
                    }

                    if (SelectedDescription == null)
                    {
                        ErrorMessage = "Error: No devices found";
                        return false;
                    }
                }
                #endregion


                #region Opening, Powering and detecting device version
                //Opening HAL
                Console.Write("Opening...");
                if (device.Open(SelectedDescription) == true)
                {
                    Console.WriteLine("OK");
                }
                else return false;


                //Powering HAL
                Console.Write("Powering...");
                if (device.PowerOn() == true)
                {
                    Console.WriteLine("OK");
                }
                else return false;


                //Identifying HAL
                Console.Write("Indentifing...");
                if (device.Identify() == true)
                {
                    Console.WriteLine("OK");
                }
                else return false;


                if (device.ActiveDeviceDescription.Resource.Version == 0)
                {
                    ErrorMessage = "Error: Unknown version " + device.ActiveDeviceDescription.Resource.ReservedVersion;
                    return false;
                }

                Console.WriteLine();
                Console.WriteLine("Device vendor: " + device.ActiveDeviceDescription.Resource.Vendor);
                Console.WriteLine("Device version: " + device.ActiveDeviceDescription.Resource.Version);
                Console.WriteLine("Device serial number: " + device.ActiveDeviceDescription.Resource.SerialNumber);
                #endregion


                #region Loading and setting up calibration
                Console.Write("Getting calibration...");
                Resource_MultiChannel mresource = device.ActiveDeviceDescription.Resource as Resource_MultiChannel;
                //If internal device calibration is supported
                if (mresource.IsROMSupported() == true)
                {//ROM supported - reading from ROM

                    byte[] buf = null;

                    //Reading calibration data from device's memory to buffer
                    if (device.ReadMemory(ref buf) == true)
                    {//if ROM read successfully - transfer to calibration

                        //Decoding buffer to calibtaion data
                        CalibrationProfileIOResult result = CalibrationProfile.ReadFromBuffer(ref device.ActiveDeviceDescription.Calibration, mresource, buf);
                        if (result == CalibrationProfileIOResult.OK) //Translate OK
                        {
                            //After successfull read version and serial must be assigned to CalibrationFile
                            device.ActiveDeviceDescription.Calibration.Parameters.Version = device.ActiveDeviceDescription.Resource.Version;
                            device.ActiveDeviceDescription.Calibration.Parameters.FullSerialNumber = device.ActiveDeviceDescription.Resource.SerialNumber;
                            //Serial is not validated because received from already validated device resource
                            Console.WriteLine("OK");
                        }
                        else  //ROM translate or check sum error
                        {
                            device.ActiveDeviceDescription.Calibration.Clear();
                        }
                    }//ROM Access error
                    else
                    {
                        device.ActiveDeviceDescription.Calibration.Clear();
                    }
                }//if ROM supported
                else Console.WriteLine(" not supported");

                Console.WriteLine("");
                //else calibration must be read from calibration file eeg.cal
                //CalibrationFile.Read(ref device.ActiveDeviceDescription.Calibration, "eeg.cal");  
                #endregion


                #region Configuring device
                //Setting input mode to global input mode. EEG(data) or Impedance
                mresource.DataType = DataType;

                //Test signal mode disabled
                mresource.TestSignal = false;

                //Setting up referent operation mode
                //Getting list of supported referent types
                List<ReferentOperationMode> RefModeList = mresource.GetReferentOperationModeList(false);
                Console.Write("Supported referents: ");
                foreach (ReferentOperationMode refmode in RefModeList)
                    Console.Write(refmode + " ");
                Console.WriteLine();
                //Selecting first supported mode
                //mresource.EmitterFactory.RefMode = ReferentOperationMode.RefElectrode;
                //mresource.EmitterFactory.RefMode = ReferentOperationMode.Joined;
                //RefModeList[0];

                mresource.EmitterFactory.RefMode = RefModeList[0];

                mresource.EmitterFactory.ImpedanceChannelEnabled = true;
                mresource.EmitterFactory.AccelerometerChannelEnabled = true;


                //Setting up sampling frequency
                //GEtting list of supported sampling frequencies
                List<double> FreqList = mresource.GetSamplingFrequencyList(false);
                Console.Write("Supported frequencies: ");
                foreach (double freq in FreqList)
                    Console.Write(freq + " ");
                Console.WriteLine();

                //Selecting first frequency in list, generally nominal 
                //int SamplingFrequency = (int)mresource.GetNominalSamplingFrequency();
                int SamplingFrequency = (int)FreqList[0];
                mresource.EmitterFactory.SamplingFrequency = SamplingFrequency;

                //Setting other data buffer parameters
                //Getting BYTES PER SAMPLE FROM FIRST ENCOUNTERED DATA CHANNEL
                mresource.DataBuffer.BytesPerTick = mresource.Interface_GetBytesPerSample(UID.NOP);

                mresource.DataBuffer.TicksMin = 2;
                mresource.DataBuffer.TicksMax = 100;
                mresource.DataBuffer.TicksSkip = 10;// 10;


                //Enable Front panel LED in Impedace Test mode
                mresource.FrontLedEnabled = true;
                mresource.FrontLedThesholdEnabled = false;

                //Set edge value, can be changed in any time
                //All impedances above this value will be highligted
                //Only 5, 10, 20, 40 values are indicated
                mresource.FrontLedThesholdValue = 10;

                Console.WriteLine();
                #endregion


                #region Starting data input
                Console.Write("Starting...");
                if (device.Start() == true)
                {
                    Console.WriteLine("OK");
                }
                else return false;


                Console.WriteLine("Emitters__________________");
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("UID\tIndex\tAlign\tBPS\tHigh\tLow\tNotch\tUnitsPerSample");

                foreach (EmitterDescription emm in mresource.EmitterFactory)
                {
                    sb.Append(emm.UID.ToString());
                    sb.Append("\t");
                    sb.Append(emm.HardwareIndex.ToString());
                    sb.Append("\t");
                    sb.Append(emm.Align.ToString());
                    sb.Append("\t");
                    sb.Append(emm.BytesPerSample.ToString());
                    sb.Append("\t");
                    sb.Append(emm.HardHighPass.ToString());
                    sb.Append("\t");
                    sb.Append(emm.HardLowPass.ToString());
                    sb.Append("\t");
                    sb.Append(emm.HardNotch.ToString());
                    sb.Append("\t");
                    sb.Append(emm.UnitsPerSample.ToString());
                    sb.AppendLine("\t");
                }
                Console.WriteLine(sb.ToString());


                Console.WriteLine("Calibration_____________");
                //sb.Clear();
                foreach (EmitterDescription emm in mresource.EmitterFactory)
                {
                    sb.Append(emm.UID.ToString());
                    sb.Append("\t");

                    if (emm.CalEntryLink != null)
                    {
                        sb.Append(emm.CalEntryLink.ToString());
                        sb.Append("\t");
                    }
                    else sb.Append(" no calibration");

                    sb.AppendLine("\t");
                }
                Console.WriteLine(sb.ToString());

                Console.WriteLine("Input parameters_____________");
                Console.WriteLine("Data Type: " + mresource.DataType.ToString());
                Console.WriteLine("Sampling Frequency: " + mresource.EmitterFactory.SamplingFrequency.ToString());

                Console.WriteLine("BytesPerTick: " + mresource.DataBuffer.BytesPerTick.ToString());
                Console.WriteLine("Minimum ticks to transmit: " + mresource.DataBuffer.TicksMin.ToString());
                Console.WriteLine("Maximum ticks to transmit: " + mresource.DataBuffer.TicksMax.ToString());

                Console.WriteLine("Internal number of channels: " + mresource.InputHardwareChannelCountInternal.ToString());
                Console.WriteLine("External number of channels: " + mresource.InputHardwareChannelCountExternal.ToString());
                Console.WriteLine();
                #endregion


                //Start Data asquisition task
                Console.WriteLine("Executing input task...");
                if (InputTask != null)
                    InputTask();


                bool DisplayImpedances = false;

                #region Displaying impedances
                if (DisplayImpedances)
                {
                    Console.WriteLine("----- EmitterFactory -----");
                    foreach (EmitterDescription em in mresource.EmitterFactory)
                    {
                        ///Console.WriteLine(em.UID + "\t RF=" + em.ImpedancePreliminaryValue + "\t R=" + em.ImpedanceResultValue);
                        Console.WriteLine(em.UID + "\t RValue=" + em.ImpedanceResultValue);//;;em.ImpedancePreliminaryValue + "\t R=" + em.ImpedanceResultValue);
                    }

                    Console.WriteLine("----- ImpedanceArray -----");
                    foreach (ImpedanceElement el in mresource.ImpedanceArray)
                    {
                        Console.WriteLine(el.LogicalUID +
                            // " Name= " + el.Name +
                            // " Active:" + el.Active +
                            // " InMontage:" + el.PresentInMontage +
                            //" REF:" + el.HardwareReferent +
                            //  " LED:" + el.LedState + " (" + el.LedIndex + ")" +
                            // " PVal:" + el.PreliminaryValue +
                            "\t RValue:" + el.ResultValue);
                    }

                }
                #endregion


                #region Displaying power mode
                Console.WriteLine("Power mode: " + device.ActiveDeviceDescription.Resource.PowerMode);
                #endregion

                Console.WriteLine("Complete");
                return true;
            }
            finally
            {
                //Memorizing error code
                HALErrorCode errorcode = device.HALError.Code;

                Running = false;

                #region Finalising device
                if (device != null)
                {
                    if (device.IsStarted)
                    {
                        Console.Write("Stopping...");
                        if (device.Stop() == true)
                        {
                            Console.WriteLine("OK");
                        }
                    }

                    if (device.IsPowered)
                    {
                        Console.Write("Powering off...");
                        if (device.PowerOff() == true)
                        {
                            Console.WriteLine("OK");
                        }
                    }

                    if (device.IsOpened)
                    {
                        Console.Write("Closing...");
                        if (device.Close() == true)
                        {
                            Console.WriteLine("OK");
                        }
                    }

                    if (errorcode != HALErrorCode.OK)
                        ErrorMessage = "Error: " + errorcode.ToString();
                }
                #endregion
            }

        }//Launch
    }
}
