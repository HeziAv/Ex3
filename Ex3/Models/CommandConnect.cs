using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;

namespace Ex3.Models
{
    public class CommandConnect
    {
        const string lonPath = "get /position/longitude-deg\r\n";
        const string latPath = "get /position/latitude-deg\r\n";

        public bool isConnect
        {
            get;
            set;
        }

        public double lon
        {
            get;
            set;
        }

        public double lat
        {
            get;
            set;
        }

        private CommandConnect()
        {
            isConnect = false;
        }


        #region Singleton
        private static CommandConnect m_Instance = null;
        public static CommandConnect Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new CommandConnect();
                }
                return m_Instance;
            }
        }
        #endregion

        TcpClient _client = new TcpClient();
        public void disconnect()
        {
            isConnect = false;
            _client.Close();
            Console.WriteLine("Command channel :You are disconnected");

        }
        public void connect(string IP,int port)
        {
            IPEndPoint ep1 = new IPEndPoint(IPAddress.Parse(IP), port);
            while (!_client.Connected)
            {
                _client.Connect(ep1);
                Debug.WriteLine("Command channel :You are connected");
                isConnect = true;
            }
        }

        public string parser(string data)
        {
            int startindex = data.IndexOf((char)39);
            int Endindex = data.LastIndexOf((char)39);
            string outputstring = data.Substring(startindex + 1, Endindex - startindex - 1);
            return outputstring;
        }

        public double sendInfo(string text)
        {
            string[] lines;
            lines = text.Split('\n');
            if (!isConnect)
            {
                return -1;
            }
            NetworkStream ns = _client.GetStream();
            byte[] buffWriter = Encoding.ASCII.GetBytes(lonPath);
            ns.Write(buffWriter, 0, buffWriter.Length);
            System.IO.StreamReader line1 = new System.IO.StreamReader(ns);
            string buffer = line1.ReadLine();
            string par = parser(buffer);
            double fpar = (float)Convert.ToDouble(par);
            return fpar;

        }


        public double getLon()
        {
            return sendInfo(lonPath);
        }


        public double getLat()
        {
            return sendInfo(latPath);
        }


       
    }
}