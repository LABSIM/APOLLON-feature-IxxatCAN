using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labsim.apollon.feature.IxxatCAN
{
    class Program
    {
        static void Main(string[] args)
        {
            // simple test 

            AbstractCANHandle handle = null;

            // init
            handle = new handle.ActiveSeatHandle();
            handle.SelectDevice();
            handle.InitSocket(0);
         
            (handle as handle.ActiveSeatHandle).BeginSession();

            for (uint i = 0; i < 5; ++i)
            {

                (handle as handle.ActiveSeatHandle).BeginTrial();

                (handle as handle.ActiveSeatHandle).Start(
                    AngularAcceleration:
                        /* rad/s^2 (SI) */ 0.025,
                    AngularSpeedSaturation:
                        /* rad/s (SI)   */ 1.0,
                    MaxStimDuration:
                        /* ms (SI)      */ 1000.25
                );

                (handle as handle.ActiveSeatHandle).Stop();

                (handle as handle.ActiveSeatHandle).Reset();

                (handle as handle.ActiveSeatHandle).EndTrial();
            }

            (handle as handle.ActiveSeatHandle).EndSession();

            //handle.onHandleDeactivationRequested();
        }
    }
}
