namespace Trading.Helpers
{
    using System.ComponentModel;
    using System.Windows.Forms;

    public static class BackgroundWorker
    {
        public static System.ComponentModel.BackgroundWorker Launch(DoWorkEventHandler doWork, ProgressChangedEventHandler progressChange)
        {
            var bw = new System.ComponentModel.BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            bw.DoWork += doWork;
            bw.ProgressChanged += progressChange;

            bw.RunWorkerCompleted += (s, args) =>
            {
                if (args.Error != null) // if an exception occurred during DoWork,
                    MessageBox.Show(args.Error.ToString()); // do your error handling here
            };

            bw.RunWorkerAsync();

            return bw;
        }
    }
}
