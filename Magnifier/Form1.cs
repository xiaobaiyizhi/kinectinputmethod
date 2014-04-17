using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Magnifier
{
	public partial class MagnifierForm : Form
	{
		PictureBox pictureBox1 = new PictureBox(); // Have a picture box
		Bitmap printscreen;
		int zoom = 1; // Variable for zoom value
		
		private static int MAGNIFIER_WIDTH = 200;
		private static int MAGNIFIER_HEIGHT = 80;
		private static int MAXZOOM = 4;

		public MagnifierForm()
		{
			InitializeComponent();

			pictureBox1.Dock = DockStyle.Fill; // Occupy the full area of the form
			BLL.FocusSetting.SetNoFocus("Form1");
			pictureBox1.BorderStyle = BorderStyle.FixedSingle; // Have a single border of clear representation

			Controls.Add(pictureBox1); // Add the control to the form

			FormBorderStyle = FormBorderStyle.None; // Make the form borderless to make it as lens look

			Timer timer = new Timer(); // Have a timer for frequent update

			timer.Interval = 50; // Set the interval for the timer

			timer.Tick += timer_Tick; // Hool the event to perform desire action
			printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); // Have a bitmap to store the image of the screen          

			timer.Start(); //Start the timer


			this.Width = MAGNIFIER_WIDTH;
			this.Height = MAGNIFIER_HEIGHT;
			this.TopMost = true;
		}

		void timer_Tick(object sender, EventArgs e)
		{
			zoom = Math.Min(zoom, MAXZOOM);
			int zoom_reflect = MAXZOOM - zoom+1;
			int BitmapWidth = zoom_reflect * MAGNIFIER_WIDTH / MAXZOOM;//投影大小
			int BitmapHeight = zoom_reflect * MAGNIFIER_HEIGHT / MAXZOOM;

			Bitmap lensbmp=new Bitmap(BitmapWidth,BitmapHeight);

			var graphics = Graphics.FromImage(lensbmp as Image); // Get the image of the captured screen
			var position = Cursor.Position; // Get the position of cursor
			
			graphics.CopyFromScreen(position.X-BitmapWidth/2, position.Y-BitmapHeight/2, 0, 0, Screen.PrimaryScreen.Bounds.Size); // Get the copy of screen

			this.pictureBox1.Image = new Bitmap(lensbmp, MAGNIFIER_WIDTH, MAGNIFIER_HEIGHT); // Assign lens bitmap with zoom level to the picture box
			
			Left = Math.Max(position.X-MAGNIFIER_WIDTH/2,0); // Place form nearer to cursor X value
			Top = position.Y+MAGNIFIER_HEIGHT/2 ; // Place form nearer to cursor Y value
			if((Top+MAGNIFIER_HEIGHT)>Screen.PrimaryScreen.Bounds.Height)
				Top = position.Y - 3 * MAGNIFIER_HEIGHT / 2; 
			this.Width = MAGNIFIER_WIDTH;
			this.Height = MAGNIFIER_HEIGHT;
			this.TopMost = true;
		}



		// Override OnKeyDown for zoom in and zoom out actions

		protected override void OnKeyDown(KeyEventArgs e)
		{

			if (e.KeyValue == 73) // Set "i" as the key for Zoom In.

				zoom++; // Increase zoom by 1 item greater

			else if (e.KeyValue == 79) // Set "o" as the key for Zoom Out

				zoom=zoom>1?zoom-1:1; // Decrease zoom by 1 item smaller

			else if (e.KeyValue == 27) // Set "Esc" to close the magnifier
			{

				Close(); // Close the form

				Dispose(); // Dispose the form

			}

			base.OnKeyDown(e);

		}

	}
}
