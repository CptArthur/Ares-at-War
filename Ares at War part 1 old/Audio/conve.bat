@echo off
setlocal enabledelayedexpansion

for %%F in (*.wav) do (
    set "filename=%%~nF"
    
    REM Check if the WAV file has a valid format chunk
    ffprobe -hide_banner -show_format "%%F" >nul 2>&1
    if !errorlevel! EQU 0 (
        xWMAEncode "%%F" "!filename!.xwm"
    ) else (
        echo Skipping file: %%F
    )
)

echo Conversion completed.
pause