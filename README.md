# Work in progress

# SteamDesktopAuthenticator-Mod-47
- A **beta** desktop implementation of Steam's Mobile Authenticator app.
- ★This modded version was created to have different GUI or/and  functions than the Original
- It was modified from the [official version](https://github.com/Jessecar96/SteamDesktopAuthenticator)
- This **mod is complete** has all the files needed to run the program

# Disclaimer:
**YOU ARE USING THIS PROGRAM AT YOUR OWN RISK!**

**THE SOFTWARE IS PROVIDED "AS IT IS", WITHOUT WARRANTY OF ANY KIND.**

**We (SteamDesktopAuthenticator contributors) provide no support or help in using this program.**

**Using this program puts you at a significant risk of losing your Steam account due to your own neglegence.**

--------------------

## ALWAYS MAKE BACKUPS OF:
- `maFiles` Folder
- revocation code
- encription key (if u choose to add one to the app)
- an entire SteamDesktopAuthenticator app including your `maFiles` Folder
- Backup Steam Guard Codes (Steam > Settings > Manage Steam Guard Account Security > Get Backup Steam Guard Codes)

## If you lose one of them your account may be lost.
  - if u lose your revocation code:
    - **Scenario 1:** if u have the `maFiles` Folder, you can try and decript the `.maFile` (if u added an encription) using this app, after that open the file with Notepad and search `revocation_code`
    - **Scenario 2:** if u have Backup Steam Guard Codes, you can remove your Authenticator using one of those codes
  - if u lose Backup Steam Guard Codes but u have the rest, get them again from steam
  - other situations, your account is gone. **Don't lose your data!!!**


--------------------

## [★Download v0.3.1.2](https://github.com/hyt47/SteamDesktopAuthenticator-Modded/releases)
- Last File Update 2015.12.16 12:56 PM (GMT +2 hours)

--------------------


**★ WARNING:**
  - **Do not delete or lose the maFiles directory! This will remove all your Steam account info!**
  - **Doing this will completely lock you out of your Steam account!**
  - If you don't want this program anymore, read the section below **"Remove authenticator:"**
  
--------------------

## Updating:
- **Method A:** (works on version v0.3.1.2 and newer versions)
  - Create a `New Folder`
  - Extract the new version inside it
  - Open `Steam Desktop Authenticator.exe` > File > Import .maFile
  - if the imported file is encripted it will be added as decrypted
  - after u add all your accounts u can ecrypt the data if u want. 
  
- **Method B:** (manual update - works on any version)
  - Create a `New Folder`
  - Extract the new version inside it
  - copy `maFiles` from your old program to this new one
  - Run the new version of `Steam Desktop Authenticator.exe` and test if u see your accounts
  - Arhive the old program and keep it as a backup
  - Move the new version where u want

**★ WARNING:**
  - **Do not delete or lose the maFiles directory! This will remove all your Steam account info!**
  - **Doing this will completely lock you out of your Steam account!**
  
--------------------

## Remove authenticator:
- **Method A:**
  - Open Steam > Settings > Manage Steam Guard Account Security > Remove Steam Guard Authenticator
  
- **Method B:**
  - Open your browser
    - go to https://store.steampowered.com/account/
    - Login
    - go to https://store.steampowered.com/twofactor/manage
    - Remove Steam Guard Authenticator
  
- **Method C:**
  - open `Steam Desktop Authenticator.exe` click "remove authenticator" button

--------------------

## Features:
- Generate login codes and confirm trades on multiple Steam accounts with ease.
- Enable Steam's mobile auth on new accounts.
- Encryption of sensitive account details.
- The Program runs only once
- Import maFile
- Resizable GUI
- Someone reported that this program supports only 8 accounts - [Add More Accounts Tutorial](https://github.com/hyt47/SteamDesktopAuthenticator-Modded/blob/master/Tutorial_Add_More_Accounts)
- Steam allows only 1 device/SteamDesktopAuthenticator to act as an Authenticator!

--------------------

## Errors:
- **XP App Error on startup:** application failed to initialize properly 0xc0000135
  - **Repair:** install Microsoft .NET Framework 4

- **XP App Error on button click Trade Confirmations:** The Underlying Connection Was Closed. Could Not Establish Trust Relationship for the SSL/TSL secure channel
  - **Repair:** You need [XP Service Pack 3 - WindowsXP-KB936929-SP3-x86-ENU.exe](https://www.microsoft.com/en-us/download/details.aspx?id=24)

--------------------

## Info:
- To compile I used Visual Studio 2013 and I was Running Windows 7 x64
- Tested in VMware with Windows XP x86 & Windows 7 x64

--------------------

##Thx to [Jessecar96](https://github.com/Jessecar96) (Official Developer) & [geel9](https://github.com/geel9)
- Now we have a program we can mod and use :)

##★ Mod creaded by Angelus (hyt47), Enjoy ^_^
