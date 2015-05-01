using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using iDroneCup;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;

namespace iDroneExemplos

{
    public partial class MainForm : Form
    {

        void Button15Click(object sender, System.EventArgs e)
        {
            //Connects and starts drone
            mDrone.droneLigar();
            mDrone.iDroneCup_ChangeWifiChannel(2);
            if (mDrone.droneObterAltitude() == 0)
                mDrone.droneDescolar();
            //changes camera
            mDrone.droneMudarCamara(Drone.DroneCamera.INFERIOR);



        }
		
    }
}
