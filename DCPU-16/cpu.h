/*
Mar
28
2012
START_CLASSIFIED_TRANSMISSION

TITLE: dcpu specs, classified, not final
TO: redacted
DATE: 20120328
VERSION: 4

16 bit architecture
0x10000 words of ram (128kb)
8 registers (A, B, C, X, Y, Z, I, J)
program counter (PC)
stack pointer (SP)
overflow (O)

all values are 16 bit unsigned and are set to 0 on startup

opcodes, 16bits: bbbbbbaaaaaaoooo


Basic opcodes: (4 bits)
    0x0: non-basic instruction - see below
    0x1: SET a, b - sets a to b
    0x2: ADD a, b - sets a to a+b, sets O to 0x0001 if there's an overflow, 0x0 otherwise
    0x3: SUB a, b - sets a to a-b, sets O to 0xffff if there's an underflow, 0x0 otherwise
    0x4: MUL a, b - sets a to a*b, sets O to ((a*b)>>16)&0xffff
    0x5: DIV a, b - sets a to a/b, sets O to ((a<<16)/b)&0xffff. if b==0, sets a and O to 0 instead.
    0x6: MOD a, b - sets a to a%b. if b==0, sets a to 0 instead.
    0x7: SHL a, b - sets a to a<<b, sets O to ((a<<b)>>16)&0xffff
    0x8: SHR a, b - sets a to a>>b, sets O to ((a<<16)>>b)&0xffff
    0x9: AND a, b - sets a to a&b
    0xa: BOR a, b - sets a to a|b
    0xb: XOR a, b - sets a to a^b
    0xc: IFE a, b - performs next instruction only if a==b
    0xd: IFN a, b - performs next instruction only if a!=b
    0xe: IFG a, b - performs next instruction only if a>b
    0xf: IFB a, b - performs next instruction only if (a&b)!=0
    
Non-basic opcodes always have their lower four bits unset, have one value and a six bit opcode.
In binary, they have the format: aaaaaaoooooo0000
The value (a) is in the same six bit format as defined earlier.

Non-basic opcodes: (6 bits)
         0x00: reserved for future expansion
         0x01: JSR a - pushes the address of the next instruction to the stack, then sets PC to a
    0x02-0x3f: reserved

aaaaaa, bbbbbb: - brackets = memory lookup of value
0-7: register value - register values
8-15: [register value] - value at address in registries
16-23: [next word of ram + register value] - memory address offset by register value
24: POP - value at stack address, then increases stack counter
25: PEEK - value at stack address
26: PUSH - decreases stack address, then value at stack address
27: SP - current stack pointer value - current stack address
28: PC - program counter - current program counter
29: O - overflow - current value of the overflow
30: [next word of ram] - memory address
31: next word of ram - literal, does nothing on assign
32-63: literal value 0-31 - literal, does nothing on assign
*/

typedef unsigned short word_t;
typedef word_t instruction_t;

typedef unsigned char argument_t; // 6 bits
typedef unsigned char opcode_t; // 4 bits
typedef argument_t nonbasicOpcode_t; // 6 bits

typedef unsigned char bool_t;

/*
OPCODE DEFINITIONS
0: Nonbasic opcode is specified in b bits
1: SET a, b - sets value of a to b
2: ADD a, b - adds b to a, sets O
3: SUB a, b - subtracts b from a, sets O
4: MUL a, b - multiplies a by b, sets O
5: DIV a, b - divides a by b, sets O
6: MOD a, b - remainder of a over b
7: SHL a, b - shifts a left b places, sets O
8: SHR a, b - shifts a right b places, sets O
9: AND a, b - binary and of a and b
10: BOR a, b - binary or of a and b
11: XOR a, b - binary xor of a and b
12: IFE a, b - skips one instruction if a!=b
13: IFN a, b - skips one instruction if a==b
14: IFG a, b - skips one instruction if a<=b
15: IFB a, b - skips one instruction if (a&b)==0

Non-basic opcodes always have their lower four bits unset, have one value and a six bit opcode.
In binary, they have the format: aaaaaaoooooo0000
The value (a) is in the same six bit format as defined earlier.

Non-basic opcodes: (6 bits)
         0x00: reserved for future expansion
         0x01: JSR a - pushes the address of the next instruction to the stack, then sets PC to a
    0x02-0x3f: reserved

*/

// Defines so we can switch on them
#define OP_NONBASIC 0
#define OP_SET 1
#define OP_ADD 2
#define OP_SUB 3
#define OP_MUL 4
#define OP_DIV 5
#define OP_MOD 6
#define OP_SHL 7
#define OP_SHR 8
#define OP_AND 9
#define OP_BOR 10
#define OP_XOR 11
#define OP_IFE 12
#define OP_IFN 13
#define OP_IFG 14
#define OP_IFB 15

#define OP_JSR 1

/*
ARGUMENT DEFINITIONS
0-7: register value - register values
8-15: [register value] - value at address in registries
16-23: [next word of ram + register value] - memory address offset by register value
24: POP - value at stack address, then increases stack counter
25: PEEK - value at stack address
26: PUSH - decreases stack address, then value at stack address
27: SP - current stack pointer value - current stack address
28: PC - program counter - current program counter
29: O - overflow - current value of the overflow
30: [next word of ram] - memory address
31: next word of ram - literal, does nothing on assign
32-63: literal value 0-31 - literal, does nothing on assign
*/

#define ARG_REGISTER_START 0
#define ARG_REGISTER_END 8
#define ARG_REGISTER_INDEX_START 8
#define ARG_REGISTER_INDEX_END 16
#define ARG_REGISTER_NEXTWORD_INDEX_START 16
#define ARG_REGISTER_NEXTWORD_INDEX_END 24
#define ARG_POP 24
#define ARG_PEEK 25
#define ARG_PUSH 26
#define ARG_SP 27
#define ARG_PC 28
#define ARG_O 29
#define ARG_NEXTWORD_INDEX 30
#define ARG_NEXTWORD 31
#define ARG_LITERAL_START 32
// Uses the extra bits we have since we don't use a real 6-bit type
#define ARG_LITERAL_END 64 

// Readers and writers for instruction parts
opcode_t getOpcode(instruction_t instruction) {
    return instruction & 0xF;
}

argument_t getArgument(instruction_t instruction, bool_t which) {
    // First 6 bits for true (b), second 6 for false (a)
    return ((instruction >> 4) >> 6 * which) & 0x3F;
}

instruction_t setOpcode(instruction_t instruction, opcode_t opcode) {
    return (instruction & 0xFFF0) | opcode; // Clear low 4 bits and OR in opcode.
}

instruction_t setArgument(instruction_t instruction, bool_t which, argument_t argument) {
    if(!which) {
        return (instruction & 0xFC0F) | (((word_t) argument) << 4); // A argument (second in bits)
    } else {
        return (instruction & 0x03FF) | (((word_t) argument) << 10); // B argument (first in bits)
    }
}

// Does an argument reference the "next word"?
// This affects where the next instruction is, and where the b argument's next word is.
bool_t usesNextWord(argument_t argument) {
    return (argument >= ARG_REGISTER_NEXTWORD_INDEX_START && argument < ARG_REGISTER_NEXTWORD_INDEX_END) 
        || argument == ARG_NEXTWORD_INDEX 
        || argument == ARG_NEXTWORD;
}

// Is an argument const: should we never save to it?
bool_t isConst(argument_t argument) {
    return (argument >= ARG_LITERAL_START && argument < ARG_LITERAL_END) 
        || argument == ARG_NEXTWORD;
    // We *can* still assign to POP/PEEK or read PUSH, but it's very strange.
}

// How many words does a given instruction take?
word_t getInstructionLength(instruction_t instruction) {
    if(getOpcode(instruction) == OP_NONBASIC) {
        // 1 argument, so 1 extra word max.
        return 1 + usesNextWord(getArgument(instruction, 1));
    } else {
        return 1 + usesNextWord(getArgument(instruction, 0)) + usesNextWord(getArgument(instruction, 1));
    }
}

// Get the offset from the instructuion (1 or 2) for the next word for each argument
// Or 0 if invalid or unised
word_t getNextWordOffset(instruction_t instruction, bool_t which) {
    if(getOpcode(instruction) == OP_NONBASIC) {
        // 1 argument, so 1 extra word max.
        return (which == 0) && usesNextWord(getArgument(instruction, 1)); 
    } else {
        if(!usesNextWord(getArgument(instruction, which))) {
             return 0;
        }
        
        // Is used. 2 if it's b and a also uses its word, 1 otherwise.
        return 1 + (which && usesNextWord(getArgument(instruction, 0)));
    }
}
