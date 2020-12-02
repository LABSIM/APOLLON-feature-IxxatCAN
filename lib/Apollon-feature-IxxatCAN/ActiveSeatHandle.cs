// avoid namespace pollution
namespace Labsim.apollon.feature.IxxatCAN.handle
{

    public class ActiveSeatHandle
        : AbstractCANHandle
    {

        #region CAN messages definition

        public struct CAN
        {

            #region enums

            public enum EventType : byte
            {

                APOLLON_EVENT_BEGINSESSION = 0, // session begining event -> ADWin init
                APOLLON_EVENT_ENDSESSION = 1, // session end event -> ADWin closure
                APOLLON_EVENT_BEGINTRIAL = 2, // trial begin event -> ADWin start streaming status (optionnal)
                APOLLON_EVENT_ENDTRIAL = 3, // trial end event -> ADWin stop streaming status (optionnal)
                APOLLON_EVENT_START = 4, // start stimulus event -> ADWin start action immediately after reception
                APOLLON_EVENT_STOP = 5, // stop stimulus event -> ADWin stop/freeze action immediately after reception
                APOLLON_EVENT_RESET = 6, // reset event -> ADwin reset position to initial condition immediately after reception
                APOLLON_EVENT_UNKNOWN

            } /* enum EventType */

            public enum EventArgType : byte
            {

                APOLLON_EVENT_ARG_ANGULAR_ACCELERATION = 0, // associated with APOLLON_EVENT_START event to provide accel value
                APOLLON_EVENT_ARG_ANGULAR_SPEED_SATURATION = 1, // associated with APOLLON_EVENT_START event to provide speed value
                APOLLON_EVENT_ARG_MAX_STIM_DURATION = 2, // associated with APOLLON_EVENT_START event to provide duration value
                APOLLON_EVENT_ARG_ACQUITTAL = 4, // associated with APOLLON_EVENT_START event to provide acquittal
                APOLLON_EVENT_ARG_UNKNOWN

            } /* enum EventArgType */

            public enum FlagType : byte
            {

                APOLLON_FLAG_NONE = 0, // empty flag
                APOLLON_FLAG_UNKNOWN

            } /* enum FlagType */

            #endregion

            // v1.0
            //#region messages

            //[System.Runtime.InteropServices.StructLayout(
            //    System.Runtime.InteropServices.LayoutKind.Sequential,
            //    CharSet = System.Runtime.InteropServices.CharSet.Ansi,
            //    Pack = 1
            //)]
            //public struct MsgInfo
            //{

            //    public byte bEvent; // type (see EventType::APOLLON_EVENT_* constants)
            //    public byte bFlags; // additional flags (see FlagType::APOLLON_FLAG_* constants)

            //}; /* struct MsgInfo */

            //[System.Runtime.InteropServices.StructLayout(
            //    System.Runtime.InteropServices.LayoutKind.Sequential,
            //    CharSet = System.Runtime.InteropServices.CharSet.Ansi,
            //    Pack = 1
            //)]
            //public struct Msg
            //{

            //    public MsgInfo info; // message information

            //    [System.Runtime.InteropServices.MarshalAs(
            //        System.Runtime.InteropServices.UnmanagedType.ByValArray,
            //        ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1
            //    )]
            //    public byte[] payload; // payload type is dependant of MsgInfo.bEvent

            //}; /* struct Msg */

            //#endregion

            //#region payloads

            //[System.Runtime.InteropServices.StructLayout(
            //    System.Runtime.InteropServices.LayoutKind.Sequential,
            //    CharSet = System.Runtime.InteropServices.CharSet.Ansi,
            //    Pack = 1
            //)]
            //public struct PayloadStartEvent
            //{

            //    public System.Double dAngularAcceleration;      // rad/s^2 (SI)
            //    public System.Double dAngularSpeedSaturation;   // rad/s (SI)
            //    public System.Double dMaxStimDuration;          // ms (SI)

            //}; /* struct PayloadStartEvent */

            //#endregion

        }; /* CAN */

        #endregion 

        #region CAN event implementation

        public void BeginSession()
        {

            // build up the transmitted data 

            // v1.0 
            //this.TransmitData(
            //    new CAN.Msg()
            //    {
            //        info = new CAN.MsgInfo()
            //        {
            //            bEvent = (byte)CAN.EventType.APOLLON_EVENT_BEGINSESSION,
            //            bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
            //        },
            //        payload = null
            //    }
            //);

            // v2.0 - raw
            this.TransmitRawData(
                new byte[] {
                    (byte)CAN.EventType.APOLLON_EVENT_BEGINSESSION
                }
            );

        } /* BeginSession() */

        public void EndSession()
        {

            // build up the transmitted data

            // v1.0
            //this.TransmitData(
            //    new CAN.Msg()
            //    {
            //        info = new CAN.MsgInfo()
            //        {
            //            bEvent = (byte)CAN.EventType.APOLLON_EVENT_ENDSESSION,
            //            bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
            //        },
            //        payload = null
            //    }
            //);

            // v2.0 - raw
            this.TransmitRawData(
                new byte[] {
                    (byte)CAN.EventType.APOLLON_EVENT_ENDSESSION
                }
            );

        } /* EndSession() */

        public void BeginTrial()
        {

            // build up the transmitted data

            // v1.0
            //this.TransmitData(
            //    new CAN.Msg()
            //    {
            //        info = new CAN.MsgInfo()
            //        {
            //            bEvent = (byte)CAN.EventType.APOLLON_EVENT_BEGINTRIAL,
            //            bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
            //        },
            //        payload = null
            //    }
            //);

            // v2.0 - raw
            this.TransmitRawData(
                new byte[] {
                    (byte)CAN.EventType.APOLLON_EVENT_BEGINTRIAL
                }
            );

        } /* EndSession() */

        public void EndTrial()
        {

            // build up the transmitted data

            // v1.0
            //this.TransmitData(
            //    new CAN.Msg()
            //    {
            //        info = new CAN.MsgInfo()
            //        {
            //            bEvent = (byte)CAN.EventType.APOLLON_EVENT_ENDTRIAL,
            //            bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
            //        },
            //        payload = null
            //    }
            //);

            // v2.0 - raw
            this.TransmitRawData(
                new byte[] {
                    (byte)CAN.EventType.APOLLON_EVENT_ENDTRIAL
                }
            );

        } /* EndSession() */

        public void Start(double AngularAcceleration, double AngularSpeedSaturation, double MaxStimDuration)
        {

            // build up the transmitted data

            // v1.0
            //this.TransmitData(
            //    new CAN.Msg()
            //    {
            //        info = new CAN.MsgInfo()
            //        {
            //            bEvent = (byte)CAN.EventType.APOLLON_EVENT_START,
            //            bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
            //        },
            //        payload = ApollonAbstractCANHandle.Serialize(
            //            new CAN.PayloadStartEvent()
            //            {
            //                dAngularAcceleration = AngularAcceleration,
            //                dAngularSpeedSaturation = AngularSpeedSaturation,
            //                dMaxStimDuration = MaxStimDuration
            //            }
            //        )
            //    }
            //);

            // v2.0

            // Prepare 6 bytes array to send (x3 times) :
            // 0x04 0x00 bytes representation of (AngularAcceleration)
            // 0x04 0x01 bytes representation of (AngularSpeedSaturation)
            // 0x04 0x02 bytes representation of (MaxStimDuration)

            byte[] 
                angular_acceleration_msg = new byte[1 + 1 + 4],
                angular_speed_saturation_msg = new byte[1 + 1 + 4],
                max_stim_duration_msg = new byte[1 + 1 + 4],
                acquittal_msg = new byte[1 + 1];

            // Allways 0x04

            System.Buffer.BlockCopy(
                src:
                    new byte[] {
                        (byte)CAN.EventType.APOLLON_EVENT_START
                    },
                srcOffset:
                    0,
                dst:
                    angular_acceleration_msg,
                dstOffset:
                    0,
                count: 
                    1
            );
            System.Buffer.BlockCopy(
                src:
                    new byte[] {
                        (byte)CAN.EventType.APOLLON_EVENT_START
                    },
                srcOffset:
                    0,
                dst:
                    angular_speed_saturation_msg,
                dstOffset:
                    0,
                count:
                    1
            );
            System.Buffer.BlockCopy(
                src:
                    new byte[] {
                        (byte)CAN.EventType.APOLLON_EVENT_START
                    },
                srcOffset:
                    0,
                dst:
                    max_stim_duration_msg,
                dstOffset:
                    0,
                count:
                    1
            );
            System.Buffer.BlockCopy(
                src:
                    new byte[] {
                        (byte)CAN.EventType.APOLLON_EVENT_START
                    },
                srcOffset:
                    0,
                dst:
                    acquittal_msg,
                dstOffset:
                    0,
                count:
                    1
            );

            // Convert args types to their bytes representation

            System.Buffer.BlockCopy(
                src:
                    new byte[] {
                        (byte)CAN.EventArgType.APOLLON_EVENT_ARG_ANGULAR_ACCELERATION // 0x00 : it's accel
                    },
                srcOffset:
                    0,
                dst:
                    angular_acceleration_msg,
                dstOffset:
                    1,
                count:
                    1
            );
            System.Buffer.BlockCopy(
                src:
                    new byte[] {
                        (byte)CAN.EventArgType.APOLLON_EVENT_ARG_ANGULAR_SPEED_SATURATION // 0x01 : it's speed
                    },
                srcOffset:
                    0,
                dst:
                    angular_speed_saturation_msg,
                dstOffset:
                    1,
                count:
                    1
            );
            System.Buffer.BlockCopy(
                src:
                    new byte[] {
                        (byte)CAN.EventArgType.APOLLON_EVENT_ARG_MAX_STIM_DURATION // 0x02 : it's duration
                    },
                srcOffset:
                    0,
                dst:
                    max_stim_duration_msg,
                dstOffset:
                    1,
                count:
                    1
            );
            System.Buffer.BlockCopy(
                src:
                    new byte[] {
                        (byte)CAN.EventArgType.APOLLON_EVENT_ARG_ACQUITTAL // 0x04 : it's start acq
                    },
                srcOffset:
                    0,
                dst:
                    acquittal_msg,
                dstOffset:
                    1,
                count:
                    1
            );
            
            // Convert args values to their bytes representation

            System.Buffer.BlockCopy(
                src:
                    System.BitConverter.GetBytes(
                        System.Convert.ToSingle(
                            AngularAcceleration
                        )
                    ),
                srcOffset:
                    0,
                dst:
                    angular_acceleration_msg,
                dstOffset:
                    2,
                count:
                    4
            );
            System.Buffer.BlockCopy(
                src:
                    System.BitConverter.GetBytes(
                        System.Convert.ToSingle(
                            AngularSpeedSaturation
                        )
                    ),
                srcOffset:
                    0,
                dst:
                    angular_speed_saturation_msg,
                dstOffset:
                    2,
                count:
                    4
            );
            System.Buffer.BlockCopy(
                src:
                    System.BitConverter.GetBytes(
                        System.Convert.ToSingle(
                            MaxStimDuration
                        )
                    ),
                srcOffset:
                    0,
                dst:
                    max_stim_duration_msg,
                dstOffset:
                    2,
                count:
                    4
            );

            // send messages
            //System.Threading.Thread.Sleep(10); //Wait 10ms just to be sure can bus transmition is complete
            this.TransmitRawData(angular_acceleration_msg);
            this.TransmitRawData(angular_speed_saturation_msg);
            this.TransmitRawData(max_stim_duration_msg);
            this.TransmitRawData(acquittal_msg);

        } /* EndSession() */

        public void Stop()
        {

            // build up the transmitted data

            // v1.0
            //this.TransmitData(
            //    new CAN.Msg()
            //    {
            //        info = new CAN.MsgInfo()
            //        {
            //            bEvent = (byte)CAN.EventType.APOLLON_EVENT_STOP,
            //            bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
            //        },
            //        payload = null
            //    }
            //);

            // v2.0 - raw
            this.TransmitRawData(
                new byte[] {
                    (byte)CAN.EventType.APOLLON_EVENT_STOP
                }
            );

        } /* EndSession() */

        public void Reset()
        {

            // build up the transmitted data

            // v1.0
            //this.TransmitData(
            //    new CAN.Msg()
            //    {
            //        info = new CAN.MsgInfo()
            //        {
            //            bEvent = (byte)CAN.EventType.APOLLON_EVENT_RESET,
            //            bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
            //        },
            //        payload = null
            //    }
            //);

            // v2.0 - raw
            this.TransmitRawData(
                new byte[] {
                    (byte)CAN.EventType.APOLLON_EVENT_RESET
                }
            );

        } /* EndSession() */

        #endregion

        // ctor
        public ActiveSeatHandle()
            : base()
        {

            //this.m_handleID = ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle;
        }

    } /* class ApollonActiveSeatHandle */

} /* namespace Labsim.apollon.backend */
