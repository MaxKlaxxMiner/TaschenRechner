
; ulong UIntX_Add(ulong* rp, ulong* up, ulong* vp, long n)
;                 rcx,       rdx,       r8,        r9
UIntX_Add proc export

  xor eax, eax ; - temp register for carry flag, default: 0 -

  ; - check n % 2 == 0 -
  shr r9, 1
  jnc @l2

  ; - 1 limb -
  mov r10, [rdx]
  add r10, [r8]
  mov [rcx], r10

  sbb eax, eax ; - save carry -

  test r9, r9
  je @end ; - no more limbs -

  ; - move pointers for 1 limb -
  add rdx, 8
  add r8, 8
  add rcx, 8

nops 0
_@l2::
@l2:
  shr r9, 1
  jnc @l4

  shr eax, 1 ; - reload carry -

  ; - 2 limbs -
  mov r10, [rdx]
  mov r11, [rdx + 8]
  adc r10, [r8]
  adc r11, [r8 + 8]
  mov [rcx], r10
  mov [rcx + 8], r11

  sbb eax, eax ; - save carry -

  test r9, r9
  je @end ; - no more limbs -

  ; - move pointers for 2 limbs -
  add rdx, 16
  add r8, 16
  add rcx, 16

nops 0
_@l4::
@l4:
  shr r9, 1
  jnc @l8

  shr eax, 1 ; - reload carry -

  ; - 4 limbs -
  mov rax, [rdx]
  mov r10, [rdx + 8]
  adc rax, [r8]
  mov r11, [rdx + 16]
  adc r10, [r8 + 8]
  mov [rcx], rax
  mov rax, [rdx + 24]
  adc r11, [r8 + 16]
  mov [rcx + 8], r10
  adc rax, [r8 + 24]
  mov [rcx + 16], r11
  mov [rcx + 24], rax

  sbb eax, eax ; - save carry -

  test r9, r9
  je @end ; - no more limbs -

  ; - move pointers for 4 limbs -
  add rdx, 32
  add r8, 32
  add rcx, 32

nops 0
_@l8::
@l8:
  shr r9, 1
  jnc @l16

  shr eax, 1 ; - reload carry -

  ; - 8 limbs -
  mov rax, [rdx]
  mov r10, [rdx + 8]
  adc rax, [r8]
  mov r11, [rdx + 16]
  adc r10, [r8 + 8]
  mov [rcx], rax
  adc r11, [r8 + 16]
  mov [rcx + 8], r10
  mov rax, [rdx + 24]
  mov [rcx + 16], r11
  mov r10, [rdx + 32]
  adc rax, [r8 + 24]
  mov r11, [rdx + 40]
  adc r10, [r8 + 32]
  mov [rcx + 24], rax
  adc r11, [r8 + 40]
  mov [rcx + 32], r10
  mov rax, [rdx + 48]
  mov [rcx + 40], r11
  mov r10, [rdx + 56]
  adc rax, [r8 + 48]
  adc r10, [r8 + 56]
  mov [rcx + 48], rax
  mov [rcx + 56], r10

  sbb eax, eax ; - save carry -

  test r9, r9
  je @end ; - no more limbs -

  ; - move pointers for 8 limbs -
  add rdx, 64
  add r8, 64
  add rcx, 64

nops 0
_@l16::
@l16:
  ; - save registers -
  push rbx
  push r14
  push r15
  push rsi
  push rdi

  shr eax, 1 ; - reload carry -

nops 0
_@loop::
@loop:
  ; - 16 limbs -
  mov r10, [rdx]
  mov r11, [rdx + 8]
  mov rbx, [rdx + 16]
  adc r10, [r8]
  mov rax, [rdx + 24]
  adc r11, [r8 + 8]
  mov r14, [rdx + 32]
  adc rbx, [r8 + 16]
  mov r15, [rdx + 40]
  adc rax, [r8 + 24]
  mov rsi, [rdx + 48]
  adc r14, [r8 + 32]
  mov rdi, [rdx + 56]
  adc r15, [r8 + 40]
  mov [rcx], r10
  adc rsi, [r8 + 48]
  mov [rcx + 8], r11
  adc rdi, [r8 + 56]
  mov [rcx + 16], rbx
  mov [rcx + 24], rax
  mov [rcx + 32], r14
  mov [rcx + 40], r15
  mov [rcx + 48], rsi
  mov [rcx + 56], rdi
  mov r10, [rdx + 64]
  mov r11, [rdx + 72]
  mov rbx, [rdx + 80]
  adc r10, [r8 + 64]
  mov rax, [rdx + 88]
  adc r11, [r8 + 72]
  mov r14, [rdx + 96]
  adc rbx, [r8 + 80]
  mov r15, [rdx + 104]
  adc rax, [r8 + 88]
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
  mov [rcx + 80], rbx
  mov [rcx + 88], rax
  mov [rcx + 96], r14
  mov [rcx + 104], r15
  mov [rcx + 112], rsi
  mov [rcx + 120], rdi
  lea rcx, [rcx + 128]

  dec r9
  jnz @loop

  sbb eax, eax ; - save carry -

  ; - restore registers -
  pop rdi
  pop rsi
  pop r15
  pop r14
  pop rbx
nops 0
_@end::
@end:
  neg eax
_@ret::
ret
UIntX_Add endp

; align 64
nops 0
