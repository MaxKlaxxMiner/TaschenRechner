include dll-basic.inc
include Add.asm

.data

alignPointers dq mpn_add_n
                 ;@dllend

alignPointersCount dq (alignPointersCount - alignPointers) / qword

.code

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
