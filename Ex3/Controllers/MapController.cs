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

namespace Ex3.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult display(string ip, int port) 
        {
            CommandConnect.Instance.connect(ip, port);
                      
                ViewBag.lat = CommandConnect.Instance.getLat();
                ViewBag.lon = CommandConnect.Instance.getLon();
                
                       

            return View();
        }

        [HttpGet]
        public ActionResult updatedDisplay(string ip, int port, int second)
        {
            Debug.WriteLine("we are in updatedDisplay");
            CommandConnect.Instance.connect(ip, port);
            Session["time"] = second;
            return View();
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




        public ActionResult save(string ip, int port, int times, int second, string file)
        {
            CommandConnect.Instance.connect(ip, port);

            ViewBag.lat = CommandConnect.Instance.getLat();
            ViewBag.lon = CommandConnect.Instance.getLon();

            return View();
        }





    }
}