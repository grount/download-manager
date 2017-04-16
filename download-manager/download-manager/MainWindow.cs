using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace download_manager
{
    public partial class MainWindow : Form
    {
        string m_selectedPath;
        string m_fullPath;
        bool m_isSaveSelected;
        bool m_isUrlSelected;
        DataGridViewProgressColumn m_column;
        FileDownload m_Downloader;
        private int m_ThreadIndex;

        public MainWindow()
        {
            InitializeComponent();
            m_isSaveSelected = false;
            m_selectedPath = "";
            m_column = new DataGridViewProgressColumn();
            m_Downloader = new FileDownload {m_DataGrid = downloadDataGridView};
            m_ThreadIndex = 0;
            ManageDownloadDataGridView();
        }

        private void ManageDownloadDataGridView()
        {
            downloadDataGridView.ColumnCount = 4;
            downloadDataGridView.Columns[0].Name = "File Name";
            downloadDataGridView.Columns[1].Name = "File Path";
            downloadDataGridView.Columns[2].Name = "Transfer Rate";
            downloadDataGridView.Columns[3].Name = "Data Downloaded";

            for (int i = 0; i < downloadDataGridView.ColumnCount; i++)
            {
                downloadDataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            downloadDataGridView.Columns.Add(m_column);
            downloadDataGridView.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            m_column.HeaderText = "Progress";
        }

        //private async Task HttpClientGetAsync()
        //{
        //    if (m_isUrlSelected == true)
        //    {
        //        HttpClient client = new HttpClient();
        //        var response = await client.GetAsync(m_selectedPath);
        //        var filetype = response.Content.Headers.ContentType.MediaType;
        //        var imageArray = await response.Content.ReadAsByteArrayAsync();
        //    }
        //}

        private void SaveTheFileDialog(string url)
        {
            string extenstion = Path.GetExtension(url);
            string fileName = Path.GetFileName(url);

            downloadSaveFileDialog.Title = "Save the file";
            downloadSaveFileDialog.FileName = fileName;
            downloadSaveFileDialog.Filter = "|*" + extenstion;

            if (downloadSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                m_isSaveSelected = true;
                m_fullPath = Path.GetFullPath(downloadSaveFileDialog.FileName);
            }
            else
            {
                m_isSaveSelected = false;
            }
        }

        private void InvalidPath()
        {
            MessageBox.Show("Please select a folder", "No folder selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DownloadButton_Click_1(object sender, EventArgs e)
        {
            if (m_isSaveSelected == true)
            {
                for (var i = 0; i < 2; i++) // downloadDataGridView.RowCount - 1
                {
                    for (m_ThreadIndex = 0; m_ThreadIndex < 2; m_ThreadIndex++)
                    {
                        m_Downloader.DownloadProgressChanged += DownloadProgressChanged;
                        m_Downloader.DownloadCompleted += DownloadCompleted;
                        var i1 = i;
                        Task.Factory.StartNew(() => m_Downloader.Start(i1));
                        i++;
                    }
                    Task.WaitAll();
                }
                urlTextBox.Clear();
            }
            else
            {
                InvalidPath();
            }
        }

        private void AddButton_Click(object sender, EventArgs e) // TOOD cancel in add, adds still
        {
            string url = urlTextBox.Text;
            m_Downloader.m_UrlQueue.Enqueue(url);

            if (url == "")
            {
                throw new ApplicationException("No url selected!");
            }

            m_isUrlSelected = true;
            SaveTheFileDialog(url);
            object[] row = new object[] { Path.GetFileName(m_fullPath), m_fullPath, "0 KB/s", 0 };
            downloadDataGridView.Rows.Add(row);

        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Invoke(new EventHandler<DownloadProgressChangedEventArgs>(DownloadProgressChangedHandler), sender, e);
        }

        private void DownloadCompleted(object sender, DownloadCompletedEventArgs e)
        {
            Invoke(new EventHandler<DownloadCompletedEventArgs>(DownloadCompletedEventHandler), sender, e);
        }

        private void DownloadCompletedEventHandler(object sender, DownloadCompletedEventArgs e)
        {
            m_Downloader.m_StopWatch.Reset();
            downloadDataGridView.Rows[e.m_CurrentThreadIndex].Cells[2].Value = "0 KB/s";
        }

        private void DownloadProgressChangedHandler(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadDataGridView.Rows[e.ThreadIndex].Cells[4].Value = e.ProgressPercentage;
            downloadDataGridView.Rows[e.ThreadIndex].Cells[2].Value = $"{e.DownloadSpeed} KB/s";
            downloadDataGridView.Rows[e.ThreadIndex].Cells[3].Value =
                $"{(e.RecievedSize / 1024d / 1024d):0.00} MB / {(e.TotalSize / 1024d / 1024d):0.00} MB";
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
