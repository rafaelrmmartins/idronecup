﻿
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
//        const int m4_area_obj = 7000;
//        const int m4_hsv_hlow = 4;
//        const int m4_hsv_hhi = 168;
//        const int m4_hsv_vlow = 89;
//        const int m4_hsv_vhi = 255;
//        const int m4_hsv_slow = 132;
//        const int m4_hsv_shi = 255;
//        const bool m4_hsv_h = true;
//        const bool m4_hsv_s = true;
//        const bool m4_hsv_v = true;
//        const bool m4_hsv_invert = true;

//        void mission4_Click(object sender, EventArgs e)
//        {
//            // TODO: verificar ligacao, etc

//            mDrone.droneMudarCamara(Drone.DroneCamera.INFERIOR);

//            resetDroneTrajVal();
//            mDrone.droneDescolar();
//            mDrone.dronePairar();

//            do
//            {
//                while (true) //get new image
//                {
//                    mDrone.imageChange += new Drone.droneImageHandler(atualizarImagemOnly);

//                    if (ImageFrame.Bitmap != null)
//                        break;
//                }

//                imgsize.X = ImageFrame.Width;
//                imgsize.Y = ImageFrame.Height;

//                // TODO: better way to check HUE, SAT and VAL
//                // TODO: select HSV parameters for mission 4 CHECK

//                img1 = ProImg.HsvROI(ImageFrame, m4_hsv_hlow, m4_hsv_hhi, m4_hsv_slow, m4_hsv_shi, m4_hsv_vlow, m4_hsv_vhi, m4_hsv_h, m4_hsv_s, m4_hsv_v, m4_hsv_invert);

//                img1 = img1.SmoothGaussian(9);

//                // TODO: select area parameter for mission 4 : function of height?

//                ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

//                droneTraj.ObjectTracking2(ProImg.Obj_centroid, imgsize);

//                mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 1.0f, droneTraj.Vel_rot_z_drone); ;

//                //refresh form
//                //                pictureBox1.Image = ImageFrame.Bitmap;
//                //                pictureBox2.Image = img1.Bitmap;

//                //EstadoDrone();
//            } while (mDrone.droneObterAltitude() < 3.0f);

//            resetDroneTrajVal();

//            mDrone.dronePairar();

//            while (true)
//            {

//                while (true) //get new image
//                {
//                    mDrone.imageChange += new Drone.droneImageHandler(atualizarImagemOnly);

//                    if (ImageFrame.Bitmap != null)
//                        break;
//                }

//                imgsize.X = ImageFrame.Width;
//                imgsize.Y = ImageFrame.Height;

//                // TODO: better way to check HUE, SAT and VAL
//                // TODO: select HSV parameters for mission 4 CHECK

//                img1 = ProImg.HsvROI(ImageFrame, m4_hsv_hlow, m4_hsv_hhi, m4_hsv_slow, m4_hsv_shi, m4_hsv_vlow, m4_hsv_vhi, m4_hsv_h, m4_hsv_s, m4_hsv_v, m4_hsv_invert);

//                img1 = img1.SmoothGaussian(9);

//                // TODO: select area parameter for mission 4 : function of height?

//                ProImg.Deteccao_Circulo(img1, ImageFrame, m4_area_obj);

//                droneTraj.ObjectTracking2(ProImg.Obj_centroid, imgsize);

//                if (mDrone.droneObterAltitude() > 1.5f)
//                    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, -0.5f, droneTraj.Vel_rot_z_drone);
 
//                if (mDrone.droneObterAltitude() < 1.5f)
//                    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0, droneTraj.Vel_rot_z_drone); ;
 
//                    //refresh form
////                pictureBox1.Image = ImageFrame.Bitmap;
////                pictureBox2.Image = img1.Bitmap;

//                //EstadoDrone();

//                //if (mDrone.droneObterAltitude() < 0.25f)
//                //    break;
//                if ((Math.Abs(ProImg.Obj_centroid.X - (imgsize.X / 2)) < 5) && (Math.Abs(ProImg.Obj_centroid.Y - (imgsize.Y / 2)) < 5))
//                {
//                    mDrone.dronePairar();
//                    break;
//                }
//            }
 
//            resetDroneTrajVal();            
//            mDrone.droneAterrar();
 
//            return;
//        }
 
//        void resetDroneTrajVal()
//        {
//            droneTraj.Vel_rot_z_drone = 0;
//            droneTraj.Vel_x_drone= 0;
//            droneTraj.Vel_y_drone= 0;
//            droneTraj.Vel_z_drone = 0;

//            mDrone.droneMoverPRO(0f, 0f, 0f, 0f);

//            return;
//        }

//        void atualizarImagemOnly(object sender, droneImageChangeEventArgs data)
//        {
//            ImageFrame.Bitmap = data.droneImagem;
//        }
    }

}