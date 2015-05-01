
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
        const int m4_area_obj = 1;
        const int m4_hsv_hlow = 1;
        const int m4_hsv_hhi = 1;
        const int m4_hsv_vlow = 1;
        const int m4_hsv_vhi = 1;
        const int m4_hsv_slow = 1;
        const int m4_hsv_shi = 1;

        void mission4_Click(object sender, EventArgs e)
        {
            // TODO: verificar ligacao, etc

            mDrone.droneMudarCamara(Drone.DroneCamera.INFERIOR);

            mDrone.droneDescolar();

            do
            {       // TODO: verificar vZ ? neg as docs : pos as logic
                mDrone.droneMoverPRO(0f, 0f, 1f, 0f);

                EstadoDrone();

            } while (mDrone.droneObterAltitude() < 3.0f);

            do
            {
                mDrone.droneMoverPRO(0f, 0f, -1f, 0f);

                EstadoDrone();
                                                  // TODO: testar possibilidade de reconhecer area a esta altura
            } while (mDrone.droneObterAltitude() > 1.5f);

            while (true)
            {
                while (true)
                {
                    mDrone.imageChange += new Drone.droneImageHandler(atualizarImagemOnly);

                    if (ImageFrame.Bitmap != null)
                        break;
                }

                imgsize.X = ImageFrame.Width;
                imgsize.Y = ImageFrame.Height;

                // TODO: better way to check HUE, SAT and VAL
                // TODO: select HSV parameters for mission 4

                img1 = ProImg.HsvROI(ImageFrame, m4_hsv_hlow, m4_hsv_hhi, m4_hsv_slow, m4_hsv_shi, m4_hsv_vlow, m4_hsv_vhi, true, true, true, true);

                img1 = img1.SmoothGaussian(9);

                // TODO: select area parameter for mission 4 : function of height?

                ProImg.Deteccao_Circulo(img1, ImageFrame, (m4_area_obj*mDrone.droneObterAltitude()));

                droneTraj.ObjectTracking2(ProImg.Obj_centroid, imgsize);

                mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0.01f, droneTraj.Vel_rot_z_drone);


                //refresh form
                pictureBox1.Image = ImageFrame.Bitmap;
                pictureBox2.Image = img1.Bitmap;
                EstadoDrone();

                if (mDrone.droneObterAltitude() < 0.01f)
                    break;
            }

            resetDroneTrajVal();
            mDrone.droneMoverPRO(0, 0, 0, 0);
            mDrone.droneAterrar();

            return;
        }

        void resetDroneTrajVal()
        {
            droneTraj.Vel_rot_z_drone = 0;
            droneTraj.Vel_x_drone= 0;
            droneTraj.Vel_y_drone= 0;
            droneTraj.Vel_z_drone = 0;

            return;
        }


        void atualizarImagemOnly(object sender, droneImageChangeEventArgs data)
        {
            ImageFrame.Bitmap = data.droneImagem;
        }
    }
}