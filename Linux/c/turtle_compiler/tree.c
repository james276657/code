// syntaxtree.c
// OIT Portland, Fall 2008

#include "tree.h"
#include <stdlib.h>
#include "symtable.h"

//
// functions to create treenodes
//

treenode * makenode(NODE_TYPE nt, int attribute)
{
	treenode * n = (treenode*)malloc(sizeof(treenode));
	n->type = nt;
	n->attribute.value = attribute;
	n->parent = NULL;
	n->children = NULL;
	n->next = NULL;
	
	return n;
}

treenode * makenode_BLOCK()
{
	treenode * n = makenode(NT_BLOCK, 0);
	return n;
}

treenode * makenode_NUMBER(int value)
{
	treenode * n = makenode(NT_NUMBER, value);
	return n;
}

treenode * makenode_COLOR_NAME(COLOR_TYPE color)
{
	
	//printf("tree c making node COLOR NAME with first parameter %i \n", color);
	treenode * n = makenode(NT_COLOR_NAME, color);
	return n;
}

treenode * makenode_FUNCTION(FUNCTION_TYPE func)
{
	treenode * n = makenode(NT_FUNCTION, func);
	return n;
}

treenode * makenode_TURTLE_CMD0(TURTLE_CMD cmd)
{
	treenode * n = makenode(NT_TURTLE_CMD, (int)cmd);
	return n;
}

treenode * makenode_TURTLE_CMD1(TURTLE_CMD cmd, treenode * param)
{
	treenode * n = makenode(NT_TURTLE_CMD, (int)cmd);
	addchild(n, param);
	return n;
}

treenode * makenode_TURTLE_CMD2(TURTLE_CMD cmd, treenode * param1, treenode * param2)
{
    treenode * n = makenode(NT_TURTLE_CMD, (int)cmd);
    addchild(n, param1);
    addchild(n, param2);
    return n;
}

treenode * makenode_IF(treenode * condition, treenode * block)
{
	//printf("tree c making node if\n");
	treenode * n = makenode(NT_IF, 0);
	addchild(n, condition);
	addchild(n, block);
	return n;
}

treenode * makenode_IFELSE(treenode * condition, treenode * trueblock, treenode * falseblock)
{
	treenode * n = makenode(NT_IFELSE, 0);
	addchild(n, condition);
	addchild(n, trueblock);
	addchild(n, falseblock);
	return n;
}

treenode * makenode_OPERATOR(OPERATOR_TYPE op, treenode * lhs, treenode * rhs)
{
	//printf("making node OPERATOR with first parameter %i second parameter %i third parameter % i\n, op, lhs, rhs");
	treenode * n = makenode(NT_OPERATOR, 0);
	addchild(n, lhs);
	addchild(n, rhs);
	//printf("made node OPERATOR\n");
	return n;
}

treenode * makenode_REPEAT(treenode * loop_for, treenode * block)
{
	//printf("making node REPEAT with first parameter %i second parameter %i \n", loop_for, block);
	treenode * n = makenode(NT_REPEAT, 0);
	addchild(n, loop_for);
	addchild(n, block);
	return n;
}

treenode * makenode_DECLARATION(VARIABLE_TYPE vartype, treenode * variable)
{
	// TODO
	return NULL;
}

treenode * makenode_VARIABLE(int symentry)
{
	// TODO
	return NULL;
}

treenode * makenode_ASSIGNMENT(treenode * lhs, treenode * rhs)
{
	// TODO
	return NULL;
}


//
// children
//

void addchild(treenode * block, treenode * child)
{
	// add child to the end of the sibbling list
	treenode * sibbling = block->children;
	if (sibbling != NULL)
	{
		// there are already childen, so find the last sibbling...
		while (sibbling->next != NULL)
			sibbling = sibbling->next;
		// ... and add the child to the end of the list
		sibbling->next = child;
	}
	else
	{
		// first child
		block->children = child;
	}

	child->next = NULL;
	child->parent = block;
}

treenode * firstchild(treenode * node)
{
	return (treenode*)(node->children);
}

treenode * nextchild(treenode * node, treenode * sibbling)
{
	return (treenode*)(sibbling->next);
}

void adoptchildren(treenode * newparent, treenode * oldparent)
{
	// remove all children from oldparent, add them to newparent

	// find the last child of the new parent
	treenode * sibbling = newparent->children;
	if (sibbling != NULL)
	{
		// there are already childen, so find the last sibbling...
		while (sibbling->next != NULL)
			sibbling = sibbling->next;
		// ... and add the new children to the end of the list
		sibbling->next = oldparent->children;
	}
	else
	{
		// the newparent has no children, so just move the children over
		newparent->children = oldparent->children;
	}

	// adjust the parent pointers
	sibbling = newparent->children;
	while (sibbling != NULL)
	{
		sibbling->parent = newparent;
		sibbling = sibbling->next;
	}

	// delete oldparent node (no longer needed)
	free(oldparent);
}


//
// print tree
//

void printindent(FILE * f, int indent)
{
	for (; indent > 0; indent--)
		fprintf(f, "    ");
}

char * nodetypestring(NODE_TYPE t)
{
	switch (t)
	{
	case NT_BLOCK:
		return "BLOCK";
	case NT_NUMBER:
		return "NUMBER";
	case NT_COLOR_NAME:
		return "COLOR_NAME";
	case NT_FUNCTION:
		return "FUNCTION";
	case NT_TURTLE_CMD:
		return "TURTLE_CMD";
	case NT_IF:
		return "IF";
	case NT_IFELSE:
		return "IFELSE";
	case NT_OPERATOR:
		return "OPERATOR";
	case NT_REPEAT:
		return "REPEAT";
	case NT_DECLARATION:
		return "DECLARATION";
	case NT_VARIABLE:
		return "VARIABLE";
	case NT_ASSIGNMENT:
		return "ASSIGNMENT";
	default:
		return "UNKNOWN";
	}
}

char * turtlecmdtypestring(TURTLE_CMD cmd)
{
	switch (cmd)
	{
	case CMD_HOME:
		return "HOME";
	case CMD_FD:
		return "FD";
	case CMD_BK:
		return "BK";
	case CMD_RT:
		return "RT";
	case CMD_LT:
		return "LT";
	case CMD_SETC:
		return "SETC";
	case CMD_PD:
		return "PD";
	case CMD_PU:
		return "PU";
	}
}

char * colorstring(COLOR_TYPE c)
{
	switch (c)
	{
	case BLACK:
		return "BLACK";
	case WHITE:
		return "WHITE";
	case ORANGE:
		return "ORANGE";
	case YELLOW:
		return "YELLOW";
	case LIME:
		return "LIME";
	case CYAN:
		return "CYAN";
	case BLUE:
		return "BLUE";
	case MAGENTA:
		return "MAGENTA";
	case RED:
		return "RED";
	case BROWN:
		return "BROWN";
	case GREEN:
		return "GREEN";
	case TURQUOISE:
		return "TURQUOISE";
	case SKY:
		return "SKY";
	case VIOLET:
		return "VIOLET";
	case PINK:
		return "PINK";
	default:
		return "UNKNOWN";
	}
}

char * functionstring(FUNCTION_TYPE f)
{
	switch (f)
	{
	case FT_COLOR:
		return "COLOR";
	case FT_XCOR:
		return "XCOR";
	case FT_YCOR:
		return "YCOR";
	default:
		return "UNKNOWN";
	}
}

char * operatorstring(OPERATOR_TYPE op)
{
	switch (op)
	{
	case OT_EQUALS:
		return "=";
	case OT_LESSTHAN:
		return "<";
	case OT_GREATERTHAN:
		return ">";
	case OT_PLUS:
		return "+";
	case OT_MINUS:
		return "-";
	case OT_TIMES:
		return "*";
	case OT_DIVIDE:
		return "/";
	default:
		return "UNKNOWN";
	}
}

char * typestring(VARIABLE_TYPE type)
{
	switch (type)
	{
	case VT_INT:
		return "INT";
	default:
		return "UNKNOWN";
	}
}

void printnode(FILE * f, treenode * node, int indent)
{
	treenode * child;

	printindent(f, indent);
	fprintf(f, "%s", nodetypestring(node->type));
	switch (node->type)
	{
	case NT_NUMBER:
		fprintf(f, " %d\n", node->attribute.value);
		break;

	case NT_COLOR_NAME:
		fprintf(f, " %s\n", colorstring(node->attribute.color));
		break;

	case NT_FUNCTION:
		fprintf(f, " %s\n", functionstring(node->attribute.func));
		break;

	case NT_TURTLE_CMD:
		fprintf(f, " %s\n", turtlecmdtypestring(node->attribute.cmd));
		break;

	case NT_BLOCK:
	case NT_IF:
	case NT_IFELSE:
	case NT_REPEAT:
	default:
		fprintf(f, "\n");
		break;

	case NT_OPERATOR:
		fprintf(f, " %s\n", operatorstring(node->attribute.op));
		break;

	case NT_DECLARATION:
		fprintf(f, " %s\n", typestring(node->attribute.vartype));
		break;

	case NT_VARIABLE:
		fprintf(f, " %s\n", symtable[node->attribute.symentry].lexeme);
		break;
	}

	indent++;
	child = firstchild(node);
	while (child != NULL)
	{
		printnode(f, child, indent);
		child = nextchild(node, child);
	}
}

void printtree(FILE * f, treenode * node)
{
	int indent = 0;

	printnode(f, node, indent);
}

