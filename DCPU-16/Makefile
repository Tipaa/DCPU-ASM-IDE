all: cpu assembler

assembler: assembler.c cpu.h
	gcc assembler.c -std=c99 -o assembler


cpu: cpu.c cpu.h
	gcc cpu.c -std=c99 -o cpu

clean:
	rm -f assembler cpu

