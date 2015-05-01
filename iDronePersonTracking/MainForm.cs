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
		DroneTrajectoria droneTraj= new DroneTrajectoria();
		
		bool prova1=false;
		
		public MainForm()
		{
			InitializeComponent();
		}
		
		void atualizarImagem(object sender, droneImageChangeEventArgs data)
		{
			ImageFrame.Bitmap = data.droneImagem;
						
			if(ImageFrame.Bitmap!=null)
				Processamento(ImageFrame);				
		}

        
		void Processamento(Image<Bgr, Byte> Img)
		{
			//determina parametros de controlo, por processamento de imagem, para o tipo de controlo: Segue Objecto que utiliza a câmara 1(frente do drone) 
			if(checkBox2.Checked)
				Segue_Objecto_CAM1(Img,Convert.ToInt32(H_Lval.Text),Convert.ToInt32(H_Hval.Text),Convert.ToInt32(S_Lval.Text),Convert.ToInt32(S_Hval.Text),Convert.ToInt32(V_Lval.Text),Convert.ToInt32(V_Hval.Text), H.Checked, S.Checked, V.Checked,Invert.Checked, Convert.ToDouble(Area.Text));
			
			if(checkBox3.Checked)
				Segue_Objecto_CAM2(Img,Convert.ToInt32(H_Lval.Text),Convert.ToInt32(H_Hval.Text),Convert.ToInt32(S_Lval.Text),Convert.ToInt32(S_Hval.Text),Convert.ToInt32(V_Lval.Text),Convert.ToInt32(V_Hval.Text), H.Checked, S.Checked, V.Checked,Invert.Checked);
			
			if(checkBox4.Checked)
				Centra_Linha_CAM2(Img);
			
			if(checkBox6.Checked)
				Centra_Circulo_CAM1(Img,Convert.ToInt32(H_Lval.Text),Convert.ToInt32(H_Hval.Text),Convert.ToInt32(S_Lval.Text),Convert.ToInt32(S_Hval.Text),Convert.ToInt32(V_Lval.Text),Convert.ToInt32(V_Hval.Text), H.Checked, S.Checked, V.Checked,Invert.Checked, Convert.ToDouble(Area.Text));
			
			//mostra imagens
			pictureBox1.Image=Img.Bitmap;
			pictureBox2.Image=img1.Bitmap;
			
			//activa tipo de controlo seleccionado no drone
			if(checkBox5.Checked && checkBox2.Checked )
				mDrone.droneMoverPRO(droneTraj.Vel_x_drone,0,droneTraj.Vel_z_drone,droneTraj.Vel_rot_z_drone);
						
			if(checkBox5.Checked && checkBox3.Checked)
			{
				if( mDrone.droneObterAltitude()<2.0f)//sobe até à altura de 2 metros
					mDrone.droneMoverPRO(droneTraj.Vel_x_drone,droneTraj.Vel_y_drone,0.25f,0);
				else
					mDrone.droneMoverPRO(droneTraj.Vel_x_drone,droneTraj.Vel_y_drone,0,0);
			}
			
			if(checkBox5.Checked && checkBox4.Checked){
				if( mDrone.droneObterAltitude()<1.5f)//sobe até à altura de 1.5 metros
					mDrone.droneMoverPRO(0,droneTraj.Vel_y_drone,0.25f,0);
				else
					mDrone.droneMoverPRO(-0.02f,droneTraj.Vel_y_drone,0,0);
			}
			
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
			Image<Bgr, Byte> imgdata = new Image<Bgr, byte>(180, 200, new Bgr(0, 0, 0));
			
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
			
			pictureBox4.Image = imgdata.Bitmap;
		}
		
		//Controlos
		#region
		
		void Button1Click(object sender, EventArgs e)
		{
			mDrone = new iDroneCup.Drone();
			mDrone.droneLigar();
            mDrone.iDroneCup_ChangeWifiChannel(Convert.ToInt32(wifi_channel.Text));
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


        void Button15Click();

	}
}
