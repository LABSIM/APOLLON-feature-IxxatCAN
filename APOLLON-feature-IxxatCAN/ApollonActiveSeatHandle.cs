// avoid namespace pollution
namespace Labsim.apollon.backend.handle
{

    public class ApollonActiveSeatHandle
        : ApollonAbstractCANHandle
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

            public enum FlagType : byte
            {

                APOLLON_FLAG_NONE = 0, // empty flag
                APOLLON_FLAG_UNKNOWN

            } /* enum FlagType */

            #endregion

            #region messages

            [System.Runtime.InteropServices.StructLayout(
                System.Runtime.InteropServices.LayoutKind.Sequential,
                CharSet = System.Runtime.InteropServices.CharSet.Ansi,
                Pack = 1
            )]
            public struct MsgInfo
            {

                public byte bEvent; // type (see EventType::APOLLON_EVENT_* constants)
                public byte bFlags; // additional flags (see FlagType::APOLLON_FLAG_* constants)

            }; /* struct MsgInfo */

            [System.Runtime.InteropServices.StructLayout(
                System.Runtime.InteropServices.LayoutKind.Sequential,
                CharSet = System.Runtime.InteropServices.CharSet.Ansi,
                Pack = 1
            )]
            public struct Msg
            {

                public MsgInfo info; // message information

                [System.Runtime.InteropServices.MarshalAs(
                    System.Runtime.InteropServices.UnmanagedType.ByValArray,
                    ArraySubType = System.Runtime.InteropServices.UnmanagedType.U1
                )]
                public byte[] payload; // payload type is dependant of MsgInfo.bEvent

            }; /* struct Msg */

            #endregion

            #region payloads

            [System.Runtime.InteropServices.StructLayout(
                System.Runtime.InteropServices.LayoutKind.Sequential,
                CharSet = System.Runtime.InteropServices.CharSet.Ansi,
                Pack = 1
            )]
            public struct PayloadStartEvent
            {

                public System.Double dAngularAcceleration;      // rad/s^2 (SI)
                public System.Double dAngularSpeedSaturation;   // rad/s (SI)
                public System.Double dMaxStimDuration;          // ms (SI)

            }; /* struct PayloadStartEvent */

            #endregion

        }; /* CAN */

        #endregion 

        #region CAN event implementation

        public void BeginSession()
        {

            // build up the transmitted data
            this.TransmitData(
                new CAN.Msg()
                {
                    info = new CAN.MsgInfo()
                    {
                        bEvent = (byte)CAN.EventType.APOLLON_EVENT_BEGINSESSION,
                        bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
                    },
                    payload = null
                }
            );

        } /* BeginSession() */

        public void EndSession()
        {

            // build up the transmitted data
            this.TransmitData(
                new CAN.Msg()
                {
                    info = new CAN.MsgInfo()
                    {
                        bEvent = (byte)CAN.EventType.APOLLON_EVENT_ENDSESSION,
                        bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
                    },
                    payload = null
                }
            );

        } /* EndSession() */

        public void BeginTrial()
        {

            // build up the transmitted data
            this.TransmitData(
                new CAN.Msg()
                {
                    info = new CAN.MsgInfo()
                    {
                        bEvent = (byte)CAN.EventType.APOLLON_EVENT_BEGINTRIAL,
                        bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
                    },
                    payload = null
                }
            );

        } /* EndSession() */

        public void EndTrial()
        {

            // build up the transmitted data
            this.TransmitData(
                new CAN.Msg()
                {
                    info = new CAN.MsgInfo()
                    {
                        bEvent = (byte)CAN.EventType.APOLLON_EVENT_ENDTRIAL,
                        bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
                    },
                    payload = null
                }
            );

        } /* EndSession() */

        public void Start(double AngularAcceleration, double AngularSpeedSaturation, double MaxStimDuration)
        {

            // build up the transmitted data
            this.TransmitData(
                new CAN.Msg()
                {
                    info = new CAN.MsgInfo()
                    {
                        bEvent = (byte)CAN.EventType.APOLLON_EVENT_START,
                        bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
                    },
                    payload = ApollonAbstractCANHandle.Serialize(
                        new CAN.PayloadStartEvent()
                        {
                            dAngularAcceleration = AngularAcceleration,
                            dAngularSpeedSaturation = AngularSpeedSaturation,
                            dMaxStimDuration = MaxStimDuration
                        }
                    )
                }
            );

        } /* EndSession() */

        public void Stop()
        {

            // build up the transmitted data
            this.TransmitData(
                new CAN.Msg()
                {
                    info = new CAN.MsgInfo()
                    {
                        bEvent = (byte)CAN.EventType.APOLLON_EVENT_STOP,
                        bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
                    },
                    payload = null
                }
            );

        } /* EndSession() */

        public void Reset()
        {

            // build up the transmitted data
            this.TransmitData(
                new CAN.Msg()
                {
                    info = new CAN.MsgInfo()
                    {
                        bEvent = (byte)CAN.EventType.APOLLON_EVENT_RESET,
                        bFlags = (byte)CAN.FlagType.APOLLON_FLAG_NONE
                    },
                    payload = null
                }
            );

        } /* EndSession() */

        #endregion

        // ctor
        public ApollonActiveSeatHandle()
            : base()
        {
            //this.m_handleID = ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle;
        }

    } /* class ApollonActiveSeatHandle */

} /* namespace Labsim.apollon.backend */
