DCPU-EMU
========

DCPU-EMU is a set of tools for working with the DCPU-16 architecture, to be used in Notch's upcoming game about assembly programming in space (no really!), 0x10c_. At the moment, it includes an assembler ("assembler"), and an emulator ("cpu"). Assembly programs are .asm files, and are assembled into .bin binary dumps which can be executed by the emulator.

.. _0x10c: http://0x10c.com

Features
--------
 * Binary program files.
 * 32x16 "video memory" area for output! IN COLOR!
 * Support for data in assembly files.
 * Correct semantics for edge-case instructions like ``ADD PUSH, PUSH``.
 * Very fast.

Compiling
---------

The included Makefile will compile the emulator and assembler on any system with gcc and make::

    anovak@hypernova:~/workspace/dcpu-emu$ make
    gcc cpu.c -std=c99 -o cpu
    gcc assembler.c -std=c99 -o assembler
    anovak@hypernova:~/workspace/dcpu-emu$ 
    
Assembling Programs
-------------------

The first thing you will probably want to do is assemble the included "Hello Word" program, ``hello.asm``::

    anovak@hypernova:~/workspace/dcpu-emu$ ./assembler hello.asm hello.bin
    ...
    Binary program hello.bin assembled successfully.
    anovak@hypernova:~/workspace/dcpu-emu$
    
Note that you need to specify both input and output filenames for the assembler.

Running Programs
----------------

Once you have a .bin file, you can run it with the included emulator::

    anovak@hypernova:~/workspace/dcpu-emu$ ./cpu hello.bin
    ...
    Hello world!














    ====CYCLE 0x0050====
    A:  0xbeef	B:  0x0000	C:  0x0000
    X:  0x0000	Y:  0x0000	Z:  0x0000
    I:  0x0006	J:  0x0000
    PC: 0x001a	SP: 0x0000	O:  0x0000
    Instruction: 0x7dc1
    SYSTEM HALTED
    anovak@hypernova:~/workspace/dcpu-emu$ 
    
Although you didn't see it, the emulated system executed the entire "Hello World" program, displaying the values of the registers and the contents of video memory as it ran. When the halt instruction was encountered--in this case, a "subtract 1 from the program counter" instruction--the (trivial) infinite loop was detected and the emulator stopped.

Notes on Video Memory
---------------------

Notch's original Hello World program_ copied data to an area starting at 0x8000, which he described as "Video ram". He has also posted an image_ of text output in-game, depicting what appears to be a 32-column, 16-line display. This display system has been implemented in the emulator as a block of memory from 0x8000 to 0x8400, where every word corresponds to one character. This appears to be how Notch intends his strings to work, and leaves extra bits for color (which is planned).

.. _program: http://pastebin.com/raw.php?i=qb7k8fNa
.. _image: https://twitter.com/#!/notch/status/185412095452524545/photo/1

Under this implementation, Notch's sample "Hello World" program behaves much as one would expect it to. However, the specifications behind the implementation have not been explicitly discussed by Notch, and may change at a later time to be compatible with the actual game.

At the moment there is implementation of three color styles, but since Notch's color format can't yet be decoded, that's all we have. See the included hello2.asm, based on Notch's color demo_.

.. _demo: http://i.imgur.com/XIXc4.jpg

Nothing about keyboard input is known, so keyboard input is as yet unimplemented.
    
