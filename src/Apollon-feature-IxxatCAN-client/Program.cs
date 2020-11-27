using System;

namespace Labsim.apollon.feature.IxxatCAN
{
    class Program
    {
        enum messageID : short
        {
            NoMoreData = -1,
            BeginSession,
            EndSession,
            BeginTrial,
            EndTrial,
            Start,
            Stop,
            Reset
        }

        public static int Main(String[] args)
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

            // instantiate a client connection with the first non-ipv6 address
            Int32 port = 8888; // same as server
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("127.0.0.1", port);

            // some status boolean
            bool bEndSession = false;

            // Get a client stream for reading and writing.
            System.Net.Sockets.NetworkStream stream = client.GetStream();

            // double slot
            byte[] data = new byte[8];
            
            // until end session
            do
            {

                // Wait for 'messages' from the unity app server.
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-client] -- INFO : Wait for messages..."
                );

                // Read the batch of the TcpServer response bytes.
                switch((messageID)System.Convert.ToInt16(stream.ReadByte()))
                {
                    case messageID.BeginSession:
                        {
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [BeginSession]."
                            );
                            (handle as handle.ActiveSeatHandle).BeginSession();
                        }
                        break;
                    case messageID.BeginTrial:
                        {
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [BeginTrial]."
                            );
                            (handle as handle.ActiveSeatHandle).BeginTrial();
                        }
                        break;
                    case messageID.Start:
                        {
                            // 3 messages

                            /* 1st - rad/s^2 (SI) */
                            stream.Read(data, 0, 8);
                            System.Double dAngularAcceleration = System.BitConverter.ToDouble(data,0);

                            /* 2nd - rad/s (SI) */
                            stream.Read(data, 0, 8);
                            System.Double dAngularSpeedSaturation = System.BitConverter.ToDouble(data, 0);

                            /* 3rd - ms (SI) */
                            stream.Read(data, 0, 8);
                            System.Double dMaxStimDuration = System.BitConverter.ToDouble(data, 0);

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
                        break;
                    case messageID.Stop:
                        {
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [Stop]."
                            );
                            (handle as handle.ActiveSeatHandle).Stop();
                        }
                        break;
                    case messageID.Reset:
                        {
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [Reset]."
                            );
                            (handle as handle.ActiveSeatHandle).Reset();
                        }
                        break;
                    case messageID.EndTrial:
                        {
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [EndTrial]."
                            );
                            (handle as handle.ActiveSeatHandle).EndTrial();
                        }
                        break;
                    case messageID.EndSession:
                        {
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-client] -- INFO : received [EndSession]."
                            );
                            (handle as handle.ActiveSeatHandle).EndSession();
                            bEndSession = true;
                        }
                        break;
                    case messageID.NoMoreData:
                        {
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-client] -- INFO : waiting for fresh data."
                            );
                        }
                        break;
                    default:
                        {
                            Console.WriteLine(
                                DateTime.Now.ToString("HH:mm:ss.ffffff")
                                + " - [Apollon-feature-IxxatCAN-client] -- ERROR : received [UNKNOWN]."
                            );
                            bEndSession = true;
                        }
                        break;
                } /* switch() */
               
            } while (!bEndSession);

            // stop
            stream.Close();
            client.Close();
            return 0;

        } /* static Main */

    } /* class Program */
  
}/* namespace */
