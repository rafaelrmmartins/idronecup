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

        void Mission3_Click(object sender, System.EventArgs e)
        {
            mDrone.droneDescolar();
            mDrone.iDroneCup_Hover();

            resetDroneTrajVal();

            do {
                mDrone.droneMoverPRO(0.2f, 0f, -0.5f, 0f);

                EstadoDrone();

            } while(mDrone.droneObterAltitude()<0.75f);

            resetDroneTrajVal();

            mDrone.droneMudarCamara(Drone.DroneCamera.INFERIOR);

            while(true)
            {

                //processing

                while (true) //get new image
                {
                    mDrone.imageChange += new Drone.droneImageHandler(atualizarImagemOnly);

                    if (ImageFrame.Bitmap != null)
                        break;
                }

                imgsize.X = ImageFrame.Width;
                imgsize.Y = ImageFrame.Height;

                img1 = ProImg.Deteccao_Linha(ImageFrame);

                droneTraj.ObjectTracking3(ProImg.Obj_centroid, imgsize);
                
                //outputs

                mDrone.droneMoverPRO(0.25f, droneTraj.Vel_y_drone, 0f, droneTraj.Vel_rot_z_drone);

                //refresh form
                pictureBox1.Image = ImageFrame.Bitmap;
                pictureBox2.Image = img1.Bitmap;

                EstadoDrone();

                //condiçao de final percurso

                img1 = ProImg.HsvROI(ImageFrame, 0, 0, 0, 0, 0, 66, false, false, true, false);

                img1 = img1.SmoothGaussian(9);

                // TODO: select area parameter for mission 4 : function of height?

                ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

                if ((ProImg.Obj_centroid.X == -1) || (ProImg.Obj_centroid.Y == -1))
                {
                    resetDroneTrajVal();
                    break;
                }

            }
            while(true) //aterragem
            {
                while (true) //get new image
                {
                    mDrone.imageChange += new Drone.droneImageHandler(atualizarImagemOnly);

                    if (ImageFrame.Bitmap != null)
                        break;
                }

                imgsize.X = ImageFrame.Width;
                imgsize.Y = ImageFrame.Height;

                // TODO: better way to check HUE, SAT and VAL
                img1 = ProImg.HsvROI(ImageFrame, 0, 0, 0, 0, 0, 66, false, false, true, false);

                img1 = img1.SmoothGaussian(9);

                // TODO: select area parameter for mission 4 : function of height?

                ProImg.Deteccao_Circulo(img1, ImageFrame, 100);
                
                droneTraj.ObjectTracking2(ProImg.Obj_centroid, imgsize);
 
                //output
                mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0f, droneTraj.Vel_rot_z_drone);

                //refresh form
                pictureBox1.Image = ImageFrame.Bitmap;
                pictureBox2.Image = img1.Bitmap;

                EstadoDrone();

                //break out
                if ((Math.Abs(ProImg.Obj_centroid.X - (imgsize.X/2)) < 10) && (Math.Abs(ProImg.Obj_centroid.Y - (imgsize.Y/2)) < 10))
                    break;
            }

            resetDroneTrajVal();
            mDrone.droneAterrar();

            return;
        }

    }
}
