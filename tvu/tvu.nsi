; tvu.nsi
;
; "Tv Underground Downloader" installer script 
;
; 
;--------------------------------
;Include Modern UI
!include MUI2.nsh

;--------------------------------
; The name of the installer
Name "Tv Underground Downloader"

;--------------------------------
; The file to write
OutFile ".\bin\Release\tvud_installer_0.6.1.exe"
SetCompress force			; force compressor
SetCompressor /SOLID LZMA	; define lzma compressor

;--------------------------------
; The default installation directory
InstallDir $PROGRAMFILES\Tvunderground_Downloader

;--------------------------------
; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\TVUndergroundDownloader" "Install_Dir"


;--------------------------------
;Version Information
VIProductVersion "0.6.1.0"
VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "Tv Underground Downloader"
VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" "Tv Underground Downloader"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion" "0.6.1.0"

;--------------------------------
; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------
;Interface Settings
!define MUI_ABORTWARNING
!define MUI_FINISHPAGE_RUN "$INSTDIR\TvUndergroundDownloader.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Start application"

;--------------------------------
;Interface logo and icon
!define MUI_ICON ".\Resources\appicon1.ico"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP ".\Resources\logo.png"
!define MUI_HEADERIMAGE_RIGHT

;--------------------------------
; Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "..\license.txt"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
; define language
!insertmacro MUI_LANGUAGE "English"


Function .onInit
	
  ReadRegStr $R0 HKLM \
  "Software\Microsoft\Windows\CurrentVersion\Uninstall\TVUndergroundDownloader" \
  "UninstallString"
  StrCmp $R0 "" done
 
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
  "TVUnderground Downloader is already installed. $\n$\nClick `OK` to remove the \
  previous version or `Cancel` to cancel this upgrade." \
  IDOK uninst
  Abort
 
;Run the uninstaller
uninst:
  ClearErrors
  ExecWait '$R0 _?=$INSTDIR' ;Do not copy the uninstaller to a temp file
 
  IfErrors no_remove_uninstaller done
    ;You can either use Delete /REBOOTOK in the uninstaller or add some code
    ;here to remove the uninstaller. Use a registry key to check
    ;whether the user has chosen to uninstall. If you are using an uninstaller
    ;components page, make sure all sections are uninstalled.
  no_remove_uninstaller:
 
done:
 
FunctionEnd


; The stuff to install
Section "Tvunderground Downloader (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File ".\bin\Release\TvUndergroundDownloader.exe"
  File ".\bin\Release\TvUndergroundDownloader.exe.manifest"
  File ".\bin\Release\TvUndergroundDownloader.exe.config"
  File ".\bin\Release\System.Data.SQLite.dll"
  File ".\bin\Release\System.Data.SQLite.xml"
  File ".\bin\Release\System.Data.SQLite.Linq.dll"
  File /r ".\bin\Release\*.dll"
  File "..\README.txt"
  File "..\license.txt"
    
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\TVUndergroundDownloader "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TVUndergroundDownloader" "DisplayName" "TV Underground Downloader"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TVUndergroundDownloader" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TVUndergroundDownloader" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TVUndergroundDownloader" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\TVUnderground Downloader"
  CreateShortCut "$SMPROGRAMS\TVUnderground Downloader\TVUndergroundDownloader.lnk" "$INSTDIR\TvUndergroundDownloader.exe" "" "$INSTDIR\TvUndergroundDownloader.exe" 0
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TVUndergroundDownloader"
  DeleteRegKey HKLM SOFTWARE\TVUndergroundDownloader

  ; Remove files and uninstaller
  Delete $INSTDIR\TvUndergroundDownloader.exe
  Delete $INSTDIR\TvUndergroundDownloader.exe.manifest
  Delete $INSTDIR\TvUndergroundDownloader.exe.config
  Delete $INSTDIR\System.Data.SQLite.dll
  Delete $INSTDIR\System.Data.SQLite.xml
  Delete $INSTDIR\System.Data.SQLite.Linq.dll
  Delete $INSTDIR\README.txt
  Delete $INSTDIR\license.txt
  Delete $INSTDIR\uninstall.exe

  
  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\TVUnderground Downloader\*.*"

  ; Remove directories used
  RMDir /r "$SMPROGRAMS\TVUnderground Downloader"
  RMDir /r "$INSTDIR"

SectionEnd

