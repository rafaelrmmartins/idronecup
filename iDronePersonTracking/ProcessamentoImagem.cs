/*
 * Created by SharpDevelop.
 * User: João L. Vilaça
 * Date: 22/11/2013
 * Time: 18:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;

namespace iDroneExemplos
{
	/// <summary>
	/// Description of ProcessamentoImagem.
	/// </summary>
	public class ProcessamentoImagem
	{
		
		public Point Obj_centroid=new Point(-1,-1);
		public double Obj_area_actual=0;
		
		      
		
        public ProcessamentoImagem()
		{
		}
		
		#region Image Processing
		 
		 
		//Esta função detecta uma bounding box das regiões a branco na imagem com uma área superior areaV
		//img - imagem binaria; showRecOnImg - imagem a cores para sobrepor o rectangulo, areaV - área minima em pixeis dos objectos a identificar
        public void Deteccao_Rectangulo(Image<Gray, Byte> img, Image<Bgr, Byte> showRecOnImg, double areaV)
        {
            Image<Gray, Byte> imgForContour = new Image<Gray, byte>(img.Width, img.Height);
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
            
            Obj_centroid.X=-1;
            Obj_centroid.Y=-1;

            for (; seq != null && seq.Ptr.ToInt64() != 0; seq = seq.HNext)
            {
                Rectangle bndRec = CvInvoke.cvBoundingRect(seq, 2);
                Obj_area_actual = CvInvoke.cvContourArea(seq, MCvSlice.WholeSeq, 1) * -1;
                if (Obj_area_actual > areaV) 
                {
                    CvInvoke.cvRectangle(showRecOnImg, new Point(bndRec.X, bndRec.Y),
                        new Point(bndRec.X + bndRec.Width, bndRec.Y + bndRec.Height),
                        new MCvScalar(0, 0, 255), 2, LINE_TYPE.CV_AA, 0);
                	Obj_centroid.X=bndRec.X+bndRec.Width/2;
                	Obj_centroid.Y=bndRec.Y+bndRec.Height/2;
					return;                	
                }

            }

        }
        
        
        //Esta função detecta a linhas a preto
        //guarda o centroide da linha com área maior
        //desenha o centroide e o contorno da linha seleccionada 
        //img - imagem a cores BGR
        public Image<Gray, Byte> Deteccao_Linha(Image<Bgr, Byte> img)
     	{
    		
  	
       		double areaC=0;
      		double area_max=0;
       		
       		IntPtr seq_max=CvInvoke.cvCreateMemStorage(0);
	        IntPtr storage = CvInvoke.cvCreateMemStorage(0);
	        IntPtr contornos = new IntPtr();
	        MCvMoments moments = new MCvMoments();
			
	        Image<Gray, Byte> img_Contornos = new Image<Gray, Byte>(img.Width, img.Height);
			Emgu.CV.Image<Gray, Byte> img_Gray= img.Convert<Gray, Byte>();
			
			
			
			//Pré-processamento
			img_Gray.SmoothMedian(5);
			img_Gray._EqualizeHist();
			
			//Detecção da linha			
			Emgu.CV.Image<Gray, Byte> img_aux=img_Gray.Canny(1,1,5);
			img_Gray = img_aux.Not();
			img_aux = img_Gray.Erode(10);
			img_Gray = img_aux.Dilate(10);
			
						
			//detecta contornos
			CvInvoke.cvCopy(img_Gray, img_Contornos, System.IntPtr.Zero);
			CvInvoke.cvFindContours(img_Contornos, storage, ref contornos, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MCvContour)),
	                                Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL, Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, new Point(0, 0));
	
	
	 		//percorre os contornos e detecta o que tem a maior área
			Seq<Point> seq = new Seq<Point>(contornos, null);
			for (; seq != null && seq.Ptr.ToInt64() != 0; seq = seq.HNext)
	        {
				areaC = CvInvoke.cvContourArea(seq, MCvSlice.WholeSeq, 1) * -1;
	            if (areaC > area_max) 
	            {
	                area_max=areaC; 
	                seq_max=seq.Ptr;
	            }
	        }
			

				        	        
	        //calcula o centroide do contorno com maior área 
	        if(areaC>0)
	        {
	            CvInvoke.cvMoments(seq_max, ref moments,0);
	            Obj_centroid.X=(int)(moments.m10/moments.m00);
	            Obj_centroid.Y=(int)(moments.m01/moments.m00);
		        
				//desenha contorno com maior área
		        CvInvoke.cvDrawContours( img, seq_max, new MCvScalar(0, 0, 255), new MCvScalar(255, 0, 0),0, 2, LINE_TYPE.CV_AA, new Point(0,0) );
		                	
		        
		        CvInvoke.cvCircle(img, new Point(Obj_centroid.X, Obj_centroid.Y), 10, new MCvScalar(0, 255, 0), 3, LINE_TYPE.CV_AA, 0);
	        }else{
	        	Obj_centroid.X=-1;
            	Obj_centroid.Y=-1;
	        }
	        
	        return img_Gray;
			
    	}
                
       
        //Selecciona uma grama de cores no espaço HSV
        //imFrame - imagem a cores onde se quer seleccionar a gama de cores; H - 1 ou 0 para activar gama no canal H; S - 1 ou 0 para activar gama no canal S; V - 1 ou 0 para activar gama no canal V
        //L1,H1 - limite inferiro e superior do canal H; L2,H2 - limite inferiro e superior do canal S;L3,H3 - limite inferiro e superior do canal V;
        public Image<Gray, Byte> HsvROI(Image<Bgr, Byte> imgFame, int L1, int H1, int L2, int H2, int L3, int H3, bool H, bool S, bool V, bool I)
        {
            imgFame._SmoothGaussian(5);
            
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
        
        
         public void Deteccao_Circulo(Image<Gray, Byte> img, Image<Bgr, Byte> showCirOnImg, double areaV)
       {

            CircleF[] circles=img.HoughCircles(new Gray(50),//cannyThreshold:The higher threshold of the two passed to Canny edge detector
         	                                   new Gray(20),//accumulatorThreshold:Accumulator threshold at the center detection stage.The smaller it is, the more false circles may be detected.
         	                                   1,			//Resolution of the accumulator used to detect centers of the circles.
         	                                   10000,		//minDist:Minimum distance between centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one.
         	                                   80,			//minRadius:Minimal radius of the circles to search for
         	                                   500)[0];		//maxRadius:Maximal radius of the circles to search for
            
            
           
	        foreach(CircleF circle in circles) {
           		
                
	             if (circle.Area > areaV) 
                 {
	     			CvInvoke.cvCircle(showCirOnImg,new Point((int)circle.Center.X, (int)circle.Center.Y), 3, new MCvScalar(0, 255, 255), -1, LINE_TYPE.CV_AA, 0);
	     			Obj_centroid.X=(int)circle.Center.X;
	     			Obj_centroid.Y=(int)circle.Center.Y;
	      			showCirOnImg.Draw(circle, new Bgr(Color.Red), 3);
	      			return;
	             }
	             else
	             {
	             	Obj_centroid.X=-1;
          			Obj_centroid.Y=-1;
	             }
	   		}
            
        }
       
        #endregion
	}
}
