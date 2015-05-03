/*
 * Created by SharpDevelop.
 * User: João L. Vilaça
 * Date: 22/11/2013
 * Time: 17:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		private iDroneCup.Drone mDrone;
		Point centroid=new Point(-1,-1);
		Point imgsize =new Point(0,0);
		Image<Bgr, Byte> ImageFrame = new Image<Bgr, Byte>(640, 380);
		Image<Gray, Byte> img1 = new Image<Gray, Byte>(640, 380);
        ProcessamentoImagem ProImg = new ProcessamentoImagem();
        ProcessamentoImagem ProImg2 = new ProcessamentoImagem();
        Image<Gray, Byte> img2 = new Image<Gray, Byte>(640, 380);
		DroneTrajectoria droneTraj= new DroneTrajectoria();
        int state_m4 = 0;
        int state_m3 = 0;
        int state_m2 = 0;
        int state_m1 = 0;
        int mission = 0; 
        int i = 0;
		
		public MainForm()
		{
			InitializeComponent();
		}
		
		void atualizarImagem(object sender, droneImageChangeEventArgs data)
		{
			ImageFrame.Bitmap = data.droneImagem;

            if (ImageFrame.Bitmap != null)
            {
                Processamento(ImageFrame);				
            }
		}


		void Processamento(Image<Bgr, Byte> Img)
		{



			//determina parametros de controlo, por processamento de imagem, para o tipo de controlo: Segue Objecto que utiliza a câmara 1(frente do drone) 
            //if(checkBox2.Checked)
            //    Segue_Objecto_CAM1(Img,Convert.ToInt32(H_Lval.Text),Convert.ToInt32(H_Hval.Text),Convert.ToInt32(S_Lval.Text),Convert.ToInt32(S_Hval.Text),Convert.ToInt32(V_Lval.Text),Convert.ToInt32(V_Hval.Text), H.Checked, S.Checked, V.Checked,Invert.Checked, Convert.ToDouble(Area.Text));
			
            //if(checkBox3.Checked)
            //    Segue_Objecto_CAM2(Img,Convert.ToInt32(H_Lval.Text),Convert.ToInt32(H_Hval.Text),Convert.ToInt32(S_Lval.Text),Convert.ToInt32(S_Hval.Text),Convert.ToInt32(V_Lval.Text),Convert.ToInt32(V_Hval.Text), H.Checked, S.Checked, V.Checked,Invert.Checked);
			
            //if(checkBox4.Checked)
            //    Centra_Linha_CAM2(Img);
			
            //if(checkBox6.Checked)
            //    Centra_Circulo_CAM1(Img,Convert.ToInt32(H_Lval.Text),Convert.ToInt32(H_Hval.Text),Convert.ToInt32(S_Lval.Text),Convert.ToInt32(S_Hval.Text),Convert.ToInt32(V_Lval.Text),Convert.ToInt32(V_Hval.Text), H.Checked, S.Checked, V.Checked,Invert.Checked, Convert.ToDouble(Area.Text));

            //mission 4
            #region
            if (mission == 4)
            {
                if (state_m4 == 0)
                {
                    if (i > 25)
                        mDrone.iDroneCup_Hover();

                    i++;

                    if (i == 50)
                    {
                        state_m4 = 1;
                        i = 0;
                    }
                }
                if (state_m4 == 1)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, m4_hsv_hlow, m4_hsv_hhi, m4_hsv_slow, m4_hsv_shi, m4_hsv_vlow, m4_hsv_vhi, m4_hsv_h, m4_hsv_s, m4_hsv_v, m4_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    // TODO: select area parameter for mission 4 : function of height?

                    ProImg.Deteccao_Circulo(img1, ImageFrame, m4_area_obj);

                    droneTraj.ObjectTracking2(ProImg.Obj_centroid, imgsize);

                    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 1f, droneTraj.Vel_rot_z_drone);
                    //mDrone.droneMoverPRO(0f, 0f, 1f, 0f); 


                    if (mDrone.droneObterAltitude() > 3.1f)
                    {
                        state_m4 = 2;
                        resetDroneTrajVal();
                    }
                }
                if (state_m4 == 2)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(ImageFrame, m4_hsv_hlow, m4_hsv_hhi, m4_hsv_slow, m4_hsv_shi, m4_hsv_vlow, m4_hsv_vhi, m4_hsv_h, m4_hsv_s, m4_hsv_v, m4_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, ImageFrame, (m4_area_obj));

                    droneTraj.ObjectTracking2(ProImg.Obj_centroid, imgsize);

                    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, -0.15f, droneTraj.Vel_rot_z_drone);
                                                                                                                                                                                                                                                           //TODO: try values that work
                    if ((((ProImg.Obj_centroid.X - (imgsize.X / 2)) < 5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) < 5) && ((ProImg.Obj_centroid.X - (imgsize.X / 2)) > -5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) > -5)) || (mDrone.iDroneCup_Read_Altitude() < 1.7f))
                    {
                        state_m4 = 3;
                        resetDroneTrajVal();
                    }
                }
                if (state_m4 == 3)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(ImageFrame, m4_hsv_hlow, m4_hsv_hhi, m4_hsv_slow, m4_hsv_shi, m4_hsv_vlow, m4_hsv_vhi, m4_hsv_h, m4_hsv_s, m4_hsv_v, m4_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, ImageFrame, m4_area_obj);

                    droneTraj.ObjectTracking2(ProImg.Obj_centroid, imgsize);

                    if (mDrone.iDroneCup_Read_Altitude() > 1.7f)
                        mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, -0.2f, droneTraj.Vel_rot_z_drone);
                    else if (mDrone.iDroneCup_Read_Altitude() < 1.7f)
                        mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0, droneTraj.Vel_rot_z_drone);

                    if (((ProImg.Obj_centroid.X - (imgsize.X / 2)) < 4) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) < 4) && ((ProImg.Obj_centroid.X - (imgsize.X / 2)) > -4) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) > -4))
                    {
                        state_m4 = 4;
                        resetDroneTrajVal();
                    }
                }
                //if (state_m4 == 4)
                //{
                //    imgsize.X = ImageFrame.Width;
                //    imgsize.Y = ImageFrame.Height;

                //    img1 = ProImg.HsvROI(ImageFrame, m4_hsv_hlow, m4_hsv_hhi, m4_hsv_slow, m4_hsv_shi, m4_hsv_vlow, m4_hsv_vhi, m4_hsv_h, m4_hsv_s, m4_hsv_v, m4_hsv_invert);

                //    img1 = img1.SmoothGaussian(9);

                //    ProImg.Deteccao_Circulo(img1, ImageFrame, (m4_area_obj+6000));

                //    droneTraj.ObjectTracking2(ProImg.Obj_centroid, imgsize);

                //    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0, droneTraj.Vel_rot_z_drone);

                //    if (((ProImg.Obj_centroid.X - (imgsize.X / 2)) < 3) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) < 3) && ((ProImg.Obj_centroid.X - (imgsize.X / 2)) > -3) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) > -3))
                //    {
                //        state_m4 = 4;
                //        resetDroneTrajVal();
                //    }
                //}
                if (state_m4 == 4)
                {
                    mDrone.droneAterrar();
                    resetDroneTrajVal();
                    state_m4 = 0;
                    mission = 0;
                }
            }//fim
            #endregion

            //mission3
            #region
            else if (mission == 3)
            {
                if(state_m3 == 0)
                {
                    
                    //if (mDrone.droneObterAltitude() < 1.5f)
                    //    mDrone.droneMoverPRO(-0.1f, 0.1f, 0.15f, 0f);
                    //else
                    //    mDrone.droneMoverPRO(-0.1f, 0.1f, 0f, 0f);

                    if (i>15)
                        mDrone.iDroneCup_Hover();

                    i++;

                    if (i==30)
                    {
                        state_m3 = 1;
                        i = 0;
                    }
                }
                if (state_m3 == 1)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, 0, 0, 0, 0, 0, 60, false, false, true, false);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, ImageFrame, 100);
                                                                                              //TODO : testar efeito
                    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0.15f, droneTraj.Vel_rot_z_drone);

                    if (mDrone.droneObterAltitude() > 1.5f)
                    {
                        state_m3 = 111;
                        resetDroneTrajVal();
                    }
                }

                if (state_m3 == 111)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, 0, 0, 0, 0, 0, 60, false, false, true, false);

                    img1 = img1.SmoothGaussian(9);

                    // TODO: select area parameter for mission 4 : function of height?

                    ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

                    //mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 1.0f, droneTraj.Vel_rot_z_drone);
                    mDrone.droneMoverPRO(0.1f, droneTraj.Vel_y_drone, 0f, 0f);


                    if (ProImg.Obj_centroid.X == -1 && ProImg.Obj_centroid.Y == -1)
                    {
                        state_m3 = 1;
                        resetDroneTrajVal();
                    }

                }
                if (state_m3 == 1)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.Deteccao_Linha(ImageFrame);

                    droneTraj.ObjectTracking3(ProImg.Obj_centroid, imgsize);

                    mDrone.droneMoverPRO(0.10f, droneTraj.Vel_y_drone, 0f, droneTraj.Vel_rot_z_drone);

                    //condiçao de final percurso

                    img2 = ProImg2.HsvROI(ImageFrame, 0, 0, 0, 0, 0, 66, false, false, true, false);

                    img2 = img2.SmoothGaussian(9);

                    // TODO: select area parameter for mission 4 : function of height?

                    ProImg2.Deteccao_Circulo(img2, ImageFrame, 100);

                    //if ((ProImg2.Obj_centroid.X != -1) && (ProImg2.Obj_centroid.Y != -1))
                    //{
                    //    state_m3 = 2;
                    //    resetDroneTrajVal();
                    //}
                    //if ((ProImg.Obj_centroid.X == -1) && (ProImg.Obj_centroid.Y == -1))
                    //{
                    //    state_m3 = 10;
                    //    resetDroneTrajVal();
                    //}
                }
                if (state_m3 == 10)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.Deteccao_Linha(ImageFrame);

                    droneTraj.ObjectTracking3(ProImg.Obj_centroid, imgsize);

                    mDrone.droneMoverPRO(-0.15f, 0f, 0f, 0f);

                    //condiçao de voltou ao percurso

                    if ((ProImg.Obj_centroid.X != -1) && (ProImg.Obj_centroid.Y != -1))
                    {
                        state_m3 = 1;
                        resetDroneTrajVal();
                    }
                }
                if (state_m3 == 2)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, 0, 0, 0, 0, 0, 60, false, false, true, false);

                    img1 = img1.SmoothGaussian(9);

                    // TODO: select area parameter for mission 4 : function of height?
                    //TODO: aterragem!!!!
                    ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

                    //mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 1.0f, droneTraj.Vel_rot_z_drone);
                    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0f, droneTraj.Vel_rot_z_drone);

                    if (((ProImg.Obj_centroid.X - (imgsize.X / 2)) < 5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) < 5) && ((ProImg.Obj_centroid.X - (imgsize.X / 2)) > -5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) > -5))
                    {
                        state_m3 = 0;
                        mission = 0;
                        resetDroneTrajVal();
                        mDrone.droneAterrar();
                    }
                }//fim
                
            }
            #endregion

            //mission1
            #region
            else if (mission == 1)
            {
                if (state_m1 == 0)
                {
                    if (i > 50)
                    {
                        mDrone.dronePairar();
                    }
                    
                    i++;

                    if (i > 100)
                    {
                        resetDroneTrajVal();
                        i = 0;
                        state_m1 = 1;
                    }
                }
                else if (state_m1 == 1)
                {
                    mDrone.droneMoverPRO(0f, 0f, 0.10f, 0f);

                    if (mDrone.droneObterAltitude() > 1.2f)
                    {
                        state_m2 = 2;
                        resetDroneTrajVal();
                    }
                }
                else if (state_m1 == 2)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, m11_hsv_hlow, m11_hsv_hhi, m11_hsv_slow, m11_hsv_shi, m11_hsv_vlow, m11_hsv_vhi, m11_hsv_h, m11_hsv_s, m11_hsv_v, m11_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

                    droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, m2_area_obj);

                    mDrone.droneMoverPRO(0f, (droneTraj.Vel_rot_z_drone * 0.5f), droneTraj.Vel_z_drone, 0f);

                    if (((ProImg.Obj_centroid.X - (imgsize.X / 2)) < 5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) < 5) && ((ProImg.Obj_centroid.X - (imgsize.X / 2)) > -5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) > -5))
                    {
                        resetDroneTrajVal();
                        state_m1 = 22;
                        i = 0;
                    }
                }
                else if (state_m1 == 22)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, m11_hsv_hlow, m11_hsv_hhi, m11_hsv_slow, m11_hsv_shi, m11_hsv_vlow, m11_hsv_vhi, m11_hsv_h, m11_hsv_s, m11_hsv_v, m11_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

                    droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, m2_area_obj);

                    mDrone.droneMoverPRO(0.15f, 0f, droneTraj.Vel_z_drone, droneTraj.Vel_rot_z_drone);

                    if (ProImg.Obj_centroid.X == -1 && ProImg.Obj_centroid.Y == -1)
                    {
                        resetDroneTrajVal();
                        state_m1 = 3;
                        i = 0;
                    }
                }
                else if (state_m1 == 3)
                {
                    mDrone.droneMoverPRO(0.25f, 0f, 0f, 0f);

                    i++;

                    if (i == 25)
                    {
                        state_m1 = 4;
                        i = 0;
                        resetDroneTrajVal();
                    }
                }
                else if (state_m1 == 4)
                {
                    mDrone.droneMoverPRO(0f, 0.15f, 0f, 0f);

                    i++;

                    if (i == 25)
                    {
                        state_m1 = 5;
                        i = 0;
                        resetDroneTrajVal();
                    }
                }
                else if (state_m1 == 5)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, m12_hsv_hlow, m12_hsv_hhi, m12_hsv_slow, m12_hsv_shi, m12_hsv_vlow, m12_hsv_vhi, m12_hsv_h, m12_hsv_s, m12_hsv_v, m12_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

                    droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, m2_area_obj);

                    mDrone.droneMoverPRO(0f, (droneTraj.Vel_rot_z_drone * 0.5f), droneTraj.Vel_z_drone, 0f);

                    if (((ProImg.Obj_centroid.X - (imgsize.X / 2)) < 5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) < 5) && ((ProImg.Obj_centroid.X - (imgsize.X / 2)) > -5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) > -5))
                    {
                        resetDroneTrajVal();
                        state_m1 = 6;
                        i = 0;
                    }
                }
                else if (state_m1 == 6)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, m12_hsv_hlow, m12_hsv_hhi, m12_hsv_slow, m12_hsv_shi, m12_hsv_vlow, m12_hsv_vhi, m12_hsv_h, m12_hsv_s, m12_hsv_v, m12_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

                    droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, m2_area_obj);

                    mDrone.droneMoverPRO(0.15f, 0f, droneTraj.Vel_z_drone, droneTraj.Vel_rot_z_drone);

                    if (ProImg.Obj_centroid.X == -1 && ProImg.Obj_centroid.Y == -1)
                    {
                        resetDroneTrajVal();
                        state_m1 = 7;
                        i = 0;
                    }
                }
                else if (state_m1 == 7)
                {
                    mDrone.droneMoverPRO(0.25f, 0f, 0f, 0f);

                    i++;

                    if (i == 25)
                    {
                        state_m1 = 8;
                        i = 0;
                        resetDroneTrajVal();
                    }
                }
                else if (state_m1 == 8)
                {
                    mDrone.droneMoverPRO(0f, 0f, 0.10f, 0f);

                    i++;

                    if (i == 25)
                    {
                        state_m1 = 9;
                        i = 0;
                        resetDroneTrajVal();
                    }
                }
                else if (state_m1 == 9)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, m13_hsv_hlow, m13_hsv_hhi, m13_hsv_slow, m13_hsv_shi, m13_hsv_vlow, m13_hsv_vhi, m13_hsv_h, m13_hsv_s, m13_hsv_v, m13_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

                    droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, m2_area_obj);

                    mDrone.droneMoverPRO(0f, (droneTraj.Vel_rot_z_drone * 0.5f), droneTraj.Vel_z_drone, 0f);

                    if (((ProImg.Obj_centroid.X - (imgsize.X / 2)) < 5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) < 5) && ((ProImg.Obj_centroid.X - (imgsize.X / 2)) > -5) && ((ProImg.Obj_centroid.Y - (imgsize.Y / 2)) > -5))
                    {
                        resetDroneTrajVal();
                        state_m1 = 10;
                        i = 0;
                    }
                }
                else if (state_m1 == 10)
                {
                    imgsize.X = ImageFrame.Width;
                    imgsize.Y = ImageFrame.Height;

                    img1 = ProImg.HsvROI(Img, m13_hsv_hlow, m13_hsv_hhi, m13_hsv_slow, m13_hsv_shi, m13_hsv_vlow, m13_hsv_vhi, m13_hsv_h, m13_hsv_s, m13_hsv_v, m13_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, ImageFrame, 100);

                    droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, m2_area_obj);

                    mDrone.droneMoverPRO(0.15f, 0f, droneTraj.Vel_z_drone, droneTraj.Vel_rot_z_drone);

                    if (ProImg.Obj_centroid.X == -1 && ProImg.Obj_centroid.Y == -1)
                    {
                        resetDroneTrajVal();
                        state_m1 = 11;
                        i = 0;
                    }
                }
                else if (state_m1 == 11)
                {
                    mDrone.droneMoverPRO(0.25f, 0f, 0f, 0f);

                    i++;

                    if (i == 25)
                    {
                        state_m1 = 12;
                        i = 0;
                        resetDroneTrajVal();
                    }
                }
                else if (state_m1 == 12)
                {
                    mDrone.iDroneCup_Land();

                    state_m1 = 0;
                    i = 0;
                    resetDroneTrajVal();
                }
                //fim
            }
            #endregion

            //mission2
            #region
            else if (mission == 2)
            {
                if (state_m2 == 0)
                {
                    if (i > 50)
                    {
                        mDrone.dronePairar();
                    }

                    if (i == 100)
                        m2_inic_yaw = normalize(mDrone.droneObterYaw()+3.141f);

                    if (i > 100)
                        mDrone.droneRodarDireita(0.5f);

                    i++;


                    if (Math.Abs(m2_inic_yaw - normalize(mDrone.droneObterYaw())) < 0.1f )
                    {
                        resetDroneTrajVal();
                        i = 0;
                        state_m2 = 11;
                    }
                }
                else if (state_m2 == 11)
                {
                    mDrone.dronePairar();

                    i++;

                    if(i>50)
                    { 
                        i = 0;
                        state_m2 = 1;
                    }
                }
                else if (state_m2 == 1)
                {
                    mDrone.droneMoverPRO(0f, 0f, 0.1f, 0f);

                    if (mDrone.droneObterAltitude() > 1.1f)
                    {
                        state_m2 = 2;
                        resetDroneTrajVal();
                    }
                }
                else if (state_m2 == 2)
                {
                    imgsize.X = Img.Width;
                    imgsize.Y = Img.Height;

                    img1 = ProImg.HsvROI(Img, m2_hsv_hlow, m2_hsv_hhi, m2_hsv_slow, m2_hsv_shi, m2_hsv_vlow, m2_hsv_vhi, m2_hsv_h, m2_hsv_s, m2_hsv_v, m2_hsv_invert);

                    img1 = img1.SmoothGaussian(9);

                    ProImg.Deteccao_Circulo(img1, Img, m2_area_obj);

                    droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, m2_area_ref);

                    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, 0f, droneTraj.Vel_z_drone, droneTraj.Vel_rot_z_drone);

                    if (mDrone.droneObterAltitude() < 0.60f)
                    {
                        state_m2 = 3;
                        resetDroneTrajVal();
                    }
                }
                else if (state_m2 == 3)
                {
                    mDrone.droneAterrar();
                    state_m2 = 0;
                    mission = 0;
                }
            } //fim
            #endregion

            //mostra imagens
			pictureBox1.Image=Img.Bitmap;
			pictureBox2.Image=img1.Bitmap;

            #region
            //activa tipo de controlo seleccionado no drone
            //if (checkBox5.Checked && checkBox2.Checked)
            //    mDrone.droneMoverPRO(droneTraj.Vel_x_drone, 0, droneTraj.Vel_z_drone, droneTraj.Vel_rot_z_drone);

            //if (checkBox5.Checked && checkBox3.Checked)
            //{
            //    if (mDrone.droneObterAltitude() < 2.0f)//sobe até à altura de 2 metros
            //        mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0.25f, 0);
            //    else
            //        mDrone.droneMoverPRO(droneTraj.Vel_x_drone, droneTraj.Vel_y_drone, 0, 0);
            //}

            //if (checkBox5.Checked && checkBox4.Checked)
            //{
            //    if (mDrone.droneObterAltitude() < 1.5f)//sobe até à altura de 1.5 metros
            //        mDrone.droneMoverPRO(0, droneTraj.Vel_y_drone, 0.25f, 0);
            //    else
            //        mDrone.droneMoverPRO(-0.02f, droneTraj.Vel_y_drone, 0, 0);
            //}
            #endregion
            //mostra na interface o estado de algumas variaveis do drone
			
            EstadoDrone();			
		}
		
		void Segue_Objecto_CAM1(Image<Bgr, Byte> Img, int HUE_L, int HUE_H, int SAT_L, int SAT_H, int VAL_L, int VAL_H,bool HUE, bool SAT, bool VAL, bool INV, double area_obje_desej)
		{
			imgsize.X=Img.Width;
			imgsize.Y=Img.Height;
						
			img1=ProImg.HsvROI(Img,HUE_L,HUE_H,SAT_L,SAT_H,VAL_L,VAL_H,HUE,SAT,VAL,INV);
			
			ProImg.Deteccao_Rectangulo(img1,Img,area_obje_desej);
			
			droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, area_obje_desej);
			
		}
		
		void Segue_Objecto_CAM2(Image<Bgr, Byte> Img, int HUE_L, int HUE_H, int SAT_L, int SAT_H, int VAL_L, int VAL_H,bool HUE, bool SAT, bool VAL, bool INV)
		{
			imgsize.X=Img.Width;
			imgsize.Y=Img.Height;
						
			img1=ProImg.HsvROI(Img,HUE_L,HUE_H,SAT_L,SAT_H,VAL_L,VAL_H,HUE,SAT,VAL,INV);
			
			ProImg.Deteccao_Rectangulo(img1,Img,300);
			
			droneTraj.ObjectTracking2(ProImg.Obj_centroid, imgsize);
	
		}
		
		void Centra_Linha_CAM2(Image<Bgr, Byte> Img)
		{
			imgsize.X=Img.Width;
			imgsize.Y=Img.Height;
			
			img1=ProImg.Deteccao_Linha(Img);
			
			droneTraj.ObjectTracking3(ProImg.Obj_centroid, imgsize);
			
		}
		
		void Centra_Circulo_CAM1(Image<Bgr, Byte> Img, int HUE_L, int HUE_H, int SAT_L, int SAT_H, int VAL_L, int VAL_H,bool HUE, bool SAT, bool VAL, bool INV, double area_obje_desej)
		{
			imgsize.X=Img.Width;
			imgsize.Y=Img.Height;
						
			img1=ProImg.HsvROI(Img,HUE_L,HUE_H,SAT_L,SAT_H,VAL_L,VAL_H,HUE,SAT,VAL,INV);
			
			img1 = img1.SmoothGaussian(9);
			
			ProImg.Deteccao_Circulo(img1,Img,area_obje_desej);
			
			droneTraj.ObjectTracking1(ProImg.Obj_centroid, imgsize, ProImg.Obj_area_actual, area_obje_desej);
			
		}


		void EstadoDrone()
		{					
			Image<Bgr, Byte> imgdata = new Image<Bgr, byte>(180, 300, new Bgr(0, 0, 0));
			
			MCvFont f = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.4, 0.4);
			string s = "Bateria: " + mDrone.droneObterValorBateria().ToString()+"%";
			imgdata.Draw(s, ref f, new Point(5, 10), new Bgr(0, 255, 0));
			
			s = "Yaw: " + mDrone.droneObterYaw().ToString();
			imgdata.Draw(s, ref f, new Point(5, 30), new Bgr(0, 255, 0));
			
			s =  "Pitch: " + mDrone.droneObterPitch().ToString();
			imgdata.Draw(s, ref f, new Point(5, 50), new Bgr(0, 255, 0));
			
			s =  "Roll: " + mDrone.droneObterRoll().ToString();
			imgdata.Draw(s, ref f, new Point(5, 70), new Bgr(0, 255, 0));
			
			s =  "Altura: " + mDrone.droneObterAltitude().ToString();
			imgdata.Draw(s, ref f, new Point(5, 90), new Bgr(0, 255, 0));
			
			s = "vel x: " + droneTraj.Vel_x_drone.ToString();
			imgdata.Draw(s, ref f, new Point(5, 110), new Bgr(0, 255, 255));
			
		    s = "vel y: " + droneTraj.Vel_y_drone.ToString();
			imgdata.Draw(s, ref f, new Point(5, 130), new Bgr(0, 255, 255));
			
			s = "vel z: " + droneTraj.Vel_z_drone.ToString();
			imgdata.Draw(s, ref f, new Point(5, 150), new Bgr(0, 255, 255));
			
			s = "rot z: " + droneTraj.Vel_rot_z_drone.ToString();
			imgdata.Draw(s, ref f, new Point(5, 170), new Bgr(0, 255, 255));
			
			s = "Area Objecto: " + ProImg.Obj_area_actual.ToString();
			imgdata.Draw(s, ref f, new Point(5, 190), new Bgr(255, 255, 0));

            s = "Stage M1: " + state_m1.ToString();
            imgdata.Draw(s, ref f, new Point(5, 210), new Bgr(255, 255, 0));

            s = "Stage M2: " + state_m2.ToString() + "     i: " + i.ToString();
            imgdata.Draw(s, ref f, new Point(5, 230), new Bgr(255, 255, 0));

			
			pictureBox4.Image = imgdata.Bitmap;
		}
		
		//Controlos
		#region
		
		void Button1Click(object sender, EventArgs e)
		{
			mDrone = new iDroneCup.Drone();
			mDrone.droneLigar();
            //mDrone.iDroneCup_ChangeWifiChannel(Convert.ToInt32(wifi_channel.Text));
			mDrone.droneResetEmergencia();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			mDrone.droneDesligar();
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			mDrone.droneMudarCamara(Drone.DroneCamera.FRONTAL);
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			mDrone.droneMudarCamara(Drone.DroneCamera.INFERIOR);
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			
		}
		
		void Button5Click(object sender, EventArgs e)
		{
			mDrone.droneRemoverLigação();
			this.Close();
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox1.Checked)
				mDrone.imageChange += new Drone.droneImageHandler(atualizarImagem);
			else
				mDrone.imageChange -= new Drone.droneImageHandler(atualizarImagem);
		}
		
		void Button6Click(object sender, System.EventArgs e)
		{
			if(mDrone!=null){
				mDrone.droneDescolar();
			}
		}
		
		void Button7Click(object sender, System.EventArgs e)
		{
			if(mDrone!=null){
				checkBox2.Checked=false;
				mDrone.droneAterrar();
			}
		}
		
		void Button8Click(object sender, System.EventArgs e)
		{
			if(mDrone!=null){
				checkBox2.Checked=false;
				mDrone.droneEmergencia();
			}
		}
		
		void CheckBox2CheckedChanged(object sender, System.EventArgs e)
		{
			checkBox3.Checked=false;
			checkBox4.Checked=false;			
		}
		
		void CheckBox3CheckedChanged(object sender, System.EventArgs e)
		{
			checkBox2.Checked=false;
			checkBox4.Checked=false;
		}
		
		void CheckBox4CheckedChanged(object sender, System.EventArgs e)
		{
			checkBox2.Checked=false;
			checkBox3.Checked=false;
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			mDrone.droneResetEmergencia();
		}
		
		#endregion

        void mission4_Click(object sender, EventArgs e)
        {
            //mDrone.droneCalibrar();
            mDrone.droneMudarCamara(Drone.DroneCamera.INFERIOR);
            mDrone.iDroneCup_TakeOff();
            //mDrone.iDroneCup_Hover();
            mission = 4;
            state_m4 = 0;
        }

        void mission3_Click(object sender, System.EventArgs e)
        {
            //mDrone.droneCalibrar();
            mDrone.droneMudarCamara(Drone.DroneCamera.INFERIOR);
            Button6Click(sender, e);
            //mDrone.iDroneCup_Hover();
            mission = 3;
            state_m3 = 0;
        }

        float m2_inic_yaw;

        float normalize(float value)
        {
            float twoPi = (float)(Math.PI) + (float)(Math.PI);
            while (value <= -Math.PI) value += twoPi;
            while (value > Math.PI) value -= twoPi;

            return value;
        }  

        void mission2_Click(object sender, System.EventArgs e)
        {
            mDrone.droneMudarCamara(Drone.DroneCamera.FRONTAL);
            mDrone.droneDescolar();
            mission = 2;
            i = 0;
            state_m2 = 0;
        }

        void mission1_Click(object sender, System.EventArgs e)
        {
            mDrone.droneMudarCamara(Drone.DroneCamera.FRONTAL);
            mDrone.droneDescolar();
            mission = 1;
            state_m1 = 0;
            i = 0;
        }

        void resetDroneTrajVal()
        {
            droneTraj.Vel_rot_z_drone = 0;
            droneTraj.Vel_x_drone = 0;
            droneTraj.Vel_y_drone = 0;
            droneTraj.Vel_z_drone = 0;

            mDrone.droneMoverPRO(0f, 0f, 0f, 0f);

            return;
        }

        const int m4_area_obj = 7000;
        const int m4_hsv_hlow = 4;
        const int m4_hsv_hhi = 168;
        const int m4_hsv_vlow = 89;
        const int m4_hsv_vhi = 255;
        const int m4_hsv_slow = 132;
        const int m4_hsv_shi = 255;
        const bool m4_hsv_h = true;
        const bool m4_hsv_s = true;
        const bool m4_hsv_v = true;
        const bool m4_hsv_invert = true;

        const int m11_area_obj = 7000;
        const int m11_hsv_hlow = 4;
        const int m11_hsv_hhi = 168;
        const int m11_hsv_vlow = 89;
        const int m11_hsv_vhi = 255;
        const int m11_hsv_slow = 132;
        const int m11_hsv_shi = 255;
        const bool m11_hsv_h = true;
        const bool m11_hsv_s = true;
        const bool m11_hsv_v = true;
        const bool m11_hsv_invert = true;

        const int m12_area_obj = 7000;
        const int m12_hsv_hlow = 4;
        const int m12_hsv_hhi = 168;
        const int m12_hsv_vlow = 89;
        const int m12_hsv_vhi = 255;
        const int m12_hsv_slow = 132;
        const int m12_hsv_shi = 255;
        const bool m12_hsv_h = true;
        const bool m12_hsv_s = true;
        const bool m12_hsv_v = true;
        const bool m12_hsv_invert = true;

        const int m13_area_obj = 7000;
        const int m13_hsv_hlow = 4;
        const int m13_hsv_hhi = 168;
        const int m13_hsv_vlow = 89;
        const int m13_hsv_vhi = 255;
        const int m13_hsv_slow = 132;
        const int m13_hsv_shi = 255;
        const bool m13_hsv_h = true;
        const bool m13_hsv_s = true;
        const bool m13_hsv_v = true;
        const bool m13_hsv_invert = true;


        const int m2_area_ref = 20000;
        const int m2_area_obj = 4000;
        const int m2_hsv_hlow = 56;
        const int m2_hsv_hhi = 93;
        const int m2_hsv_vlow = 109;
        const int m2_hsv_vhi = 202;
        const int m2_hsv_slow = 63;
        const int m2_hsv_shi = 255;
        const bool m2_hsv_h = true;
        const bool m2_hsv_s = true;
        const bool m2_hsv_v = false;
        const bool m2_hsv_invert = false;

        private void calibrar_Click(object sender, EventArgs e)
        {
            mDrone.iDroneCup_Calibration();
        }
        
    }
}
