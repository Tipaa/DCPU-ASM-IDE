#include <stdio.h>
#include <stdlib.h>

#include "cpu.h"

// Set this to 1 to print argument fetch info.
#define DEBUG 0

// We use defines here so we can have global arrays with these as sizes
#define MEMORY_LIMIT 0x10000
#define NUM_REGISTERS 8
#define CONSOLE_START 0x8000
// Console is 32 columns, 16 lines of text. 1 word per character.
#define TERM_WIDTH 32
#define TERM_HEIGHT 16
#define CONSOLE_END (CONSOLE_START + TERM_WIDTH * TERM_HEIGHT)

word_t memory[MEMORY_LIMIT];
word_t registers[NUM_REGISTERS];
word_t programCounter;
word_t stackPointer;
word_t overflow;

word_t cycle;

word_t literals[ARG_LITERAL_END - ARG_LITERAL_START]; // So we can have pointers to them

#define NUM_COLORS 0x1FF
char* colorTable[NUM_COLORS];


// Opcodes and argument codes defined in header

// Get a pointer to the given argument's memory location
// And do any side effects the argument evaluation should have
// Including incrementing PC when getting next word.
// And taking cycles to do so.
word_t* evaluateArgument(argument_t argument) {    
    // Handle all ranges
    if(argument >= ARG_REGISTER_START && argument < ARG_REGISTER_END) {
        // register value - register values
        word_t regNumber = argument - ARG_REGISTER_START;
        if(DEBUG) {
            printf("register %d\n", regNumber);
        }
        return &registers[regNumber];
    }
    
    if(argument >= ARG_REGISTER_INDEX_START && argument < ARG_REGISTER_INDEX_END) {
        // [register value] - value at address in register
        word_t regNumber = argument - ARG_REGISTER_INDEX_START;
        if(DEBUG) {
            printf("[register %d]\n", regNumber);
        }
        return &memory[registers[regNumber]];
    }
    
    if(argument >= ARG_REGISTER_NEXTWORD_INDEX_START && argument < ARG_REGISTER_NEXTWORD_INDEX_END) {
        // [next word of ram + register value] - memory address offset by register value
        word_t regNumber = argument - ARG_REGISTER_NEXTWORD_INDEX_START;
        if(DEBUG) {
            printf("[0x%04hx + register %d]\n", memory[programCounter], regNumber);
        }
        cycle++; // Take a cycle
        return &memory[registers[regNumber] + memory[programCounter++]];
    }
    
    if(argument >= ARG_LITERAL_START && argument < ARG_LITERAL_END) {
        // literal value 0-31 - literal, does nothing on assign
        if(DEBUG) {
            printf("literal 0x%02hx\n", argument - ARG_LITERAL_START);
        }
        return &literals[argument - ARG_LITERAL_START];
    }
    
    // Now single values
    switch(argument) {
    case ARG_POP:
        // POP - value at stack address, then increases stack counter
        if(DEBUG) {
            printf("POP\n");
        }
        return &memory[stackPointer++];
        break;
    case ARG_PEEK:
        // PEEK - value at stack address
        if(DEBUG) {
            printf("PEEK\n");
        }
        return &memory[stackPointer];
        break;
    case ARG_PUSH:
        // PUSH - decreases stack address, then value at stack address
        if(DEBUG) {
            printf("PUSH\n");
        }
        return &memory[--stackPointer];
        break;
    case ARG_SP:
        // SP - current stack pointer value - current stack address
        if(DEBUG) {
            printf("stack pointer\n");
        }
        return &stackPointer;
        break;
    case ARG_PC:
        // PC - program counter - current program counter
        if(DEBUG) {
            printf("program counter\n");
        }
        return &programCounter;
        break;
    case ARG_O:
        // O - overflow - current value of the overflow
        if(DEBUG) {
            printf("overflow\n");
        }
        return &overflow;
        break;
    case ARG_NEXTWORD_INDEX:
        // [next word of ram] - memory address
        if(DEBUG) {
            printf("[0x%04hx]\n", memory[programCounter]);
        }
        cycle++; // Take a cycle
        return &memory[memory[programCounter++]];
        break;
    case ARG_NEXTWORD:
        // next word of ram - literal, does nothing on assign
        // Handling of this is tricky.
        // If it's used in a and not in b, it means the next word.
        // If it's used in b and not in a, it means the next word.
        // If it's used in a AND b, it means the next word in a, and the one after that in b
        // (and the next word again when we go to write back to a)
        if(DEBUG) {
            printf("0x%04hx\n", memory[programCounter]);
        }
        cycle++; // Take a cycle
        return &memory[programCounter++];
        break;
    }
}

int main(int argc, char** argv) {
    printf("\033[2J"); // Clear screen
    
    // Set up literal table
    for(int i = 0; i < ARG_LITERAL_END - ARG_LITERAL_START; i++) {
        literals[i] = i;
    }
    
    // Set up color table
    for(int i = 0; i < NUM_COLORS; i++) {
        colorTable[i] = "\033[0m"; // Reset formatting
    }
    // TODO: Only guessed mappings
    colorTable[0b111000011] = "\033[1;33;44m"; // Yellow on blue
    colorTable[0b011100000] = "\033[1;37;40m"; // White on black
    
    // Initialize
    for(int i = 0; i < MEMORY_LIMIT; i++) {
        memory[i] = 0;
    }
    
    for(word_t i = 0; i < NUM_REGISTERS; i++) {
        registers[i] = 0;
    }
    
    programCounter = 0;
    stackPointer = 0;
    overflow = 0;
    
    cycle = 0;
    
    // Load program
    if(argc != 2) {
        printf("Error: please specify .bin file to execute\n");
        exit(1);
    }
    
    FILE* programFile = fopen(argv[1], "r");
    if(!programFile) {
        printf("Error: could not open %s\n", argv[1]);
        exit(1);
    }
    
    fread(memory, sizeof(word_t), MEMORY_LIMIT, programFile); // Read MEMORY_LIMIT words, or all that's there.
    fclose(programFile);
    
    // Run
    bool_t videoDirty = 0;
    while(1) {
        // Fetch
        // PC always points to the *next* instruction/word
        word_t executingPC = programCounter; // For halt checker.
        instruction_t instruction = memory[programCounter++];
        
        // Decode & get arguments
        opcode_t opcode = getOpcode(instruction);
        nonbasicOpcode_t nonbasicOpcode;
        word_t* aLoc;
        word_t* bLoc;
        bool_t skipStore;
        if(opcode == OP_NONBASIC) { 
            // Real opcode is in a bits, b bits hold a argument
            nonbasicOpcode = (nonbasicOpcode_t) getArgument(instruction, 0);
            aLoc = evaluateArgument(getArgument(instruction, 1));
            skipStore = 1;
        } else {
            // Two normal args
            aLoc = evaluateArgument(getArgument(instruction, 0));
            bLoc = evaluateArgument(getArgument(instruction, 1));
            skipStore = isConst(getArgument(instruction, 0)); // If a is a literal, don't write to it.
        }
        word_t result;
        
        
        // Execute, setting flags
        unsigned int resultWithCarry; // Some opcodes use this internal variable
        bool_t skipNext = 0; // Set to skip the next instruction
        switch(opcode) {
        case OP_NONBASIC:
            // Handle nonbasic opcodes
            
            skipStore = 1; // Don't go writing to a
            
            switch(nonbasicOpcode) {
            case OP_JSR:
                // 0x01: JSR a - pushes the address of the next instruction to the stack, then sets PC to a
                memory[--stackPointer] = programCounter;
                programCounter = *aLoc;
                cycle += 2; // 2 cycles
                break;
            default:
                printf("Error: reserved OP_NONBASIC\n");
                exit(1);
            }
            break;
            
        case OP_SET:
            // SET a, b - sets value of a to b
            result = *bLoc;
            cycle += 1; // 1 cycle
            break;
            
        case OP_ADD:
            // ADD a, b - adds b to a, sets O
            result = *aLoc + *bLoc;
            overflow = (result < *aLoc || result < *bLoc);
            cycle += 2; // 2 cycles
            break;
            
        case OP_SUB:
            // SUB a, b - subtracts b from a, sets O
            result = *aLoc - *bLoc;
            overflow = (result > *aLoc) ? 0xFFFF : 0;
            cycle += 2; // 2 cycles
            break;
            
        case OP_MUL:
            // MUL a, b - multiplies a by b, sets O
            resultWithCarry = (unsigned int) *aLoc * (unsigned int) *bLoc;
            result = (word_t) (resultWithCarry & 0xFFFF); // Low word
            overflow = (word_t) (resultWithCarry >> 16); // High word
            cycle += 2; // 2 cycles
            break;
            
        case OP_DIV:
            // DIV a, b - divides a by b, sets O
            // Sets O to ((a<<16)/b)&0xffff. if b==0, sets a and O to 0 instead.
            if(*bLoc != 0) {
                resultWithCarry = ((unsigned int) *aLoc << 16) / (unsigned int) *bLoc;
                result = (word_t) (resultWithCarry >> 16); // High word
                overflow = (word_t) (resultWithCarry & 0xFFFF); // Low word
            } else {
                result = 0;
                overflow = 0;
            }
            cycle += 3; // 3 cycles
            break;
            
        case OP_MOD:
            // MOD a, b - remainder of a over b
            if(*bLoc != 0) {
                result = *aLoc % *bLoc;
            } else {
                // Specified not to set O
                result = 0;
            }
            cycle += 3; // 3 cycles
            break;
            
        case OP_SHL:
            // SHL a, b - shifts a left b places, sets O
            // O = ((a<<b)>>16)&0xffff
            resultWithCarry = (unsigned int) *aLoc << *bLoc;
            result = (word_t) (resultWithCarry & 0xFFFF);
            overflow = (word_t) (resultWithCarry >> 16);
            cycle += 2; // 2 cycles
            break;
            
        case OP_SHR:
            // SHR a, b - shifts a right b places, sets O
            // O = ((a<<16)>>b)&0xffff
            resultWithCarry = ((unsigned int) *aLoc << 16) >> *bLoc;
            result = (word_t) (resultWithCarry >> 16);
            overflow = (word_t) (resultWithCarry & 0xFFFF);
            cycle += 2; // 2 cycles
            break;
            
        case OP_AND:
            // AND a, b - binary and of a and b
            result = *aLoc & *bLoc;
            cycle += 1; // 1 cycle
            break;
            
        case OP_BOR:
            // BOR a, b - binary or of a and b
            result = *aLoc | *bLoc;
            cycle += 1; // 1 cycle
            break;
            
        case OP_XOR:
            // XOR a, b - binary xor of a and b
            result = *aLoc ^ *bLoc;
            cycle += 1; // 1 cycle
            break;
            
        case OP_IFE:
            // IFE a, b - skips one instruction if a!=b
            skipStore = 1;
            skipNext = !!(*aLoc != *bLoc);
            cycle += (2 + skipNext); // 2 cycles, +1 if we skip
            break;
            
        case OP_IFN:
            // IFN a, b - skips one instruction if a==b
            skipStore = 1;
            skipNext = !!(*aLoc == *bLoc);
            cycle += (2 + skipNext); // 2 cycles, +1 if we skip
            break;
            
        case OP_IFG:
            // IFG a, b - skips one instruction if a<=b
            skipStore = 1;
            skipNext = !!(*aLoc <= *bLoc);
            cycle += (2 + skipNext); // 2 cycles, +1 if we skip
            break;
            
        case OP_IFB:
            // IFB a, b - skips one instruction if (a&b)==0
            skipStore = 1;
            skipNext = (!(*aLoc & *bLoc));
            cycle += (2 + skipNext); // 2 cycles, +1 if we skip
            break;
        }
        
        // Store result back to a (if we're not skipping it)
        if(!skipStore) {
            // Look for halt
            if(aLoc == &programCounter && result == executingPC
                && ((opcode == OP_SET && getArgument(instruction, 1) == ARG_NEXTWORD) 
                || (opcode == OP_SUB && getArgument(instruction, 1) == ARG_LITERAL_START + 1))) {
            
                // If we're setting the PC to the thing we just ran
                // And we just ran a set to next word value (unconditional jump)
                // Or we just ran a subtract 1
                // Then we know we can't ever stop looping
                
                printf("SYSTEM HALTED\n");
                exit(0);
            }
            
            if(aLoc >= &memory[CONSOLE_START] && aLoc < &memory[CONSOLE_END]) {
                // Writing to video memory
                videoDirty = 1;
            }
            
            *aLoc = result;
        }
        
        // Skip the next instruction if an if said to
        if(skipNext) {
            programCounter += getInstructionLength(memory[programCounter]);
        }
        
        // Now render the video memory
        if(videoDirty) {
            printf("\033[%d;%dH", 1, 1); // Zero cursor
            for(int i = 0; i < TERM_HEIGHT; i++) {
                for(int j = 0; j < TERM_WIDTH; j+=1) { // Count in words
                    word_t toPrint = memory[CONSOLE_START + i * TERM_WIDTH + j];
                    // Print the word as one character. High bits are formatting.
                    // TODO: Formatting format?
                    int colorData = toPrint >> 7;
                    printf("%s", colorTable[colorData]);
                    char letter = (toPrint & 0x7F);
                    if(letter == '\0') {
                        letter = ' '; // To overwrite stuff
                    }
                    printf("%c", letter);  // Seems to be that character data is 7-bit ASCII.
                }
                printf("\n");
            }
            
            videoDirty = 0;
        }
        
        
        // Print out execution data
        printf("\033[%d;%dH", TERM_HEIGHT + 1, 1); // Set cursor after "screen" area
        printf("====CYCLE 0x%04hx====\n", cycle);
        printf("A:  0x%04hx\tB:  0x%04hx\tC:  0x%04hx\n", registers[0], registers[1], registers[2]);
        printf("X:  0x%04hx\tY:  0x%04hx\tZ:  0x%04hx\n", registers[3], registers[4], registers[5]);
        printf("I:  0x%04hx\tJ:  0x%04hx\n", registers[6], registers[7]);
        printf("PC: 0x%04hx\tSP: 0x%04hx\tO:  0x%04hx\n", programCounter, stackPointer, overflow);
        printf("Instruction: 0x%04hx\n", instruction);
        
    }    
    
    return 0;
}

