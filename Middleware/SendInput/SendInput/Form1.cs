using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Surface.Core;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Sender SendNew;
        Sender SendUpdate;
        Sender SendDelete;
        public Form1()
        {
            InitializeComponent();
            SendNew = new Sender(1111);
            SendUpdate = new Sender(1112);
            SendDelete = new Sender(1113);
            InitializeSurfaceInput();

        }
        private void InitializeSurfaceInput()
        {
            //...

            // Create a target for surface input.
            TouchTarget touchTarget = new TouchTarget(IntPtr.Zero, EventThreadChoice.OnBackgroundThread);
            touchTarget.EnableInput();
            touchTarget.TouchDown += new EventHandler<TouchEventArgs>(NewInput);
            touchTarget.TouchMove += new EventHandler<TouchEventArgs>(UpdateInput);
            touchTarget.TouchUp += new EventHandler<TouchEventArgs>(DeleteInput);
        }

        private void NewInput(object sender, TouchEventArgs e)
        {
            TouchPoint tmp = e.TouchPoint;
            if (tmp.IsTagRecognized)
            {
                SendNew.doSend(tmp);
            }
           //ID=16842750, Position=(345,9481, 319,9756), CenterPosition=(345,9481, 319,9756), Type=Tag (Schema=0x00000000; Series=0x0000000000000000; ExtendedValue=0x0000000000000000; Value=0x0000000000000000)
        }
        private void UpdateInput(object sender, TouchEventArgs e)
        {
            TouchPoint tmp = e.TouchPoint;
            if (tmp.IsTagRecognized)
            {
                SendUpdate.doSend(tmp);
            }
        }
        private void DeleteInput(object sender, TouchEventArgs e)
        {
            TouchPoint tmp = e.TouchPoint;
            if (tmp.IsTagRecognized)
            {
                SendDelete.deleteSend(tmp);
            }
            //ID=16842750, Position=(345,9481, 319,9756), CenterPosition=(345,9481, 319,9756), Type=Tag (Schema=0x00000000; Series=0x0000000000000000; ExtendedValue=0x0000000000000000; Value=0x0000000000000000)
        }
    }
}
