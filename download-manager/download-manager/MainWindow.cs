using System;
using System.IO;
using System.Net.Http;
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


        public MainWindow()
        {
            InitializeComponent();
            m_isSaveSelected = false;
            m_selectedPath = "";
            m_column = new DataGridViewProgressColumn();
            m_Downloader = new FileDownload {m_DataGrid = downloadDataGridView};
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
            downloadSaveFileDialog.ShowDialog();

            if (downloadSaveFileDialog.FileName != "")
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
                //for (; m_downloadListIndex < downloadDataGridView.RowCount - 1; m_downloadListIndex++) // TODO create multithreaded download
                //{
                //    await StartDownloadAsync();
                //}

                m_Downloader.DownloadProgressChanged += DownloadProgressChanged;
                
                Thread t = new Thread(m_Downloader.Start);
                t.Start();
                urlTextBox.Clear();
            }
            else
            {
                InvalidPath();
            }
        }


        //private async Task StartDownloadAsync()
        //{
        //    WebClient wc = new WebClient();
        //    Uri uri = new Uri(m_Downloader.m_UrlQueue.Dequeue());
        //    wc.DownloadProgressChanged += DownloadProgressChanged;
        //    wc.DownloadFileCompleted += (sender, e) =>
        //    {
        //        m_stopWatch.Reset();
        //        downloadDataGridView.Rows[m_downloadListIndex].Cells[2].Value = "0 KB/s";
        //    };
        //        m_stopWatch.Start();

        //    try
        //    {
        //        await wc.DownloadFileTaskAsync(uri, downloadDataGridView.Rows[m_downloadListIndex].Cells[1].Value.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void AddButton_Click(object sender, EventArgs e)
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

        private void DownloadProgressChangedHandler(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadDataGridView.Rows[0].Cells[4].Value = e.ProgressPercentage;
            downloadDataGridView.Rows[0].Cells[2].Value = $"{e.DownloadSpeed} KB/s";
            downloadDataGridView.Rows[0].Cells[3].Value =
                $"{(e.RecievedSize / 1024d / 1024d):0.00} MB / {(e.TotalSize / 1024d / 1024d):0.00} MB";
        }
    }
}
