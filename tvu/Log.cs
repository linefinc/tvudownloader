using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace  TvUndergroundDownloader
{

    public interface iLogTarget
    {
        void WriteText(string text);
    }

    sealed class Log
    {
        
        private static volatile Log instance;     // singleton istance
        private static object syncRoot = new Object();  // singleton sync object
        private List<iLogTarget> ListLogTarget; // log target
        private bool VerboseMode = false;

        public static Log Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Log();
                    }
                }

                return instance;
            }
        }

        public Log()
        {
            ListLogTarget = new List<iLogTarget>();
        }

        public void SetVerboseMode(bool value)
        {
            VerboseMode = value;
        }

        public static void logInfo (string text)
        {

            text = "[" + DateTime.Now.ToString() + "]" + text;
            if (text.IndexOf(Environment.NewLine) == -1)
            {
                text += Environment.NewLine;
            }
            
            Log.instance.ListLogTarget.ForEach(delegate(iLogTarget temp) { temp.WriteText(text); });
        }

        public static void logDebug(string text)
        {
#if DEBUG
            text = "[" + DateTime.Now.ToString() + "]" + text;
            if (text.IndexOf(Environment.NewLine) == -1)
            {
                text += Environment.NewLine;
            }

            Log.instance.ListLogTarget.ForEach(delegate(iLogTarget temp) { temp.WriteText(text); });
#endif
        }


        public static void logVerbose(string text)
        {
            if (Log.instance.VerboseMode == true)
            {
                text = "[" + DateTime.Now.ToString() + "]" + text;
                if (text.IndexOf(Environment.NewLine) == -1)
                {
                    text += Environment.NewLine;
                }

                Log.instance.ListLogTarget.ForEach(delegate(iLogTarget temp) { temp.WriteText(text); });
            }
        }



        public void AddLogTarget(iLogTarget target)
        {
            Log.instance.ListLogTarget.Add(target);
        }
        
    }
    /// <summary>
    /// Log Target File 
    /// </summary>
    public class LogTargetFile : iLogTarget
    {
        private string fileNameLog;
        private long fileLength = 0;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileNameLog">File name target</param>
        public LogTargetFile(string fileNameLog)
        {
            this.fileNameLog = fileNameLog;

            if (File.Exists(fileNameLog) == true)
            {
                FileInfo fi = new FileInfo(fileNameLog);
                fileLength = fi.Length;

            }
        }

        public void WriteText(string text)
        {
            // TODO: Optimizze with buffer
            
            fileLength += text.Length;
            
            StreamWriter w = File.AppendText(fileNameLog);
            w.WriteLine(text);
            w.Flush();
            w.Close();


            if (fileLength > 1000000)
            {

                FileStream fs = new FileStream(fileNameLog, FileMode.Open);

                int length = (int)fs.Length;


                byte[] buffer = new byte[length];

                fs.Read(buffer, 0, length);
                string str = System.Text.ASCIIEncoding.UTF8.GetString(buffer);

                int offset = length * 1 / 10; // 10%
                int start = str.IndexOf(Environment.NewLine, offset);
                str = str.Substring(start);
                str = str.TrimStart('\n', '\r', ' ');
                fs.Close();


                fs = new FileStream(fileNameLog, FileMode.Create);

                buffer = System.Text.ASCIIEncoding.UTF8.GetBytes(str);
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(buffer, 0, buffer.Length);

                fs.Close();
            }
        }

    }
    /// <summary>
    /// Log Target Text Box
    /// </summary>
    public class LogTargetTextBox : iLogTarget
    {
        private TextBox LogTextBox;
        private Form ParentForm;
        delegate void SetTextCallback(string text);


        public LogTargetTextBox(Form ParentForm,  TextBox LogTextBox)
        {
            this.LogTextBox = LogTextBox;
            this.ParentForm = ParentForm;
        }

        public void WriteText(string text)
        {
            // from msdn guide http://msdn.microsoft.com/en-us/library/ms171728%28VS.90%29.aspx
            if (this.LogTextBox.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(AppendText);
                ParentForm.Invoke(d, new object[] { text });
            }
            else
            {
                // It's on the same thread, no need for Invoke
                AppendText(text);
            }
        }

        private void AppendText(string text)
        {
            this.LogTextBox.Text += "[" + text.Substring(12); 
            this.LogTextBox.SelectionStart = this.LogTextBox.Text.Length;
            this.LogTextBox.ScrollToCaret();
            this.LogTextBox.Refresh();
            return;
        }
       
    }
}
