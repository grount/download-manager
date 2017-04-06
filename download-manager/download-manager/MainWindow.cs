using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace download_manager
{
    public partial class MainWindow : Form
    {
        string m_downloadUrl;
        string m_selectedPath;
        string m_fullPath;
        bool m_isSaveSelected;
        bool m_isUrlSelected;
        DataGridViewProgressColumn m_column;

        public MainWindow()
        {
            InitializeComponent();
            m_isSaveSelected = false;
            m_selectedPath = "";
            m_column = new DataGridViewProgressColumn();

            ManageDownloadDataGridView();

        }

        private void ManageDownloadDataGridView()
        {
            downloadDataGridView.ColumnCount = 3;
            downloadDataGridView.Columns[0].Name = "File Name";
            downloadDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            downloadDataGridView.Columns[1].Name = "File Path";
            downloadDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            downloadDataGridView.Columns[2].Name = "Download Speed";
            downloadDataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            downloadDataGridView.Columns.Add(m_column);
            downloadDataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            m_column.HeaderText = "Progress";

            //object[] row1 = new object[] { "test1", "test2", 50 };
            //object[] row2 = new object[] { "test1", "test2", 55 };
            //object[] row3 = new object[] { "test1", "test2", 22 };
            //object[] rows = new object[] { row1, row2, row3 };

            //foreach (object[] row in rows)
            //{
            //    downloadDataGridView.Rows.Add(row);
            //}
        }

        private async Task HttpClientGetAsync()
        {
            if (m_isUrlSelected == true)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(m_selectedPath);
                var filetype = response.Content.Headers.ContentType.MediaType;
                var imageArray = await response.Content.ReadAsByteArrayAsync();
            }
        }

        private void urlTextBox_TextChanged(object sender, EventArgs e)
        {
            m_downloadUrl = urlTextBox.Text.ToString();

            if (m_downloadUrl != "")
            {
                m_isUrlSelected = true;
            }
        }

        private void SaveTheFileDialog()
        {
            string extenstion = Path.GetExtension(m_downloadUrl);
            string fileName = Path.GetFileName(m_downloadUrl);

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

        private async void DownloadButton_Click_1(object sender, EventArgs e)
        {
            if (m_isSaveSelected == true)
            {
                await startDownloadAsync();
                urlTextBox.Clear();
            }
            else
            {
                InvalidPath();
            }
        }

        private async Task startDownloadAsync()
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(m_downloadUrl);
            //wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
            //wc.DownloadProgressChanged += (sender, e) => downloadProgressBar.Value = e.ProgressPercentage;
            wc.DownloadProgressChanged += (sender, e) => downloadDataGridView.Rows[0].Cells[2].Value = e.ProgressPercentage;
            await wc.DownloadFileTaskAsync(uri, m_fullPath);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            SaveTheFileDialog();
            object[] row = new object[] { Path.GetFileName(m_fullPath).ToString(), m_fullPath, "0 kb/s", 0};
            downloadDataGridView.Rows.Add(row);
        }
    }
}
