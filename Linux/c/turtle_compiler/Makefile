YaccAssign4:	YaccAssign4.c LexAssign4.c tree.c tree.h symtable.h symtable.c codegen.c codegen.h
	gcc -o YaccAssign4 YaccAssign4.c LexAssign4.c tree.c symtable.c codegen.c -ll -ly

YaccAssign4.c:	YaccAssign4.y
	yacc -o YaccAssign4.c -d YaccAssign4.y

LexAssign4.c:	LexAssign4.l YaccAssign4.h
	lex -o LexAssign4.c LexAssign4.l

clean: 
	rm -f YaccAssign4.c 
	rm -f YaccAssign4.h 
	rm -f LexAssign4.c 
	rm -f YaccAssign4
	
