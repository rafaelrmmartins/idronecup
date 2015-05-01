/*
 * Created by SharpDevelop.
 * User: João Vilaça
 * Date: 22/11/2013
 * Time: 17:17
 * 
 * Edited by Rafael Martins
 * iDroneCup Competition 2015
 * FEUP team "Black4HawkDown"
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace iDroneExemplos
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.button10 = new System.Windows.Forms.Button();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label_area = new System.Windows.Forms.Label();
            this.Area = new System.Windows.Forms.TextBox();
            this.V_Hval = new System.Windows.Forms.TextBox();
            this.S_Hval = new System.Windows.Forms.TextBox();
            this.H_Hval = new System.Windows.Forms.TextBox();
            this.V_Lval = new System.Windows.Forms.TextBox();
            this.S_Lval = new System.Windows.Forms.TextBox();
            this.H_Lval = new System.Windows.Forms.TextBox();
            this.Invert = new System.Windows.Forms.CheckBox();
            this.V = new System.Windows.Forms.CheckBox();
            this.S = new System.Windows.Forms.CheckBox();
            this.H = new System.Windows.Forms.CheckBox();
            this.button15 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 39);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 55);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ligar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 108);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 50);
            this.button2.TabIndex = 1;
            this.button2.Text = "Desligar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(29, 91);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(157, 33);
            this.button3.TabIndex = 2;
            this.button3.Text = "Câmara Superior";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(29, 53);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(157, 32);
            this.button4.TabIndex = 3;
            this.button4.Text = "Capturar 1 Imagem";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1304, 551);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(52, 246);
            this.button5.TabIndex = 4;
            this.button5.Text = "Sair";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(8, 146);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 394);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(29, 23);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(157, 30);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Captura Continua";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1CheckedChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(716, 146);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(640, 394);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Green;
            this.button6.Location = new System.Drawing.Point(716, 551);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(113, 97);
            this.button6.TabIndex = 16;
            this.button6.Text = "Descolar";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.Button6Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Orange;
            this.button7.Location = new System.Drawing.Point(716, 699);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(112, 98);
            this.button7.TabIndex = 17;
            this.button7.Text = "Aterrar";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.Button7Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Red;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button8.Location = new System.Drawing.Point(1101, 551);
            this.button8.Margin = new System.Windows.Forms.Padding(4);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(164, 138);
            this.button8.TabIndex = 18;
            this.button8.Text = "Aterrar (Emergencia)";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.Button8Click);
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new System.Drawing.Point(8, 18);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(248, 30);
            this.checkBox2.TabIndex = 19;
            this.checkBox2.Text = "Segue Objecto Câmara Frontal";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.CheckBox2CheckedChanged);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox3.InitialImage")));
            this.pictureBox3.Location = new System.Drawing.Point(0, 1);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1364, 138);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(165, 545);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(209, 174);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Captura";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(29, 129);
            this.button9.Margin = new System.Windows.Forms.Padding(4);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(157, 37);
            this.button9.TabIndex = 7;
            this.button9.Text = "Câmara Inferior";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.Button9Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox7);
            this.groupBox2.Controls.Add(this.checkBox6);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Location = new System.Drawing.Point(389, 544);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(256, 159);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Modos de Controlo";
            // 
            // checkBox7
            // 
            this.checkBox7.Location = new System.Drawing.Point(8, 119);
            this.checkBox7.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(139, 30);
            this.checkBox7.TabIndex = 23;
            this.checkBox7.Text = "GPS Navigation";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.Location = new System.Drawing.Point(8, 95);
            this.checkBox6.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(221, 30);
            this.checkBox6.TabIndex = 22;
            this.checkBox6.Text = "Centra Circulo";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.Location = new System.Drawing.Point(8, 69);
            this.checkBox4.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(221, 30);
            this.checkBox4.TabIndex = 21;
            this.checkBox4.Text = "Centra Linha Câmara Inferior";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.CheckBox4CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new System.Drawing.Point(8, 46);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(233, 30);
            this.checkBox3.TabIndex = 20;
            this.checkBox3.Text = "Segue Objecto Câmara Inferior";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.CheckBox3CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Location = new System.Drawing.Point(13, 545);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(144, 174);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Ligação ao Drone";
            // 
            // checkBox5
            // 
            this.checkBox5.Location = new System.Drawing.Point(397, 695);
            this.checkBox5.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(176, 30);
            this.checkBox5.TabIndex = 22;
            this.checkBox5.Text = "Activar Controlo";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.OrangeRed;
            this.button10.Location = new System.Drawing.Point(1101, 721);
            this.button10.Margin = new System.Windows.Forms.Padding(4);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(164, 76);
            this.button10.TabIndex = 23;
            this.button10.Text = "Reset";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.Button10Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.Location = new System.Drawing.Point(847, 551);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(240, 246);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 24;
            this.pictureBox4.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label_area);
            this.groupBox4.Controls.Add(this.Area);
            this.groupBox4.Controls.Add(this.V_Hval);
            this.groupBox4.Controls.Add(this.S_Hval);
            this.groupBox4.Controls.Add(this.H_Hval);
            this.groupBox4.Controls.Add(this.V_Lval);
            this.groupBox4.Controls.Add(this.S_Lval);
            this.groupBox4.Controls.Add(this.H_Lval);
            this.groupBox4.Controls.Add(this.Invert);
            this.groupBox4.Controls.Add(this.V);
            this.groupBox4.Controls.Add(this.S);
            this.groupBox4.Controls.Add(this.H);
            this.groupBox4.Location = new System.Drawing.Point(21, 719);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(627, 79);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Seleccionar Cor";
            // 
            // label_area
            // 
            this.label_area.Location = new System.Drawing.Point(493, 32);
            this.label_area.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_area.Name = "label_area";
            this.label_area.Size = new System.Drawing.Size(49, 28);
            this.label_area.TabIndex = 11;
            this.label_area.Text = "Área";
            // 
            // Area
            // 
            this.Area.Location = new System.Drawing.Point(543, 28);
            this.Area.Margin = new System.Windows.Forms.Padding(4);
            this.Area.Name = "Area";
            this.Area.Size = new System.Drawing.Size(57, 22);
            this.Area.TabIndex = 10;
            this.Area.Text = "2300";
            // 
            // V_Hval
            // 
            this.V_Hval.Location = new System.Drawing.Point(393, 49);
            this.V_Hval.Margin = new System.Windows.Forms.Padding(4);
            this.V_Hval.Name = "V_Hval";
            this.V_Hval.Size = new System.Drawing.Size(57, 22);
            this.V_Hval.TabIndex = 9;
            this.V_Hval.Text = "255";
            // 
            // S_Hval
            // 
            this.S_Hval.Location = new System.Drawing.Point(244, 48);
            this.S_Hval.Margin = new System.Windows.Forms.Padding(4);
            this.S_Hval.Name = "S_Hval";
            this.S_Hval.Size = new System.Drawing.Size(57, 22);
            this.S_Hval.TabIndex = 8;
            this.S_Hval.Text = "255";
            // 
            // H_Hval
            // 
            this.H_Hval.Location = new System.Drawing.Point(109, 49);
            this.H_Hval.Margin = new System.Windows.Forms.Padding(4);
            this.H_Hval.Name = "H_Hval";
            this.H_Hval.Size = new System.Drawing.Size(57, 22);
            this.H_Hval.TabIndex = 7;
            this.H_Hval.Text = "45";
            // 
            // V_Lval
            // 
            this.V_Lval.Location = new System.Drawing.Point(393, 18);
            this.V_Lval.Margin = new System.Windows.Forms.Padding(4);
            this.V_Lval.Name = "V_Lval";
            this.V_Lval.Size = new System.Drawing.Size(57, 22);
            this.V_Lval.TabIndex = 6;
            this.V_Lval.Text = "70";
            // 
            // S_Lval
            // 
            this.S_Lval.Location = new System.Drawing.Point(244, 18);
            this.S_Lval.Margin = new System.Windows.Forms.Padding(4);
            this.S_Lval.Name = "S_Lval";
            this.S_Lval.Size = new System.Drawing.Size(57, 22);
            this.S_Lval.TabIndex = 5;
            this.S_Lval.Text = "63";
            // 
            // H_Lval
            // 
            this.H_Lval.Location = new System.Drawing.Point(109, 20);
            this.H_Lval.Margin = new System.Windows.Forms.Padding(4);
            this.H_Lval.Name = "H_Lval";
            this.H_Lval.Size = new System.Drawing.Size(57, 22);
            this.H_Lval.TabIndex = 4;
            this.H_Lval.Text = "18";
            // 
            // Invert
            // 
            this.Invert.Location = new System.Drawing.Point(17, 43);
            this.Invert.Margin = new System.Windows.Forms.Padding(4);
            this.Invert.Name = "Invert";
            this.Invert.Size = new System.Drawing.Size(71, 30);
            this.Invert.TabIndex = 3;
            this.Invert.Text = "Invert";
            this.Invert.UseVisualStyleBackColor = true;
            // 
            // V
            // 
            this.V.Checked = true;
            this.V.CheckState = System.Windows.Forms.CheckState.Checked;
            this.V.Location = new System.Drawing.Point(337, 27);
            this.V.Margin = new System.Windows.Forms.Padding(4);
            this.V.Name = "V";
            this.V.Size = new System.Drawing.Size(48, 30);
            this.V.TabIndex = 2;
            this.V.Text = "V";
            this.V.UseVisualStyleBackColor = true;
            // 
            // S
            // 
            this.S.Checked = true;
            this.S.CheckState = System.Windows.Forms.CheckState.Checked;
            this.S.Location = new System.Drawing.Point(197, 27);
            this.S.Margin = new System.Windows.Forms.Padding(4);
            this.S.Name = "S";
            this.S.Size = new System.Drawing.Size(55, 30);
            this.S.TabIndex = 1;
            this.S.Text = "S";
            this.S.UseVisualStyleBackColor = true;
            // 
            // H
            // 
            this.H.Checked = true;
            this.H.CheckState = System.Windows.Forms.CheckState.Checked;
            this.H.Location = new System.Drawing.Point(17, 20);
            this.H.Margin = new System.Windows.Forms.Padding(4);
            this.H.Name = "H";
            this.H.Size = new System.Drawing.Size(40, 30);
            this.H.TabIndex = 0;
            this.H.Text = "H";
            this.H.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(652, 636);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(57, 32);
            this.button15.TabIndex = 26;
            this.button15.Text = "M3";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.Button15Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 806);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "iDroneExemplos";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.TextBox H_Hval;
		private System.Windows.Forms.TextBox S_Hval;
		private System.Windows.Forms.TextBox V_Hval;
		private System.Windows.Forms.TextBox Area;
		private System.Windows.Forms.Label label_area;
		private System.Windows.Forms.CheckBox H;
		private System.Windows.Forms.CheckBox S;
		private System.Windows.Forms.CheckBox V;
		private System.Windows.Forms.CheckBox Invert;
		private System.Windows.Forms.TextBox H_Lval;
		private System.Windows.Forms.TextBox S_Lval;
		private System.Windows.Forms.TextBox V_Lval;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button15;
		
	}
}
