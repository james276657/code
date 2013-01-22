using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Lab3_CST236
{
    /// <summary>
    /// Menu displays the program menu. It is implemented as a singleton
    /// so it is only instantiated once.
    /// 
    /// James Brooks 10/22/11
    /// </summary>

    public sealed class Menu //Singleton Menu
    {
        private static readonly Lazy<Menu> _instance = new Lazy<Menu>(() => new Menu());

        private string[,] menulist = new string[22, 2] { 
        { "*","*************************************************************************" }, 
        { "*","                                                                        *" }, 
        { "*","                     Test Driven Development Program                    *" }, 
        { "*","           Assignment 2 CST 236 - Bhushan Gupta OIT Winter 2011         *" }, 
        { "*","                       By James Brooks  2/22/2012                       *" }, 
        { "*","                                                                        *" }, 
        { "*"," This program collects an IQ from the user and presents an IQ           *" }, 
        { "*"," classification and recommended occupation.   The program is developed  *" }, 
        { "*"," incrementaly with a test driven development process.                   *" }, 
        { "*","                                                                        *" }, 
        { "*","*************************************************************************" }, 
        { " ", " " }, 
        { "IQ analyzer.  Given an IQ score, it returns an IQ classification", " " }, 
        { "and recommended occupation.", " " }, 
        { " ", " " }, 
        { " ", "    Press a numeric key to proceed. " }, 
        { " ", " " }, 
        { "1> ", "  Classify an IQ score" }, 
        { "2> ", "  Review test logfile. " }, 
        { "3> ", "  Exit program. " }, 
        { " ", " " }, 
        { " ", "                  Menu choice> " }, 
        };
        private int i, j;

        private Menu() { }

        public static Menu Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        public void displayMenu()
        {
            for (i = 0; i < menulist.GetLength(0); i++)
            {
                for (j = 0; j < (menulist.GetLength(menulist.Rank - 1) - 0); j++)
                {
                    Console.Write(menulist[i, j]);
                }
                if (i < menulist.GetLength(0) - 1)
                    Console.WriteLine("");
            }

        }
    }
}
