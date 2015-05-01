
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
        
        void mission4_Click(object sender, EventArgs e)
        {
            // TODO: verificar ligacao, etc

            mDrone.droneMudarCamara(Drone.DroneCamera.INFERIOR);

            mDrone.droneDescolar();

            do
            {
                mDrone.droneMoverPRO(0f, 0f, -1f, 0f);

                EstadoDrone();

            } while (mDrone.droneObterAltitude() < 3.0f);

            do
            {
                mDrone.droneMoverPRO(0f, 0f, 1f, 0f);

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

                //void Centra_Circulo_CAM1(Image<Bgr, Byte> Img, int HUE_L, int HUE_H, int SAT_L, int SAT_H, int VAL_L, int VAL_H,bool HUE, bool SAT, bool VAL, bool INV, double area_obje_desej)

                imgsize.X = ImageFrame.Width;
                imgsize.Y = ImageFrame.Height;

                // TODO: better way to check HUE, SAT and VAL
                // TODO: select HSV parameters for mission 4

                img1 = ProImg.HsvROI(ImageFrame, HUE_L, HUE_H, SAT_L, SAT_H, VAL_L, VAL_H, HUE, SAT, VAL, INV);

                img1 = img1.SmoothGaussian(9);

                // TODO: select area parameter for mission 4 : function of height?

                ProImg.Deteccao_Circulo(img1, ImageFrame, area_obje_desej);

                droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, area_obje_desej);


                mDrone.droneMoverPRO(0f, 0f, 1f, 0f);

                if (mDrone.droneObterAltitude() < 0.01f)
                    break;

                pictureBox1.Image = ImageFrame.Bitmap;
                pictureBox2.Image = img1.Bitmap;

                EstadoDrone();
            }

            mDrone.droneAterrar();

            return;
        }

        void atualizarImagemOnly(object sender, droneImageChangeEventArgs data)
        {
            ImageFrame.Bitmap = data.droneImagem;
        }
    }
}