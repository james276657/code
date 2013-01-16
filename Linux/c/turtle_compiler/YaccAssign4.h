/* A Bison parser, made by GNU Bison 2.3.  */

/* Skeleton interface for Bison's Yacc-like parsers in C

   Copyright (C) 1984, 1989, 1990, 2000, 2001, 2002, 2003, 2004, 2005, 2006
   Free Software Foundation, Inc.

   This program is free software; you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation; either version 2, or (at your option)
   any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software
   Foundation, Inc., 51 Franklin Street, Fifth Floor,
   Boston, MA 02110-1301, USA.  */

/* As a special exception, you may create a larger work that contains
   part or all of the Bison parser skeleton and distribute that work
   under terms of your choice, so long as that work isn't itself a
   parser generator using the skeleton or a modified version thereof
   as a parser skeleton.  Alternatively, if you modify or redistribute
   the parser skeleton itself, you may (at your option) remove this
   special exception, which will cause the skeleton and the resulting
   Bison output files to be licensed under the GNU General Public
   License without this special exception.

   This special exception was added by the Free Software Foundation in
   version 2.2 of Bison.  */

/* Tokens.  */
#ifndef YYTOKENTYPE
# define YYTOKENTYPE
   /* Put the tokens into the symbol table, so that GDB and other debuggers
      know about them.  */
   enum yytokentype {
     NUMBER = 258,
     COLOR_NAME = 259,
     LINENUMBER = 260,
     VARIABLE = 261,
     FD = 262,
     BK = 263,
     RT = 264,
     LT = 265,
     SETC = 266,
     SETXY = 267,
     SETX = 268,
     SETY = 269,
     HOME = 270,
     SETH = 271,
     PD = 272,
     PU = 273,
     HT = 274,
     ST = 275,
     XCOR = 276,
     YCOR = 277,
     HEADING = 278,
     RANDOM = 279,
     COLOR = 280,
     IF = 281,
     IFELSE = 282,
     REPEAT = 283,
     INT = 284,
     ILLEGAL = 285,
     UMINUS = 286
   };
#endif
/* Tokens.  */
#define NUMBER 258
#define COLOR_NAME 259
#define LINENUMBER 260
#define VARIABLE 261
#define FD 262
#define BK 263
#define RT 264
#define LT 265
#define SETC 266
#define SETXY 267
#define SETX 268
#define SETY 269
#define HOME 270
#define SETH 271
#define PD 272
#define PU 273
#define HT 274
#define ST 275
#define XCOR 276
#define YCOR 277
#define HEADING 278
#define RANDOM 279
#define COLOR 280
#define IF 281
#define IFELSE 282
#define REPEAT 283
#define INT 284
#define ILLEGAL 285
#define UMINUS 286




#if ! defined YYSTYPE && ! defined YYSTYPE_IS_DECLARED
typedef union YYSTYPE
#line 19 "YaccAssign4.y"
{
  float f;
  char *s;
  int i;
  treenode * node;
  int value;
  int symentry;
}
/* Line 1489 of yacc.c.  */
#line 120 "YaccAssign4.h"
	YYSTYPE;
# define yystype YYSTYPE /* obsolescent; will be withdrawn */
# define YYSTYPE_IS_DECLARED 1
# define YYSTYPE_IS_TRIVIAL 1
#endif

extern YYSTYPE yylval;

