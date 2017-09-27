using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace BackupCheckExecutable
{
    class Program
    {
        public static List<string> configList = new List<string>();
        public static List<string> emailConfig = new List<string>();
        public static Boolean waitForTime = false;

    
        public static void wait()
        {
            string [] time;
            time = configList[2].Split(':');
            int hour, minute;
            hour = Convert.ToInt32(time[0]);
            minute = Convert.ToInt32(time[1]);
            Console.WriteLine("Waiting for " + hour + ":" + minute);

            
            while (true)
            {
                if (DateTime.Now.Hour == hour  && DateTime.Now.Minute == minute && DateTime.Now.Second == 00)
                {

                    Console.WriteLine("Time of day is: " + DateTime.Now.TimeOfDay);
                    Console.WriteLine("Time of day is now the selected time checking if backup was successfull...");

                    checkBackupSuccessful();
                }
                else
                {
                    Console.WriteLine("Time of day is: " + DateTime.Now.TimeOfDay);
                    Thread.Sleep(1000);
                }

            }
        }
        //run on startup
        static void Main(string[] args)
        {

            string line;
            int count = 0;
            StreamReader file = new StreamReader("Config.txt");

            while ((line = file.ReadLine()) != null)
            {
                configList.Add(line);
                count++;
            }

            file.Close();
            wait();
           

      
        }

        public static void checkBackupSuccessful()
        {
           
      
            Console.WriteLine("Started Store1 Backup Check");

            string[] directories = Directory.GetDirectories(configList[0], "*", SearchOption.TopDirectoryOnly);

            List<string> directoriesList = new List<string>();


            for (int x = 0; x < directories.Length; x++)
            {
                directoriesList.Add(directories[x]);
            }

            var match = directoriesList.FirstOrDefault(stringToCheck => stringToCheck.Contains("BaseBackup"));
            directoriesList.Remove(match);

            string testDir = directories[0];
            Regex regexObj = new Regex(@"\d{4}-\d{2}-\d{2}_\d{2}_\d{2}_\d{2}", RegexOptions.IgnoreCase);

            List<string> dateStrings = new List<string>();
            List<DateTime> backupDates = new List<DateTime>();
            for (int j = 0; j < directoriesList.Count; j++)
            {
                Match results = regexObj.Match(directoriesList[j]);
                while (results.Success)
                {
                    dateStrings.Add(results.Value);
                    results = results.NextMatch();
                }

                DateTime newDate = DateTime.ParseExact(dateStrings[j], "yyyy-MM-dd_HH_mm_ss", System.Globalization.CultureInfo.InvariantCulture);
                backupDates.Add(newDate);
            }
            Console.WriteLine("Dates Processed.....Listing Dates");

            backupDates.Sort();
            for (int t = 0; t < backupDates.Count; t++)
            {
                Console.WriteLine(t + ".  " + backupDates[t]);
            }

            DateTime lastBackup = backupDates[backupDates.Count - 2];

            //if backup day is the the date 1 day before the current date
            if (lastBackup.DayOfWeek != DateTime.Today.AddDays(-1).DayOfWeek)
            {
                Console.WriteLine("Backup was unsuccessful.\nBackup was not made on " + DateTime.Today.AddDays(-1));
                Console.WriteLine("Invoking Outlook to send notification email");
                sendEmailUnsuccess();
            }
            else
            {
                Console.WriteLine("Previous days backup was successfull.\nBackup was made at: " + backupDates[backupDates.Count - 2]);
                if (configList[1].Equals("1"))
                {
                    Console.WriteLine("Invoking Outlook to send notification email");
                    sendEmailSuccess();
                }
            }
        }

        public static void sendEmailUnsuccess()
        {
            Console.WriteLine("Sending email informing of unsuccessful backup");
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                
                //add the body of the email
                oMsg.HTMLBody = "Hello,<br><br>This message is to inform you that Store1 was <b>not successfully backed up yesterday</b> " + "<u>" + DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy") + "</u>." + "<br><br>This means the backup from day <u>" + DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy") + "</u> is missing. As a result please run <b>Store1Sync</b> to resolve this issue." + "<br><br>Thanks,<br><br>Email sent curtosy of the <i>Backup Integrity Check Service</i><br><br><b><font color='red'>This is an automated message.</font></b>";
                //Add an attachment.
                //  String sDisplayName = "MyAttachment";
                int iPosition = (int)oMsg.Body.Length + 1;
                //   int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                //now attached the file
                //     Outlook.Attachment oAttach = oMsg.Attachments.Add(@"C:\\fileName.jpg", iAttachType, iPosition, sDisplayName);
                //Subject line
                oMsg.Subject = "Store1 Backup Report - " + DateTime.Today.ToString("MM/dd/yyyy") +" (ACTION REQUIRED)";
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = null;
                for (int i = 3; i < configList.Count; i++)
                {
                    Console.WriteLine("Sending email to: " + configList[i]);
                    oRecip = (Outlook.Recipient)oRecips.Add(configList[i]);
                }
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                Console.WriteLine("Email has been successfully sent.");
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
            }//end of try block
            catch (Exception ex)
            {
                Console.WriteLine("ERROR THROWN " + ex);
            }//end of catch
            wait();
        }

        public static void sendEmailSuccess()
        {
            Console.WriteLine("Sending email informing of successful backup");
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                oMsg.HTMLBody = "Hello,<br><br>This message is to inform you that Store1 was <b><font color='green'>successfully backed yesterday</font></b> " + "<u>" + DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy") + "</u>." + "<br><br>No action is needed." + "<br><br>Thanks,<br><br><i>Email Sent Courtesy of Backup Integrity Check Service</i><br><br><b><font color='red'>This is an automated message.</font></b>";
                //Add an attachment.
                //  String sDisplayName = "MyAttachment";
                int iPosition = (int)oMsg.Body.Length + 1;
                //   int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                //now attached the file
                //     Outlook.Attachment oAttach = oMsg.Attachments.Add(@"C:\\fileName.jpg", iAttachType, iPosition, sDisplayName);
                //Subject line
                oMsg.Subject = "Store1 Backup Report - " + DateTime.Today.ToString("MM/dd/yyyy");
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip=null;
                for (int i = 3; i < configList.Count; i++)
                {
                    Console.WriteLine("Sending email to: " + configList[i]);
                    oRecip = (Outlook.Recipient)oRecips.Add(configList[i]);
                }    
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                Console.WriteLine("Email has been successfully sent.");
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
            }//end of try block
            catch (Exception ex)
            {
                Console.WriteLine("ERROR THROWN " + ex);
            }//end of catch
            wait();

        }
    }
}
  