using log4net;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using SystemTrayApp.Properties;

namespace SystemTrayApp
{

    /// <summary>
    /// 
    /// </summary>
    class ProcessIcon : IDisposable
    {
        static ILog logger = null;
        static ProcessIcon()
        {
            logger = LogManager.GetLogger("ProcessIcon");
        }
        /// <summary>
        /// The NotifyIcon object.
        /// </summary>
        NotifyIcon _notifyIcon;
        StorageDetector _storageDetector;
        PolicyForm _policyForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessIcon"/> class.
        /// </summary>
        public ProcessIcon()
        {
            _policyForm = new PolicyForm();
            _notifyIcon = new NotifyIcon();

            _storageDetector = new StorageDetector();
            _storageDetector.StorageDetected += Sd_StorageDetected;

            _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            _notifyIcon.BalloonTipTitle = ConfigurationManager.AppSettings["StorageAddedMessageTitle"];
            _notifyIcon.BalloonTipText = ConfigurationManager.AppSettings["StorageAddedMessageMessage"];

            logger.Info("Created");

        }

        private void Sd_StorageDetected(object sender, StorageDetectorEventArgs e)
        {
            _notifyIcon.ShowBalloonTip(5000);
            _policyForm.Drive = e.Drive;
            _policyForm.Show();
        }



        /// <summary>
        /// Displays the icon in the system tray.
        /// </summary>
        public void Display()
        {
            // Put the icon in the system tray and allow it react to mouse clicks.			
            _notifyIcon.Icon = Resources.SystemTrayApp;
            _notifyIcon.Text = "";
            _notifyIcon.Visible = true;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            // When the application closes, this will remove the icon from the system tray immediately.
            _notifyIcon.Dispose();
            _policyForm.Dispose();
        }


    }
}