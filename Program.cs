using System;
using System.Configuration;
using System.Windows.Forms;

namespace SystemTrayApp
{
	/// <summary>
	/// 
	/// </summary>
	static class Program
	{
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var setting = ConfigurationManager.AppSettings["TestMode"];
            if (!String.IsNullOrEmpty(setting) && Convert.ToBoolean(setting) == true)
            {
                // Show the system tray icon.					
                using (PolicyForm pf = new PolicyForm())
                {
                    pf.Show();

                    // Make sure the application runs!
                    Application.Run();
                }
            }
            else
            {
                // Show the system tray icon.					
                using (ProcessIcon pi = new ProcessIcon())
                {
                    pi.Display();

                    // Make sure the application runs!
                    Application.Run();
                }
            }

        }
    }
}