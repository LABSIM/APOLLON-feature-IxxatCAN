using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labsim.apollon
{
    class Program
    {
        static void Main(string[] args)
        {

            backend.handle.ApollonActiveSeatHandle handle 
                = new backend.handle.ApollonActiveSeatHandle();

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
