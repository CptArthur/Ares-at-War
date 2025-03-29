@echo off
REM Set the Space Engineers installation directory
set SEInstallDir="G:\SteamLibrary\steamapps\common\SpaceEngineers"

REM Get the name of the current directory (assumed to be the mod's folder)
for %%I in (.) do set ParentDirName=%%~nxI

REM Upload the mod with SEWorkshopTool
%SEInstallDir%\Bin64\SEWorkshopTool.exe --upload --compile --mods "%ParentDirName%" --exclude .bat .psd .md --message patch_notes.md

REM Run the Python script to post the update to Discord
python update_discord_webhook.py

pause
