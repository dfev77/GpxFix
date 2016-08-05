using System;
using System.IO;
using System.Xml;

namespace GpxFix
{
    class GpxUpdater
    {
        private const string NewFileSuffix = "_updated";
        private const string GpxFileExtension = ".gpx";

        private const string DefaultNamespace = "http://www.topografix.com/GPX/1/1";

        private void ProcessOneFile(string filePath, DateTime date)
        {
            Console.WriteLine("Updating '{0}'", filePath);

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            AddDate(doc, date);
            string newPath = GetOutputFileName(filePath);

            doc.Save(newPath);

            Console.WriteLine("New file written to '{0}'", newPath);

        }

        private void ProcessOneFile(string filePath)
        {
            ProcessOneFile(filePath, DateTime.UtcNow);
        }

        private void ProcessFolder(string folderPath)
        {
            DateTime startDate = DateTime.UtcNow;
            foreach(string filePath in Directory.GetFiles(folderPath, "*" + GpxFileExtension))
            {
                if (filePath.EndsWith(NewFileSuffix + GpxFileExtension))
                {
                    Console.WriteLine("Skip {0}, as is an already updated file", filePath);
                    continue;
                }

                ProcessOneFile(filePath, startDate);
                startDate = startDate.Subtract(TimeSpan.FromDays(1));
            }
        }

        public void Process(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                ProcessFolder(filePath);
            }
            else
            if (File.Exists(filePath))
            {
                ProcessOneFile(filePath);
            }
            else
            {
                Console.WriteLine("File '{0}' does not exist!", filePath);
                return;
            }

        }

        private void AddDate(XmlDocument gpxDocument, DateTime date)
        {
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
                                                    Path.GetFileNameWithoutExtension(fileInfo.Name) + NewFileSuffix)
                             + fileInfo.Extension;

            return newPath;
        }
    }
}
