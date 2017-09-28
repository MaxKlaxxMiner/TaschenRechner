include dll-basic.inc

.data

tmp db 1

.code

; params: rcx, rdx, r8, r9, [rsp + 40], [rsp + 48]


; ulong UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n)
;                 rcx,       rdx,       r8,        r9
align 16
UIntX_Add proc export

  xchg r9, rcx
  xor rax, rax

  ; - check n % 2 == 0 -
  shr rcx, 1
  jnc @l2

  ; - 1 limb -
  mov r10, [rdx]
  add r10, [r8]
  mov [r9], r10

  jrcxz @end ; - no more limbs -

  ; - move pointers 1 limb -
  lea rdx, [rdx + 8]
  lea r8, [r8 + 8]
  lea r9, [r9 + 8]

  adc rax, rax ; - save carry -

@l2:
  shr rcx, 1
  jnc @l4

  ; - 2 limbs -
  bt rax, 1 ; - reload carry -
  mov r10, [rdx]
  mov r11, [rdx + 8]
  adc r10, [r8]
  adc r11, [r8 + 8]
  mov [r9], r10
  mov [r9 + 8], r11

  jrcxz @end ; - no more limbs -

@l4:
  shr rcx, 1
  jnc @l8

; - todo: add 4 limbs -

@l8:

; - todo: add 8 limbs inner loop -

align 16
@end:

  adc rax, rax

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
