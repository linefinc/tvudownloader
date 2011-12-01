TV Undergroud Downloader

Copyright (C) 2011  linefinc[at]users.sourceforge.net

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.


---- Change Log ----
##### Version 0.5.2 15/10/2011 ##### 
*IMPORTANT fix to improve bandwidth control
*IMPORTANT fix to reduce fail socket
*Improve check for TV status (Complete , Running ...) , now it was check 1 time on 2 weeks
*Various GUI fix
*Fix auto close function than in some configuration not always works
*Add "Auto delete Complete channels" 



##### Version 0.5.1 02/06/2011 ##### 
*IMPORTANT fix to support TV underground site change
+Add send mail function
+Add OMPL exporter
+Add OMPL Importer (bat now not work)
+Add Show TV Underground status (Complete, incomplete...)

##### Version 0.5 09/04/2011 #####

This is a bug fix version with some add
+Add verbose log
+It's now possible exclude episodes when add RSS channel
+Add check for new version 
+Add process bar
+Add minimum file to start mule. 
+Start to implement OPML Import/Exporter (only in SVN version)
*Fix bug occur when user not use category 
*Fix RSS parser. A new parser are write to support not standard RSS channel.
*Fix Url parser to avoid duplicate links


##### Version 0.4 03/01/2011 ##### 

+Add new parser (more strong)
+Add autoscroll to log text
+Add Idle and error status for RSS feed
+Add Idle status for RSS feed after 15 days of inactivity
+RSS list are alphabetical sort
+Add Enable/Disable function in try bar
+Add alert panel to advise emule will close
+Add function to delete file from history (double click on history list)
*Fix bug in delete RSS feed
*Fix Auto close status bar
*Fix Check now sometime not work


##### Version 0.3 03/08/2010 ##### 

+Add support for Windows 7, windows Vista config (No UAC need)
+Add Installer
+Add Start With Windows
+Add support configuration file in local user path
+Add Auto start emule ( now not work)
+Redesign Config
+Add Close eMule when all as done
+Add About panel

*Fix If http://tvunderground.org.ru/are not available can cause crash
*Fix Fix delay interval > 60 min not work
*Fix close X button. Now sand application in tray bar
*Fix auto start emule. Sometime can crash
*Fix The log information not be correct store when application start minimized
*Fix error in start up minimized
*Fix tray bar bug. Now show correct menu indication "Show" or "Hide"
*Fix frieze windows when Download. Add background thread for manage network activity

#####  Version 0.2 01/08/2010 ##### 

First public release