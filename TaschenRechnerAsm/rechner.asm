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

  mov r11, rcx
  mov r10, rdx
  mov rcx, r9

  mov rax, rcx
  shr rcx, 2
  and rax, 3

jrcxz lt4

  push rdi
  push rsi

  mov rdx, [r10]
  mov r9, [r10 + 8]
  dec rcx
jmp mid

lt4:
  mov rdx, [r10]
  dec rax
jnz l2
  adc rdx, [r8]
  mov [r11], rdx
  adc rax, rax
ret

l2:
  dec rax
  mov r9, [r10 + 8]
jnz l3
  adc rdx, [r8]
  adc r9, [r8 + 8]
  mov [r11], rdx
  mov [r11 + 8], r9
  adc rax, rax
ret

l3:
  mov rcx, [r10 + 16]
  adc rdx, [r8]
  adc r9, [r8 + 8]
  adc rsi, [r8 + 16]
  mov [r11], rdx
  mov [r11 + 8], r9
  mov [r11 + 16], rcx
  setc al
ret

  align 16
top:
  adc rdx, [r8]
  adc r9, [r8 + 8]
  adc rsi, [r8 + 16]
  adc rdi, [r8 + 24]
  mov [r11], rdx
  lea r10, [r10 + 32]
  mov [r11 + 8], r9
  mov [r11 + 16], rsi
  dec rcx
  mov [r11 + 24], rdi
  lea r8, [r8 + 32]
  mov rdx, [r10]
  mov r9, [r10 + 8]
  lea r11, [r11 + 32]
mid:
  mov rsi, [r10 + 16]
  mov rdi, [r10 + 24]
jnz top

  lea r10, [r10 + 32]
  adc rdx, [r8]
  adc r9, [r8 + 8]
  adc rsi, [r8 + 16]
  adc rdi, [r8 + 24]
  lea r8, [r8 + 32]
  mov [r11], rdx
  mov [r11 + 8], r9
  mov [r11 + 16], rsi
  mov [r11 + 24], rdi
  lea r11, [r11 + 32]

  pop rsi
  pop rdi

  inc rax
  dec rax
jnz lt4
  adc rax, rax
ret

mpn_add_n endp


end
