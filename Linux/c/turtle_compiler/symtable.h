// symtable.h
// OIT Fall 2008

#ifndef _SYMTABLE_H_
#define _SYMTABLE_H_


//
// type constants used for representing types of variables
//

typedef enum
{
	VT_INT = 0
} VARIABLE_TYPE;


//
// symtable_entry structure
//

struct symtable_entry {
    char *lexeme;
    int token;
    VARIABLE_TYPE type;
};


//
// symtable array
//

#define SYM_TABLE_MAX 1024

#ifdef SYMTABLE_DEFINED
	struct symtable_entry symtable[SYM_TABLE_MAX];
#else
	extern struct symtable_entry symtable[SYM_TABLE_MAX];
#endif


//
// init_symtable()
// initializes the symbol table
//
void init_symtable(void);


//
// insert(char str[], int token)
// inserts lexme into the symbol table along with its token
// returns the index of the entry in the table, or -1 if already in the table or not able to  insert
//
int insert(char lexeme[], int token);


//
// lookup(char lexeme[])
// searches symbol table for lexeme
// returns the index of the entry if found, or -1 if not found
//
int lookup(char lexeme[]);


//
// getentry(int index)
// returns a pointer to the symbol table entry at the specified index
//
struct symtable_entry * getentry(int entry);

#endif




