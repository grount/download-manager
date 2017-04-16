using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace download_manager
{
    interface IDownloader
    {
        long m_TotalSize { get; set; }
        void Start(int index);
    }
}
