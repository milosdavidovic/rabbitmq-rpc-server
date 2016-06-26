using System;
using System.Configuration;
using System.Reflection;
namespace WinSer
{
    partial class Installer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "My Template Service.";
            this.serviceInstaller1.DisplayName = GetConfigurationValue("displayName"); 
            this.serviceInstaller1.ServiceName = GetConfigurationValue("serviceName");
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // Installer1
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;

        private string GetConfigurationValue(string key)
        {
            Assembly service = Assembly.GetAssembly(typeof(Installer));
            Configuration config = ConfigurationManager.OpenExeConfiguration(service.Location);
            if (config.AppSettings.Settings[key] != null)
            {
                return config.AppSettings.Settings[key].Value;
            }
            else
            {
                throw new IndexOutOfRangeException
                    ("Settings collection does not contain the requested key: " + key);
            }
        }
    }
}