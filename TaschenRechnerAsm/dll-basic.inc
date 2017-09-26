;      .686                                      ; create 32 bit code
;      .model flat, stdcall                      ; 32 bit memory model
      option casemap :none                      ; case sensitive 

;     include files
;     ~~~~~~~~~~~~~
;      include \masm32\include\windows.inc       ; main windows include file
;      include \masm32\include\masm32.inc        ; masm32 library include

    ; -------------------------
    ; Windows API include files
    ; -------------------------
;      include \masm32\include\gdi32.inc
;      include \masm32\include\user32.inc
;      include \masm32\include\kernel32.inc
;      include \masm32\include\Comctl32.inc
;      include \masm32\include\comdlg32.inc
;      include \masm32\include\shell32.inc
;      include \masm32\include\oleaut32.inc
;      include \masm32\include\ole32.inc
;      include \masm32\include\msvcrt.inc

;      include \masm32\include\dialogs.inc       ; macro file for dialogs
;      include \masm32\macros\macros.asm         ; masm32 macro file

;     libraries
;     ~~~~~~~~~
;      includelib \masm32\lib\masm32.lib         ; masm32 static library

    ; ------------------------------------------
    ; import libraries for Windows API functions
    ; ------------------------------------------

;includelib "C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x64\gdi32.lib"
;includelib "C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x64\user32.lib"
;includelib "C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x64\kernel32.lib"
;includelib "C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x64\Comctl32.lib"
;includelib "C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x64\comdlg32.lib"
;includelib "C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x64\shell32.lib"
;includelib "C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x64\oleaut32.lib"
;includelib "C:\Program Files (x86)\Windows Kits\8.1\Lib\winv6.3\um\x64\ole32.lib"
;includelib "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\lib\amd64\msvcrt.lib"

CStr macro p1:vararg
local l1
.data
l1 db p1,0
.code
exitm <offset l1>
endm

.code

main proc
  xor rax, rax
  inc rax
ret
main endp
