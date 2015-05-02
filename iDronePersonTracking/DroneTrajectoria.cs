/*
 * Created by SharpDevelop.
 * User: João L. Vilaça
 * Date: 25/11/2013
 * Time: 16:00
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
using iDroneCup;

namespace iDroneExemplos
{
	/// <summary>
	/// Description of Drone.
	/// </summary>
	public class DroneTrajectoria
	{
		private bool davancar_s;
		private bool drecuar_s;
		private bool desquerda_s;
		private bool ddireita_s;
		private bool dsubir_s;
		private bool ddescer_s;

		
		private float davancar_v;
		private float drecuar_v;
		private float desquerda_v;
		private float ddireita_v;
		private float dsubir_v;
		private float ddescer_v;

		
		public float Vel_x_drone ,Vel_y_drone ,Vel_z_drone , Vel_rot_z_drone ;
		
     	public DroneTrajectoria()
		{
				
				davancar_s=false;
				drecuar_s=false;
				desquerda_s=false;
				ddireita_s=false;
				dsubir_s=false;
				ddescer_s=false;

				davancar_v=0;
				drecuar_v=0;
				desquerda_v=0;
				ddireita_v=0;
				dsubir_v=0;
				ddescer_v=0;

			
		}
     	
		//método que determina a direcção (em X e Z), orientação (em Z) e peso de 
        //cada uma dessas componentes, do movimento do drone, em função da posição do
        //centroide e área do objecto detectado na imagem.

		//centroid - centroide do objecto
		//imgsize - dimensão da imagem em X e Y
		//objarea_actual - área actual do objecto
		//Area_de_referencia - área de referência
		public void ObjectTracking1(Point centroid, Point imgsize, double Area_objecto_imagem, double Area_de_referencia)
		{
				float deltaX;
				float deltaY;
				double delta_area;
				float k1=1.0f;
				float k2=1.0f;
				float k3=0.1f;				
				
				//calcula a diferença entre a orientação actual do robô e a orientação desejada (centro da imagem)
				deltaX=centroid.X-imgsize.X/2;
				deltaY=centroid.Y-imgsize.Y/2;
				delta_area=Area_objecto_imagem- Area_de_referencia;
				
				//estabelece tipo de movimento
				if(deltaX>0){//esquerda ou direita
						desquerda_s=true;
						ddireita_s=false;
				}
				else	
				{
						desquerda_s=false;
						ddireita_s=true;
				}
				
				if(deltaY>0){//subir ou descer
						dsubir_s=true;
						ddescer_s=false;
				}
				else	
				{
						dsubir_s=false;
						ddescer_s=true;
				}
				
				if(delta_area>0){//avança e recua
						drecuar_s=true;
						davancar_s=false;
				}
				else	
				{
						drecuar_s=false;
						davancar_s=true;
				}
				
			
				if(centroid.X!=-1 && Area_objecto_imagem!=0){
					//define a força do movimento, normaliza velocidades [-1..0...1]
					
					/*Vel_z_drone*/dsubir_v = ddescer_v = k1*deltaY/((float)(imgsize.Y/2));
					if(dsubir_v<0)
					{
						dsubir_v=dsubir_v*-1;
						ddescer_v=ddescer_v*-1;
					}
					
					/*Vel_rot_z_drone */ddireita_v = desquerda_v = k2*deltaX/((float)(imgsize.X/2));
					if(ddireita_v<0)
					{
						ddireita_v=ddireita_v*-1;
						desquerda_v=desquerda_v*-1;
					}
					
					if(delta_area>0)
						/*Vel_x_drone*/davancar_v = drecuar_v = k3*(float)(delta_area/(Area_objecto_imagem));
					else
						/*Vel_x_drone*/davancar_v = drecuar_v = k3*(float)(delta_area/(Area_de_referencia));
				
					if(davancar_v<0)
					{
						davancar_v=davancar_v*-1;
						drecuar_v=drecuar_v*-1;
					}
					
					
				}else
				{
					ddireita_v=0;
					desquerda_v=0;
					dsubir_v=0;
					ddescer_v=0;
					drecuar_v=0;
					davancar_v=0;
				}
				
				if(ddireita_s || desquerda_s)
				{
					if(ddireita_s)
					{
						Vel_rot_z_drone=ddireita_v*-1;
					}
					else
					{
						Vel_rot_z_drone=ddireita_v;
					}
				}else
					Vel_rot_z_drone=0;
				
				if(dsubir_s || ddescer_s)
				{
					if(dsubir_s)
					{
						Vel_z_drone =ddescer_v*-1;
					}
					else
					{
						Vel_z_drone =ddescer_v;
					}
				}else
					Vel_z_drone =0;
				
				if(drecuar_s || davancar_s)
				{
					if(drecuar_s)
					{
						Vel_x_drone=drecuar_v;
					}
					else
					{
						Vel_x_drone=drecuar_v*-1;
					}
				}else
					Vel_x_drone=0;
				
					
		}
		
		//método que determina a direcção e peso dos movimento do drone (em X e Y), em função da posição do centroide do objecto detectado na imagem.
		//centroid - centroide do objecto
		//imgsize - dimensão da imagem em X e Y
		public void ObjectTracking2(Point centroid, Point imgsize)
		{
				float deltaX;
				float deltaY;
				float k1=0.10f; //TODO: keep an eye..
				float k2=0.10f; //TODO: keep an eye..
							
				deltaX=centroid.X-imgsize.X/2;
				deltaY=centroid.Y-imgsize.Y/2;
								
				//estabelece tipo de movimento
				if(deltaX>0){//esquerda ou direita
						desquerda_s=true;
						ddireita_s=false;
				}
				else	
				{
						desquerda_s=false;
						ddireita_s=true;
				}
				
				if(deltaY>0){//avança e recua
						drecuar_s=true;
						davancar_s=false;
				}
				else	
				{
						drecuar_s=false;
						davancar_s=true;
				}
				
						
			
				if(centroid.X!=-1){
					//define a força do movimento, normaliza velocidades [-1..0...1]
					
					/*Vel_x_drone*/davancar_v = drecuar_v = k1*deltaY/((float)(imgsize.Y/2));
				
					/*Vel_y_drone*/ddireita_v = desquerda_v = k2*deltaX/((float)(imgsize.X/2));
					
								
				}else
				{
					ddireita_v=0;
					desquerda_v=0;
					dsubir_v=0;
					ddescer_v=0;
					drecuar_v=0;
					davancar_v=0;
				}
				
				if(ddireita_s || desquerda_s)
					Vel_y_drone=(float)(ddireita_v);
				else
					Vel_y_drone=0;
				
				if(drecuar_s || davancar_s)
					Vel_x_drone=(float)(drecuar_v);
				else
					Vel_x_drone=0;
					
		}
		
		//método que determina a direcção e peso  do movimento do drone (em Y), em função da posição do centroide de uma linha detectada na imagem.
		//centroid - centroide do objecto
		//imgsize - dimensão da imagem em X e Y
		public void ObjectTracking3(Point centroid, Point imgsize)
		{
				float deltaX;
				float k1=0.05f;
				
							
				deltaX=centroid.X-imgsize.X/2;
										
				//estabelece tipo de movimento
				if(deltaX>0){//esquerda ou direita
						desquerda_s=true;
						ddireita_s=false;
				}
				else	
				{
						desquerda_s=false;
						ddireita_s=true;
				}
				
				if(centroid.X!=-1){
					//define a força do movimento, normaliza velocidades [-1..0...1]
					
					/*Vel_y_drone*/ddireita_v = desquerda_v = k1*deltaX/((float)(imgsize.X/2));
					
								
				}else
				{
					ddireita_v=0;
					desquerda_v=0;
					dsubir_v=0;
					ddescer_v=0;
					drecuar_v=0;
					davancar_v=0;
				}
				
				if(ddireita_s || desquerda_s)
					Vel_y_drone=(float)(ddireita_v);
				else
					Vel_y_drone=0;
		}
	

	}
}
