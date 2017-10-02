
; ulong UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n)
;                 rcx,       rdx,       r8,        r9
UIntX_Add proc export

  xor rax, rax

  ; - check n % 2 == 0 -
  shr r9, 1
  jnc @l2

  ; - 1 limb -
  mov r10, [rdx]
  add r10, [r8]
  mov [rcx], r10

  adc rax, rax ; - save carry -

  test r9, r9
  je @end ; - no more limbs -

  ; - move pointers for 1 limb -
  lea rdx, [rdx + 8]
  lea r8, [r8 + 8]
  lea rcx, [rcx + 8]

nops 3
@l2:
  shr r9, 1
  jnc @l4

  btr rax, 1 ; - reload carry -

  ; - 2 limbs -
  mov r10, [rdx]
  mov r11, [rdx + 8]
  adc r10, [r8]
  adc r11, [r8 + 8]
  mov [rcx], r10
  mov [rcx + 8], r11

  adc rax, rax ; - save carry -

  test r9, r9
  je @end ; - no more limbs -

  ; - move pointers for 2 limbs -
  lea rdx, [rdx + 16]
  lea r8, [r8 + 16]
  lea rcx, [rcx + 16]

nops 1
@l4:
  shr r9, 1
  jnc @l8

  btr rax, 1 ; - reload carry -

  ; - 4 limbs -
  mov r10, [rdx]
  mov r11, [rdx + 8]
  adc r10, [r8]
  adc r11, [r8 + 8]
  mov [rcx], r10
  mov [rcx + 8], r11
  mov r10, [rdx + 16]
  mov r11, [rdx + 24]
  adc r10, [r8 + 16]
  adc r11, [r8 + 24]
  mov [rcx + 16], r10
  mov [rcx + 24], r11

  adc rax, rax ; - save carry -

  test r9, r9
  je @end ; - no more limbs -

  ; - move pointers for 4 limbs -
  lea rdx, [rdx + 32]
  lea r8, [r8 + 32]
  lea rcx, [rcx + 32]

nops 1
@l8:
  shr r9, 1
  jnc @l16

  btr rax, 1 ; - reload carry -

  ; - 8 limbs -
  mov r10, [rdx]
  mov r11, [rdx + 8]
  adc r10, [r8]
  adc r11, [r8 + 8]
  mov [rcx], r10
  mov [rcx + 8], r11
  mov r10, [rdx + 16]
  mov r11, [rdx + 24]
  adc r10, [r8 + 16]
  adc r11, [r8 + 24]
  mov [rcx + 16], r10
  mov [rcx + 24], r11
  mov r10, [rdx + 32]
  mov r11, [rdx + 40]
  adc r10, [r8 + 32]
  adc r11, [r8 + 40]
  mov [rcx + 32], r10
  mov [rcx + 40], r11
  mov r10, [rdx + 48]
  mov r11, [rdx + 56]
  adc r10, [r8 + 48]
  adc r11, [r8 + 56]
  mov [rcx + 48], r10
  mov [rcx + 56], r11

  adc rax, rax ; - save carry -

  test r9, r9
  je @end ; - no more limbs -

  ; - move pointers for 8 limbs -
  lea rdx, [rdx + 64]
  lea r8, [r8 + 64]
  lea rcx, [rcx + 64]

nops 1
@l16:
  ; - save registers -
  push r12
  push r13
  push r14
  push r15
  push rsi
  push rdi

  btr rax, 1 ; - reload carry -

nops 1
@loop:
  ; - 16 limbs -
  mov r10, [rdx]
  mov r11, [rdx + 8]
  mov r12, [rdx + 16]
  adc r10, [r8]
  mov r13, [rdx + 24]
  adc r11, [r8 + 8]
  mov r14, [rdx + 32]
  adc r12, [r8 + 16]
  mov r15, [rdx + 40]
  adc r13, [r8 + 24]
  mov rsi, [rdx + 48]
  adc r14, [r8 + 32]
  mov rdi, [rdx + 56]
  adc r15, [r8 + 40]
  mov [rcx], r10
  adc rsi, [r8 + 48]
  mov [rcx + 8], r11
  adc rdi, [r8 + 56]
  mov [rcx + 16], r12
  mov [rcx + 24], r13
  mov [rcx + 32], r14
  mov [rcx + 40], r15
  mov [rcx + 48], rsi
  mov [rcx + 56], rdi
  mov r10, [rdx + 64]
  mov r11, [rdx + 72]
  mov r12, [rdx + 80]
  adc r10, [r8 + 64]
  mov r13, [rdx + 88]
  adc r11, [r8 + 72]
  mov r14, [rdx + 96]
  adc r12, [r8 + 80]
  mov r15, [rdx + 104]
  adc r13, [r8 + 88]
  mov rsi, [rdx + 112]
  adc r14, [r8 + 96]
  mov rdi, [rdx + 120]
  adc r15, [r8 + 104]
  lea rdx, [rdx + 128]
  mov [rcx + 64], r10
  adc rsi, [r8 + 112]
  mov [rcx + 72], r11
  adc rdi, [r8 + 120]
  lea r8, [r8 + 128]
  mov [rcx + 80], r12
  mov [rcx + 88], r13
  mov [rcx + 96], r14
  mov [rcx + 104], r15
  mov [rcx + 112], rsi
  mov [rcx + 120], rdi
  lea rcx, [rcx + 128]

  dec r9
  jnz @loop

  adc rax, rax ; - save carry -

  ; - restore registers -
  pop rdi
  pop rsi
  pop r15
  pop r14
  pop r13
  pop r12

nops 0
@end:

ret
UIntX_Add endp

nops 19
