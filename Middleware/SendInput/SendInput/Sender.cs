using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using Microsoft.Surface.Core;

namespace WindowsFormsApplication1
{
    class Sender
    {
        private int port;
        private TcpListener listener;
        private Socket soc;
        private Stream st;
        private StreamWriter sw;
        private StreamReader sr;
        private bool canSend = true;

        public Sender(int tmpPort) {
            port = tmpPort;
            IPAddress ipAddress = Dns.Resolve("127.0.0.1").AddressList[0];
            listener = new TcpListener(ipAddress, port);
            listener.Start();
            doWork();
        }

        private void doWork()
        {
                soc = listener.AcceptSocket(); // blocks
                st = new NetworkStream(soc);
                sw = new StreamWriter(st);
                sw.AutoFlush = true; // enable automatic flushing
                sr = new StreamReader(st);
        }

        public void doSend(TouchPoint tp)
        {
            if (!canSend)
            {
                Debug.WriteLine(sr.ReadLine());
                canSend = true;
            }
                canSend = false;
                //ID=16842750, Position=(345,9481, 319,9756), CenterPosition=(345,9481, 319,9756), Type=Tag (Schema=0x00000000; Series=0x0000000000000000; ExtendedValue=0x0000000000000000; Value=0x0000000000000000)
                sw.WriteLine(tp.Id);
                sw.WriteLine(tp.Tag.Value);
                sw.WriteLine((int)tp.X);
                sw.WriteLine((int)tp.Y);
                sw.WriteLine((int)(100*tp.Orientation));

        }

        public void deleteSend(TouchPoint tp)
        {
            sw.WriteLine(tp.Tag.Value);
        }
    }
}
