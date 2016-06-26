using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinSer
{
    public class Config
    {
        public static string _conn_string = System.Configuration.ConfigurationManager.AppSettings["connectionString"];
        public static string _company_name = System.Configuration.ConfigurationManager.AppSettings["companyName"];
        public static string _service_name = System.Configuration.ConfigurationManager.AppSettings["serviceName"];
        
        public string _company_email;
        public string _email_pass;
        public string _smtp_host;

        public Config()
        { 
            _company_email = System.Configuration.ConfigurationManager.AppSettings["companyEmail"];
            _email_pass = System.Configuration.ConfigurationManager.AppSettings["emailPass"];
            _smtp_host = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];
        }   
     
        public void RefreshParams()
        {
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            _company_email = System.Configuration.ConfigurationManager.AppSettings["companyEmail"];
            _email_pass = System.Configuration.ConfigurationManager.AppSettings["emailPass"];
            _smtp_host = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];
        }
    }

}
