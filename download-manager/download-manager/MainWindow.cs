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
        readonly OptionsForm m_optionsForm;
        public static int m_numberOfSimultaneousDownloads;

        public MainWindow()
        {
            InitializeComponent();
            m_isSaveSelected = false;
            m_selectedPath = "";
            m_column = new DataGridViewProgressColumn();
            m_Downloader = new FileDownload { m_DataGrid = downloadDataGridView };
            ManageDownloadDataGridView();
            m_optionsForm = new OptionsForm();

            if (File.Exists("F:\\save.bin"))
            {
                m_numberOfSimultaneousDownloads = OptionsForm.ReadFromBinaryFile<int>("F:\\save.bin");
            }
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

            object[] arow = new object[] { "1604.exe", "F:\\1604.exe", "0 KB/s", 0 };
            downloadDataGridView.Rows.Add(arow);
            m_Downloader.m_UrlQueue.Enqueue("https://web.whatsapp.com/desktop/windows/release/x64/WhatsAppSetup.exe");

            object[] arow2 = new object[] { "1605.exe", "F:\\1605.exe", "0 KB/s", 0 };
            downloadDataGridView.Rows.Add(arow2);
            m_Downloader.m_UrlQueue.Enqueue("https://web.whatsapp.com/desktop/windows/release/x64/WhatsAppSetup.exe");

            object[] arow3 = new object[] { "1606.exe", "F:\\1606.exe", "0 KB/s", 0 };
            downloadDataGridView.Rows.Add(arow3);
            m_Downloader.m_UrlQueue.Enqueue("https://web.whatsapp.com/desktop/windows/release/x64/WhatsAppSetup.exe");

            object[] arow4 = new object[] { "1607.exe", "F:\\1607.exe", "0 KB/s", 0 };
            downloadDataGridView.Rows.Add(arow4);
            m_Downloader.m_UrlQueue.Enqueue("https://web.whatsapp.com/desktop/windows/release/x64/WhatsAppSetup.exe");

            m_isSaveSelected = true;
            m_isUrlSelected = true;
        }

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
            if (m_isSaveSelected)
            {
                int rowsCountDataGridView = downloadDataGridView.RowCount - 1;

                for (int i = 0; i < downloadDataGridView.RowCount - 1;)
                {
                    FileDownload[] fileDownloads = new FileDownload[m_numberOfSimultaneousDownloads];

                    for (int j = 0; j < m_numberOfSimultaneousDownloads; j++)
                    {
                        fileDownloads[j] = new FileDownload();
                    }

                    if (rowsCountDataGridView < m_numberOfSimultaneousDownloads)
                    {
                        m_numberOfSimultaneousDownloads = rowsCountDataGridView;
                    }

                    for (int k = 0; k < m_numberOfSimultaneousDownloads; k++, i++)
                    {
                        int index = k;
                        int rowIndex = i;
                        fileDownloads[index].DownloadProgressChanged += DownloadProgressChanged;
                        fileDownloads[index].DownloadCompleted += DownloadCompleted;
                        fileDownloads[index].m_UrlQueue.Enqueue(m_Downloader.m_UrlQueue.Dequeue());
                        string downloadPath = downloadDataGridView.Rows[rowIndex].Cells[1].Value.ToString();
                        Task.Factory.StartNew(() => fileDownloads[index].Start(downloadPath, rowIndex));
                    }

                    Task.WaitAll();
                    rowsCountDataGridView -= m_numberOfSimultaneousDownloads;
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
            object[] row = { Path.GetFileName(m_fullPath), m_fullPath, "0 KB/s", 0 };

      

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
            m_optionsForm.ShowDialog();
        }
    }
}
