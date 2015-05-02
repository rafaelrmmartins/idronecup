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

            } while(mDrone.droneObterAltitude()<0.50f);

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

                img1 = ProImg.HsvROI(ImageFrame, m4_hsv_hlow, m4_hsv_hhi, m4_hsv_slow, m4_hsv_shi, m4_hsv_vlow, m4_hsv_vhi, m4_hsv_h, m4_hsv_s, m4_hsv_v, m4_hsv_invert);

                img1 = img1.SmoothGaussian(9);

                // TODO: select area parameter for mission 4 : function of height?

                ProImg.Deteccao_Circulo(img1, ImageFrame, 100);


                //outputs

                mDrone.droneMoverPRO(0.25f, droneTraj.Vel_y_drone, 0f, droneTraj.Vel_rot_z_drone);

                //refresh form
                pictureBox1.Image = ImageFrame.Bitmap;
                pictureBox2.Image = img1.Bitmap;

                EstadoDrone();

            }

        }

    }
}
