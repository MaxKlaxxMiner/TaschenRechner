include dll-basic.inc

.data

tmp db 1

.code

; params: rcx, rdx, r8, r9, [rsp + 40], [rsp + 48]


; ulong AddAsm(ulong* rp, ulong* up, ulong* vp, long n)
;              rcx,       rdx,       r8,        r9
AddAsm proc export

  lea rcx, [rcx + r9 * 8]
  lea rdx, [rdx + r9 * 8]
  lea r8, [r8 + r9 * 8]
  neg r9

  clc

@for_loop:
  mov r10, [rdx + r9 * 8]
  adc r10, [r8 + r9 * 8]
  mov [rcx + r9 * 8], r10

  inc r9
jnz @for_loop

  adc r9, 0
  mov rax, r9
  
ret
AddAsm endp


; ulong AddAsmX2(ulong* rp, ulong* up, ulong* vp, long n)
;               rcx,       rdx,       r8,        r9
AddAsmX2 proc export

  lea rcx, [rcx + r9 * 8]
  lea rdx, [rdx + r9 * 8]
  lea r8, [r8 + r9 * 8]
  neg r9

  clc

@for_loop:
  mov rax, [rdx + r9 * 8]
  mov r10, [rdx + r9 * 8 + 8]
  adc rax, [r8 + r9 * 8]
  adc r10, [r8 + r9 * 8 + 8]
  mov [rcx + r9 * 8], rax
  mov [rcx + r9 * 8 + 8], r10
  inc r9
  inc r9
jnz @for_loop

  adc r9, 0
  mov rax, r9
  
ret
AddAsmX2 endp


end
