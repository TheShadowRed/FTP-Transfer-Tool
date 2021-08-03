/*
 * Created by SharpDevelop.
 * User: TheRedLord
 * Date: 9/20/2018
 * Time: 12:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace FTP_Transfer_Tool
{
	/// Exemplu text FTP Tool
	/// nume utilizator ftp
	/// parola utilizator ftp
	/// adresa de ftp
	/// D-locatie de unde download-locatia unde al pui 1
	/// D-locatie de unde download-locatia unde al pui 2
	/// D-locatie de unde download-locatia unde al pui 3
	/// D-locatie de unde download-locatia unde al pui n
	/// U-fisier de uploadat 1
	/// 
	/// 
	/// 
	/// 
	/// 
	/// 
	/// C-7287462837428
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		String ftpUsername;
		String ftpPassword;
		String ftpAdress;
		String CuiOperation="";
		String CuiData="";
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			ReadFile();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public void ReadFile()
		{
			int a=0;
			// Read in lines from file.
			foreach (string line in File.ReadLines("c:\\dep\\Legaturi_externe\\FTPDownload.txt")){
			string[] wordss = line.Split('-');
			CuiOperation=wordss[0];
			CuiData=wordss[1];
			}
			if(CuiOperation=="C"){
				//cui operation starts
				
				var request = (HttpWebRequest)WebRequest.Create("https://webservicesp.anaf.ro/PlatitorTvaRest/api/v3/ws/tva");
            request.ContentType = "application/json";
            request.Method = "POST";
            
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
					//need to remake the json serializer
            /*    string json = new JavaScriptSerializer().Serialize(new
                {

                    cui = CuiData,
                    data = DateTime.Now.ToString("yyyy-MM-dd")
                });
			
                streamWriter.Write("["+json+"]");
			*/
            }
           string fileLines;
            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                fileLines = streamReader.ReadToEnd();
            }
            //write anser
            File.WriteAllText("C:\\dep\\Legaturi_externe\\RaspunsCui.txt", fileLines);
				
				
				
				//cui operation ends
			}else{
        	foreach (string line in File.ReadLines("c:\\dep\\Legaturi_externe\\FTPDownload.txt"))
        	{
        		if(a==0)
        		{
        			ftpUsername=line;
        			a=1;
        		}else{
        		if(a==1)
        		{
        			ftpPassword=line;
        			a=2;
        		}else{
        		if(a==2)
        		{
        			ftpAdress=line;
        			a=3;
        		}else{
        		if(a==3)
        		{
        			string[] words = line.Split('-');
        			if(words[0]=="D")
        			{
        				DownloadFile(words[1],words[2]);	
        			}else
        			{
        				String fileName = Path.GetFileName (words[1]);
        				UploadFile(words[1],fileName);
        			}
        			
        		}
        		}
        		}
        		}
        	}
			}
			
			
			//delete file
			
		}		
		public void UploadFile(String FisierDeUploadat,String NameFiles)
		{
			using (WebClient client = new WebClient())
			{
    			client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
    			client.UploadFile(ftpAdress+NameFiles, "STOR", FisierDeUploadat);
			}
		}
		public void DownloadFile(string url, string savePath)
		{
   			 var client = new WebClient();
   			 client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
   			 client.DownloadFile(url, savePath);
		}
		public void SendCuiGetRespone()
		{
			
		}
	}
}
