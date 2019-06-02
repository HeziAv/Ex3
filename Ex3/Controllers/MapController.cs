using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Net;

namespace Ex3.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }


        // check if the url for mission 1 or mission 4
        [HttpGet]
        public ActionResult Mdisplay(string ip, int port)
        {
            try
            {
                IPAddress.Parse(ip); // try to pare the first argument
                return display(ip, port);
            }
            catch (Exception)
            {
                return readDataBase(ip, port);
            }

        }

        // mission 1 - get lon and lat and send to the view for presntion
        [HttpGet]
        public ActionResult display(string ip, int port) 
        {
            CommandConnect.Instance.connect(ip, port);
                      
            ViewBag.lat = CommandConnect.Instance.getLat(); // send request for get lat
            ViewBag.lon = CommandConnect.Instance.getLon(); // send request for get lon

            return View("display");
        }



        
        [HttpGet]
        public ActionResult save(string ip, int port, int second,int time,string file)
        {
            ViewBag.time = second;
            ViewBag.ip = ip;
            ViewBag.port = port;
            ViewBag.time = time;
            Session["time"] = second;
            Session["timeout"] = time;
            Session["filename"] = file;
            string fileName = (string)Session["file"];
            Debug.WriteLine("we are in updatedDisplay");
            CommandConnect.Instance.connect(ip, port);

            return View();
        }


        [HttpPost]
        public string getInfoDisplay4andsave()
        {
            Debug.WriteLine("ppppppppppppppppppp");
            double lat = CommandConnect.Instance.getLat();
            double lon = CommandConnect.Instance.getLon();
            double throttle = CommandConnect.Instance.getthrottle();
            double rudder = CommandConnect.Instance.getRudder();
            
            string fileName = (string)Session["fileName"];
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt";

            using (StreamWriter streamWriter = System.IO.File.AppendText(filePath))
            {
                streamWriter.WriteLine(Convert.ToString(lon) + ',' + Convert.ToString(lat) + ',' + Convert.ToString(throttle) + ',' + Convert.ToString(rudder));
            }
            return ToXml(lat, lon);
        }




        [HttpGet]
        public ActionResult updatedDisplay(string ip, int port, int second)
        {
            ViewBag.time = second;
            ViewBag.ip = ip;
            ViewBag.port = port;
            Debug.WriteLine("we are in updatedDisplay");
            CommandConnect.Instance.connect(ip, port);
            Session["time"] = second;
            return View();
        }


        [HttpGet]
        public ActionResult readDataBase(string file, int second)
        {
            //string fileName = (string)Session["fileName"];
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + file + ".txt";
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
            string[] files = System.IO.File.ReadAllLines(path);
            Session["time"] = second;
            Array.Resize(ref files, files.Length + 1);
            files[files.Length - 1] = "End,End";
            Session["arrayfile"] = files;
            return View("readDataBase");
        }


        [HttpPost]
        public string readDataBase4()
        {

            string[] files = (string[]) Session["arrayfile"];
            string line = files[0];
            string[] words = line.Split(',');
            files = files.Skip(1).ToArray();
            Session["arrayfile"] = files;
            if ((words[0]!="End")&& (words[1] != "End")){
                double lon = Convert.ToDouble(words[0]);
                double lat = Convert.ToDouble(words[1]);
                return ToXml(lat, lon);

            }
            return ToXml(-1000,-1000);
        }



        [HttpPost]
        public string getInfoDisplay4()
        {
            Debug.WriteLine("ppppppppppppppppppp");
            double lat = CommandConnect.Instance.getLat();
            double lon = CommandConnect.Instance.getLon();
            return ToXml(lat, lon);
        }



        private string ToXml(double lat,double lon)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Position");

            writer.WriteElementString("Lon", lon.ToString());
            writer.WriteElementString("Lat", lat.ToString());


            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }



       


    }
}