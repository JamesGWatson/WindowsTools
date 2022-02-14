using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashProgram
{
    /// <summary>
    /// Interaction logic for ListEntry.xaml
    /// </summary>
    
    public partial class ListEntry : UserControl
    {
        internal string Infos
        {
            get { return (labelExtras.Content as string); }
            set
            {
                labelExtras.Content = value;
            }
        }
        internal long LastProgress  = 0;
        internal long ReadSinceUpdate = 0;
        internal DateTime LastUpdate = DateTime.Now;
        internal DateTime ExpectedEnd = DateTime.Now;

        public delegate void ResetReadDelegate();
        public ResetReadDelegate ResetRead;

        public delegate void UpdateAllDelegate();
        public UpdateAllDelegate UpdateAll;

       
        internal void ResetReadMethod()
        {
            ReadSinceUpdate = 0;
            
        }

        internal void SetRead(long newdata)
        {
            ReadSinceUpdate = newdata;
        }

        internal void IncrementRead(long newdata)
        {
            ReadSinceUpdate += newdata;
        }

        internal void UpdateLabel(string newdata)
        {
            labelHashes.Content = newdata;
        }

        internal void UpdateAllMethod(long progress, string infos, long readvalue, DateTime now)
        {
            pBar.Value = progress;
            Infos = infos;
            ReadSinceUpdate = readvalue;
            LastUpdate = now;
        }

        public ListEntry()
        {
            InitializeComponent();
            //ResetRead = new ResetReadDelegate(ResetReadMethod);
            //UpdateAll = new UpdateAllDelegate(UpdateAllMethod());

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            double updated = ReadSinceUpdate;
            ReadSinceUpdate = 0;
            double seconds = (DateTime.Now - LastUpdate).TotalSeconds;
            float rate = (float)updated / 1024f / 1024f / (float)seconds; // MB/s, interval is 1/10 seconds
            pBar.Value += updated;
            labelExtras.Content = String.Format("{0:F1} MB/s", rate);
            LastUpdate = DateTime.Now;
        }

        private void labelHashes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (labelHashes.Tag != null)
            {
                Clipboard.SetText(labelHashes.Tag as string);
                MessageBox.Show(String.Format("Copied: {0}", labelHashes.Tag as string), "Clipboard", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
