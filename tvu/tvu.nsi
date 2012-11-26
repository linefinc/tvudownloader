; tvu.nsi
;
; "Tv Underground Downloader" installer script 
;
; 

;--------------------------------

; The name of the installer
Name "Tv Underground Downloader"

; The file to write
OutFile ".\publish\tvud_installer_0.5.4.exe"

; The default installation directory
InstallDir $PROGRAMFILES\Tvunderground_Downloader

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\TVUndergroundDownloader" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "Tvunderground Downloader (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File ".\bin\Release\tvu.exe"
  File "..\README.txt"
  
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
  CreateShortCut "$SMPROGRAMS\TVUnderground Downloader\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\TVUnderground Downloader\TVUndergroundDownloader.lnk" "$INSTDIR\tvu.exe" "" "$INSTDIR\tvu.exe" 0
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TVUndergroundDownloader"
  DeleteRegKey HKLM SOFTWARE\TVUndergroundDownloader

  ; Remove files and uninstaller
  Delete $INSTDIR\tvu.exe
  Delete $INSTDIR\README.txt
  Delete $INSTDIR\uninstall.exe

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\TVUnderground Downloader\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\TVUnderground Downloader"
  RMDir "$INSTDIR"

SectionEnd

