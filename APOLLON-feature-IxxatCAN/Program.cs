using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APOLLON_TestCAN
{
    class Program
    {
        static void Main(string[] args)
        {

            Labsim.apollon.backend.handle.ApollonActiveSeatHandle handle 
                = new Labsim.apollon.backend.handle.ApollonActiveSeatHandle();

            handle.onHandleActivationRequested();

            handle.BeginSession();

            for(uint i = 0; i < 5; ++i)
            {

                handle.BeginTrial();

                handle.Start(0.1, 1.0, 10000.0);

                handle.Stop();

                handle.Reset();

                handle.EndTrial();
            }

            handle.EndSession();

            handle.onHandleDeactivationRequested();
        }
    }
}
