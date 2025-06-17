using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace ModpackDonwloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string modpackDownloadLink = Console.ReadLine();
            Directory.CreateDirectory("C:\\ModpackDownloaderTemp");
            using (var fileDownloader = new WebClient())
                fileDownloader.DownloadFile(modpackDownloadLink, "C:\\ModpackDownloaderTemp\\modpack.zip");
            try { ZipFile.ExtractToDirectory("C:\\ModpackDownloaderTemp\\modpack.zip", "C:\\ModpackDownloaderTemp"); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            StreamReader fileReader = new StreamReader("C:\\ModpackDownloaderTemp\\modrinth.index.json");
            string[,] modsNamesAndLinks = new string[1024, 2];
            int linkCounter = 0;
            int pathCounter = 0;
            StreamReader reader = new StreamReader("C:\\ModpackDownloaderTemp\\modrinth.index.json");
            while (reader.EndOfStream != true)
            {
                string currentModrinthFileLine = reader.ReadLine();
                if (currentModrinthFileLine.Contains("path"))
                {
                    string currentPath = currentModrinthFileLine;
                    string editedPath = null;
                    byte pathTextCheker = 0;
                    for (int i = 0; i < currentPath.Length; i++)
                    {
                        if (currentPath[i] == '"')
                        {
                            pathTextCheker++;
                            if(pathTextCheker == 3)
                            {
                                i++;
                                for (int j = 0; j < currentPath.Length - 22; j++)
                                    editedPath = editedPath + currentPath[i];
                            }
                        }
                    }
                    modsNamesAndLinks[linkCounter, pathCounter] = editedPath;
                    pathCounter++;
                }
                if (currentModrinthFileLine.Contains("downloads"))
                {
                    string currentLink = reader.ReadLine();
                    string editedLink = null;
                    for (int i = 0; i < currentLink.Length; i++)
                        if (currentLink[i] != ' ' || currentLink[i] != '"')
                            editedLink = editedLink + currentLink[i];
                    modsNamesAndLinks[linkCounter, pathCounter] = editedLink;
                    linkCounter++;
                }
            }
            foreach (var link in modsNamesAndLinks)
                Console.WriteLine(link);
            fileReader.Close();
            Console.ReadKey();
        }
    }
}
