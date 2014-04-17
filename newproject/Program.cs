using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace newproject
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			newproject.App app=new App();
			app.InitializeComponent();
			
            try
            {
                app.Run();
				//MiniWindow win = new MiniWindow();
				app.MainWindow = app.Windows[0];
            }
            catch (System.Exception ex)
            {
               MessageBox.Show(ex.ToString());
            }

		}
	}
}
