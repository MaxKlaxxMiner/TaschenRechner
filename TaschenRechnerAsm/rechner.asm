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

  mov rax, r9
  shr rax, 1

@for_loop:

  mov r9, [rdx]
  mov r10, [rdx + 8]
  lea rdx, [rdx + 16]

  adc r9, [r8]
  adc r10, [r8 + 8]
  lea r8, [r8 + 16]

  mov [rcx], r9
  mov [rcx + 8], r10
  lea rcx, [rcx + 16]

  dec rax
jnz @for_loop

  adc rax, rax
  
ret
AddAsmX2 endp

; ulong mpn_add_n(ulong* rp, ulong* up, ulong* vp, long n)
align 16
mpn_add_n proc export

  mov r11, rcx
  mov rcx, r9

  mov rax, rcx
  shr rcx, 2
  and rax, 3

jrcxz lt4

  push rdi
  push rsi

  mov r10, [rdx]
  mov r9, [rdx + 8]
  dec rcx
jmp mid

lt4:
  mov r10, [rdx]
  dec rax
jnz l2
  adc r10, [r8]
  mov [r11], r10
  adc rax, rax
ret

l2:
  dec rax
  mov r9, [rdx + 8]
jnz l3
  adc r10, [r8]
  adc r9, [r8 + 8]
  mov [r11], r10
  mov [r11 + 8], r9
  adc rax, rax
ret

l3:
  mov rcx, [rdx + 16]
  adc r10, [r8]
  adc r9, [r8 + 8]
  adc rsi, [r8 + 16]
  mov [r11], r10
  mov [r11 + 8], r9
  mov [r11 + 16], rcx
  setc al
ret

  align 16
top:
  adc r10, [r8]
  adc r9, [r8 + 8]
  adc rsi, [r8 + 16]
  adc rdi, [r8 + 24]
  mov [r11], r10
  lea rdx, [rdx + 32]
  mov [r11 + 8], r9
  mov [r11 + 16], rsi
  dec rcx
  mov [r11 + 24], rdi
  lea r8, [r8 + 32]
  mov r10, [rdx]
  mov r9, [rdx + 8]
  lea r11, [r11 + 32]
mid:
  mov rsi, [rdx + 16]
  mov rdi, [rdx + 24]
jnz top

  lea rdx, [rdx + 32]
  adc r10, [r8]
  adc r9, [r8 + 8]
  adc rsi, [r8 + 16]
  adc rdi, [r8 + 24]
  lea r8, [r8 + 32]
  mov [r11], r10
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
