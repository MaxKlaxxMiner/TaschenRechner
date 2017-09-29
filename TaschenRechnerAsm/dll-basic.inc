option casemap :none

.code

nops macro x:req
  if x eq 1
    db 144
  endif
  if x eq 2
    db 102,144
  endif
  if x eq 3
    db 15,31,0
  endif
  if x eq 4
    db 15,31,64,0
  endif
  if x eq 5
    db 15,31,68,0,0
  endif
  if x eq 6
    db 102,15,31,68,0,0
  endif
  if x eq 7
    db 15,31,128,0,0,0,0
  endif
  if x eq 8
    db 15,31,132,0,0,0,0,0
  endif
  if x eq 9
    db 102,15,31,132,0,0,0,0,0
  endif
  if x eq 10
    db 102,102,15,31,132,0,0,0,0,0
  endif
  if x ge 11
    db 102,102,102,15,31,132,0,0,0,0,0
    nops x - 11
  endif
endm

main proc
  push 1
  pop rax
ret
main endp

; int GetAlignPointers(long[] pointers)
;
GetAlignPointers proc export
  mov rax, alignPointersCount
  mov rdx, rax
  lea r8, [alignPointers]

@for:
  mov r9, [r8]
  add r8, 8
  mov [rcx], r9
  add rcx, 8

  dec rdx
jnz @for

ret
GetAlignPointers endp
