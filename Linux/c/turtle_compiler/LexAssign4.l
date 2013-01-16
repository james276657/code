/***********************************************************************
*
* 		Assignment 2 - Turtle Lexical Analysis Program
* 		For Compilers Class OIT Winter 2012  Instructor Pete Myers
* 		from James Brooks  February 15, 2012
*
************************************************************************/
/***********************************************************************
*
* This program defines the Turtle Logo Grammar.  Lex is employed to create
* PDA c source code for the front end for a Turtle compiler.
*
* Lex source files have a suffix of .l.  They are divided into three sections seperated
* by %%.  The first section is included at the top of the output2 c file.  The second
* section defines regular expressions for Lex to create the grammar PDAs from.
*The third section is user defined functions including main.
************************************************************************/
%{

/*This is the header stuff from James Brooks' Lexer */

#include <stdlib.h>
#include <stdio.h>
#include "tree.h"
#include "YaccAssign4.h"
#include "symtable.h"

int lineno = 1;
%}
%%
\/\*([^*]|[\n]|(\*+([^*/]|[\n])))*\*\/+ {}
\/\/[^\n]*\n				{}    
fd					{char buffer [120]; sprintf(buffer,"Keyword:    fd\n");output2(buffer);return FD;}
bk					{char buffer [120]; sprintf(buffer,"Keyword:    bk\n");output2(buffer);return BK;}
rt					{char buffer [120]; sprintf(buffer,"Keyword:    rt\n");output2(buffer);return RT;}
lt					{char buffer [120]; sprintf(buffer,"Keyword:    lt\n");output2(buffer);return LT;}
setc				{char buffer [120]; sprintf(buffer,"Keyword:    setc\n");output2(buffer);return SETC;}
setxy				{char buffer [120]; sprintf(buffer,"Keyword:    setxy\n");output2(buffer);return SETXY;}
setx				{char buffer [120]; sprintf(buffer,"Keyword:    setx\n");output2(buffer);return SETX;}
sety				{char buffer [120]; sprintf(buffer,"Keyword:    sety\n");output2(buffer);return SETY;}
home				{char buffer [120]; sprintf(buffer,"Keyword:    home\n");output2(buffer);return HOME;}
seth				{char buffer [120]; sprintf(buffer,"Keyword:    seth\n");output2(buffer);return SETH;;}
pd					{char buffer [120]; sprintf(buffer,"Keyword:    pd\n");output2(buffer);return PD;}
pu					{char buffer [120]; sprintf(buffer,"Keyword:    pu\n");output2(buffer);return PU;}
ht					{char buffer [120]; sprintf(buffer,"Keyword:    ht\n");output2(buffer);return HT;}
st					{char buffer [120]; sprintf(buffer,"Keyword:    st\n");output2(buffer);return ST;}
color				{char buffer [120]; sprintf(buffer,"Keyword:    color\n");output2(buffer);return COLOR;}
xcor				{char buffer [120]; sprintf(buffer,"Keyword:    xcor\n");output2(buffer);return XCOR;}
ycor				{char buffer [120]; sprintf(buffer,"Keyword:    ycor\n");output2(buffer);return YCOR;}
heading				{char buffer [120]; sprintf(buffer,"Keyword:    heading\n");output2(buffer);return HEADING;}
random				{char buffer [120]; sprintf(buffer,"Keyword:    random\n");output2(buffer);return RANDOM;}
if					{char buffer [120]; sprintf(buffer,"Keyword:    if\n");output2(buffer);return IF;}
ifelse				{char buffer [120]; sprintf(buffer,"Keyword:    ifelse\n");output2(buffer);return IFELSE;}
repeat				{char buffer [120]; sprintf(buffer,"Keyword:    repeat\n");output2(buffer);return REPEAT;}
int					{char buffer [120]; sprintf(buffer,"Keyword:    int\n");output2(buffer);return INT;}
black				{char buffer [120]; sprintf(buffer,"Keyword:    black\n");output2(buffer);yylval.value = BLACK; return COLOR_NAME;}
white				{char buffer [120]; sprintf(buffer,"Keyword:    white\n");output2(buffer);yylval.value = WHITE; return COLOR_NAME;}
orange				{char buffer [120]; sprintf(buffer,"Keyword:    orange\n");output2(buffer);yylval.value = ORANGE; return COLOR_NAME;}
yellow				{char buffer [120]; sprintf(buffer,"Keyword:    yellow\n");output2(buffer);yylval.value = YELLOW; return COLOR_NAME;}
lime				{char buffer [120]; sprintf(buffer,"Keyword:    lime\n");output2(buffer);yylval.value = LIME; return COLOR_NAME;}
cyan				{char buffer [120]; sprintf(buffer,"Keyword:    cyan\n");output2(buffer);yylval.value = CYAN; return COLOR_NAME;}
blue				{char buffer [120]; sprintf(buffer,"Keyword:    blue\n");output2(buffer);yylval.value = BLUE; return COLOR_NAME;}
magenta				{char buffer [120]; sprintf(buffer,"Keyword:    magenta\n");output2(buffer);yylval.value = MAGENTA; return COLOR_NAME;}
red					{char buffer [120]; sprintf(buffer,"Keyword:    red\n");output2(buffer);yylval.value = RED; return COLOR_NAME;}
brown				{char buffer [120]; sprintf(buffer,"Keyword:    brown\n");output2(buffer);yylval.value = BROWN; return COLOR_NAME;}
green				{char buffer [120]; sprintf(buffer,"Keyword:    green\n");output2(buffer);yylval.value = GREEN; return COLOR_NAME;}
turquoise			{char buffer [120]; sprintf(buffer,"Keyword:    turquoise\n");output2(buffer);yylval.value = TURQUOISE; return COLOR_NAME;}
sky					{char buffer [120]; sprintf(buffer,"Keyword:    sky\n");output2(buffer);yylval.value = SKY; return COLOR_NAME;}
violet				{char buffer [120]; sprintf(buffer,"Keyword:    violet\n");output2(buffer);yylval.value = VIOLET; return COLOR_NAME;}
pink				{char buffer [120]; sprintf(buffer,"Keyword:    pink\n");output2(buffer);yylval.value = PINK; return COLOR_NAME;}
"["   				{char buffer [120]; sprintf(buffer,"l_Keyword:    %c\n",yytext[0]);output2(buffer); return yytext[0];}
"]"   				{char buffer [120]; sprintf(buffer,"l_Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
"+"   				{char buffer [120]; sprintf(buffer,"Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
"*"   				{char buffer [120]; sprintf(buffer,"Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
"="   				{char buffer [120]; sprintf(buffer,"Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
">"   				{char buffer [120]; sprintf(buffer,"Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
"<"   				{char buffer [120]; sprintf(buffer,"Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
"("   				{char buffer [120]; sprintf(buffer,"Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
")"   				{char buffer [120]; sprintf(buffer,"Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
"-"   				{char buffer [120]; sprintf(buffer,"l_Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
"/"   				{char buffer [120]; sprintf(buffer,"l_Keyword:    %c\n",yytext[0]);output2(buffer);return yytext[0];}
-?[0-9]+		        {char buffer [120]; sprintf(buffer,"Number:     %s\n", yytext);output2(buffer); sscanf(yytext, "%d", &(yylval.value)); return NUMBER; }  
[a-zA-Z][a-zA-Z0-9]*	{char buffer [120]; sprintf(buffer,"Variable: %s\n", yytext);output2(buffer);yylval.s = strdup(yytext);return VARIABLE;} 
[0-9][a-zA-Z0-9]*	{char buffer [120]; sprintf(buffer,"Illegal:    %s\n", yytext);output2(buffer);yylval.s = strdup(yytext);return ILLEGAL;} 
[ \t\r]+ 				{}
\n                  {lineno++;}
.     				{}
%%         
