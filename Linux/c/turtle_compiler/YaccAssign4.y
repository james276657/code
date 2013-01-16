%{
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include "tree.h"
#include "symtable.h"
#include "codegen.h"



void header(char *str);
void output(char *str);
void output2(char *str);

treenode * root;

%}
%union {
  float f;
  char *s;
  int i;
  treenode * node;
  int value;
  int symentry;
}

%token <value> NUMBER
%token <value> COLOR_NAME
%token <i> LINENUMBER
%token <symentry> VARIABLE 
%token FD BK RT LT SETC SETXY SETX SETY HOME SETH PD PU HT ST
%token XCOR YCOR HEADING RANDOM COLOR
%token IF IFELSE REPEAT
%token INT
%token ILLEGAL
%left '+' '-'
%left '*' '/'
%right UMINUS
%verbose

%type <node> stmts
%type <node> stmt
%type <node> expr
%type <node> function
%type <node> condition
%type <node> lhs

%%
stmts		: stmt stmts					{if ($1 != NULL) {$$ = makenode_BLOCK(); addchild($$, $1); if($2 != NULL){adoptchildren($$, $2);}root = $$;} output("y statements->statement statements\n");}
			|								{$$ = NULL; output("y statements->empty\n");}
			;
stmt		: HOME							{$$ = makenode_TURTLE_CMD0(CMD_HOME); output("y statement->HOME\n");}
			| PD							{$$ = makenode_TURTLE_CMD0(CMD_PD); output("y statement->PD\n");}
			| PU							{$$ = makenode_TURTLE_CMD0(CMD_PU);output("y statement->PU\n");}
			| FD expr						{$$ = makenode_TURTLE_CMD1(CMD_FD, $2); char buffer [120]; sprintf(buffer,"y statement->FD expression\n");output(buffer);}
			| BK expr						{$$ = makenode_TURTLE_CMD1(CMD_BK, $2);char buffer [120]; sprintf(buffer,"y statement->BK expression\n");output(buffer);}
			| RT expr						{$$ = makenode_TURTLE_CMD1(CMD_RT, $2);char buffer [120]; sprintf(buffer,"y statement->RT expression\n");output(buffer);}
			| LT expr						{$$ = makenode_TURTLE_CMD1(CMD_LT, $2);char buffer [120]; sprintf(buffer,"y statement->LT expression\n");output(buffer);}
			| SETC expr						{$$ = makenode_TURTLE_CMD1(CMD_SETC, $2);char buffer [120]; sprintf(buffer,"y statement->SETC colorname\n");output(buffer);}
			| SETXY expr expr				{$$ = makenode_TURTLE_CMD2(CMD_SETXY, $2, $3); }
			| REPEAT expr '[' stmts ']'     {$$ = makenode_REPEAT($2, $4); output("y statements->repeat\n");}
			| IF '(' condition ')' '[' stmts ']'     { $$ = makenode_IF($3, $6); output("y statements->if\n");}
			| IFELSE '(' condition ')' '[' stmts ']' '[' stmts ']'    {$$ = makenode_IFELSE($3, $6, $9); output("y statements->if\n");}
			| lhs '=' expr					{ $$ = makenode_ASSIGNMENT($1, $3); }
			;
expr        : expr '+' expr                	{$$ = makenode_OPERATOR(OT_PLUS, $1, $3); output("y expression->expression + expression\n");}
            | expr '-' expr                	{$$ = makenode_OPERATOR(OT_MINUS, $1, $3);output("y expression->expression - expression\n");}
            | expr '*' expr                	{$$ = makenode_OPERATOR(OT_TIMES, $1, $3);output("y expression->expression * expression\n");}
            | expr '/' expr                	{$$ = makenode_OPERATOR(OT_DIVIDE, $1, $3);output("y expression->expression / expression\n");}
			| COLOR_NAME					{$$ = makenode_COLOR_NAME($1);}
            | VARIABLE                     	{char buffer [120]; sprintf(buffer,"y expression->VARIABLE %lf \n",$1); output(buffer);}
            | NUMBER                       	{$$ = makenode_NUMBER($1); char buffer [120]; sprintf(buffer,"y expression->NUMBER %i \n",$1);output(buffer);}
			| function						{$$ = $1; }
			;

condition	: expr '=' expr					{$$ = makenode_OPERATOR(OT_EQUALS, $1, $3); }
			| expr '<' expr					{$$ = makenode_OPERATOR(OT_LESSTHAN, $1, $3); }
			| expr '>' expr					{$$ = makenode_OPERATOR(OT_GREATERTHAN, $1, $3); }
			;

			
function	: XCOR							{$$ = makenode_FUNCTION(FT_XCOR); }
			| YCOR							{$$ = makenode_FUNCTION(FT_YCOR); }
			| COLOR							{$$ = makenode_FUNCTION(FT_COLOR); }
			;

lhs:		VARIABLE						{$$ = makenode_VARIABLE($1); }
			;

%%

/* stuff from lex that yacc needs to know about: */

extern int yylex();
extern int yyparse();
extern FILE *yyin;
extern FILE * yyout;
extern int lineno;
extern char* yytext;


main(argc, argv)
int argc;
char** argv;
{
char buffer [150];

	remove ("james_brooks_assign4_out");
	header(argv[1]);
	if (argc > 1)
	{
		FILE *file;
		file = fopen(argv[1], "r");
		if (!file)
		{
			fprintf(stderr, "Could not open %s\n", argv[1]);
			return(1);
		}
		/* set lex to read from file instead of defaulting to STDIN: */
		yyin = file;
		sprintf(buffer,"\n");
		output(buffer);
	}
	if (argc > 2)
	{
		FILE *out_file;
		out_file = fopen(argv[2], "w");
		if (out_file == NULL)
		{
			fprintf(stderr, "Cannot open output file %s\n", argv[2]);
			exit(-1);
		}
		yyout = out_file;
	}

	
	/* parse through the input until there is no more: */
	do
	{
		yyparse();
	} 
	while (!feof(yyin));

	if (yyin != NULL)
	{
		fclose(yyin);
	}

    if (root != NULL)
	{
		fprintf(stdout, "Syntax Tree...\n");
		printtree(stdout, root);
		fprintf(stdout, "\n");


		unsigned char * program = generate(root);
		if (program != NULL)
		{
			fprintf(stdout, "Object Code...\n");
			printprogram(stdout, program);

			if (yyout != NULL)
			{
				int size = getprogramsize(program);
				fprintf(stdout, "writing %d bytes to file\n", size);
				fwrite(program, 1, size, yyout);
			}
		}
	}

	return 0;
}

yyerror(s) 
char *s;
{
	char buffer [120];
	
	sprintf(buffer,"ERROR! Line%i: %s at %s\n", lineno, s, yytext);
	output(buffer);
}

void output(char *str)
{
	FILE * outfile;
	outfile = fopen("james_brooks_assign4_out", "a");

    /* Uncomment to print to STDOUT */
	/*
	printf(str);
	*/
	fprintf(outfile,str);
    fclose(outfile);

}
void output2(char *str)
{
/*
	FILE * outfile;
	outfile = fopen("james_brooks_assign4_out", "a");

	printf(str);

	fprintf(outfile,str);
    fclose(outfile);
*/
}
void header(char *str)
{
 char buffer [120];
 
 sprintf(buffer,"\nJames Brooks CST320 Assignment 3 Instructor Pete Myers OIT Winter 2012\n");
 output(buffer);
 sprintf(buffer,"\n");
 output(buffer);
 sprintf(buffer,"turtle logo parser result for turtle program from file %s\n",str);
 output(buffer);
 sprintf(buffer,"\n");
 output(buffer);
}



