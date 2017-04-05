using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        bool m_isSaveSelected;
        bool m_isUrlSelected;

        public MainWindow()
        {
            InitializeComponent();
            m_isSaveSelected = false;
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            await httpClientGetAsync();
            saveTheFileDialog();

            if (m_isSaveSelected == false)
            {
                invalidPath();
            }
        }

        private async Task httpClientGetAsync()
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
            m_downloadUrl = urlTextBox.Text;

            if (m_downloadUrl != "")
            {
                m_isUrlSelected = true;
            }
        }

        private void saveTheFileDialog()
        {
            downloadSaveFileDialog.Title = "Save the file";
            downloadSaveFileDialog.ShowDialog();

            if (downloadSaveFileDialog.FileName != "")
            {
                m_isSaveSelected = true;
            }
            else
            {
                m_isSaveSelected = false;
            }
        }

        private void invalidPath()
        {
            MessageBox.Show("Please select a folder", "No folder selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            saveTheFileDialog();
        }
    }
}
