@echo off
echo ---------------------------------------------------------------------
echo. 
echo          ITunEsTooL�̃Z�b�g�A�b�v���s���܂��B
echo          ���y�����ӁI�zWindows7���O��OS�̏ꍇ�́A
echo          �g�p�����Ɂ~�{�^���������Ă��������B
echo          ���A�u�Ǘ��҂Ƃ��Ď��s(A)...�v�ŕK�����s���ĉ������B
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
copy .\Files\�A�[�g���[�N.pdf  "C:\Program Files\ITunEsTooL\�A�[�g���[�N.pdf"
copy .\Files\�t�H���_�쐬.pdf  "C:\Program Files\ITunEsTooL\�t�H���_�쐬.pdf"
copy .\Files\�����e�i���X.pdf  "C:\Program Files\ITunEsTooL\�����e�i���X.pdf"
copy .\Files\�ݒ�.pdf  "C:\Program Files\ITunEsTooL\�ݒ�.pdf"
mkdir "C:\Program Files\ITunEsTooL\Artwork"
mkdir "C:\Program Files\ITunEsTooL\Log"
echo ---------------------------------------------------
echo. 
echo     ITunEsTooL�̃C���X�g�[�����������܂����I
echo. 
echo ---------------------------------------------------
pause
Shortcut /t:"C:\Program Files\ITunEsTooL\ItunEsTooL.exe" /w:"C:\Program Files\ITunEsTooL" /s:1 "%USERPROFILE%\Desktop\ITunEsTooL.lnk"