using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.WinForms;
using UsbEject.Library;

namespace SystemTrayApp
{
    public class PolicyForm : Form
    {
        static ILog logger = null;
        static PolicyForm()
        {
            logger = LogManager.GetLogger("PolicyForm");
        }
        Button buttonAgree;
        private Panel panel1;
        private Panel panel2;
        Button buttonDisagree;
        HtmlPanel htmlPanel;

        public string Drive { get; set; }
        public PolicyForm()
        {
            try
            {
                InitializeComponent();

                htmlPanel = new HtmlPanel();

                htmlPanel.Text = File.ReadAllText(ConfigurationManager.AppSettings["policyFile"]);
                htmlPanel.Dock = DockStyle.Fill;

                htmlPanel.LinkClicked += HtmlPanel_LinkClicked;
                panel2.Controls.Add(htmlPanel);
                logger.Info("Created");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw;
            }
        }

        private void HtmlPanel_LinkClicked(object sender, TheArtOfDev.HtmlRenderer.Core.Entities.HtmlLinkClickedEventArgs e)
        {
            logger.Info("Link clicked:" + e.Link);
            try
            {
                System.Diagnostics.Process.Start(e.Link);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            e.Handled = true;
        }

        private void Load_Form(object sender, EventArgs e)
        {
            // Create really small form, invisible anyway.
            this.Size = new System.Drawing.Size(1024, 768);
        }

        private void InitializeComponent()
        {
            this.buttonAgree = new System.Windows.Forms.Button();
            this.buttonDisagree = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAgree
            // 
            this.buttonAgree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAgree.Location = new System.Drawing.Point(3, 3);
            this.buttonAgree.Name = "buttonAgree";
            this.buttonAgree.Size = new System.Drawing.Size(75, 23);
            this.buttonAgree.TabIndex = 0;
            this.buttonAgree.Text = "I agree";
            this.buttonAgree.Click += new System.EventHandler(this.buttonAgree_Click);
            // 
            // buttonDisagree
            // 
            this.buttonDisagree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDisagree.Location = new System.Drawing.Point(532, 3);
            this.buttonDisagree.Name = "buttonDisagree";
            this.buttonDisagree.Size = new System.Drawing.Size(90, 23);
            this.buttonDisagree.TabIndex = 1;
            this.buttonDisagree.Text = "I disagree";
            this.buttonDisagree.Click += new System.EventHandler(this.buttonDisagree_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonAgree);
            this.panel1.Controls.Add(this.buttonDisagree);
            this.panel1.Location = new System.Drawing.Point(12, 554);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(622, 30);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Location = new System.Drawing.Point(12, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(622, 535);
            this.panel2.TabIndex = 3;
            // 
            // PolicyForm
            // 
            this.ClientSize = new System.Drawing.Size(646, 596);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PolicyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void buttonAgree_Click(object sender, EventArgs e)
        {
            logger.Info("<I Agree> clicked");
            this.Hide();
        }

        private void buttonDisagree_Click(object sender, EventArgs e)
        {
            logger.Info("<I Disagree> clicked");
            this.Hide();

            var setting = ConfigurationManager.AppSettings["EjectOnDisagree"];
            if (!String.IsNullOrEmpty(setting) && Convert.ToBoolean(setting) == true)
            {
                var drive = Drive.TrimEnd('\\');
                VolumeDeviceClass volumes = new VolumeDeviceClass();
                foreach (Volume vol in volumes.Devices)
                {
                    if (vol.LogicalDrive.Equals(drive))
                    {
                        logger.Info("Attempting to eject drive: " + Drive);
                        vol.Eject(false);
                        logger.Info("Done ejecting drive.");
                        break;
                    }
                }
            }
        }
    }
}
