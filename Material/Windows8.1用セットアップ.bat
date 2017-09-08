@echo off
echo ---------------------------------------------------------------------
echo. 
echo          ITunEsTooLのセットアップを行います。
echo          ※【超注意！】Windows7より前のOSの場合は、
echo          使用せずに×ボタンを押してください。
echo          又、「管理者として実行(A)...」で必ず実行して下さい。
echo. 
echo ---------------------------------------------------------------------
pause
rd /s/q "%USERPROFILE%\Desktop\ITunEsTooL.lnk"
rd /s/q "C:\Program Files\ITunEsTooL"
mkdir "C:\Program Files\ITunEsTooL"
%~d0
cd %~d0%~p0
cd
copy .\Files\GAPI.dll  "C:\Program Files\ITunEsTooL\GAPI.dll"
copy .\Files\Interop.iTunesLib.dll  "C:\Program Files\ITunEsTooL\Interop.iTunesLib.dll"
copy .\Files\ITunEsTooL.config  "C:\Program Files\ITunEsTooL\ITunEsTooL.config"
copy .\Files\ItunEsTooL.exe  "C:\Program Files\ITunEsTooL\ItunEsTooL.exe"
copy .\Files\アートワーク.pdf  "C:\Program Files\ITunEsTooL\アートワーク.pdf"
copy .\Files\フォルダ作成.pdf  "C:\Program Files\ITunEsTooL\フォルダ作成.pdf"
copy .\Files\メンテナンス.pdf  "C:\Program Files\ITunEsTooL\メンテナンス.pdf"
copy .\Files\設定.pdf  "C:\Program Files\ITunEsTooL\設定.pdf"
mkdir "C:\Program Files\ITunEsTooL\Artwork"
mkdir "C:\Program Files\ITunEsTooL\Log"
echo ---------------------------------------------------
echo. 
echo     ITunEsTooLのインストールが完了しました！
echo. 
echo ---------------------------------------------------
pause
Shortcut /t:"C:\Program Files\ITunEsTooL\ItunEsTooL.exe" /w:"C:\Program Files\ITunEsTooL" /s:1 "%USERPROFILE%\Desktop\ITunEsTooL.lnk"