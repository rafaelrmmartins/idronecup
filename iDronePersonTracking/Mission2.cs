using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using iDroneCup;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
//daniel
namespace iDroneExemplos

{
    public partial class MainForm : Form
    {
        private void Mission2_Click(object sender, EventArgs e)
        {
           
                mDrone.droneMudarCamara(Drone.DroneCamera.FRONTAL);
          
                mDrone.droneDescolar();

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

                    img1 = ProImg.HsvROI(ImageFrame, m4_hsv_hlow, m4_hsv_hhi, m4_hsv_slow, m4_hsv_shi, m4_hsv_vlow, m4_hsv_vhi, m4_hsv_h, m4_hsv_s, m4_hsv_v, m4_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    // centra circulo com o drone
                    Centra_Circulo_CAM1(ImageFrame, Convert.ToInt32(H_Lval.Text), Convert.ToInt32(H_Hval.Text),
                        Convert.ToInt32(S_Lval.Text), Convert.ToInt32(S_Hval.Text), Convert.ToInt32(V_Lval.Text),
                        Convert.ToInt32(V_Hval.Text), H.Checked, S.Checked, V.Checked, Invert.Checked,
                        Convert.ToDouble(Area.Text));
                
                    //faz circulo a volta do objeto
                    ProImg.Deteccao_Circulo(img1, ImageFrame, (m4_area_obj*mDrone.droneObterAltitude()));

                    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0.01f, droneTraj.Vel_rot_z_drone);

 
                    //renvia imagem para as picturebox's
                    pictureBox1.Image = ImageFrame.Bitmap;
                    pictureBox2.Image = img1.Bitmap;

                    //envia informaçoes do drone
                    EstadoDrone();
               
                    //mediante a area o drone vai recuando ou avançando
                    Segue_Objecto_CAM1(ImageFrame,Convert.ToInt32(H_Lval.Text),Convert.ToInt32(H_Hval.Text),Convert.ToInt32(S_Lval.Text),Convert.ToInt32(S_Hval.Text),Convert.ToInt32(V_Lval.Text),Convert.ToInt32(V_Hval.Text), H.Checked, S.Checked, V.Checked,Invert.Checked, Convert.ToDouble(Area.Text));


                }
            

        }
}
}
