// symtable.c
// OIT Fall 2008

#include <stdio.h>
#include <strings.h>

#define SYMTABLE_DEFINED
#include "symtable.h"


int next_entry = 0;

void init_symtable(void)
{
}

int insert(char lexeme[], int token)
{
	int at = -1;
	
	if (next_entry < SYM_TABLE_MAX)
	{
		symtable[next_entry].token = token;
		symtable[next_entry].lexeme = (char*)malloc(strlen(lexeme)+1);
		strcpy(symtable[next_entry].lexeme, lexeme);
		symtable[next_entry].type = -1;
		at = next_entry++;
	}
	else
	{
		printf("Ran out of symbol table space!\n");
		exit(-1);
	}

	return at;
}


int lookup(char lexeme[])
{
	int i;
	for (i = 0; i < next_entry; i++)
	{
		if (strcasecmp(symtable[i].lexeme, lexeme) == 0)
			return i;
	}

	return -1;
}

struct symtable_entry * getentry(int index)
{
	if ((index >= 0) && (index < next_entry))
		return & (symtable[index]);

	return NULL;
}

