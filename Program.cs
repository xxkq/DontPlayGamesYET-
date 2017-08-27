using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace T1_SelfCAttempt_Solution1
{
    class Program
    {

        //-----------------------------------------------------------------------------------------------
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);
        //-----------------------------------------------------------------------------------------------
        static bool LogHndl(string msgout)
        {
            
            FileStream fout = new FileStream("C:\\Users\\Public\\ProgLog\\T2-SelfCAttempt.log", FileMode.Append);
            //string curtime = DateTime.Now.ToString() + " ----> ";
            StreamWriter sw = new StreamWriter(fout);
            Console.SetOut(sw);
            Console.WriteLine(msgout);
            sw.Close();
            return true;
        }
        static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
        static void getProcList()
        {
            try
            {

                Process[] processlist = Process.GetProcesses();
                /*
                TimeSpan start = new TimeSpan(19, 0, 0); 
                TimeSpan end = new TimeSpan(21, 0, 0); 
                TimeSpan now = DateTime.Now.TimeOfDay;
                */

                foreach (Process theprocess in processlist)
                {
                    //LogHndl(DateTime.Now.ToString() + " ----> " + "Process: { 0} ID: { 1}", theprocess.ProcessName, theprocess.Id);
                    //Console.Write(theprocess.ProcessName);
                    //Console.Write(" " + theprocess.Id + " \n");
                    string procs = theprocess.ToString();
                    if (procs.Contains("csgo") || procs.Contains("rainbowsix") || procs.Contains("javaw")
                        || procs.Contains("Warframe") || procs.Contains("steam") || procs.Contains("Steam")
                        || procs.Contains("RainbowSix") || procs.Contains("DyingLight")
                        || procs.Contains("Broforce") || procs.Contains("cobalt") || procs.Contains("BF2")
                        /*|| procs.Contains("")*/)
                    {
                        LogHndl(DateTime.Now.ToString() + " ----> " + " ----> " + theprocess.ProcessName + " is about to get killed!");
                        theprocess.Kill();
                        //LogHndl(DateTime.Now.ToString() + " ----> " + "\n\n" + theprocess.ProcessName + " has been killed!\n\n");
                    }
                }
                //LogHndl(DateTime.Now.ToString() + " ----> " + "EOFunc achieved!");
                System.Threading.Thread.Sleep(5000);
                getProcList();
            }
            catch(Exception x)
            {
                FileInfo fi = new FileInfo("C:\\Users\\Public\\ProgLog\\T2-SelfCAttempt.log");
                if(IsFileLocked(fi) == true)
                {
                    
                }
                LogHndl(DateTime.Now.ToString() + " ----> ERROR ----> " + x.Message);
                getProcList();
            }
        }

        static void Main(string[] args)
        {
            //---------------------------------------------------------------------------------------------------------------
            int isCritical = 1;  // we want this to be a Critical Process
            int BreakOnTermination = 0x1D;  // value for BreakOnTermination (flag)
            Process.EnterDebugMode();  //acquire Debug Privileges
            NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
            //---------------------------------------------------------------------------------------------------------------
            getProcList();
            //LogHndl(DateTime.Now.ToString() + " ----> " + "Press enter to continue...");
            //Console.ReadLine();
        }
    }
}
