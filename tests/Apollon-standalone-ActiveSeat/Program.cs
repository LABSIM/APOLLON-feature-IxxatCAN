namespace Labsim.apollon
{
    class Program
    {
        static void Main(string[] args)
        {
            // simple test 

            feature.IxxatCAN.AbstractCANHandle handle = null;

            // init
            handle = new feature.IxxatCAN.handle.ActiveSeatHandle();
            handle.SelectDevice();
            handle.InitSocket(0);

            (handle as feature.IxxatCAN.handle.ActiveSeatHandle).BeginSession();

            for (uint i = 0; i < 10; ++i)
            {

                (handle as feature.IxxatCAN.handle.ActiveSeatHandle).BeginTrial();

                (handle as feature.IxxatCAN.handle.ActiveSeatHandle).Start(
                    AngularAcceleration:
                        /* rad/s^2 (SI) */ 6.8 + i,
                    DeltaStimDuration:
                        /* s (SI)       */ -2.5 + i,
                    MaxStimDuration:
                        /* ms (SI)      */ 2.70 + i
                );

                (handle as feature.IxxatCAN.handle.ActiveSeatHandle).Stop();

                (handle as feature.IxxatCAN.handle.ActiveSeatHandle).Reset();

                (handle as feature.IxxatCAN.handle.ActiveSeatHandle).EndTrial();
            }

            (handle as feature.IxxatCAN.handle.ActiveSeatHandle).EndSession();

            //handle.onHandleDeactivationRequested();
        }
    }
}
