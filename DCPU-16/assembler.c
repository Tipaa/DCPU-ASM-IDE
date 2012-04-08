#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

#include "cpu.h"

// How many chars can we have in any token?
// This should be enough. Don't name things really long.
// If you need more than 1024 characters in a label, write your own assembler.
#define MAX_CHARS 1024

struct argumentStruct {
    argument_t argument;
    word_t nextWord;
    char* labelReference; // If NULL, nextWord is valid (or unused). Otherwise, nextWord should point to this
};

struct assembledInstruction {
    char* label; // Label for this instruction
    word_t* data; // If not NULL, use this data instead.
    word_t dataLength; // How many words of data are there?
    word_t address;
    opcode_t opcode;
    struct argumentStruct a;
    struct argumentStruct b;
    struct assembledInstruction* next;
};



// Get opcode number from string name
opcode_t opcodeFor(char* command) {
    if(!strcmp(command, "set")) {
        return OP_SET;
    }
    
    if(!strcmp(command, "add")) {
        return OP_ADD;
    }
    
    if(!strcmp(command, "sub")) {
        return OP_SUB;
    }
    
    if(!strcmp(command, "mul")) {
        return OP_MUL;
    }
    
    if(!strcmp(command, "div")) {
        return OP_DIV;
    }
    
    if(!strcmp(command, "mod")) {
        return OP_MOD;
    }
    
    if(!strcmp(command, "shl")) {
        return OP_SHL;
    }
    
    if(!strcmp(command, "shr")) {
        return OP_SHR;
    }
    
    if(!strcmp(command, "and")) {
        return OP_AND;
    }
    
    if(!strcmp(command, "bor")) {
        return OP_BOR;
    }
    
    if(!strcmp(command, "xor")) {
        return OP_XOR;
    }
    
    if(!strcmp(command, "ife")) {
        return OP_IFE;
    }
    
    if(!strcmp(command, "ifn")) {
        return OP_IFN;
    }
    
    if(!strcmp(command, "ifg")) {
        return OP_IFG;
    }
    
    if(!strcmp(command, "ifb")) {
        return OP_IFB;
    }

    // If it's not on the list, assume it's nonbasic
    return OP_NONBASIC;

}

// Get nonbasic opcode number from string name
nonbasicOpcode_t nonbasicOpcodeFor(char* command) {
    if(!strcmp(command, "jsr")) {
        return OP_JSR;
    }
    
    // If it's not on the list, error
    printf("Error: unknown instruction %s\n", command);
    exit(1);
    return 0;
}

// What register number is this character?
// All are 1 character
// -1 = not a register
int registerFor(char regName) {
    switch(regName) {
    case 'a':
        return 0;
        break;
    case 'b':
        return 1;
        break;
    case 'c':
        return 2;
        break;
    case 'x':
        return 3;
        break;
    case 'y':
        return 4;
        break;
    case 'z':
        return 5;
        break;
    case 'i':
        return 6;
        break;
    case 'j':
        return 7;
        break;
    default:
        return -1;
        break;
    }
}

// Get the argument value for a given string
struct argumentStruct argumentFor(char* arg) {
    struct argumentStruct toReturn;
    toReturn.labelReference = NULL; // By default
    
    if(strlen(arg) == 0) {
        printf("Error: empty argument string\n", arg);
        exit(1);
    }
    
    // If it starts with 0-9, it's a number
    if(arg[0] >= '0' && arg[0] <= '9') {
        int argValue;
        char* format;
    
        if(strlen(arg) > 2 && arg[0] == '0' && arg[1] == 'x') {
            // If length > 2 and it starts with 0x, it's hex
            format = "%x";
        } else {
            // Decimal
            format = "%d";
        }
        
        if(sscanf(arg, format, &argValue) != 1) {
            printf("Error: invalid literal value: %s\n", arg);
            exit(1);
        }
        
        // Is it small enough to fit in the argument?
        if(argValue < ARG_LITERAL_END - ARG_LITERAL_START) {
            toReturn.argument = ARG_LITERAL_START + argValue;
            return toReturn;
        }
        
        // Otherwise we have to use the next word
        toReturn.argument = ARG_NEXTWORD;
        toReturn.nextWord = argValue;
        return toReturn;
    }
    
    // If it starts with a bracket, it's an indexing thing
    if(arg[0] == '[' || arg[0] == '(') {
        // Could be just a register, hex+register, label+register, or hex
        
        
        if(strlen(arg) == 3 && (arg[2] == ']' || arg[2] == ')')) {
            // If it's just 1 char in brackets, it must be a register
            int regNum = registerFor(arg[1]);
            if(regNum != -1) {
                toReturn.argument = ARG_REGISTER_INDEX_START + regNum;
                return toReturn;
            } else {
                printf("Error: invalid [register]: %s\n", arg);
                exit(1);
            }
        }
        
        // Is there a hex value?
        int hexValue;
        if(sscanf(arg + 1, "0x%x", &hexValue) == 1) {
            // Is there a +register?
            char regName;
            if(sscanf(arg + 1, "0x%x+%c", &hexValue, &regName) == 2) {
                // TODO: enforce closing ]/)
                int regNum = registerFor(regName);
                if(regNum != -1) {
                    toReturn.argument = ARG_REGISTER_NEXTWORD_INDEX_START + regNum;
                    toReturn.nextWord = hexValue;
                    return toReturn;
                } else {
                    printf("Error: invalid register name '%c' in: %s\n", regName, arg);
                    exit(1);
                }
            } else {
                // Just hex in brackets
                // TODO: enforce closing ]/)
                toReturn.argument = ARG_NEXTWORD_INDEX;
                toReturn.nextWord = hexValue;
                return toReturn;
            }
        } else {
            char* labelStart = arg + 1;
            
            // We have label or label + register
            // Label runs from index 1 to before first + or ]/)
            char* labelEnd = strchr(arg, '+');
            if(labelEnd == NULL) {
                labelEnd = strchr(arg, ']');
            }
            if(labelEnd == NULL) {
                labelEnd = strchr(arg, ')');
            }
            
            if(labelEnd == NULL) {
                printf("Error: Unterminated label in argument: %s\n", arg);
                exit(1);
            }
            
            // Store the label
            char* label = (char*) malloc((labelEnd - labelStart) + 1);
            strncpy(label, labelStart, (labelEnd - labelStart));
            label[labelEnd - labelStart] = '\0';
            
            toReturn.labelReference = label;
            
            // Try to parse a register from there
            char regName;
            if(sscanf(labelEnd, "+%c", &regName) == 1) {
                // Should be a label+register
                int regNum = registerFor(regName);
                if(regNum != -1) {
                    toReturn.argument = ARG_REGISTER_NEXTWORD_INDEX_START + regNum;
                    return toReturn;
                } else {
                    printf("Error: invalid register name '%c' in: %s (specifically, in %s)\n", regName, arg, labelEnd);
                    exit(1);
                }
            } else {
                // Must just be a label
                toReturn.argument = ARG_NEXTWORD_INDEX;
                return toReturn;
            } 
        }
    }
    
    // Check for reserved words
    if(!strcmp(arg, "pop")) {
        toReturn.argument = ARG_POP;
        return toReturn;
    }
    
    if(!strcmp(arg, "peek")) {
        toReturn.argument = ARG_PEEK;
        return toReturn;
    }
    
    if(!strcmp(arg, "push")) {
        toReturn.argument = ARG_PUSH;
        return toReturn;
    }
    
    if(!strcmp(arg, "sp")) {
        toReturn.argument = ARG_SP;
        return toReturn;
    }
    
    if(!strcmp(arg, "pc")) {
        toReturn.argument = ARG_PC;
        return toReturn;
    }
    
    if(!strcmp(arg, "o")) {
        toReturn.argument = ARG_O;
        return toReturn;
    }
    
    // Is it a register?
    if(strlen(arg) == 1) {
        int regNum = registerFor(arg[0]);
        if(regNum != -1) {
            toReturn.argument = ARG_REGISTER_START + regNum;
            return toReturn;
        }
    }
    
    // If it's not any of those things, it must be a bare label.
    // Meaning we put the address in the next word.
    toReturn.argument = ARG_NEXTWORD;
    
    // Store the label for later
    char* label = (char*) malloc(strlen(arg) + 1);
    strcpy(label, arg);
    
    toReturn.labelReference = label;
    return toReturn;
    
}

void Assemble(char* fil, char* out)
{
	char** input = (char**) malloc(strlen(fil)+strlen(out)+3);
	input[0] = "";
	input[1] = fil;
	input[2] = out;
	main(3,input);
}

int main(int argc, char** argv) {
    // Open files
    if(argc != 3) {
        printf("Error: please specify source and binary filenames\n");
        exit(1);
    }
    
    FILE* sourceFile = fopen(argv[1], "r");
    if(!sourceFile) {
        printf("Error: could not open %s\n", argv[1]);
        exit(1);
    }
    
    FILE* binaryFile = fopen(argv[2], "w");
    if(!binaryFile) {
        printf("Error: could not open %s\n", argv[1]);
        exit(1);
    }
    
    word_t address = 0;
    
    struct assembledInstruction* head = NULL;
    struct assembledInstruction* tail = NULL;
    
    while(1) {
        fscanf(sourceFile, " "); // Consume whitespace
        int nextChar = fgetc(sourceFile);
        if(nextChar == ';') {
            // Comment: Discard the rest of the line
            while(nextChar != EOF && nextChar != '\n') {
                nextChar = fgetc(sourceFile);
            }
            continue; // Next line
        } else {
            // Put it back
            ungetc(nextChar, sourceFile);
        }
        
        char* label;
        char command[MAX_CHARS], arg1[MAX_CHARS], arg2[MAX_CHARS];
        // Detect labels
        label = (char*) malloc(MAX_CHARS);
        if(fscanf(sourceFile, ":%s ", label) == 1) {
            // lower-case the label
            int i = 0;
            while(label[i] != '\0') {
                label[i] = tolower(label[i]);
                i++;
            }
        } else {
            free(label);
            label = NULL;
        }
        
        // No comments between labels and code. No spaces in arguments.
        
        
        // Read the command
        if(fscanf(sourceFile, "%s", command) == 1) {
            // Got an instruction
            
            // Add a new instruction to our linked list.
            struct assembledInstruction* instruction = (struct assembledInstruction*) malloc(sizeof(struct assembledInstruction));
            if(head == NULL) {
                head = instruction;
                tail = instruction;
            } else {
                tail->next = instruction;
                tail = tail->next;
            }
            instruction->next = NULL;
            instruction->address = address;
            instruction->label = label; // Set if there was a lable, null otherwise.
            instruction->data = NULL; // Not a data record.
            
            
            // lower-case the command
            int i = 0;
            while(command[i] != '\0') {
                command[i] = tolower(command[i]);
                i++;
            }
            
            if(!strcmp(command, "dat")) {
                // A mix of quoted strrings and deciimal or hex literals, separated by commas and spaces
                // Strings are one letter per word
                
                // Set up a buffer to store the data in.
                // TODO: If they put more than MAX_CHARS data words in one dat, we crash
                instruction->data = (word_t*) malloc(MAX_CHARS * sizeof(word_t));
                instruction->dataLength = 0;
                
                while(1) {
                    // Consume whitespace and leading "
                    fscanf(sourceFile, " ");
                    
                    int nextChar = fgetc(sourceFile);
                    if(nextChar == '"') {
                        printf("Reading string.\n");
                        // A string literal!
                        // Read the string
                        bool_t escaped = 0;
                        while(1) {
                            nextChar = fgetc(sourceFile);
                            char toPut;
                            if(escaped) {
                                // Handle escape sequence translation.
                                switch(nextChar) {
                                case 'n':
                                    toPut = '\n';
                                    break;
                                case 't':
                                    toPut = '\t';
                                    break;
                                case '\\':
                                    toPut = '\\';
                                    break;
                                case '"':
                                    toPut = '"';
                                    break;
                                default:
                                    printf("Error: unrecognized escape sequence \\%c\n", nextChar);
                                    exit(1);
                                }
                                escaped = 0;
                            } else if(nextChar == '"') {
                                break;
                            } else if(nextChar == '\\') {
                                escaped = 1;
                                continue; // Loop around again
                            } else {
                                // Normal character
                                toPut = nextChar;
                            }
                            
                            instruction->data[instruction->dataLength++] = toPut;
                            printf("%c", toPut);
                        }
                        
                        printf("\n");
                    } else {
                    
                        int nextNextChar = fgetc(sourceFile); // check for hex 0x
                        // Unget the last 2 characters
                        fseek(sourceFile, -2, SEEK_CUR);
                       
                        if(nextChar == '0' && nextNextChar == 'x') {
                            // A hex literal!
                            printf("Reading hex literal\n");
                            if(!fscanf(sourceFile, "0x%hx", &instruction->data[instruction->dataLength]) == 1) {
                                printf("Error: expected hex literal\n");
                                exit(1);
                            }
                            instruction->dataLength++;
                            
                        } else if(fscanf(sourceFile, "%hu", &instruction->data[instruction->dataLength]) == 1) {
                            // A decimal literal!
                            printf("Reading decimal literal\n");
                            instruction->dataLength++;
                        } else {
                            // Not a real literal.
                            printf("Out of literals\n");
                            break; // Done reading literals
                       }
                    }
                    
                    // After each literal, there may be a comma. Consume it.
                    fscanf(sourceFile, ",");
                }
                
                // Now we read all the data
                // Advance address apropriately
                address += instruction->dataLength;
                
            } else if(fscanf(sourceFile, " %s", arg1) == 1) {
                 // A command with normal arguments
            
                // lower-case the first argument (and strip trailing , if it exists)
                i = 0;
                while(arg1[i] != '\0') {
                    if(arg1[i] == ',' && arg1[i + 1] == '\0') {
                        arg1[i] = '\0';
                        continue;
                    }

                    arg1[i] = tolower(arg1[i]);
                    i++;
                }
                
                printf("Argument 1: %s\n", arg1);

                // Determine opcode
                instruction->opcode = opcodeFor(command);
                printf("Basic opcode: %d\n", instruction->opcode);
                
                instruction->a = argumentFor(arg1);
                
                // Advance address
                address++; // For instruction
                if(usesNextWord(instruction->a.argument)) {
                    address++;
                }
                
                if(instruction->opcode == OP_NONBASIC) {
                    // No second argument, and first argument taked b bits
                    instruction->b = instruction->a;
                    
                    instruction->a.argument = (argument_t) nonbasicOpcodeFor(command);
                    instruction->a.labelReference = NULL;
                    printf("Nonbasic opcode: %d\n", instruction->a.argument);
                } else {
                    // Second argument
                    if(fscanf(sourceFile, "%s", arg2) != 1) {
                        printf("Error: missing second argument for %s (got %s)\n", command, arg2);
                        exit(1);
                    }
                    
                    // lower-case the second argument
                    i = 0;
                    while(arg2[i] != '\0') {
                        arg2[i] = tolower(arg2[i]);
                        i++;
                    }

                    instruction->b = argumentFor(arg2);
                    
                    if(usesNextWord(instruction->b.argument)) {
                        address++;
                    }
                }
            }
        } else {
            printf("No more valid code.\n");
            fclose(sourceFile);
            
            // Start assembling the actual binary code
            // Do label references for each instruction
            // b arguments on nonbasic obcodes won't have non-NULL label references
            for(struct assembledInstruction* instruction = head; instruction != NULL; instruction = instruction->next) {
                printf("Assembling for address 0x%04hx\n", instruction->address);
                
                if(instruction->data != NULL) {
                    continue; // A data block
                }
                
                // Fill in label reference for a
                if(instruction->a.labelReference != NULL) {
                    printf("Unresolved label for a: %s\n", instruction->a.labelReference);
                    for(struct assembledInstruction* other = head; other != NULL; other = other->next) {
                        // Satisfy a
                        if(other->label != NULL && !strcmp(other->label, instruction->a.labelReference)) {
                            // Match!
                            printf("Resolved %s to address 0x%04hx\n", instruction->a.labelReference, other->address);
                            instruction->a.nextWord = other->address;
                            instruction->a.labelReference = NULL;
                            break;
                        }
                    }
                }
                
                // And for b
                if(instruction->b.labelReference != NULL) {
                    printf("Unresolved label for b: %s\n", instruction->b.labelReference);
                    for(struct assembledInstruction* other = head; other != NULL; other = other->next) {        
                        // Satisfy b
                        if(other->label != NULL && !strcmp(other->label, instruction->b.labelReference)) {
                            // Match!
                            printf("Resolved %s to address 0x%04hx\n", instruction->b.labelReference, other->address);
                            instruction->b.nextWord = other->address;
                            instruction->b.labelReference = NULL;
                            break;
                        }
                    }
                }
                
                // Any label references left?
                if(instruction->a.labelReference != NULL) {
                    printf("Error: unresolved label: %s\n", instruction->a.labelReference); 
                    exit(1);
                }
                if(instruction->b.labelReference != NULL) {
                    printf("Error: unresolved label: %s\n", instruction->b.labelReference); 
                    exit(1);
                }
            }
            
            // Now all label references are satisfied, so we can just write out code
            for(struct assembledInstruction* instruction = head; instruction != NULL; instruction = instruction->next) {
                
                if(instruction->data != NULL) {
                    printf("DATA: 0x%04hx words\n", instruction->dataLength);
                    fwrite(instruction->data, sizeof(word_t), instruction->dataLength, binaryFile);
                    continue; // A data instruction
                }
                
                // Put in parts
                instruction_t packed = 0;
                packed = setOpcode(packed, instruction->opcode);
                packed = setArgument(packed, 0, instruction->a.argument);
                packed = setArgument(packed, 1, instruction->b.argument);
                
                // Save instruction
                printf("0x%04hx: Assembled instruction: 0x%04hx\n", address, packed);
                fwrite(&packed, sizeof(instruction_t), 1, binaryFile);
                
                // Save extra words if necessary
                if(instruction->opcode != OP_NONBASIC && usesNextWord(instruction->a.argument)) {
                    printf("0x%04hx: Extra Word A: 0x%04hx\n", ++address, instruction->a.nextWord);
                    fwrite(&(instruction->a.nextWord), sizeof(word_t), 1, binaryFile);
                }
                if(usesNextWord(instruction->b.argument)) {
                    printf("0x%04hx: Extra Word B: 0x%04hx\n", ++address, instruction->b.nextWord);
                    fwrite(&(instruction->b.nextWord), sizeof(word_t), 1, binaryFile);
                }
            
            }
            
            printf("Binary program %s assembled successfully.\n", argv[2]);
            
            fclose(binaryFile);
            break;
        }
    }
}
