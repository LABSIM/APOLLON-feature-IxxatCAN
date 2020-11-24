// using directives 
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.feature.IxxatCAN
{

    public abstract class AbstractCANHandle
        : System.IDisposable
    {

        #region CAN members

        // Reference to the used VCI device.
        private Ixxat.Vci4.IVciDevice m_VCIDevice;

        // Reference to the CAN controller.
        private Ixxat.Vci4.Bal.Can.ICanControl m_CANController;

        // Reference to the CAN message communication channel.
        private Ixxat.Vci4.Bal.Can.ICanChannel m_CANChannel;

        // Reference to the CAN message scheduler.
        private Ixxat.Vci4.Bal.Can.ICanScheduler m_CANScheduler;

        // Reference to the message writer of the CAN message channel.
        private Ixxat.Vci4.Bal.Can.ICanMessageWriter m_CANMessageWriter;

        // v1.0
        // Reference to the message reader of the CAN message channel.
        //private Ixxat.Vci4.Bal.Can.ICanMessageReader m_CANMessageReader;

        // v1.0
        // Thread that handles the message reception.
        //private System.Threading.Thread m_RxThread;

        // v1.0
        // Quit flag for the receive thread.
        private long m_RxEnd = 0;

        // v1.0
        // Event that's set if at least one message was received.
        //private System.Threading.AutoResetEvent m_RxEvent;

        #endregion

        #region CAN device selection

        // Select the first CAN adapter.
        public void SelectDevice()
        {

            // temporary
            Ixxat.Vci4.IVciDeviceManager deviceManager = null;
            Ixxat.Vci4.IVciDeviceList deviceList = null;
            System.Collections.IEnumerator deviceEnum = null;

            // encapsulate
            try
            {

                // Get device manager from VCI server
                deviceManager = Ixxat.Vci4.VciServer.Instance().DeviceManager;

                // Get the list of installed VCI devices
                deviceList = deviceManager.GetDeviceList();

                // Get enumerator for the list of devices
                deviceEnum = deviceList.GetEnumerator();

                // Get first CAN device
                // #TODO be more specific to select the apropriate device in list
                deviceEnum.MoveNext();
                this.m_VCIDevice = deviceEnum.Current as Ixxat.Vci4.IVciDevice;

                // print the bus type, controller type, device name and serial number of first found controller
                Ixxat.Vci4.IVciCtrlInfo info = this.m_VCIDevice.Equipment[0];
                object serialNumberGuid = this.m_VCIDevice.UniqueHardwareId;
                string serialNumberText = AbstractCANHandle.GetSerialNumberText(ref serialNumberGuid);
                //UnityEngine.Debug.Log(
                //    "<color=Blue>Info: </color> ApollonAbstractCANHandle.SelectDevice() : found CAN device [{ BusType: "
                //    + info.BusType
                //    + " },{ ControllerType: "
                //    + info.ControllerType
                //    + " },{ Interface: "
                //    + this.m_VCIDevice.Description
                //    + " },{ Serial_Number: "
                //    + serialNumberText
                //    + " }]"
                //);

            }
            catch (System.Exception ex)
            {

                // log
                //UnityEngine.Debug.LogError(
                //    "<color=Red>Error: </color> ApollonAbstractCANHandle.SelectDevice() : failed with error ["
                //    + ex.Message
                //    + "] => "
                //    + ex.StackTrace
                //);

            }
            finally
            {

                // Dispose device manager ; it's no longer needed.
                AbstractCANHandle.DisposeVciObject(deviceManager);

                // Dispose device list ; it's no longer needed.
                AbstractCANHandle.DisposeVciObject(deviceList);

                // Dispose device list ; it's no longer needed.
                AbstractCANHandle.DisposeVciObject(deviceEnum);

            } /* try */

        } /* SelectDevice() */

        #endregion

        #region CAN socket init & setup

        // Opens the specified socket, creates a message channel, initializes
        // and starts the CAN controller.
        public bool InitSocket(System.Byte canNo)
        {

            // temp
            Ixxat.Vci4.Bal.IBalObject bal = null;
            bool bRslt = false;

            // encapsulate
            try
            {

                // Open bus access layer
                bal = this.m_VCIDevice.OpenBusAccessLayer();

                // Open a message channel for the CAN controller
                this.m_CANChannel = bal.OpenSocket(canNo, typeof(Ixxat.Vci4.Bal.Can.ICanChannel)) as Ixxat.Vci4.Bal.Can.ICanChannel;

                // Open the scheduler of the CAN controller
                this.m_CANScheduler = bal.OpenSocket(canNo, typeof(Ixxat.Vci4.Bal.Can.ICanScheduler)) as Ixxat.Vci4.Bal.Can.ICanScheduler;

                // Initialize the message channel

                // v1.0
                //this.m_CANChannel.Initialize(
                //    receiveFifoSize:
                //        1024,
                //    transmitFifoSize:
                //        128,
                //    exclusive:
                //        false
                //);

                // v2.0
                // dont need buffer as there isnt on ADwin side
                this.m_CANChannel.Initialize(
                    receiveFifoSize: 
                        1,
                    transmitFifoSize:
                        1,
                    exclusive:
                        true
                );

                // v1.0
                //// Get a message reader object
                //this.m_CANMessageReader = this.m_CANChannel.GetMessageReader();

                // v1.0
                //// Initialize message reader
                //this.m_CANMessageReader.Threshold = 1;
                
                // v1.0
                //// Create and assign the event that's set if at least one message was received.
                //this.m_RxEvent = new System.Threading.AutoResetEvent(false);
                //this.m_CANMessageReader.AssignEvent(this.m_RxEvent);

                // Get a message wrtier object
                this.m_CANMessageWriter = this.m_CANChannel.GetMessageWriter();

                // v1.0
                //// Initialize message writer
                //this.m_CANMessageWriter.Threshold = 1;

                // Activate the message channel
                this.m_CANChannel.Activate();

                // Open the CAN controller
                this.m_CANController = bal.OpenSocket(canNo, typeof(Ixxat.Vci4.Bal.Can.ICanControl)) as Ixxat.Vci4.Bal.Can.ICanControl;

                // Initialize the CAN controller

                // v1.0 
                //this.m_CANController.InitLine(
                //    operatingMode:
                //        Ixxat.Vci4.Bal.Can.CanOperatingModes.Standard
                //        | Ixxat.Vci4.Bal.Can.CanOperatingModes.Extended
                //        | Ixxat.Vci4.Bal.Can.CanOperatingModes.ErrFrame,
                //    bitrate:
                //        Ixxat.Vci4.Bal.Can.CanBitrate.Cia125KBit
                //);

                // v2.0 
                // Dont need extended mode (11 bits is enought)
                this.m_CANController.InitLine(
                    operatingMode: 
                        Ixxat.Vci4.Bal.Can.CanOperatingModes.Standard, 
                    bitrate: 
                        Ixxat.Vci4.Bal.Can.CanBitrate.Cia125KBit
                );

                // log
                //UnityEngine.Debug.Log(
                //    "<color=Blue>Info: </color> ApollonAbstractCANHandle.InitSocket() : setup CAN socket [{ LineStatus: "
                //    + this.m_CANController.LineStatus
                //    + " }]"
                //);

                // v1.0
                //// Set the acceptance filter for std identifiers
                //this.m_CANController.SetAccFilter(
                //    select:
                //        Ixxat.Vci4.Bal.Can.CanFilter.Std,
                //    code:
                //        (uint)Ixxat.Vci4.Bal.Can.CanAccCode.All,
                //    mask:
                //        (uint)Ixxat.Vci4.Bal.Can.CanAccMask.All
                //);

                // v1.0
                //// Set the acceptance filter for ext identifiers
                //this.m_CANController.SetAccFilter(
                //    select:
                //        Ixxat.Vci4.Bal.Can.CanFilter.Ext,
                //    code:
                //        (uint)Ixxat.Vci4.Bal.Can.CanAccCode.All,
                //    mask:
                //        (uint)Ixxat.Vci4.Bal.Can.CanAccMask.All
                //);

                // Start the CAN controller
                this.m_CANController.StartLine();

                // success
                bRslt = true;

            }
            catch (System.Exception ex)
            {

                // log
                //UnityEngine.Debug.LogError(
                //    "<color=Red>Error: </color> ApollonAbstractCANHandle.InitSocket() : failed with error ["
                //    + ex.Message
                //    + "] => "
                //    + ex.StackTrace
                //);

                // fail
                bRslt = false;

            }
            finally
            {

                // Dispose bus access layer
                AbstractCANHandle.DisposeVciObject(bal);

            } /* try */

            // return
            return bRslt;

        } /* InitSocket() */

        #endregion

        #region CAN send/receive definition

        protected void TransmitData<T>(T obj)
            where T : struct
        {

            // get factory & instanciate en emplty shell message
            Ixxat.Vci4.IMessageFactory factory
                = Ixxat.Vci4.VciServer.Instance().MsgFactory;
            Ixxat.Vci4.Bal.Can.ICanMessage canMsg
                = (Ixxat.Vci4.Bal.Can.ICanMessage)factory.CreateMsg(typeof(Ixxat.Vci4.Bal.Can.ICanMessage));

            // build up the data from object serializing
            byte[] data = AbstractCANHandle.Serialize<T>(obj);

            // configure the empty shell
            canMsg.TimeStamp = 0;
            canMsg.Identifier = 0x100;
            canMsg.FrameType = Ixxat.Vci4.Bal.Can.CanMsgFrameType.Data;
            canMsg.DataLength = (byte)data.Length;
            canMsg.SelfReceptionRequest = true;  // show this message in the console window

            // fill up the data  
            for (int idx = 0; idx < data.Length; ++idx)
            {
                canMsg[idx] = data[idx];
            }

            // Write the CAN message into the transmit FIFO
            this.m_CANMessageWriter.SendMessage(canMsg);

        } /* TransmitData() */

        protected void TransmitRawData(byte[] data)
        {

            // get factory & instanciate en emplty shell message
            Ixxat.Vci4.IMessageFactory factory
                = Ixxat.Vci4.VciServer.Instance().MsgFactory;
            Ixxat.Vci4.Bal.Can.ICanMessage canMsg
                = (Ixxat.Vci4.Bal.Can.ICanMessage)factory.CreateMsg(typeof(Ixxat.Vci4.Bal.Can.ICanMessage));

            // configure the empty shell
            canMsg.Identifier = 1; // Message ID
            canMsg.TimeStamp = 0; // No delay
            canMsg.ExtendedFrameFormat = false; // 11 bits
            canMsg.RemoteTransmissionRequest = false;
            canMsg.SelfReceptionRequest = false;  // do not show this message in the console window
            canMsg.FrameType = Ixxat.Vci4.Bal.Can.CanMsgFrameType.Data; // Will allways be a data frame
            canMsg.DataLength = (byte)data.Length;

            // fill up the data  
            for (int idx = 0; idx < data.Length; ++idx)
            {
                canMsg[idx] = data[idx];
            }

            // Write the CAN message into the transmit FIFO
            this.m_CANMessageWriter.SendMessage(canMsg);

        } /* TransmitRawData() */

        // v1.0
        //// This method is the works as receive thread.
        //protected void AsynCANReaderCallback()
        //{

        //    // buffer
        //    Ixxat.Vci4.Bal.Can.ICanMessage[] msgArray;

        //    // loop
        //    do
        //    {

        //        // Wait 100 msec for a message reception
        //        if (this.m_RxEvent.WaitOne(100, false))
        //        {

        //            // take all messages
        //            if (this.m_CANMessageReader.ReadMessages(out msgArray) > 0)
        //            {

        //                // flush FIFO
        //                foreach (Ixxat.Vci4.Bal.Can.ICanMessage entry in msgArray)
        //                {

        //                    // do nothing actually see Frank for status ?

        //                } /* foreach() */

        //            } /* if() */

        //        } /* if() */

        //    } while (0 == this.m_RxEnd);

        //} /* AsynCANReaderCallback() */

        #endregion

        #region CAN utility methods

        // Serialize arbitrary data to byte array
        // for more info, see [https://www.genericgamedev.com/general/converting-between-structs-and-byte-arrays/]
        protected static byte[] Serialize<T>(T obj)
            where T : struct
        {

            // determine the byte size of our structs
            int objectSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            // allocating raw segment
            byte[] array = new byte[objectSize];

            // allocate a pointer to an unmanaged memory space
            System.IntPtr buffer = System.Runtime.InteropServices.Marshal.AllocHGlobal(objectSize);

            // marshal (copy) our structure to the allocated unmanaged memory
            System.Runtime.InteropServices.Marshal.StructureToPtr(
                structure:
                    obj,
                ptr:
                    buffer,
                fDeleteOld:
                    /* a voir si false (?)... */ true
            );

            // "memcopy" - copy between the unmanaged memory and our byte array
            System.Runtime.InteropServices.Marshal.Copy(buffer, array, 0, objectSize);

            // clean unmanaged memory
            System.Runtime.InteropServices.Marshal.FreeHGlobal(buffer);

            // result
            return array;

        } /* Serialize() */

        // Deserialize arbitrary data from byte array
        // for more info, see [https://www.genericgamedev.com/general/converting-between-structs-and-byte-arrays/]
        protected static T Deserialize<T>(byte[] array)
            where T : struct
        {

            // determine the byte size of our structs
            int objectSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            // allocate a pointer to an unmanaged memory space
            System.IntPtr buffer = System.Runtime.InteropServices.Marshal.AllocHGlobal(objectSize);

            // "memcopy" - copy between our byte array and the unmanaged memory 
            System.Runtime.InteropServices.Marshal.Copy(array, 0, buffer, objectSize);

            // marshal (copy) the allocated unmanaged memory to our structure
            T obj = System.Runtime.InteropServices.Marshal.PtrToStructure<T>(buffer);

            // clean unmanaged memory
            System.Runtime.InteropServices.Marshal.FreeHGlobal(buffer);

            // result
            return obj;

        } /* Deserialize() */

        // Returns the UniqueHardwareID GUID number as string which
        // shows the serial number.
        // Note: This function will be obsolete in later version of the VCI.
        // Until VCI Version 3.1.4.1784 there is a bug in the .NET API which
        // returns always the GUID of the interface. In later versions there
        // the serial number itself will be returned by the UniqueHardwareID property.
        private static string GetSerialNumberText(ref object serialNumberGuid)
        {
            // temp
            string resultText;

            // check if the object is really a GUID type
            if (serialNumberGuid.GetType() == typeof(System.Guid))
            {
                // convert the object type to a GUID
                System.Guid tempGuid = (System.Guid)serialNumberGuid;

                // copy the data into a byte array
                byte[] byteArray = tempGuid.ToByteArray();

                // serial numbers starts always with "HW"
                if (((char)byteArray[0] == 'H') && ((char)byteArray[1] == 'W'))
                {

                    // run a loop and add the byte data as char to the result string
                    resultText = "";
                    int i = 0;
                    while (true)
                    {

                        // the string stops with a zero
                        if (byteArray[i] != 0)
                        {

                            resultText += (char)byteArray[i];

                        }
                        else
                        {

                            break;

                        } /* if() */

                        // increment
                        i++;

                        // stop also when all bytes are converted to the string
                        // but this should never happen
                        if (i == byteArray.Length)
                        {

                            break;

                        } /* if() */

                    } /* while() */

                }
                else
                {

                    // if the data did not start with "HW" convert only the GUID to a string
                    resultText = serialNumberGuid.ToString();

                } /* if() */

            }
            else
            {

                // if the data is not a GUID convert it to a string
                string tempString = (string)(string)serialNumberGuid;
                resultText = "";
                for (int i = 0; i < tempString.Length; i++)
                {
                    if (tempString[i] != 0)
                        resultText += tempString[i];
                    else
                        break;
                }

            } /* if() */

            // result
            return resultText;

        } /* GetSerialNumberText() */

        // This method tries to dispose the specified object.
        // The VCI interfaces provide access to native driver resources. 
        // Because the .NET garbage collector is only designed to manage memory, 
        // but not native OS and driver resources the application itself is 
        // responsible to release these resources via calling 
        // IDisposable.Dispose() for the obects obtained from the VCI API 
        // when these are no longer needed. 
        // Otherwise native memory and resource leaks may occure.
        private static void DisposeVciObject(object obj)
        {

            if (null != obj)
            {

                System.IDisposable dispose = obj as System.IDisposable;
                if (null != dispose)
                {

                    dispose.Dispose();
                    obj = null;

                } /* if() */

            } /* if() */

        } /* DisposeVciObject() */

        #endregion

        // ctor     
        public AbstractCANHandle()
            : base()
        { }
        
        // Dispose pattern
        public void Dispose()
        {

            this.Dispose(true);
            System.GC.SuppressFinalize(this);

        } /* Dispose() */

        protected void Dispose(bool bDisposing = true)
        {

            //
            // Dispose all hold VCI objects.
            //

            // v1.0
            // Dispose message reader
            //ApollonAbstractCANHandle.DisposeVciObject(this.m_CANMessageReader);

            // Dispose message writer 
            AbstractCANHandle.DisposeVciObject(this.m_CANMessageWriter);

            // Dispose CAN channel
            AbstractCANHandle.DisposeVciObject(this.m_CANChannel);

            // Dispose CAN controller
            AbstractCANHandle.DisposeVciObject(this.m_CANController);

            // Dispose VCI device
            AbstractCANHandle.DisposeVciObject(this.m_VCIDevice);

        } /* Dispose(bool) */

        //#region event handling 

        //public /*override*/ void onHandleActivationRequested(/*object sender, ApollonBackendManager.EngineHandleEventArgs arg*/)
        //{

        //    // check
        //    //if (this.ID == arg.HandleID)
        //    //{

        //        // select device
        //        this.SelectDevice();

        //        // log
        //        //UnityEngine.Debug.Log(
        //        //    "<color=Blue>Info: </color> ApollonAbstractCANHandle.onHandleActivationRequested() : device selected"
        //        //);

        //        // init connection
        //        if (!this.InitSocket(0))
        //        {

        //            // log
        //            //UnityEngine.Debug.LogError(
        //            //    "<color=Red>Error: </color> ApollonAbstractCANHandle.onHandleActivationRequested() : failed to initialize connection, exit"
        //            //);

        //            // abort
        //            this.Dispose();

        //        } /* if() */

        //        // bind & start the receive thread
        //        //this.m_RxThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.AsynCANReaderCallback));
        //        //this.m_RxThread.Start();

        //        // pull-up
        //        //base.onHandleActivationRequested(sender, arg);

        //    //} /* if() */

        //} /* onHandleActivationRequested() */

        //// unregistration
        //public /*override*/ void onHandleDeactivationRequested(/*object sender, ApollonBackendManager.EngineHandleEventArgs arg*/)
        //{

        //    // check
        //    //if (this.ID == arg.HandleID)
        //    //{

        //        // tell receive thread to quit
        //        System.Threading.Interlocked.Exchange(ref this.m_RxEnd, 1);

        //        // wait for termination of receive thread
        //        //this.m_RxThread.Join();

        //        // pull-up
        //        //base.onHandleDeactivationRequested(sender, arg);

        //    //} /* if() */

        //} /* onHandleDeactivationRequested() */

        //#endregion

    } /* class AbstractCANHandle */

} /* } namespace */