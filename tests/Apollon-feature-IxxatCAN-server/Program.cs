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
            // Set the TcpListener on port 13000.
            Int32 port = 8888;
            System.Net.IPAddress localAddr = System.Net.IPAddress.Loopback;

            // TcpListener server = new TcpListener(port);
            var server = new System.Net.Sockets.TcpListener(localAddr, port);
            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-feature-IxxatCAN-server] -- INFO : server created ["
                + localAddr 
                + ":"
                + port
                + "]."
            );

            // Start listening for client requests.
            server.Start();
            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-feature-IxxatCAN-server] -- INFO : server started"
            );

            // Perform a blocking call to accept requests.
            // You could also use server.AcceptSocket() here.
            System.Net.Sockets.TcpClient client = server.AcceptTcpClient();

            //System.Net.Sockets.TcpClient client = null;
            //using (var acceptTask = server.AcceptTcpClientAsync())
            //{

            //    Console.WriteLine(
            //        DateTime.Now.ToString("HH:mm:ss.ffffff")
            //        + " - [Apollon-feature-IxxatCAN-server] -- INFO : server is waiting for connection, launch client."
            //    );

            //    // launch client process

            //    // wait for client completion 
            //    acceptTask.Wait();
            //    client = acceptTask.Result;

            //    Console.WriteLine(
            //        DateTime.Now.ToString("HH:mm:ss.ffffff")
            //        + " - [Apollon-feature-IxxatCAN-server] -- INFO : accepted client ["
            //        + client.Client.LocalEndPoint
            //        + "]."
            //    );
            //}

            // Get a stream object for reading and writing
            System.Net.Sockets.NetworkStream stream = client.GetStream();
            
            // begin
            stream.WriteByte(System.Convert.ToByte(messageID.BeginSession));
            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [BeginSession]."
            );

            // auto seed
            Random autoRand = new Random();

            // simple loop
            for (uint i = 0; i < 5; ++i)
            {

                // -------------------------------------------------------------------------------- //

                stream.WriteByte(System.Convert.ToByte(messageID.BeginTrial));
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [BeginTrial]."
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

                stream.WriteByte(System.Convert.ToByte(messageID.Start));
                stream.Write(System.BitConverter.GetBytes(dAngularAcceleration),0,8);
                stream.Write(System.BitConverter.GetBytes(dAngularSpeedSaturation),0,8);
                stream.Write(System.BitConverter.GetBytes(dMaxStimDuration),0,8);
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

                // -------------------------------------------------------------------------------- //

                stream.WriteByte(System.Convert.ToByte(messageID.Stop));
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [Stop]."
                );

                // -------------------------------------------------------------------------------- //

                stream.WriteByte(System.Convert.ToByte(messageID.Reset));
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [Reset]."
                );

                // -------------------------------------------------------------------------------- //

                stream.WriteByte(System.Convert.ToByte(messageID.EndTrial));
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [EndTrial]."
                );

                // -------------------------------------------------------------------------------- //

            } /* for() */

            stream.WriteByte(System.Convert.ToByte(messageID.EndSession));
            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-feature-IxxatCAN-server] -- INFO : sended [EndSession]."
            );

            // end
            stream.Close();
            server.Stop();
            Console.ReadLine();
            return 0;

        } /* static Main */

    } /* class Program */

}/* namespace */
