include dll-basic.inc

.data

tmp db 1

.code

; params: rcx, rdx, r8, r9, [rsp + 40], [rsp + 48]


; ulong AddAsm(ulong* rp, ulong* up, ulong* vp, long n)
;              rcx,       rdx,       r8,        r9
align 16
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
align 16
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

; ulong mpn_add_n(ulong* rp, ulong* up, ulong* vp, long n)
align 16
mpn_add_n proc export

  mov rdi, rcx
  mov rsi, rdx
  mov rdx, r8
  mov rcx, r9

  mov rax, rcx
  shr rcx, 2
  and rax, 3
jrcxz lt4

  mov r8, [rsi]
  mov r9, [rsi + 8]
  dec rcx
jmp mid

lt4:
  dec rax
  mov r8, [rsi]
jnz l2
  adc r8, [rdx]
  mov [rdi], r8
  adc rax, rax
ret

l2:
  dec rax
  mov r9, [rsi + 8]
jnz l3
  adc r8, [rdx]
  adc r9, [rdx + 8]
  mov [rdi], r8
  mov [rdi + 8], r9
  adc rax, rax
ret

l3:
  mov r10, [rsi + 16]
  adc r8, [rdx]
  adc r9, [rdx + 8]
  adc r10, [rdx + 16]
  mov [rdi], r8
  mov [rdi + 8], r9
  mov [rdi + 16], r10
  setc al
ret

  align 16
top:
  adc r8, [rdx]
  adc r9, [rdx + 8]
  adc r10, [rdx + 16]
  adc r11, [rdx + 24]
  mov [rdi], r8
  lea rsi, [rsi + 32]
  mov [rdi + 8], r9
  mov [rdi + 16], r10
  dec rcx
  mov [rdi + 24], r11
  lea rdx, [rdx + 32]
  mov r8, [rsi]
  mov r9, [rsi + 8]
  lea rdi, [rdi + 32]
mid:
  mov r10, [rsi + 16]
  mov r11, [rsi + 24]
jnz top

  lea rsi, [rsi + 32]
  adc r8, [rdx]
  adc r9, [rdx + 8]
  adc r10, [rdx + 16]
  adc r11, [rdx + 24]
  lea rdx, [rdx + 32]
  mov [rdi], r8
  mov [rdi + 8], r9
  mov [rdi + 16], r10
  mov [rdi + 24], r11
  lea rdi, [rdi + 32]

  inc rax
  dec rax
jnz lt4
  adc rax, rax
ret

mpn_add_n endp


end
