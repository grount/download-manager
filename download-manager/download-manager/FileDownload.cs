using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace download_manager
{
    class FileDownload : IDownloader
    {
        Stopwatch m_StopWatch;
        public Queue<string> m_UrlQueue { get; set; }
        DownloadStatus e_DownloadStatus;
        public int m_BufferSize { get; set; }
        public int m_BytesSize { get; set; }
        public DataGridView m_DataGrid { get; set; }
        public string m_DownloadDestination { get; set; }

        public FileDownload()
        {
            m_UrlQueue = new Queue<string>();
            m_StopWatch = new Stopwatch();
            e_DownloadStatus = new DownloadStatus();
            m_BufferSize = 1024;

        }

        public void Start()
        {
            Download();
        }

        void Download()
        {
            WebRequest webRequest = null;
            WebResponse webResponse = null;
            Stream remoteStream = null;
            Stream localStream = null;

            // StreamReader reader = null;
            int bytesProcessed = 0;
            try
            {
                webRequest = WebRequest.Create(m_UrlQueue.Dequeue());

                if (webRequest != null)
                {
                    e_DownloadStatus = DownloadStatus.Downloading;
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                    webResponse = webRequest.GetResponse();
                    if (webResponse != null)
                    {
                        m_StopWatch.Start(); // TODOD when completed stop watch..
                        remoteStream = webResponse.GetResponseStream();
                        m_DownloadDestination = m_DataGrid.Rows[0].Cells[1].Value.ToString();
                        localStream = File.Create(m_DownloadDestination);

                        byte[] buffer = new byte[m_BufferSize];
                        int bytesRead;

                        do
                        {
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);
                            localStream.Write(buffer, 0, bytesRead);

                            bytesProcessed += bytesRead;
                        } while (bytesRead > 0);
                    }
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (webResponse != null) webResponse.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();
            }

        }

        private void UpdateDownloadProgress(int bytesRead) // TODO learn how to make event.
        {

        }
    }

}