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
        public void Mission3()
        {
            
        }

        void Button15Click(object sender, System.EventArgs e)
        {
            int i = 0;
            //Connects and starts drone
            //mDrone.droneLigar();
            if (mDrone.droneObterAltitude() == 0)
            {
                mDrone.droneDescolar();
               while (i<30){
                   i++;
                   mDrone.droneAvancar(0.2f);
               }
            }

            //changes camera
            mDrone.droneMudarCamara(Drone.DroneCamera.INFERIOR);
            do {
                //mDrone.imageChange += new Drone.droneImageHandler(atualizarImagem);
                //mDrone.droneAvancar(0.3f);
            }while(true);


        }
		
    }
}
