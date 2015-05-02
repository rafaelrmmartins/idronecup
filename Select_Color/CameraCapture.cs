//----------------------------------------------------------------------------
//  João L. Vilaça
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using iDroneCup;

namespace Select_Color
{
	public partial class Select_Color : Form
	{
		#region Globle Var

		private Capture capture;
		private bool captureInProgress;
		private bool imageInProgress;
		
		String filenameload;
		Image<Bgr, Byte> ImageFrame = new Image<Bgr, Byte>(640, 360);
		Image<Bgr, Byte> ImageHSVwheel;
		Image<Hsv, Byte> hsvImage = new Image<Hsv, Byte>(0, 0);
		int diff_LH;
		private iDroneCup.Drone mDrone;

		#endregion

		public Select_Color()
		{
			InitializeComponent();
			
			// Load HSV Image
			if(File.Exists("HSV-Wheel.png"))
				ImageHSVwheel = new Image<Bgr, Byte>("HSV-Wheel.png");
			else
				ImageHSVwheel = new Image<Bgr, Byte>("c:\\HSV-Wheel.png");
			
			imageBox3.Image = ImageHSVwheel;
			Application.Idle += ProcessFrame;
			
		}

		#region Main Program

		void atualizarImagem(object sender, droneImageChangeEventArgs data)
		{
			ImageFrame.Bitmap = data.droneImagem;
			ImageProcessing();
			
			
		}
		
		private void ProcessFrame(object sender, EventArgs e)
		{
			
			if (captureInProgress)
			{
                do
                {
                    ImageFrame = capture.QueryFrame();
                } while (ImageFrame == null);

				ImageFrame = ImageFrame.Resize(imageBox1.Width, imageBox1.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
				imageBox1.Image = ImageFrame;
				
				ImageProcessing();
				
			}

			if (imageInProgress)
			{
				ImageFrame = new Image<Bgr, byte>(filenameload);
				int[] whD = scaleImage(ImageFrame.Width, ImageFrame.Height);
				ImageFrame = ImageFrame.Resize(whD[0], whD[1], Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
				imageBox1.Image = ImageFrame;
				
				ImageProcessing();
			}
			

		}
		
		
		private void ImageProcessing()
		{
			ImageFrame._SmoothGaussian(5);
			Image<Gray, Byte> ImageFrameDetection = cvAndHsvImage(
				ImageFrame,
				Convert.ToInt32(numeric_HL.Value), Convert.ToInt32(numeric_HH.Value),
				Convert.ToInt32(numeric_SL.Value), Convert.ToInt32(numeric_SH.Value),
				Convert.ToInt32(numeric_VL.Value), Convert.ToInt32(numeric_VH.Value),
				checkBox_EH.Checked, checkBox_ES.Checked, checkBox_EV.Checked, checkBox_IV.Checked);

			if (iB2C == 0) imageBox2.Image = ImageFrameDetection;

			if (iB2C == 1)
			{
				Image<Bgr, Byte> imgF = new Image<Bgr, Byte>(ImageFrame.Width, ImageFrame.Height);
				Image<Bgr, Byte> imgD = ImageFrameDetection.Convert<Bgr, Byte>();
				CvInvoke.cvAnd(ImageFrame, imgD, imgF, IntPtr.Zero);
				imageBox2.Image = imgF;
			}

			if (iB2C == 2)
			{
				Image<Bgr, Byte> imgF = new Image<Bgr, Byte>(ImageFrame.Width, ImageFrame.Height);
				Image<Bgr, Byte> imgD = ImageFrameDetection.Convert<Bgr, Byte>();
				CvInvoke.cvAnd(ImageFrame, imgD, imgF, IntPtr.Zero);
				for (int x = 0; x < imgF.Width; x++)
					for (int y = 0; y < imgF.Height; y++)
				{
					{
						Bgr c = imgF[y, x];
						if (c.Red == 0 && c.Blue == 0 && c.Green == 0)
						{
							imgF[y, x] = new Bgr(255, 255, 255);
						}
					}
				}

				imageBox2.Image = imgF;
			}


			Image<Gray, Byte> ImageHSVwheelDetection = cvAndHsvImage(
				ImageHSVwheel,
				Convert.ToInt32(numeric_HL.Value), Convert.ToInt32(numeric_HH.Value),
				Convert.ToInt32(numeric_SL.Value), Convert.ToInt32(numeric_SH.Value),
				Convert.ToInt32(numeric_VL.Value), Convert.ToInt32(numeric_VH.Value),
				checkBox_EH.Checked, checkBox_ES.Checked, checkBox_EV.Checked, checkBox_IV.Checked);
			
			if (checkBox_VAr.Checked){
				
				RecDetection(ImageFrameDetection, ImageFrame, Convert.ToInt32(numeric_VAr.Value));
			}
			
			imageBox4.Image = ImageHSVwheelDetection;
			
			imageBox1.Image=ImageFrame;
		}

		#endregion

		#region Image Processing

		private void RecDetection(Image<Gray, Byte> img, Image<Bgr, Byte> showRecOnImg, int areaV)
		{
			Image<Gray, Byte> imgForContour = new Image<Gray, Byte>(img.Width, img.Height);
			CvInvoke.cvCopy(img, imgForContour, System.IntPtr.Zero);


			IntPtr storage = CvInvoke.cvCreateMemStorage(0);
			IntPtr contour = new IntPtr();

			CvInvoke.cvFindContours(
				imgForContour,
				storage,
				ref contour,
				System.Runtime.InteropServices.Marshal.SizeOf(typeof(MCvContour)),
				Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL,
				Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE,
				new Point(0, 0));


			Seq<Point> seq = new Seq<Point>(contour, null);

			for (; seq != null && seq.Ptr.ToInt64() != 0; seq = seq.HNext)
			{
				Rectangle bndRec = CvInvoke.cvBoundingRect(seq, 2);
				double areaC = CvInvoke.cvContourArea(seq, MCvSlice.WholeSeq, 1) * -1;
				if (areaC > areaV)
				{
					CvInvoke.cvRectangle(showRecOnImg, new Point(bndRec.X, bndRec.Y),
					                     new Point(bndRec.X + bndRec.Width, bndRec.Y + bndRec.Height),
					                     new MCvScalar(0, 0, 255), 2, LINE_TYPE.CV_AA, 0);
				}

			}

		}
		private Image<Gray, Byte> cvAndHsvImage(Image<Bgr, Byte> imgFame, int L1, int H1, int L2, int H2, int L3, int H3, bool H, bool S, bool V, bool I)
		{
			Image<Hsv, Byte> hsvImage = imgFame.Convert<Hsv, Byte>();
			Image<Gray, Byte> ResultImage = new Image<Gray, Byte>(hsvImage.Width, hsvImage.Height);
			Image<Gray, Byte> ResultImageH = new Image<Gray, Byte>(hsvImage.Width, hsvImage.Height);
			Image<Gray, Byte> ResultImageS = new Image<Gray, Byte>(hsvImage.Width, hsvImage.Height);
			Image<Gray, Byte> ResultImageV = new Image<Gray, Byte>(hsvImage.Width, hsvImage.Height);

			Image<Gray, Byte> img1 = inRangeImage(hsvImage, L1, H1, 0);
			Image<Gray, Byte> img2 = inRangeImage(hsvImage, L2, H2, 1);
			Image<Gray, Byte> img3 = inRangeImage(hsvImage, L3, H3, 2);
			Image<Gray, Byte> img4 = inRangeImage(hsvImage, 0, L1, 0);
			Image<Gray, Byte> img5 = inRangeImage(hsvImage, H1, 180, 0);

			#region checkBox Color Mode

			if (H)
			{
				if (I)
				{
					CvInvoke.cvOr(img4, img5, img4, System.IntPtr.Zero);
					ResultImageH = img4;
				}
				else { ResultImageH = img1; }
			}

			if (S) ResultImageS = img2;
			if (V) ResultImageV = img3;

			if (H && !S && !V) ResultImage = ResultImageH;
			if (!H && S && !V) ResultImage = ResultImageS;
			if (!H && !S && V) ResultImage = ResultImageV;

			if (H && S && !V)
			{
				CvInvoke.cvAnd(ResultImageH, ResultImageS, ResultImageH, System.IntPtr.Zero);
				ResultImage = ResultImageH;
			}

			if (H && !S && V)
			{
				CvInvoke.cvAnd(ResultImageH, ResultImageV, ResultImageH, System.IntPtr.Zero);
				ResultImage = ResultImageH;
			}

			if (!H && S && V)
			{
				CvInvoke.cvAnd(ResultImageS, ResultImageV, ResultImageS, System.IntPtr.Zero);
				ResultImage = ResultImageS;
			}

			if (H && S && V)
			{
				CvInvoke.cvAnd(ResultImageH, ResultImageS, ResultImageH, System.IntPtr.Zero);
				CvInvoke.cvAnd(ResultImageH, ResultImageV, ResultImageH, System.IntPtr.Zero);
				ResultImage = ResultImageH;
			}
			#endregion

			CvInvoke.cvErode(ResultImage, ResultImage, (IntPtr)null, 1);

			return ResultImage;
		}
		private Image<Gray, Byte> inRangeImage(Image<Hsv, Byte> hsvImage, int Lo, int Hi, int con)
		{
			Image<Gray, Byte> ResultImage = new Image<Gray, Byte>(hsvImage.Width, hsvImage.Height);
			Image<Gray, Byte> IlowCh = new Image<Gray, Byte>(hsvImage.Width, hsvImage.Height, new Gray(Lo));
			Image<Gray, Byte> IHiCh = new Image<Gray, Byte>(hsvImage.Width, hsvImage.Height, new Gray(Hi));
			CvInvoke.cvInRange(hsvImage[con], IlowCh, IHiCh, ResultImage);
			return ResultImage;
		}
		private int[] scaleImage(int wP, int hP)
		{
			int[] dR = new int[2];
			int ra;
			if (wP != 0)
			{
				ra = (100 * 320) / wP;
				wP = 320;
				hP = (hP * ra) / 100;
				if (hP != 0 && hP > 240)
				{
					ra = (100 * 240) / hP;
					hP = 240;
					wP = (wP * ra) / 100;
				}
				dR[0] = wP;
				dR[1] = hP;
			}
			return dR;
		}

		#endregion

		#region controlos

		private void numeric_Lo_ValueChanged(object sender, EventArgs e)
		{
			trackBar_HL.Value = Convert.ToInt32(numeric_HL.Value);
			trackBar_SL.Value = Convert.ToInt32(numeric_SL.Value);
			trackBar_VL.Value = Convert.ToInt32(numeric_VL.Value);
		}
		private void numeric_Hi_ValueChanged(object sender, EventArgs e)
		{
			trackBar_HH.Value = Convert.ToInt32(numeric_HH.Value);
			trackBar_SH.Value = Convert.ToInt32(numeric_SH.Value);
			trackBar_VH.Value = Convert.ToInt32(numeric_VH.Value);

		}
		private void numeric_VAr_ValueChanged(object sender, EventArgs e)
		{
			trackBar_VAr.Value = Convert.ToInt32(numeric_VAr.Value);
		}

		private void trackBar_Lo_ValueChanged(object sender, EventArgs e)
		{
			if (trackBar_HL.Value >= trackBar_HH.Value && !checkBox_LH.Checked)
				trackBar_HH.Value = trackBar_HL.Value;
			if (checkBox_LH.Checked && trackBar_HL.Value + diff_LH <= 180)
				trackBar_HH.Value = trackBar_HL.Value + diff_LH;
			numeric_HL.Value = trackBar_HL.Value;
			numeric_SL.Value = trackBar_SL.Value;
			numeric_VL.Value = trackBar_VL.Value;
		}
		private void trackBar_Hi_ValueChanged(object sender, EventArgs e)
		{
			if (trackBar_HH.Value <= trackBar_HL.Value && !checkBox_LH.Checked)
				trackBar_HL.Value = trackBar_HH.Value;

			if (checkBox_LH.Checked && trackBar_HH.Value - diff_LH >= 0)
				trackBar_HL.Value = trackBar_HH.Value - diff_LH;

			numeric_HH.Value = trackBar_HH.Value;
			numeric_SH.Value = trackBar_SH.Value;
			numeric_VH.Value = trackBar_VH.Value;
		}
		private void trackBar_VAr_ValueChanged(object sender, EventArgs e)
		{
			numeric_VAr.Value = trackBar_VAr.Value;
		}

		private void checkBox_LH_CheckedChanged(object sender, EventArgs e)
		{
			diff_LH = trackBar_HH.Value - trackBar_HL.Value;
		}

		
		bool ib3C;
		int ib3C_HV, ib3C_Lable_X, ib3C_Lable_Y;
		private void imageBox3_MouseClick(object sender, MouseEventArgs e)
		{
			Image<Hsv, Byte> imgHsv = ImageHSVwheel.Convert<Hsv, Byte>();
			Hsv hsv = imgHsv[e.Y, e.X];
			if (!ib3C)
			{
				ib3C_HV = Convert.ToInt32(hsv.Hue);
				ib3C_Lable_X = imageBox3.Location.X + e.X;
				ib3C_Lable_Y = imageBox3.Location.Y + e.Y;
				label_L.Location = new Point(ib3C_Lable_X, ib3C_Lable_Y);
				label_H.Hide();
			}
			if (ib3C)
			{
				label_H.Show();
				if (ib3C_HV < hsv.Hue)
				{
					trackBar_HL.Value = ib3C_HV;
					trackBar_HH.Value = Convert.ToInt32(hsv.Hue);
					label_H.Location = new Point(imageBox3.Location.X + e.X, imageBox3.Location.Y + e.Y);
				}
				else
				{
					trackBar_HH.Value = ib3C_HV;
					trackBar_HL.Value = Convert.ToInt32(hsv.Hue);
					ib3C_HV = 0;
					label_H.Location = new Point(ib3C_Lable_X, ib3C_Lable_Y);
					label_L.Location = new Point(imageBox3.Location.X + e.X, imageBox3.Location.Y + e.Y);
				}
			}
			ib3C = !ib3C;
		}

		int iB2C;
		private void imageBox2_Click(object sender, EventArgs e)
		{
			iB2C++;
			if (iB2C > 2) iB2C = 0;
		}

		void Button1Click(object sender, EventArgs e)
		{
			imageInProgress = false;

			if (capture == null)
			{
				try
				{
					capture = new Capture();
				}
				catch (NullReferenceException excpt)
				{
					MessageBox.Show(excpt.Message);
				}
			}

			if (capture != null)
			{
				if (captureInProgress)
				{
					captureInProgress = false;
				}
				else
				{
					captureInProgress = true;
				}
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			captureInProgress = false;
			OpenFileDialog OpenFile = new OpenFileDialog();
			if (OpenFile.ShowDialog() == DialogResult.OK)
			{
				filenameload = OpenFile.FileName;
				imageInProgress = true;
			}
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			if(mDrone!=null)
				mDrone.droneRemoverLigação();
			this.Close();
		}
		
		void Button4Click(object sender, EventArgs e)
		{
			trackBar_HH.Value = 0;
			trackBar_HL.Value = 0;
			trackBar_SH.Value = 255;
			trackBar_SL.Value = 0;
			trackBar_VH.Value = 255;
			trackBar_VL.Value = 0;
			checkBox_LH.Checked = false;
			checkBox_IV.Checked = false;
		}
		
		
		
		void Button6Click(object sender, EventArgs e)
		{
			
			mDrone = new iDroneCup.Drone();
			mDrone.droneLigar();
			checkBox1.Enabled=true;
		}
		
		
		
		void Button5Click(object sender, EventArgs e)
		{
			mDrone.droneMudarCamara(Drone.DroneCamera.PROXIMA);
			//mDrone.droneDesligar();
			//mDrone.droneRemoverLigação();
		}
		
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(checkBox1.Checked)
				mDrone.imageChange += new Drone.droneImageHandler(atualizarImagem);
			else
				mDrone.imageChange -= new Drone.droneImageHandler(atualizarImagem);
		}
		#endregion
		
		

	}
}
