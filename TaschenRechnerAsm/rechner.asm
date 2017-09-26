include dll-basic.inc

.data

tmp db 1

.code

; params: rcx, rdx, r8, r9, [rsp + 40], [rsp + 48]

AsmTest proc export

  mov rax, 123

ret
AsmTest endp

end
