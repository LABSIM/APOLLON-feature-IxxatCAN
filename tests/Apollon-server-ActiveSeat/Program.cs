using System;

namespace Labsim.apollon
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
            Int32 service_port = 8888;
            System.Net.IPEndPoint serviceEndPoint
                = new System.Net.IPEndPoint(
                    System.Net.IPAddress.Loopback,
                    service_port
                );
            //System.Net.IPAddress localAddr = System.Net.IPAddress.Loopback;

            // TcpListener server = new TcpListener(port);
            var server = new System.Net.Sockets.TcpListener(serviceEndPoint);
            //var server = new System.Net.Sockets.TcpListener(localAddr, port);
            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-server-ActiveSeat] -- INFO : server created ["
                + serviceEndPoint
                + "]."
            );

            // Start listening for client requests.
            server.Start();
            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-server-ActiveSeat] -- INFO : server started"
            );

            // Perform a blocking call to accept requests.
            // You could also use server.AcceptSocket() here.
            System.Net.Sockets.TcpClient client = server.AcceptTcpClient();

            //System.Net.Sockets.TcpClient client = null;
            //using (var acceptTask = server.AcceptTcpClientAsync())
            //{

            //    Console.WriteLine(
            //        DateTime.Now.ToString("HH:mm:ss.ffffff")
            //        + " - [Apollon-server-ActiveSeat] -- INFO : server is waiting for connection, launch client."
            //    );

            //    // launch client process

            //    // wait for client completion 
            //    acceptTask.Wait();
            //    client = acceptTask.Result;

            //    Console.WriteLine(
            //        DateTime.Now.ToString("HH:mm:ss.ffffff")
            //        + " - [Apollon-server-ActiveSeat] -- INFO : accepted client ["
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
                + " - [Apollon-server-ActiveSeat] -- INFO : sended [BeginSession]."
            );

            // auto seed
            Random autoRand = new Random();

            // simple loop
            for (uint i = 0; i < 10; ++i)
            {

                // -------------------------------------------------------------------------------- //

                stream.WriteByte(System.Convert.ToByte(messageID.BeginTrial));
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-server-ActiveSeat] -- INFO : sended [BeginTrial]."
                );

                // -------------------------------------------------------------------------------- //

                System.Double
                    /* 1st - rad/s^2 (SI) */
                    dAngularAcceleration
                        = autoRand.NextDouble() - 0.5,
                    /* 2nd - rad/s (SI) */
                    dAngularSpeedSaturation
                        = autoRand.NextDouble() - 0.5,
                    /* 3rd - ms (SI) */
                    dMaxStimDuration
                        = autoRand.NextDouble() - 0.5;

                stream.WriteByte(System.Convert.ToByte(messageID.Start));
                stream.Write(System.BitConverter.GetBytes(dAngularAcceleration), 0, 8);
                stream.Write(System.BitConverter.GetBytes(dAngularSpeedSaturation), 0, 8);
                stream.Write(System.BitConverter.GetBytes(dMaxStimDuration), 0, 8);
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-server-ActiveSeat] -- INFO : sended [Start] with args [dAngularAcceleration:"
                    + dAngularAcceleration
                    + "], [dAngularSpeedSaturation:"
                    + dAngularSpeedSaturation
                    + "], [dMaxStimDuration:"
                    + dMaxStimDuration
                    + "] !"
                );

                System.Threading.Thread.Sleep(1000); //Wait 10ms just to be sure can bus transmition is complete

                // -------------------------------------------------------------------------------- //

                stream.WriteByte(System.Convert.ToByte(messageID.Stop));
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-server-ActiveSeat] -- INFO : sended [Stop]."
                );

                // -------------------------------------------------------------------------------- //

                stream.WriteByte(System.Convert.ToByte(messageID.Reset));
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-server-ActiveSeat] -- INFO : sended [Reset]."
                );

                // -------------------------------------------------------------------------------- //

                stream.WriteByte(System.Convert.ToByte(messageID.EndTrial));
                Console.WriteLine(
                    DateTime.Now.ToString("HH:mm:ss.ffffff")
                    + " - [Apollon-server-ActiveSeat] -- INFO : sended [EndTrial]."
                );

                // -------------------------------------------------------------------------------- //

            } /* for() */

            stream.WriteByte(System.Convert.ToByte(messageID.EndSession));
            Console.WriteLine(
                DateTime.Now.ToString("HH:mm:ss.ffffff")
                + " - [Apollon-server-ActiveSeat] -- INFO : sended [EndSession]."
            );

            // end
            stream.Close();
            server.Stop();
            Console.ReadLine();
            return 0;

        } /* static Main */

    } /* class Program */

}/* namespace */
