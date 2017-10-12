# BackupCheckExecutable
Checks if backup was made previous night and sends email notification dependending on success or failure. Program was designed for company that blocks SMTP ports but uses Outlook for email.

Summary

Store1 Restoration Utility is software that allows the user to analyze, restore, and recover files or folders from mass store 1 backup. The software includes various functions like search by date range, custom directories search, and individual file/folder history. The goal of this software is to have an application that will make recovering data from the backup more efficient and less time consuming. Currently the backup is made arranged by folder whom names are the date of the backup. This makes searching for a particular file or folder that was backed up over 7 days hard to track (would involve going into each days folder and then subfolders to find what you are looking for) and also find the most recent version to restore. In this scenario the competition for the software would be windows explorer which has decent search capabilities for files and folders. To compete, the Store1 Restoration Utility has been specifically customised for store1 making the search faster than windows explorer and often easier if the user knows what they are looking for. 

The configuration file "Config.txt" is used to: 

1. Outline the PATH of the "store1_backup" folder

2. Choose whether a email notification will be sent when the backup is successful 

3. Outline when the backup check should occur 

4. Outline the recipients who shall receive the email

________________________________________________________

The configuration file "Config.txt" is used to: 

1. Outline the PATH of the "store1_backup" folder

2. Choose whether a email notification will be sent when the backup is successful 

3. Outline when the backup check should occur 

4. Outline the recipients who shall receive the email
