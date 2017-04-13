using System;
using System.IO;

namespace download_manager
{
    public class DownloadCompletedEventArgs : EventArgs
    {
        public int m_CurrentThreadIndex { get; private set; }

        public DownloadCompletedEventArgs(int currentThreadIndex)
        {
            m_CurrentThreadIndex = currentThreadIndex;
        }
    }
}