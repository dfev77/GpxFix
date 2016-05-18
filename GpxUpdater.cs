using System;
using System.IO;
using System.Xml;

namespace GpxFix
{
    class GpxUpdater
    {
        private const string DefaultNamespace = "http://www.topografix.com/GPX/1/1";

        public void Process(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File '{0}' does not exist!", filePath);
                return;
            }

            Console.WriteLine("Updating '{0}'", filePath);

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            AddDate(doc);
            string newPath = GetOutputFileName(filePath);

            doc.Save(newPath);

            Console.WriteLine("New file written to '{0}'", newPath);
        }

        private void AddDate(XmlDocument gpxDocument)
        {
            DateTime date = new DateTime(1900, 1, 1);
            XmlNamespaceManager nsmanager = new XmlNamespaceManager(gpxDocument.NameTable);
            nsmanager.AddNamespace("x", DefaultNamespace);

            XmlNodeList nodesList = gpxDocument.SelectNodes("/x:gpx/x:trk/x:trkseg/x:trkpt", nsmanager);
            foreach (XmlNode node in nodesList)
            {
                XmlElement newNode = gpxDocument.CreateElement("time", gpxDocument.DocumentElement.NamespaceURI);
                newNode.InnerText = date.ToString("s") + "Z";

                node.AppendChild(newNode);
                date = date.AddSeconds(1);
            }
        }

        private string GetOutputFileName(string inputFileName)
        {
            FileInfo fileInfo = new FileInfo(inputFileName);
            string newPath = Path.Combine(fileInfo.Directory.FullName,
                                                    Path.GetFileNameWithoutExtension(fileInfo.Name) + ".updated");
            newPath = Path.ChangeExtension(newPath, fileInfo.Extension);

            return newPath;
        }
    }
}
