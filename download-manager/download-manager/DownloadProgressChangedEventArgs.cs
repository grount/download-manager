using System;

namespace download_manager
{
    public class DownloadProgressChangedEventArgs : EventArgs
    {
        public Int64 RecievedSize { get; private set; }
        public Int64 TotalSize { get; private set; }
        public int DownloadSpeed { get; private set; }
        public int ProgressPercentage { get; private set; }
        public int ThreadIndex { get; private set; }

        public DownloadProgressChangedEventArgs(Int64 recivedSize, Int64 totalSize, int downloadSpeed, int threadIndex)
        {
            RecievedSize = recivedSize;
            TotalSize = totalSize;
            DownloadSpeed = downloadSpeed;
            ProgressPercentage = (int)(100 * ((double)RecievedSize / TotalSize));
            ThreadIndex = threadIndex;
        }
    }
}
