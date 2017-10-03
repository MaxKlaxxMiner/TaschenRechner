include dll-basic.inc
include Add.asm

.data

alignPointers dq UIntX_Copy, @len,
                 @dllend

alignPointersCount dq (alignPointersCount - alignPointers) / qword

.code

; void UIntX_Copy(ulong* rp, ulong* sp, long n)
UIntX_Copy proc export
  ; rcx = rp, rdx = sp, r8 = n

  ; -- check: n % 2 == 0 --
  shr r8, 1
  jnc @l2

  ; -- copy 1 limb --
  mov rax, [rdx]
  add rdx, 8

  mov [rcx], rax
  add rcx, 8

  ; -- no more limbs --
  test r8, r8
  je @end

@l2:

  ; -- check: n % 4 == 0 --
  shr r8, 1
  jnc @l4

  ; -- copy 2 limbs --
  movdqu xmm0, [rdx]
  add rdx, 16

  movdqu [rcx], xmm0
  add rcx, 16

  ; -- no more limbs --
  test r8, r8
  je @end

@l4:

  ; -- check: n % 8 == 0 --
  shr r8, 1
  jnc @l8

  ; -- copy 4 limbs --
  movdqu xmm0, [rdx]
  movdqu xmm1, [rdx + 16]
  add rdx, 32

  movdqu [rcx], xmm0
  movdqu [rcx + 16], xmm1
  add rcx, 32

  ; -- no more limbs --
  test r8, r8
  je @end

@l8:

  test cl, 8
  jz @loop

  dec r8
  shl r8, 6
  movdqu xmm0, [rdx + r8 + 8]
  movdqu xmm1, [rdx + r8 + 16 + 8]
  movdqu xmm2, [rdx + r8 + 32 + 8]
  movdqu xmm3, [rdx + r8 + 48]
  movdqa [rcx + r8 + 8], xmm0
  movdqa [rcx + r8 + 16 + 8], xmm1
  movdqa [rcx + r8 + 32 + 8], xmm2
  movdqu [rcx + r8 + 48], xmm3
  shr r8, 6
  mov rax, [rdx]
  add rdx, 8
  mov [rcx], rax
  add rcx, 8

  test r8, r8
  je @end

align 16
@loop:

  ; -- copy 8 limbs --
  movdqu xmm0, [rdx]
  movdqu xmm1, [rdx + 16]
  movdqu xmm2, [rdx + 32]
  movdqu xmm3, [rdx + 48]
  add rdx, 64

  movdqa [rcx], xmm0
  movdqa [rcx + 16], xmm1
  movdqa [rcx + 32], xmm2
  movdqa [rcx + 48], xmm3
  add rcx, 64

  dec r8
  jnz @loop

@end:
ret
UIntX_Copy endp

@len::


; ulong mpn_add_n(ulong* rp, ulong* up, ulong* vp, long n)
align 16
mpn_add_n proc export

  mov rax, r9
  shr r9, 2
  and rax, 3

  test r9, r9
je lt4

  push rdi
  push rsi

  mov r10, [rdx]
  mov r11, [rdx + 8]
  dec r9
jmp mid

align 16
lt4:
  mov r10, [rdx]
  dec rax
jnz l2
  adc r10, [r8]
  mov [rcx], r10
  adc rax, rax
ret

align 16
l2:
  dec rax
  mov r11, [rdx + 8]
jnz l3
  adc r10, [r8]
  adc r11, [r8 + 8]
  mov [rcx], r10
  mov [rcx + 8], r11
  adc rax, rax
ret

align 16
l3:
  mov r9, [rdx + 16]
  adc r10, [r8]
  adc r11, [r8 + 8]
  adc r9, [r8 + 16]
  mov [rcx], r10
  mov [rcx + 8], r11
  mov [rcx + 16], r9
  setc al
ret

align 16
top:
  adc r10, [r8]
  adc r11, [r8 + 8]
  mov [rcx], r10
  adc rsi, [r8 + 16]
  lea rdx, [rdx + 32]
  adc rdi, [r8 + 24]
  mov [rcx + 8], r11
  dec r9
  lea r8, [r8 + 32]
  mov r10, [rdx]
  mov [rcx + 16], rsi
  mov r11, [rdx + 8]
  mov [rcx + 24], rdi
  lea rcx, [rcx + 32]
mid:
  mov rsi, [rdx + 16]
  mov rdi, [rdx + 24]
jnz top

  lea rdx, [rdx + 32]
  adc r10, [r8]
  adc r11, [r8 + 8]
  adc rsi, [r8 + 16]
  adc rdi, [r8 + 24]
  lea r8, [r8 + 32]
  mov [rcx], r10
  mov [rcx + 8], r11
  mov [rcx + 16], rsi
  mov [rcx + 24], rdi
  lea rcx, [rcx + 32]

  pop rsi
  pop rdi

  inc rax
  dec rax
jnz lt4
  adc rax, rax
ret

mpn_add_n endp

@dllend::

end
