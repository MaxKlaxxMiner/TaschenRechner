include dll-basic.inc

.data

tmp db 1

.code

; params: rcx, rdx, r8, r9, [rsp + 40], [rsp + 48]


; ulong UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n)
;                 rcx,       rdx,       r8,        r9
align 16
UIntX_Add proc export

;|  cy: ?  |  rax: ?  |  rcx: rp  |  rdx: up  |  r8: vp  |  r9: n  |  r10: ?  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  mov r10, rcx
;|  cy: ?  |  rax: ?  |  rcx: rp  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  mov rcx, r9
;|  cy: ?  |  rax: ?  |  rcx: n  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  xor rax, rax
;|  cy: ?  |  rax: 0  |  rcx: n  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  shr rcx, 1
;|  cy: n % 2  |  rax: 0  |  rcx: n / 2  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  jnc @l2
;|  cy: 1  |  rax: 0  |  rcx: n / 2  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi

  mov r11, [rdx]
;|  cy: 1  |  rax: 0  |  rcx: n / 2  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: up[0]  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  add r11, [r8]
;|  cy: c  |  rax: 0  |  rcx: n / 2  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: up[0] + vp[0]  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  mov [r10], r11

  jrcxz @end

;  lea rdx, [rdx + 8]
;  lea r8, [r8 + 8]
;  lea r10, [r10 + 8]

@l2:
;|  cy: 0  |  rax: 0  |  rcx: n / 2  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  shr rcx, 1
;|  cy: n / 2 % 2  |  rax: 0  |  rcx: n / 4  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  jnc @l4
;|  cy: 1  |  rax: 0  |  rcx: n / 4  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi

; - todo: add 2 limbs -

@l4:
;|  cy: 0  |  rax: 0  |  rcx: n / 4  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  shr rcx, 1
;|  cy: n / 4 % 2  |  rax: 0  |  rcx: n / 8  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  jnc @l8
;|  cy: 1  |  rax: 0  |  rcx: n / 8  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi

; - todo: add 4 limbs -

@l8:
;|  cy: 0  |  rax: 0  |  rcx: n / 8  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi

; - todo: add 8 limbs inner loop -

align 16
@end:

;|  cy: c  |  rax: 0  |  rcx: 0  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  adc rax, rax
;|  cy: c  |  rax: c  |  rcx: 0  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi

ret
UIntX_Add endp


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

align 16
lt4:
  mov r10, [rdx]
  dec rax
jnz l2
  adc r10, [r8]
  mov [r11], r10
  adc rax, rax
ret

align 16
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

align 16
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


end
