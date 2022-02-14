using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Security;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

namespace HashProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int BUFFER_SIZE = 1024 * 1024 * 10;
        //long currentProgress = 0;
        BackgroundWorker timer = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
        bool globalPause = false;
        //HashAlgorithm hash;
        string logHeader = "";
        string dirTo = "E:\\Destination";
        //todo: option to save txt log with file, name + date

        ParallelOptions pOptions = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0)) };


        public MainWindow()
        {
            InitializeComponent();

            timer.DoWork += delegate
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    timer.ReportProgress(0);
                }
            };
            timer.RunWorkerAsync();
        
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeTokenHandle()
                : base(true)
            {
            }

            [DllImport("kernel32.dll")]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [SuppressUnmanagedCodeSecurity]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool CloseHandle(IntPtr handle);

            protected override bool ReleaseHandle()
            {
                return CloseHandle(handle);
            }
        }


        private void DoAsUser(string user, string filename)
        {
            SafeTokenHandle sth;
            bool returnValue = LogonUser("Asgard", "local", "", 2, 0, out sth);

            SafeAccessTokenHandle sath = new SafeAccessTokenHandle(IntPtr.Zero);
            WindowsIdentity.RunImpersonated(sath, () => { Do(filename); });
        }

        private void Do(string filename)
        {
            //TODO: move into ListEntry

            //TODO: if directory, else if file
            //TODO: check if file already exists at destination


            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 3;
            //int filecount = files.Length;
            //Parallel.For(0, filecount, options, i =>
            //foreach (string file in files)
            foreach (string file in new string[] { filename })
            {
                //string file = files[i];
                string filenameonly = file.Substring(file.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);
                string newfile = dirTo + System.IO.Path.DirectorySeparatorChar + filenameonly;
                long currentProgress = 0;
                long thisRead = 0;
                //Dictionary<string, string> firstHashValues = new Dictionary<string, string>();
                Dictionary<string, HashAlgorithm> firstHashValues = new Dictionary<string, HashAlgorithm>();
                Dictionary<string, HashAlgorithm> secondHashValues = new Dictionary<string, HashAlgorithm>();
                long fileLength = 0;

                ListEntry le = new ListEntry();
                le.Background = this.Background;
                le.Foreground = this.Foreground;
                le.labelPath.Content = file;
                le.labelName.Content = filenameonly;
                le.labelHashes.Content = "Hashing and copying";

                wrapPanel1.Children.Add(le);

                List<HashAlgorithm> hashList = new List<HashAlgorithm>();
                if (cb_hashMD5.IsChecked ?? false) hashList.Add(MD5.Create());
                if (cb_hashSHA1.IsChecked ?? false) hashList.Add(SHA1.Create());
                if (cb_hashSHA256.IsChecked ?? false) hashList.Add(SHA256.Create());
                if (cb_hashSHA384.IsChecked ?? false) hashList.Add(SHA384.Create());
                if (cb_hashSHA512.IsChecked ?? false) hashList.Add(SHA512.Create());

                // surprisingly, this lot is run second, after the initial read and copy.
                foreach (HashAlgorithm h in hashList)
                {
                    string hashname = h.GetType().DeclaringType.FullName;
                    hashname = hashname.Substring(hashname.LastIndexOf(".") + 1);

                    secondHashValues.Add(hashname, HashAlgorithm.Create(hashname));
                }

                BackgroundWorker secondHasher = new BackgroundWorker();
                secondHasher.WorkerSupportsCancellation = true;
                secondHasher.DoWork += delegate (object ss, DoWorkEventArgs ee)
                {
                    FileInfo fileInfo = new FileInfo(newfile);
                    if (fileLength != fileInfo.Length / BUFFER_SIZE)
                    {
                            //le.labelHashes.Content = "File is wrong size."; //TODO: error here
                            //le.UpdateLabel("File is wrong size.");
                        }
                    fileInfo = null;

                    long position = 0;
                    byte[] buffer = new byte[BUFFER_SIZE];

                    using (FileStream fs = new FileStream(newfile, FileMode.Open))
                    {
                        fs.Seek(position, SeekOrigin.Begin);
                        int read = 0;

                        while ((read = fs.Read(buffer, 0, BUFFER_SIZE)) > 0 && !secondHasher.CancellationPending)
                        {
                            Parallel.ForEach(hashList, pOptions, h =>
                            //foreach (HashAlgorithm h in hashList)
                            {
                                string hashname = h.GetType().DeclaringType.FullName;
                                hashname = hashname.Substring(hashname.LastIndexOf(".") + 1);

                                secondHashValues[hashname].TransformBlock(buffer, 0, read, buffer, 0);
                            });
                            currentProgress += read / BUFFER_SIZE;
                                le.IncrementRead(read);
                                thisRead += read;
                        }
                    }

                    //quick? parallel not needed?
                    foreach (HashAlgorithm h in hashList)
                    {
                        string hashname = h.GetType().DeclaringType.FullName;
                        hashname = hashname.Substring(hashname.LastIndexOf(".") + 1);

                        secondHashValues[hashname].TransformFinalBlock(new byte[0], 0, 0);
                    }
                    ee.Result = secondHashValues;
                };

                secondHasher.RunWorkerCompleted += delegate (object ss, RunWorkerCompletedEventArgs ee)
                {
                    Dictionary<string, HashAlgorithm> results = (Dictionary<string, HashAlgorithm>)ee.Result;

                    foreach (KeyValuePair<string, HashAlgorithm> row in results)
                    {
                        //string thisHash = BytesToHexideximal(row.Value.Hash);
                        //bool match = thisHash == firstHashValues[row.Key];
                        //TODO: not string compare
                        bool match = String.Join("", row.Value.Hash) == String.Join("", firstHashValues[row.Key].Hash);  //row.Value.Hash.Equals(firstHashValues[row.Key].Hash);
                        //TODO: this will only work for the last one to complete...
                        le.labelHashes.Content = match ? "Hash match (click to copy)." : "Hashes do not match.";
                        le.labelHashes.Tag = BytesToHexideximal(row.Value.Hash); //TODO: dictionary
                    }                
                };


                /* ----------------- Initial hashing while copying  ------------------- */

                //HashAlgorithm hash = HashAlgorithm.Create(hashname);

                FileInfo fileInfo = new FileInfo(file); //TODO: check not directory
                                                        //TODO: save file info and start datetime to file/log, etc.
                fileLength = fileInfo.Length;// / BUFFER_SIZE;             
                le.pBar.Maximum = fileLength;
                le.pBar.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 200));
                fileInfo = null;

                BackgroundWorker hashAndCopy = new BackgroundWorker();
                hashAndCopy.WorkerReportsProgress = true;
                hashAndCopy.WorkerSupportsCancellation = true;

                
                foreach (HashAlgorithm h in hashList)
                {
                    string hashname = h.GetType().DeclaringType.FullName;
                    hashname = hashname.Substring(hashname.LastIndexOf(".") + 1);

                    firstHashValues.Add(hashname, HashAlgorithm.Create(hashname));
                }

                hashAndCopy.DoWork += delegate (object ss, DoWorkEventArgs ee)
                {
                    byte[] buffer = new byte[BUFFER_SIZE];

                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        fs.Seek(0, SeekOrigin.Begin);
                        int read = 0;
                        while ((read = fs.Read(buffer, 0, BUFFER_SIZE)) > 0 && !hashAndCopy.CancellationPending)
                        {
                            Parallel.ForEach(hashList, pOptions, (Action<HashAlgorithm>)(h =>
                            //foreach (HashAlgorithm h in hashList)
                            {
                                string hashname = h.GetType().DeclaringType.FullName;
                                hashname = hashname.Substring(hashname.LastIndexOf(".") + 1);

                                firstHashValues[hashname].TransformBlock(buffer, 0, read, buffer, 0);
                                
                                //hash.TransformBlock(buffer, 0, read, buffer, 0);
                                
                            }));

                            currentProgress += read / BUFFER_SIZE;
                            le.IncrementRead(read);
                            thisRead += read;

                            //definitely don't write byte for each algo
                            using (FileStream writer = new FileStream(newfile, FileMode.Append))
                            {
                                writer.Write(buffer, 0, read);
                            }
                        }
                    }
                    //hash.TransformFinalBlock(new byte[0], 0, 0);
                    foreach (HashAlgorithm h in hashList)
                    {
                        string hashname = h.GetType().DeclaringType.FullName;
                        hashname = hashname.Substring(hashname.LastIndexOf(".") + 1);

                        firstHashValues[hashname].TransformFinalBlock(new byte[0], 0, 0);
                    }

                    //string output = BytesToHexideximal(hash.Hash);

                    //ee.Result = hash;
                    //hash = null;
                    ee.Result = firstHashValues;
                };
                hashAndCopy.RunWorkerCompleted += delegate (object ss, RunWorkerCompletedEventArgs ee)
                {
                    le.pBar.Foreground = new SolidColorBrush(Color.FromRgb(0x6, 0xB0, 0x25));
                    le.pBar.Value = 0;
                    currentProgress = 0;
                    secondHasher.RunWorkerAsync();
                };

                hashAndCopy.RunWorkerAsync();
            }
        }

        private string BytesToHexideximal(byte[] input)
        {
            return (String.Join("",input.Select(x => $"{x:X2}")));;
        }

        private void TickAction()
        {
            //progressBarFile.Value = currentProgress;
        }

        private void wrapPanel1_DragEnter(object sender, DragEventArgs e)
        {
            // nothing?
        }

        private void wrapPanel1_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("FileDrop"))
            {
                foreach (string filename in e.Data.GetData("FileDrop") as object[])
                {
                    if (Directory.Exists(filename)) // is directory
                    {
                        //TODO: get recursive list
                        //TODO: prevent ui lockup
                        List<string> allSubFiles = Directory.EnumerateFiles(filename, "*.*", SearchOption.AllDirectories).ToList();
                        long size = allSubFiles.Sum(x => (new FileInfo(x)).Length);
                        // possibility to throuw "UnauthorizedAccessException" if user does not have access to some files???
                    }
                    else
                    {
                        DoAsUser(null, filename);
                    }
                }
            }
        }
    }
}
