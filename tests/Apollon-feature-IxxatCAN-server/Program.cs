using System;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;

namespace Labsim.apollon.feature.IxxatCAN
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-feature-IxxatCAN-server] -- INFO : Server mode"
            );

            // our child client process
            Process pipeClient = new Process();

            using (
                AnonymousPipeServerStream pipeServer
                    = new AnonymousPipeServerStream(
                        PipeDirection.Out,
                        HandleInheritability.Inheritable
                    )
            ) {

                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-server] -- INFO : Pipe server instantiated ! Current TransmissionMode: {0}.",
                    pipeServer.TransmissionMode
                );

                // Pass the client process a handle to the server.
                pipeClient.StartInfo.FileName = "Apollon-feature-IxxatCAN-client.exe";
                pipeClient.StartInfo.Arguments = pipeServer.GetClientHandleAsString();
                pipeClient.StartInfo.UseShellExecute = false;

                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-server] -- INFO : Pipe client instantiated & initialized ! starting it."
                );

                // start client process & dispose local server copy
                pipeClient.Start();
                pipeServer.DisposeLocalCopyOfClientHandle();
                
                // simulate the "classical experiment scenario"

                // encapsulate
                try
                {
                    // Read user input and send that to the client process.
                    using (StreamWriter sw = new StreamWriter(pipeServer))
                    {
                        // mark as sutoFlush
                        sw.AutoFlush = true;
                    
                        // Send a 'sync message' and wait for client to receive it. 
                        sw.WriteLine("BeginSession");
                        Console.WriteLine(
                            DateTime.Now.ToString("HH:mm:ss.ffffff")
                            + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [BeginSession]."
                        );
                        pipeServer.WaitForPipeDrain();
                        Console.WriteLine(
                            DateTime.Now.ToString("HH:mm:ss.ffffff")
                            + " - [Apollon-feature-IxxatCAN-server] -- INFO : pipe drained."
                        );
                        
                        // auto seed
                        Random autoRand = new Random();

                        // simple loop
                        for (uint i = 0; i < 5; ++i)
                        {

                            // -------------------------------------------------------------------------------- //

                            sw.WriteLine("BeginTrial");
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [BeginTrial]."
                            );
                            pipeServer.WaitForPipeDrain();
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : pipe drained."
                            );

                            // -------------------------------------------------------------------------------- //

                            System.Double
                                /* 1st - rad/s^2 (SI) */
                                dAngularAcceleration
                                    = autoRand.NextDouble(),
                                /* 2nd - rad/s (SI) */
                                dAngularSpeedSaturation
                                    = autoRand.NextDouble(),
                                /* 3rd - ms (SI) */
                                dMaxStimDuration
                                    = autoRand.NextDouble();

                            sw.WriteLine("Start");
                            sw.WriteLine(dAngularAcceleration);
                            sw.WriteLine(dAngularSpeedSaturation);
                            sw.WriteLine(dMaxStimDuration);
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [Start] with args [dAngularAcceleration:"
                                + dAngularAcceleration
                                + "], [dAngularSpeedSaturation:"
                                + dAngularSpeedSaturation
                                + "], [dMaxStimDuration:"
                                + dMaxStimDuration
                                + "] !"
                            );
                            pipeServer.WaitForPipeDrain();
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : pipe drained."
                            );

                            // -------------------------------------------------------------------------------- //

                            sw.WriteLine("Stop");
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [Stop]."
                            );
                            pipeServer.WaitForPipeDrain();
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : pipe drained."
                            );

                            // -------------------------------------------------------------------------------- //

                            sw.WriteLine("Reset");
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [Reset]."
                            );
                            pipeServer.WaitForPipeDrain();
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : pipe drained."
                            );

                            // -------------------------------------------------------------------------------- //

                            sw.WriteLine("EndTrial");
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [EndTrial]."
                            );
                            pipeServer.WaitForPipeDrain();
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-server] -- INFO : pipe drained."
                            );

                            // -------------------------------------------------------------------------------- //

                        } /* for() */

                        sw.WriteLine("EndSession");
                        Console.WriteLine(
                            DateTime.Now.ToString("HH:mm:ss.ffffff")
                            + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [EndSession]."
                        );
                        pipeServer.WaitForPipeDrain();
                        Console.WriteLine(
                            DateTime.Now.ToString("HH:mm:ss.ffffff")
                            + " - [Apollon-feature-IxxatCAN-server] -- INFO : pipe drained."
                        );

                    } /* using */
                }
                catch (IOException e)
                {

                    // Catch the IOException that is raised if the pipe is broken
                    // or disconnected.
                    Console.WriteLine(
                        DateTime.Now.ToString("HH:mm:ss.ffffff")
                        + " - [Apollon-feature-IxxatCAN-server] -- ERROR : {0}", 
                        e.Message
                    );

                } /* try() */

            } /* using */

            //close
            pipeClient.WaitForExit();
            pipeClient.Close();
            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-feature-IxxatCAN-server] -- INFO : Client quit. Server terminating."
            );
            
        } /* static void Main() */

    } /* class Program*/

}/* namespace */
