using System;
using System.IO;
using System.IO.Pipes;

namespace Labsim.apollon.feature.IxxatCAN
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {

                // instantiate backend implementation
                AbstractCANHandle handle = new handle.ActiveSeatHandle();
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : instantiated generic CANHandle"
                );
                
                // initialize it 
                handle.SelectDevice();
                handle.InitSocket(0);
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : backend CANHandle intialized"
                );

                using (
                    PipeStream pipeClient 
                        = new AnonymousPipeClientStream(PipeDirection.In, args[0])
                ) {

                    // info
                    Console.WriteLine(
                        DateTime.Now.ToString("HH:mm:ss.ffffff")
                        + " - [Apollon-feature-IxxatCAN-client] -- INFO : Input pipe initialized, current TransmissionMode: {0}.",
                       pipeClient.TransmissionMode
                    );

                    using (
                        StreamReader sr = new StreamReader(pipeClient)
                    ) {
                        
                        // a buffer :) 
                        string buffer;

                        // some status boolean
                        bool bEndSession = false;
                        
                        // Wait for 'messages' from the unity app server.
                        do
                        {

                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-client] -- INFO : Wait for messages..."
                            );
                            buffer = sr.ReadLine();

                            if(buffer.StartsWith("BeginSession"))
                            {
                                Console.WriteLine(
                                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [BeginSession]."
                                );
                                (handle as handle.ActiveSeatHandle).BeginSession();
                            }
                            else if(buffer.StartsWith("BeginTrial"))
                            {
                                Console.WriteLine(
                                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [BeginTrial]."
                                );
                                (handle as handle.ActiveSeatHandle).BeginTrial();
                            }
                            else if(buffer.StartsWith("Start"))
                            {
                                System.Double
                                    /* 1st - rad/s^2 (SI) */
                                    dAngularAcceleration 
                                        = System.Double.Parse(sr.ReadLine()),
                                    /* 2nd - rad/s (SI) */
                                    dAngularSpeedSaturation
                                        = System.Double.Parse(sr.ReadLine()),
                                    /* 3rd - ms (SI) */
                                    dMaxStimDuration
                                        = System.Double.Parse(sr.ReadLine());

                                Console.WriteLine(
                                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [Start] with args [dAngularAcceleration:"
                                    + dAngularAcceleration
                                    + "], [dAngularSpeedSaturation:"
                                    + dAngularSpeedSaturation
                                    + "], [dMaxStimDuration:"
                                    + dMaxStimDuration
                                    + "] !"
                                );
                              
                                (handle as handle.ActiveSeatHandle).Start(
                                    dAngularAcceleration,
                                    dAngularSpeedSaturation,
                                    dMaxStimDuration
                                );

                            }
                            else if (buffer.StartsWith("Stop"))
                            {
                                Console.WriteLine(
                                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [Stop]."
                                );
                                (handle as handle.ActiveSeatHandle).Stop();
                            }
                            else if (buffer.StartsWith("Reset") )
                            {
                                Console.WriteLine(
                                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [Reset]."
                                );
                                (handle as handle.ActiveSeatHandle).Reset();
                            }
                            else if (buffer.StartsWith("EndTrial") )
                            {
                                Console.WriteLine(
                                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [EndTrial]."
                                );
                                (handle as handle.ActiveSeatHandle).EndTrial();
                            }
                            else if (buffer.StartsWith("EndSession") )
                            {
                                Console.WriteLine(
                                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [EndSession]."
                                );
                                (handle as handle.ActiveSeatHandle).EndSession();
                                bEndSession = true;
                            }

                        }
                        while (!bEndSession);


                    } /* using */
                } /* using */

            }
            else
            {

                Console.Write(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-client] -- WARNING : This executable is not intended to be launched without any CLI args..."
                );
                Console.ReadLine();
                return;

            } /* if() */

            // end prompt
            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-feature-IxxatCAN-client] -- INFO : THE END."
            );
            Console.ReadLine();

        } /* static void Main() */
    } /* class Program*/
}/* namespace */
