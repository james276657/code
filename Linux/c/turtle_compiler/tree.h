// syntaxtree.h
// OIT Portland, Fall 2008

#ifndef _SYNTAXTREE_H_
#define _SYNTAXTREE_H_

#include <stdio.h>
#include "symtable.h"


typedef enum
{
	NT_BLOCK = 0,
	NT_NUMBER = 1,
	NT_COLOR_NAME = 2,
	NT_FUNCTION = 3,
	NT_TURTLE_CMD = 4,
	NT_IF = 5,
	NT_IFELSE = 6,
	NT_OPERATOR = 7,
	NT_REPEAT = 8,
	NT_DECLARATION = 9,
	NT_VARIABLE = 10,
	NT_ASSIGNMENT = 11
} NODE_TYPE;

typedef enum
{
	OT_EQUALS = 0,
	OT_LESSTHAN = 1,
	OT_GREATERTHAN = 2,
	OT_PLUS = 3,
	OT_MINUS = 4,
	OT_TIMES = 5,
	OT_DIVIDE = 6
} OPERATOR_TYPE;


typedef enum
{
	BLACK = 0x00,
	WHITE = 0x01,
	ORANGE = 0x02,
	YELLOW = 0x03,
	LIME = 0x04,
	CYAN = 0x05,
	BLUE = 0x06,
	MAGENTA = 0x07,
	RED = 0x08,
	BROWN = 0x09,
	GREEN = 0x0a,
	TURQUOISE = 0x0b,
	SKY = 0x0c,
	VIOLET = 0x0d,
	PINK = 0x0e
} COLOR_TYPE;

typedef enum
{
	FT_COLOR = 0,
	FT_XCOR = 1,
	FT_YCOR = 2
} FUNCTION_TYPE;

typedef enum
{
	CMD_HOME = 0x00,
	CMD_FD = 0x01,
	CMD_BK = 0x02,
	CMD_RT = 0x03,
	CMD_LT = 0x04,
	CMD_SETC = 0x05,
	CMD_SETXY = 0x07,
	CMD_PD = 0x0b,
	CMD_PU = 0x0c
} TURTLE_CMD;


typedef struct treenode_t
{
	NODE_TYPE type;
	void * parent;			// treenode pointer
	void * children;		// treenode pointer
	union
	{
		int value;
		COLOR_TYPE color;
		FUNCTION_TYPE func;
		TURTLE_CMD cmd;
		OPERATOR_TYPE op;
		VARIABLE_TYPE vartype;
		int symentry;
	} attribute;
	void * next;			// treenode pointer, used for lists of children
} treenode;

// functions to create treenodes
treenode * makenode_BLOCK();
treenode * makenode_NUMBER(int value);
treenode * makenode_COLOR_NAME(COLOR_TYPE color);
treenode * makenode_FUNCTION(FUNCTION_TYPE func);
treenode * makenode_TURTLE_CMD0(TURTLE_CMD cmd);
treenode * makenode_TURTLE_CMD1(TURTLE_CMD cmd, treenode * param);
treenode * makenode_TURTLE_CMD2(TURTLE_CMD cmd, treenode * param1, treenode * param2);
treenode * makenode_IF(treenode * condition, treenode * block);
treenode * makenode_IFELSE(treenode * condition, treenode * trueblock, treenode * falseblock);
treenode * makenode_OPERATOR(OPERATOR_TYPE op, treenode * lhs, treenode * rhs);
treenode * makenode_REPEAT(treenode * loop_for, treenode * block);
treenode * makenode_DECLARATION(VARIABLE_TYPE vartype, treenode * variable);
treenode * makenode_VARIABLE(int symentry);
treenode * makenode_ASSIGNMENT(treenode * lhs, treenode * rhs);

// children
void addchild(treenode * block, treenode * child);
treenode * firstchild(treenode * node);
treenode * nextchild(treenode * node, treenode * sibbling);
void adoptchildren(treenode * newparent, treenode * oldparent);

// print tree
void printtree(FILE * f, treenode * node);

#endif


