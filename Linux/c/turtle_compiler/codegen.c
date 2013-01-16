// codegen.c
// OIT Fall 8007

//			Turtle codegen.c  
//			Modified by James Brooks 3/15/2012
//			For CST320 - Compilers Winter 2012
//

#include <stdio.h>
#include <stdlib.h>
#include "codegen.h"
#include "tree.h"
#include "symtable.h"
#include "YaccAssign4.h"


// constants
#define MAX_PROGRAM	0x10000
#define STACK_SIZE	0x00010


// forward
int generate_static_variables(unsigned char * program, int pc);
int generateNUMBER(unsigned char * program, int pc, treenode * root);
int generateCOLOR_NAME(unsigned char * program, int pc, treenode * root);
int generateVARIABLE(unsigned char * program, int pc, treenode * root);
int generateFUNCTION(unsigned char * program, int pc, treenode * root);
int generateOPERATOR(unsigned char * program, int pc, treenode * root);
int generate_expression(unsigned char * program, int pc, treenode * root);
int generateTURTLE_CMD(unsigned char * program, int pc, treenode * root);
int generateIF(unsigned char * program, int pc, treenode * root);
int generateIFELSE(unsigned char * program, int pc, treenode * root);
int generateREPEAT(unsigned char * program, int pc, treenode * root);
int generateASSIGNMENT(unsigned char * program, int pc, treenode * root);
int generate_statement(unsigned char * program, int pc, treenode * root);
int generateBLOCK(unsigned char * program, int pc, treenode * root);


//
// functions to generate program
//


int makeint(unsigned char hi, unsigned char lo)
{
	return (((int)hi << 8) & 0xff00) | (lo & 0x00ff);
}

unsigned char hibyte(int x)
{
	return (unsigned char)((x >> 8) & 0x00ff);
}

unsigned char lobyte(int x)
{
	return (unsigned char)(x & 0x00ff);
}

TURTLE_OPERATION func_to_turtle_op(FUNCTION_TYPE func)
{
	switch (func)
	{
		case FT_COLOR:
			return TURTLE_OPERATION_COLOR;
		case FT_XCOR:
			return TURTLE_OPERATION_XCOR;
		case FT_YCOR:
			return TURTLE_OPERATION_YCOR;
		default:
			return -1;
	}
}

TURTLE_OPERATION turtle_cmd_to_turtle_op(TURTLE_CMD cmd)
{
	switch (cmd)
	{
		case CMD_HOME:
			return TURTLE_OPERATION_HOME;
		case CMD_PU:
			return TURTLE_OPERATION_PU;
		case CMD_PD:
			return TURTLE_OPERATION_PD;
		case CMD_FD:
			return TURTLE_OPERATION_FD;
		case CMD_BK:
			return TURTLE_OPERATION_BK;
		case CMD_RT:
			return TURTLE_OPERATION_RT;
		case CMD_LT:
			return TURTLE_OPERATION_LT;
		case CMD_SETC:
			return TURTLE_OPERATION_SETC;
		case CMD_SETXY:
			return TURTLE_OPERATION_SETXY;
		default:
			return -1;
	}
}

COLORS color_type_to_colors(COLOR_TYPE c)
{
	switch(c)
	{
		case BLACK:
			return COLOR_BLACK;
		case WHITE:
			return COLOR_WHITE;
		case ORANGE:
			return COLOR_ORANGE;
		case YELLOW:
			return COLOR_YELLOW;
		case LIME:
			return COLOR_LIME;
		case CYAN:
			return COLOR_CYAN;
		case BLUE:
			return COLOR_BLUE;
		case MAGENTA:
			return COLOR_MAGENTA;
		case RED:
			return COLOR_RED;
		case BROWN:
			return COLOR_BROWN;
		case GREEN:
			return COLOR_GREEN;
		case TURQUOISE:
			return COLOR_TURQUOISE;
		case SKY:
			return COLOR_SKY;
		case VIOLET:
			return COLOR_VIOLET;
		case PINK:
			return COLOR_PINK;
		default:
			return -1;
	}
}

int generate_static_variables(unsigned char * program, int pc)
{
	// TODO	
	return pc;
}

int generateNUMBER(unsigned char * program, int pc, treenode * root)
{
	// LOAD_R G1 value
	program[pc++] = OPCODE_LOAD_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = hibyte(root->attribute.value);
	program[pc++] = lobyte(root->attribute.value);

	// PUSH_R G1
	program[pc++] = OPCODE_PUSH_R;
	program[pc++] = REGISTER_G1;

	return pc;
}

int generateCOLOR_NAME(unsigned char * program, int pc, treenode * root)
{
	//Push a value representing a color (0-15) on the stack  J. Brooks

	program[pc++] = OPCODE_LOAD_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = hibyte(root->attribute.color);
	program[pc++] = lobyte(root->attribute.color);

	// PUSH_R G1
	program[pc++] = OPCODE_PUSH_R;
	program[pc++] = REGISTER_G1;
	
	return pc;
}

int generateVARIABLE(unsigned char * program, int pc, treenode * root)
{
	// TODO
	return pc;
}

int generateFUNCTION(unsigned char * program, int pc, treenode * root)
{

	//Functions like (color = black) are put in the program steam here  J. Brooks
	
	//printf("Entering generateFunction with root type %i and attribute %i\n",root->type,root->attribute.func);

	switch (root->attribute.func)
	{
		case FT_COLOR:
			program[pc++] = OPCODE_TURTLE;
			program[pc++] = TURTLE_OPERATION_COLOR;
			program[pc++] = OPCODE_PUSH_R;
			program[pc++] = REGISTER_RE;
			break;

		case FT_XCOR:
			program[pc++] = OPCODE_TURTLE;
			program[pc++] = TURTLE_OPERATION_XCOR;
			program[pc++] = OPCODE_PUSH_R;
			program[pc++] = REGISTER_RE;
			break;

		case FT_YCOR:
			program[pc++] = OPCODE_TURTLE;
			program[pc++] = TURTLE_OPERATION_YCOR;
			program[pc++] = OPCODE_PUSH_R;
			program[pc++] = REGISTER_RE;
			break;
	}

	return pc;
}

int generateOPERATOR(unsigned char * program, int pc, treenode * root)
{
	//Math and boolean operators are handeled here J. Brooks

    //Get values for left and right values out of the tree J. Brooks

	// generate code for each child
	treenode * child = firstchild(root);
	while (child != NULL)
	{
		pc = generate_expression(program, pc, child);
		child = nextchild(root, child);
	}

	// POP_R G2
	program[pc++] = OPCODE_POP_R;
	program[pc++] = REGISTER_G2;
	
	// POP_R G1
	program[pc++] = OPCODE_POP_R;
	program[pc++] = REGISTER_G1;

	//Process math functions J. Brooks

	if (root->attribute.op == OT_PLUS)
	{
		// ADD_R G1 G2
		program[pc++] = OPCODE_ADD_R;
		program[pc++] = REGISTER_G1;
		program[pc++] = REGISTER_G2;
	}

	if (root->attribute.op == OT_MINUS)
	{
		// Sub_R G1 G2
		program[pc++] = OPCODE_SUB_R;
		program[pc++] = REGISTER_G1;
		program[pc++] = REGISTER_G2;
	}

	if (root->attribute.op == OT_TIMES)
	{
		// Mul_R G1 G2
		program[pc++] = OPCODE_MUL_R;
		program[pc++] = REGISTER_G1;
		program[pc++] = REGISTER_G2;
	}

	if (root->attribute.op == OT_DIVIDE)
	{
		// Divide_R G1 G2
		program[pc++] = OPCODE_DIV_R;
		program[pc++] = REGISTER_G1;
		program[pc++] = REGISTER_G2;
	}

	
	//Process boolean functions  J. Brooks
	//Flags are set by the compares for the jumps done by the caller (if/ifelse) 
	
	if (root->attribute.op == OT_EQUALS)
	{
		// Compare G1 G2
		program[pc++] = OPCODE_CMP_RR;
		program[pc++] = REGISTER_G1;
		program[pc++] = REGISTER_G2;
		program[pc++] = OPCODE_LOAD_R;
		program[pc++] = REGISTER_G1;
		program[pc++] = hibyte(1);
		program[pc++] = lobyte(1);
		
	}

	if (root->attribute.op == OT_LESSTHAN)
	{
		// Compare G1 G2
		program[pc++] = OPCODE_CMP_RR;
		program[pc++] = REGISTER_G1;
		program[pc++] = REGISTER_G2;
		program[pc++] = OPCODE_LOAD_R;
		program[pc++] = REGISTER_G1;
		program[pc++] = hibyte(1);
		program[pc++] = lobyte(1);
		
	}

	if (root->attribute.op == OT_GREATERTHAN)
	{
		// Compare G1 G2
		program[pc++] = OPCODE_CMP_RR;
		program[pc++] = REGISTER_G1;
		program[pc++] = REGISTER_G2;
		program[pc++] = OPCODE_LOAD_R;
		program[pc++] = REGISTER_G1;
		program[pc++] = hibyte(1);
		program[pc++] = lobyte(1);
		
	}
	
	// PUSH_R G1
	program[pc++] = OPCODE_PUSH_R;
	program[pc++] = REGISTER_G1;
	
	return pc;
}

int generate_expression(unsigned char * program, int pc, treenode * root)
{
	switch (root->type)
	{
		case NT_OPERATOR:
			pc = generateOPERATOR(program, pc, root);
			break;

		case NT_NUMBER:
			pc = generateNUMBER(program, pc, root);
			break;

		case NT_FUNCTION:
			pc = generateFUNCTION(program, pc, root);
			break;

		case NT_COLOR_NAME:
			pc = generateCOLOR_NAME(program, pc, root);
			break;

		// TODO: add rest here

		default:
			break;
	}

	return pc;
}

int generateTURTLE_CMD(unsigned char * program, int pc, treenode * root)
{
	// if the cmd expects 0 parameters...
	if (root->attribute.cmd == CMD_PU
	 || root->attribute.cmd == CMD_PD
	 || root->attribute.cmd == CMD_HOME)
	{
	
	}
	// if the cmd expects 1 parameter...
	if (root->attribute.cmd == CMD_FD
	 || root->attribute.cmd == CMD_BK
	 || root->attribute.cmd == CMD_SETC
	 || root->attribute.cmd == CMD_RT
	 || root->attribute.cmd == CMD_LT) // TODO: add a bunch of ||s
	{
		// generate code for child
		treenode * child = firstchild(root);
		pc = generate_expression(program, pc, child);

		// POP_R P1
		program[pc++] = OPCODE_POP_R;
		program[pc++] = REGISTER_P1;
	}
	// if the cmd expects 2 parameters...
	if (root->attribute.cmd == CMD_SETXY)
	{
		// generate code for child
		treenode * child = firstchild(root);
		pc = generate_expression(program, pc, child);

		// POP_R P1
		program[pc++] = OPCODE_POP_R;
		program[pc++] = REGISTER_P1;

		child = nextchild(root,child);
		pc = generate_expression(program, pc, child);

		// POP_R P1
		program[pc++] = OPCODE_POP_R;
		program[pc++] = REGISTER_P2;
	
	}
	
	// TURTLE attr.turtleop
	
	program[pc++] = OPCODE_TURTLE;
	program[pc++] = turtle_cmd_to_turtle_op(root->attribute.cmd);
	//printf("generated turtle_cmd with opcode %i and op %i\n",program[pc-2],program[pc-1]);

	return pc;
}

int generateIF(unsigned char * program, int pc, treenode * root)
{
	int temppc, calcpc;
	
	//Go process the boolean operator first J. Brooks
	
	treenode * child = firstchild(root);
	pc = generateOPERATOR(program, pc, child);
	
	//Pop the exteraneous push first
	program[pc++] = OPCODE_POP_R;
	program[pc++] = REGISTER_G1;
	
	//Jump around the the false parameter load if bool is true (True cmp val is already loaded by Operator processing)
	program[pc++] = OPCODE_JEq;
	program[pc++] = hibyte(4);
	program[pc++] = lobyte(4);

    //Load for false process jmp
	program[pc++] = OPCODE_LOAD_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = hibyte(0);
	program[pc++] = lobyte(0);
	program[pc++] = OPCODE_PUSH_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = OPCODE_POP_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = OPCODE_LOAD_R;
	program[pc++] = REGISTER_G2;
	program[pc++] = hibyte(0);
	program[pc++] = lobyte(0);
	program[pc++] = OPCODE_CMP_RR;
	program[pc++] = REGISTER_G1;
	program[pc++] = REGISTER_G2;

	//jmp around block if test was false
	program[pc++] = OPCODE_JEq;
    temppc = pc;
	//Reserve jump relative param spots
	program[pc++] = hibyte(0);
	program[pc++] = lobyte(0);

	//Process the block
	child = nextchild(root,child);
	pc = generateBLOCK(program, pc, child);

	//calc the block size
	calcpc = pc - temppc - 2;
	
	//fix up the jump relative
	program[temppc++] = hibyte(calcpc);
	program[temppc++] = lobyte(calcpc);
	
	return pc;
}

int generateIFELSE(unsigned char * program, int pc, treenode * root)
{
	int temppc, calcpc;

    //Similar to if processing, except a true block and a false block	

	treenode * child = firstchild(root);
	pc = generateOPERATOR(program, pc, child);

	program[pc++] = OPCODE_POP_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = OPCODE_JEq;
	program[pc++] = hibyte(4);
	program[pc++] = lobyte(4);
	program[pc++] = OPCODE_LOAD_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = hibyte(0);
	program[pc++] = lobyte(0);
	program[pc++] = OPCODE_PUSH_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = OPCODE_POP_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = OPCODE_LOAD_R;
	program[pc++] = REGISTER_G2;
	program[pc++] = hibyte(0);
	program[pc++] = lobyte(0);
	program[pc++] = OPCODE_CMP_RR;
	program[pc++] = REGISTER_G1;
	program[pc++] = REGISTER_G2;
	program[pc++] = OPCODE_JEq;
 
	//Setup to jump the true block
	temppc = pc;
	program[pc++] = hibyte(0);
	program[pc++] = lobyte(0);

	//Make the true block
	child = nextchild(root,child);
	pc = generateBLOCK(program, pc, child);

	//Size the true block
	calcpc = pc - temppc + 1;
	program[temppc++] = hibyte(calcpc);
	program[temppc++] = lobyte(calcpc);

    //Setup to jump the false block
	program[pc++] = OPCODE_JMPRe;
    temppc = pc;
	program[pc++] = hibyte(0);
	program[pc++] = lobyte(0);

	//Make the false block
	child = nextchild(root,child);
	pc = generateBLOCK(program, pc, child);

	//Size the false block
	calcpc = pc - temppc-2;
	program[temppc++] = hibyte(calcpc);
	program[temppc++] = lobyte(calcpc);

	
	return pc;
}

int generateREPEAT(unsigned char * program, int pc, treenode * root)
{
	int temppc, temppc2, calcpc;
	//printf("Entering generateREAPET with root type %i and attribute %i\n",root->type,root->attribute.func);

	//Repeat build.  First get the repeat count
	treenode * child = firstchild(root);
	pc = generate_expression(program, pc, child);
	program[pc++] = OPCODE_POP_R;
	program[pc++] = REGISTER_G1;
	
	//Bump loop count for pre decrement
	program[pc++] = OPCODE_INC_R; 
	program[pc++] = REGISTER_G1;

	//Push loop count
	program[pc++] = OPCODE_PUSH_R;
	program[pc++] = REGISTER_G1;
	
	//Jump to pre decrement processing
	program[pc++] = OPCODE_JMPRe;

    temppc = pc;
	program[pc++] = hibyte(0);
	program[pc++] = lobyte(0);

	child = nextchild(root,child);

	//Process the block
	temppc2 = pc;
	pc = generateBLOCK(program, pc, child);

    //Fix up the jump
	calcpc = pc - temppc-2;
	program[temppc++] = hibyte(calcpc);
	program[temppc++] = lobyte(calcpc);

	//Decr and compare loop count
	program[pc++] = OPCODE_POP_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = OPCODE_DEC_R; 
	program[pc++] = REGISTER_G1;
	program[pc++] = OPCODE_PUSH_R;
	program[pc++] = REGISTER_G1;
	program[pc++] = OPCODE_LOAD_R;
	program[pc++] = REGISTER_G2;
	program[pc++] = hibyte(0);
	program[pc++] = lobyte(0);
	program[pc++] = OPCODE_CMP_RR;
	program[pc++] = REGISTER_G1;
	program[pc++] = REGISTER_G2;

	//Loop done jump to finish
	program[pc++] = OPCODE_JEq;
	program[pc++] = hibyte(3);
	program[pc++] = lobyte(3);

	//Continue more repeat jump back to top of block
	program[pc++] = OPCODE_JMPRe;
	calcpc = temppc2 - pc - 2;
	program[pc++] = hibyte(calcpc);
	program[pc++] = lobyte(calcpc);

	//Clean the stack
	program[pc++] = OPCODE_POP_R;
	program[pc++] = REGISTER_G1;

	return pc;
}

int generateASSIGNMENT(unsigned char * program, int pc, treenode * root)
{
	// TODO
	return pc;
}

int generate_statement(unsigned char * program, int pc, treenode * root)
{
	switch (root->type)
	{
		case NT_TURTLE_CMD:
			pc = generateTURTLE_CMD(program, pc, root);
			break;

		case NT_IF:
			pc = generateIF(program, pc, root);
			break;

		case NT_IFELSE:
			pc = generateIFELSE(program, pc, root);
			break;

		case NT_REPEAT:
			pc = generateREPEAT(program, pc, root);
			break;

		case NT_ASSIGNMENT:
			pc = generateASSIGNMENT(program, pc, root);
			break;

		default:
			break;
	}

	return pc;
}

int generateBLOCK(unsigned char * program, int pc, treenode * root)
{
	treenode * child = firstchild(root);
	while (child != NULL)
	{
		pc = generate_statement(program, pc, child);
		child = nextchild(root, child);
	}

	return pc;
}

unsigned char * generate(treenode * root)
{
	unsigned char * program = (unsigned char *)malloc(MAX_PROGRAM);
	int pc = 0;

	// header
	pc += 8;
	
	// static variables
	program[2] = hibyte(pc);
	program[3] = lobyte(pc);
	pc = generate_static_variables(program, pc);
	 
	// program
	program[4] = hibyte(pc);
	program[5] = lobyte(pc);
	pc = generateBLOCK(program, pc, root);
	program[pc++] = OPCODE_EXIT;

	// stack
	program[6] = hibyte(pc);
	program[7] = lobyte(pc);
	pc += STACK_SIZE;
	
	// fix up header's size member
	program[0] = hibyte(pc);
	program[1] = lobyte(pc);

	return program;
}

int getprogramsize(unsigned char * program)
{
	return makeint(program[0], program[1]);
}


//
// print program
//

char * registername(REGISTER r)
{
	switch(r)
	{
	case REGISTER_P1:
		return "p1";
	case REGISTER_P2:
		return "p2";
	case REGISTER_RE:
		return "re";
	case REGISTER_G1:
		return "g1";
	case REGISTER_G2:
		return "g2";
	case REGISTER_G3:
		return "g3";
	case REGISTER_G4:
		return "g4";
	case REGISTER_PC:
		return "pc";
	case REGISTER_ST:
		return "st";
	case REGISTER_FL:
		return "fl";
	default:
		return "UNKNOWN";
	}
}

char * turtle_operation_string(TURTLE_OPERATION t)
{
	switch (t)
	{
		case TURTLE_OPERATION_HOME:
			return "HOME";
		case TURTLE_OPERATION_FD:
			return "FD";
		case TURTLE_OPERATION_BK:
			return "BK";
		case TURTLE_OPERATION_RT:
			return "RT";
		case TURTLE_OPERATION_LT:
			return "LT";
		case TURTLE_OPERATION_PU:
			return "PU";
		case TURTLE_OPERATION_PD:
			return "PD";
		case TURTLE_OPERATION_SETC:
			return "SETC";
		case TURTLE_OPERATION_SETXY:
			return "SETXY";
		case TURTLE_OPERATION_COLOR:
			return "COLOR";
		case TURTLE_OPERATION_XCOR:
			return "XCOR";
		case TURTLE_OPERATION_YCOR:
			return "YCOR";
		default:
			return "UNKNOWN";
	}
}

int printopcode(FILE * f, unsigned char * program, int i)
{
	REGISTER r1;
	REGISTER r2;
	unsigned char valh;
	unsigned char vall;
	unsigned char var1h;
	unsigned char var1l;
	unsigned char var2h;
	unsigned char var2l;
	unsigned char deltah;
	unsigned char deltal;
	unsigned char addrh;
	unsigned char addrl;
	TURTLE_OPERATION t;

	// print memory address first
	fprintf(f, "    %04x ", i & 0xffff);

	// find the opcode
	OPCODE op = program[i++];

	switch (op)
	{
	case OPCODE_LOAD_R:
		r1 = program[i++];
		valh = program[i++];
		vall = program[i++];
		fprintf(f, "LOAD_R %s %04x\n", registername(r1), makeint(valh, vall));
		break;

	case OPCODE_LOAD_V:
		var1h = program[i++];
		var1l = program[i++];
		valh = program[i++];
		vall = program[i++];
		fprintf(f, "LOAD_V %04x %04x\n", makeint(var1h, var1l), makeint(valh, vall));
		break;

	case OPCODE_MOVE_RR:
		r1 = program[i++];
		r2 = program[i++];
		fprintf(f, "MOVE_RR %s %s\n", registername(r1), registername(r2));
		break;

	case OPCODE_MOVE_RV:
		r1 = program[i++];
		var1h = program[i++];
		var1l = program[i++];
		fprintf(f, "MOVE_RV %s %04x\n", registername(r1), makeint(var1h, var1l));
		break;

	case OPCODE_MOVE_VR:
		var1h = program[i++];
		var1l = program[i++];
		r1 = program[i++];
		fprintf(f, "MOVE_VR %04x %s\n", makeint(var1h, var1l), registername(r1));
		break;

	case OPCODE_MOVE_VV:
		var1h = program[i++];
		var1l = program[i++];
		var2h = program[i++];
		var2l = program[i++];
		fprintf(f, "MOVE_VV %04x %04x\n", makeint(var1h, var1l), makeint(var2h, var2l));
		break;

	case OPCODE_CMP_RR:
		r1 = program[i++];
		r2 = program[i++];
		fprintf(f, "CMP_RR %s %s\n", registername(r1), registername(r2));
		break;

	case OPCODE_JMPRe:
		deltah = program[i++];
		deltal = program[i++];
		fprintf(f, "JMPRe %04x\n", makeint(deltah, deltal));
		break;

	case OPCODE_JMPTo:
		addrh = program[i++];
		addrl = program[i++];
		fprintf(f, "JMPTo %04x\n", makeint(addrh, addrl));
		break;

	case OPCODE_JEq:
		deltah = program[i++];
		deltal = program[i++];
		fprintf(f, "JEq %04x\n", makeint(deltah, deltal));
		break;

	case OPCODE_JGt:
		deltah = program[i++];
		deltal = program[i++];
		fprintf(f, "JGt %04x\n", makeint(deltah, deltal));
		break;

	case OPCODE_JLt:
		deltah = program[i++];
		deltal = program[i++];
		fprintf(f, "JLt %04x\n", makeint(deltah, deltal));
		break;

	case OPCODE_DEC_R:
		r1 = program[i++];
		fprintf(f, "DEC_R %s\n", registername(r1));
		break;

	//Added this case to solve unknown listing	
	case OPCODE_INC_R:
		r1 = program[i++];
		fprintf(f, "INC_R %s\n", registername(r1));
		break;

	case OPCODE_ADD_R:
		r1 = program[i++];
		r2 = program[i++];
		fprintf(f, "ADD_R %s %s\n", registername(r1), registername(r2));
		break;

	case OPCODE_SUB_R:
		r1 = program[i++];
		r2 = program[i++];
		fprintf(f, "SUB_R %s %s\n", registername(r1), registername(r2));
		break;

	case OPCODE_MUL_R:
		r1 = program[i++];
		r2 = program[i++];
		fprintf(f, "MUL_R %s %s\n", registername(r1), registername(r2));
		break;

	case OPCODE_DIV_R:
		r1 = program[i++];
		r2 = program[i++];
		fprintf(f, "DIV_R %s %s\n", registername(r1), registername(r2));
		break;

	case OPCODE_PUSH_R:
		r1 = program[i++];
		fprintf(f, "PUSH_R %s\n", registername(r1));
		break;

	case OPCODE_POP_R:
		r1 = program[i++];
		fprintf(f, "POP_R %s\n", registername(r1));
		break;

	case OPCODE_PEEK_R:
		r1 = program[i++];
		fprintf(f, "PEEK_R %s\n", registername(r1));
		break;

	case OPCODE_TURTLE:
		t = (TURTLE_OPERATION)program[i++];
		fprintf(f, "TURTLE %s\n", turtle_operation_string(t));
		break;

	case OPCODE_EXIT:
		fprintf(f, "EXIT\n");
		break;

	default:
		fprintf(f, "UNKNOWN\n");
		break;
	}

	return i;
}

void printprogram(FILE * f, unsigned char * program)
{
	int i;
	OPCODE op;

	// header
	int size = makeint(program[0], program[1]);
	int variables = makeint(program[2], program[3]);
	int code = makeint(program[4], program[5]);
	int stack = makeint(program[6], program[7]);
	fprintf(f, "header->\n");
	fprintf(f, "    size      = 0x%04x\n", size);
	fprintf(f, "    variables = 0x%04x\n", variables);
	fprintf(f, "    code      = 0x%04x\n", code);
	fprintf(f, "    stack     = 0x%04x\n", stack);
	fprintf(f, "<-header\n");

	// static variables
	fprintf(f, "static variables->\n");
	i = variables;
	fprintf(f, "    0x%04x bytes reserved\n", code - variables);
	fprintf(f, "<-static variables\n");
	
	// code
	fprintf(f, "code->\n");
	i = code;
	do
	{
		op = (OPCODE)program[i];
		i = printopcode(f, program, i);
	}
	while (op != OPCODE_EXIT);
	fprintf(f, "<-code\n");

	// stack
	fprintf(f, "stack->\n");
	fprintf(f, "    0x%04x bytes reserved\n", size - stack);
	fprintf(f, "<-stack\n");
}

