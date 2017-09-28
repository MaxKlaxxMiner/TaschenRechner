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
  xor rax, rax
;|  cy: ?  |  rax: 0  |  rcx: rp  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  mov rcx, r9
;|  cy: ?  |  rax: 0  |  rcx: n  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  shr rcx, 1
;|  cy: n % 2  |  rax: 0  |  rcx: n / 2  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi
  jnc @l2
;|  cy: 1  |  rax: 0  |  rcx: n / 2  |  rdx: up  |  r8: vp  |  r9: n  |  r10: rp  |  r11: ?  |  n/a: r12, r13, r14, r15, r16, rsi, rdi

; - todo: add 1 limb -

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

@end:

; - todo: set carry-flag into rax -

ret
UIntX_Add endp

end
