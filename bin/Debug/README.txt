BackupCheckExecutable README 
____________________________

Summary: The BackupCheckExecutables primary purpose is to check if the prior nights backup was successfull and
         send an email notification based on whether the backup was successfull or not. 




The configuration file "Config.txt" is used to: 

1. Outline the PATH of the "store1_backup" folder

2. Choose whether a email notification will be sent when the backup is successful 

3. Outline when the backup check should occur 

4. Outline the recipients who shall receive the email




How to setup Config.txt
_______________________


Line 0: Holds the PATH of the store1_backup folder 

Line 1: Holds whether a email should be sent pending a successfull backup (1 indicates yes send email AND 0 indicates no email)

Line 2: Holds when the back check should be run (PREFERABLY it should run the morning after the backup (Store1Sync) has occurred)

Line 3 - infinite: Holds the recipients who shall receive the email.

