TV Underground Downloader

Copyright (C) 2017  linefinc[at]users.sourceforge.net

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
##### Version 0.9.1 26/01/2018  #####
* Improve stability
* Add wizard
* Minor UI update
* Add free space check
* Add support for page "https://tvunderground.org.ru/index.php?show=episodes&sid=XXXXXX"
* Add delete when completed (experimental)

##### Version 0.9.0 (Alpha) 03/09/2017  #####
* Added smtp ssl support
* Added embedded web server (now experimental future)
* Splitted the application core from UI 
* Added support for web UI
* A lots of small changes to improve code quality and stability

##### Version 0.8.1 (BUG FIX) 17/04/2017  #####
* Bug fix: Avoid crash with wrong xml configuration file
* Add logging for unmanaged errors

##### Version 0.8.0 12/04/2017  #####
* Rewrite all the core engine to be portable on Linux
* Deep code cleanup

##### Version 0.7.1 11/01/2017 #####
+ Bug fix 
+ Add small changes for a better support for future version 

##### Version 0.7.0 12/11/2016  #####
+ Add first support to aMule
+ Add import/export to simplify future migration
+ Add cookies section on options form (so we can manually setup)
* Rewrite eMule code to be extendible
* Improve speed

##### Version 0.6.2 07/08/2016  #####
* Enable workaround for old web browser component needs by Google CAPTCHA
* Add support for UTF8
* Minor fix


##### Version 0.6.1 18/02/2016  #####
* add support for windows 10
* spell check
* minor fix

##### Version 0.6.0 beta 27/12/2015  #####
+ add sqlite instead of XML database
+ add support for http://tvunderground.org.ru and http://www.tvunderground.org.ru as link
* improve memory usage
* improve performance
* better list view handling
* some clean up code

##### Version 0.5.6 27/09/2014 [git 6cc9157] #####
* Fix control on max simulations download
* Fix delete file from history
* Code clean up
* Change: now a duplicate feed is skipped by default

##### Version 0.5.6 01/02/2014 [git 9315b10] #####
* Update new TV Underground site web
+ Add pending List Box Pending file
+ Add support to Gzip in emule communication

##### Version 0.5.5 17/10/2013 [git a3b048a] ##### 
+ Add On Hiatus status
+ Add Edit dialog
+ Add "limit for feed"
+ Add customizable limit for feed
+ Add possibility to disable single feed
* Fix incorrect sorting on RSS list
* Fix reduce memory foot print
* Update project to vs2010es

##### Version 0.5.4 26/11/2012 [SVN rev 176] ##### 
* Fix bug when Ed2k link contains special characters
* Improve register value mapping
* Fix crash when SMTP server not replay
* Fix delete episode that non works correctly 

##### Version 0.5.3 10/10/2012 [SVN rev 167] ##### 
* Fix download limitator
+ Add new panel for add rss channel and remove obsolete function
* Fix ed2k parser
+ Add hash table to optimize search in history
+ Add ability to user to define update check interval
+ Add log trim to 1M
+ Add cache for feed link
+ Add single instance for application
* Fix mail sender 
* other small fix 

##### Version 0.5.2 15/10/2011 [svn rev 124] ##### 
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
*Fix URL parser to avoid duplicate links

##### Version 0.4 03/01/2011 ##### 
+Add new parser (more strong)
+Add auto scroll to log text
+Add Idle and error status for RSS feed
+Add Idle status for RSS feed after 15 days of inactivity
+RSS list are alphabetical sort
+Add Enable/Disable function in try bar
+Add alert panel to advise eMule will close
+Add function to delete file from history (double click on history list)
*Fix bug in delete RSS feed
*Fix Auto close status bar
*Fix Check now sometime not work

##### Version 0.3 03/08/2010 ##### 
+Add support for Windows 7, windows Vista configuration (No UAC need)
+Add Installer
+Add Start With Windows
+Add support configuration file in local user path
+Add Auto start eMule ( now not work)
+Redesign Configuration 
+Add Close eMule when all as done
+Add About panel
*Fix If http://tvunderground.org.ru/are not available can cause crash
*Fix Fix delay interval > 60 min not work
*Fix close X button. Now sand application in tray bar
*Fix auto start eMule. Sometime can crash
*Fix The log information not be correct store when application start minimized
*Fix error in start up minimized
*Fix tray bar bug. Now show correct menu indication "Show" or "Hide"
*Fix frieze windows when Download. Add background thread for manage network activity

#####  Version 0.2 01/08/2010 ##### 
First public release
