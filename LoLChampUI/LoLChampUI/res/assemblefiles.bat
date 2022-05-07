@echo off
"..\rabcdasm\rabcasm.exe" "..\asasm\0.0.1.30\AirGeneratedContent-0\AirGeneratedContent-0.main.asasm"
"..\rabcdasm\abcreplace.exe" "..\asasm\0.0.1.30\AirGeneratedContent.swf" 0 "..\asasm\0.0.1.30\AirGeneratedContent-0\AirGeneratedContent-0.main.abc"

"..\rabcdasm\rabcasm.exe" "..\asasm\0.0.1.30\resources-en_US-1\resources-en_US-1.main.asasm"
"..\rabcdasm\abcreplace.exe" "..\asasm\0.0.1.30\resources-en_US.swf" 1 "..\asasm\0.0.1.30\resources-en_US-1\resources-en_US-1.main.abc"