/*
* Assignment 4, c program components,  by James Brooks 5/21/2012
* CST240 Unix Spring 2012 OIT  
* This program, cscrape, searches portland.craigslist.org in "all for sale" for items specified by search terms
* The program is intended to be driven by the bash script searchcraigs
*/

/* Program includes */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <regex.h>
#include <ctype.h>
#include <unistd.h>


/* Program constants */

/*Error codes */
int const SUCCESS = 0;
int const INVALID_COMMAND_LINE_OPTION = -1;
int const INVALID_COMMAND_LINE_PARAMETER = -2;
int const ERROR_RETRIEVING_RAW_HTML_FILE = -3;
int const NO_NEW_LINKS_FOUND = -4;
int const NO_SEARCH_TERMS_SPECIFIED = -5;

/*History timer */
int const HISTORY_LIFE_IN_SECONDS = 2592000; 	/*30 days */

/*GP Buffer size */
int const STRING_BUFFER_MAX_SIZE = 1024;
int const STRING_BUFFER_ORDINARY_SIZE = 256;

/*GP Booleans */
int const FALSE = 0;
int const TRUE = 1;
int const EQUAL = 0;

/*Wget constants */
char const WGET_PART_A[] = "`wget --timeout=5 --quiet -U Mozilla -t 1 -O ";
char const WGET_PART_B[] = " http://portland.craigslist.org/search/sss\?query=";
char const WGET_PART_C[] = "&srchType=T&minAsk=&maxAsk=`";
char const WGET_PART_D[] = "&srchType=A&minAsk=&maxAsk=`";
 

/* Program global variables */

/* Simple type globals */
/************************************************************************
*
*  Debug flag to see all debugging set to 5  to see none set to 0
*
*************************************************************************/
int debug=0;				/* Debug flag  FALSE = debug off  1+ =  debug on */

int pipeplay=0;				/*pipeplay = TRUE turns off verbosity for pipe useage */	
int searchcontent = 0;		/*Flag for search content.  0 = title search only   This doesn't work right.  It seems all content is always searched*/
int newhistory = 0;			/*Flag for new history.  Deletes old history file */


int firstpass;				/*First term - create new link output file Else append./

/* Function prototypes */
int managesearchterms(char *user, char searchterms[]);
int getrawpage(char *user, char *inputfile);
int extractnewlinks(char *user, char *inputfile);
char* getdate();
char* getuser();
void signon();
void useage();
void pm(char message[]);

/* main Program */
int main(int argc, char ** argv)
{
	/* local variables */
	int i,k,j,l,m;			
    	int locallinksfound;
	char c;
 	char space[] = " ";
	char lf[] = "\n";
    	char output_string[STRING_BUFFER_MAX_SIZE];
	
	char searchterms[STRING_BUFFER_MAX_SIZE]; 		/*Items to be searched for.  Replaces current list if specified */
	char searchterm[STRING_BUFFER_ORDINARY_SIZE]; 		/*One item to be searched for.  */
	char outputfile[STRING_BUFFER_ORDINARY_SIZE];

	char * home;
    	char rawlinkpath[STRING_BUFFER_ORDINARY_SIZE];	
	
	FILE *rp;

	/* Get the input arguments */
	
	searchterms[0] = '\0';
	
	for (i = 1; i < argc; i++)
	{
		/* Look for options arguments with a dash */
		if (argv[i][0] == '-')
		{
			/* Found a dash so check for valid arguments */
			/* See if its -C */
			if (strcmp(argv[i], "-C") == FALSE)
			{
				searchcontent = TRUE;
			}
			/* See if its -H */
			else if (strcmp(argv[i], "-H") == FALSE)
			{
				newhistory = TRUE;
				if(debug>3){printf("Found -H\n");}
			}
			/* See if its -Q */
			else if (strcmp(argv[i], "-Q") == FALSE)
			{
				pipeplay = TRUE;
				if(debug>3){printf("Found -Q\n");}
			}
			/* If not -C , -H  or -Q then error */
			else
			{
				fprintf(stderr,"Command line error detected. Argument in error was %s. Program cscrape\n\n", argv[i]);
				useage();
				exit(INVALID_COMMAND_LINE_OPTION);
			}
		}
		else
		{
			//Make a single string of the search terms
			if (searchterms[0] == '\0')
			{
				strcpy(searchterms,argv[i]);
			}
			else 
			{
				strcat (searchterms," "); 
				strcat(searchterms,argv[i]);
			}
		}
	}
	
	if(debug>3){printf("\n\nHi there from craigslist web scraper\n\n");}

	/* Set up path for raw page file */
	home=getenv("HOME");
	strcat(home,"/");
	strcpy(rawlinkpath,home);
	strcat(rawlinkpath,".cscrape.craiglinks");
	
	if(debug>3){printf("\nScraping from file %s\n\n",rawlinkpath);}
	
	/*firstpass for first search term */
	firstpass = TRUE;
	
	/* Get searchterms from command line or previous useage file */
	i = managesearchterms(getuser(),searchterms);
	
	locallinksfound = 0;
	
	if (i == TRUE)
	{
		if(debug>3){printf("Search terms are %s\n",searchterms);}
		k = 0;
		j = 0;
		/*Seperate the search terms and run page get and extract for each one */
		while (j <= strlen(searchterms))
		{
			if ((searchterms[j] != ' ') && (j < strlen(searchterms)))
			{
				j++;
			}
			else 
			{
				m = 0;
				for (l = k; l < j; l++)
				{
					searchterm[m] = searchterms[l];
					m++;
				}
				searchterm[m] = '\0';
				if(debug>3){printf("Current search term is %s\n",searchterm);}
				j++;
				k = j; 

				/*Get the raw page with wget */
				i = getrawpage(searchterm,rawlinkpath);
				if (i != 0)
				{
					fprintf(stderr,"\nError getting raw html file - Program cscrape.c\n");
					exit(ERROR_RETRIEVING_RAW_HTML_FILE);
				}
				/*Extract the links and compare to history looking for new links */
				i = extractnewlinks(getuser(),rawlinkpath);
				if (locallinksfound == 0){locallinksfound = i;}
			}
		}
	}
	else
	{
		if(debug>3){printf("No search terms found");}
		fprintf(stderr,"No search terms specified, Program cscrape\n");
		exit(NO_SEARCH_TERMS_SPECIFIED);
	}

	/* Links found test */
	if (locallinksfound > 0)
	{
		//if (pipeplay != TRUE)
		if (TRUE)
		{
			//pm("\n\nFound new links from cscrape\n\n");
			strcpy(outputfile,home);
			strcat(outputfile,".");
			strcat(outputfile,getuser());
			strcat(outputfile,".cscrape.newlinks.output");
			rp = fopen(outputfile, "r");
			while(fgets(output_string, STRING_BUFFER_ORDINARY_SIZE, rp) != NULL)
			{	
				printf("%s",output_string);
			}
			fclose(rp);
		}
		
		return SUCCESS;
		if(debug>3){printf("\n\nDone scraping from file %s\n",rawlinkpath);}
	}
	else
	{
		//pm("\n\nNo new links found from cscrape\n\n");
		if(debug>3){printf("\n\nDone scraping from file %s\n",rawlinkpath);}
		printf("No new links found\n");
		return NO_NEW_LINKS_FOUND;
	}
}

/* Program functions */
int managesearchterms(char *user, char *searchterms)
{
	char searchtermfile[STRING_BUFFER_ORDINARY_SIZE];
	char * home4;
	FILE *wp;			 /* declare output pointer */
	FILE *rp;			 /* declare output pointer */

    	home4=getenv("HOME");
	strcpy(searchtermfile,home4);
	strcat(searchtermfile,".");
	strcat(searchtermfile,user);
	strcat(searchtermfile,".cscrape.searchterms");

	if(searchterms[0] !='\0')
	{
		if(debug>3){printf("Search term from arguments are %s\n",searchterms);}
		//Save the search terms for next time maybe
		if(debug>3){printf("Search term file is %s\n",searchtermfile);}
		wp=fopen(searchtermfile, "w");
		fprintf(wp,"%s",searchterms);
		fclose(wp);
		return TRUE;
	}
	else
	{
		if( access( searchtermfile, F_OK ) != -1 ) 
		{
		
			rp=fopen(searchtermfile, "r");
			fgets(searchterms, STRING_BUFFER_MAX_SIZE, rp);
			fclose(rp);
			return TRUE;
		}
		return FALSE;
	}
}

int getrawpage(char *item, char *outputfile)
{
	char wgetstring[STRING_BUFFER_MAX_SIZE];
	int sysret;
	
	/*Construct the wget command*/
	
	strcpy(wgetstring,WGET_PART_A);
	strcat(wgetstring,outputfile);
	strcat(wgetstring,WGET_PART_B);
	strcat(wgetstring,item);

	/*Note this switch doesn't seem to make a difference in the results  <revisit>*/
	if (searchcontent == TRUE){strcat(wgetstring,WGET_PART_D);} else {strcat(wgetstring,WGET_PART_C);}

	if(debug>3){printf("Wget string is %s\n",wgetstring);}
	sysret = system(wgetstring);
	if(debug>3){printf("System returned %i after wget command\n",sysret);}
	return sysret;
}

int extractnewlinks(char *user, char *infile)
{
	char line[STRING_BUFFER_ORDINARY_SIZE];
	char line2[STRING_BUFFER_ORDINARY_SIZE];
	char line3[STRING_BUFFER_ORDINARY_SIZE];
	char extractedlinksfile[STRING_BUFFER_ORDINARY_SIZE];
	char historyfile[STRING_BUFFER_ORDINARY_SIZE];
	char newlinksfile[STRING_BUFFER_ORDINARY_SIZE];
	char tempfile[STRING_BUFFER_ORDINARY_SIZE];
	char outputfile[STRING_BUFFER_ORDINARY_SIZE];

	char * home3;
	char *l,*m;

	int i,j,p,q,r,s;
	int test1, test2, test3;
	int new = 0;
	int newlinksfound = 0;
    	int linksfound = 0;
	
    	home3=getenv("HOME");
	if(debug>4){printf("The home dir is %s\n",home3);}
	
	strcpy(historyfile,home3);
	strcat(historyfile,".");
	strcat(historyfile,user);
	strcat(historyfile,".cscrape.history");
	strcpy(extractedlinksfile,home3);
	strcat(extractedlinksfile,".");
	strcat(extractedlinksfile,user);
	strcat(extractedlinksfile,".cscrape.linkoutput");
	strcpy(newlinksfile,home3);
	strcat(newlinksfile,".");
	strcat(newlinksfile,user);
	strcat(newlinksfile,".cscrape.newlinks");
	strcpy(tempfile,home3);
	strcat(tempfile,".");
	strcat(tempfile,user);
	strcat(tempfile,".temp");
	strcpy(outputfile,home3);
	strcat(outputfile,".");
	strcat(outputfile,user);
	strcat(outputfile,".cscrape.newlinks.output");
	
	if(debug>4){printf("The home dir is %s\n",home3);}
	if(debug>4){printf("The input file to Extract links from is %s\n",infile);}
	if(debug>4){printf("User history file is %s\n",historyfile);}
	if(debug>4){printf("Link extraction output file is %s\n",extractedlinksfile);}
	if(debug>4){printf("New link output file is %s\n",newlinksfile);}
	
	/* Remove the history file on request.  All links found then will be new links */
	if (newhistory == TRUE)
	{
		/* Dont do this for each search term */
		newhistory = FALSE;
		if( access( historyfile, F_OK ) != -1 )
		{
			if (remove(historyfile) == -1)
			{
				if(debug>3){printf("Problem removing file %s",historyfile);}
			}
		}
	}
	
	FILE *fp;            /* declare the file pointer */
	FILE *wp;			 /* declare output pointer */
	FILE *cp;			 /* declare compare file pointer */
	
	int rc;
    	regex_t * myregex = calloc(1, sizeof(regex_t));
 
	//Get the epoch seconds from the system to add to links
    	strcpy(line2,getdate());
	line2[10]=' ';

	// Compile a regular expression to look for links
    	rc = regcomp( myregex, "\\.html", REG_EXTENDED | REG_NOSUB );

	//Extract the links from the raw wget file
	if(debug>4){printf("Opening %s\n",extractedlinksfile);}
	wp=fopen(extractedlinksfile, "w");
	if(debug>4){printf("Opening %s\n",infile);}
	fp=fopen(infile, "r");	
	if(debug>4){printf("File open return %i Opening %s\n",fp,infile);}
	
	if(debug>4){printf("Runnning while loop\n");}
	while(fgets(line, STRING_BUFFER_ORDINARY_SIZE, fp) != NULL)
    	{
		if (0 == regexec(myregex, line, 0 , 0 , 0 ) )
		{
			l = &line[0];
			while isspace(*l){++l;}
			line3[0] = '\0';
			strcpy(line3,line2);
			strcat(line3,l);
			
			if(debug>4){printf("%s\n", line3);}
			fprintf(wp,"%s", line3);
			linksfound = TRUE;
		}
    	}
	if(debug>4){printf("Done with while loop\n");}
    	fclose(fp);  /* close the file prior to exiting the routine */
	fclose(wp);

	//Free myregex
	free(myregex);
	
	//Filter for new links if any links found by comparing to history 
	if (linksfound == TRUE)
	{
		newlinksfound = 0;

		if( access( historyfile, F_OK ) == -1 ) 
		{
			if(debug>3){printf("User history file %s does not exist.  Creating new one.\n",historyfile);}
			//Create new history file
			wp = fopen(historyfile, "w");
			fp = fopen(extractedlinksfile, "r");
			while(fgets(line, STRING_BUFFER_ORDINARY_SIZE, fp) != NULL)
			{
				fprintf(wp,"%s",line);
			}
			fclose(fp);  
			fclose(wp);
			//All links are new links here
			wp = fopen(newlinksfile, "w");
			fp = fopen(extractedlinksfile, "r");
			while(fgets(line, STRING_BUFFER_ORDINARY_SIZE, fp) != NULL)
			{
				fprintf(wp,"%s",line);
			}
			fclose(fp);  
			fclose(wp);
			newlinksfound = 1;
		} 
		else 
		{
			//Compare history against current links
			wp = fopen(newlinksfile, "w");
			fp = fopen(extractedlinksfile, "r");
			while(fgets(line, STRING_BUFFER_ORDINARY_SIZE, fp) != NULL)
			{
				new = 1;
				cp = fopen(historyfile, "r");
				while(fgets(line2, STRING_BUFFER_ORDINARY_SIZE, cp) != NULL)
				{
					p = 0;
					q = 0;
					while ((line[p] != '<') && (p < strlen(line))){p++;}
					while ((line2[q] != '<')&& (q < strlen(line2))){q++;}
					if ((line[p] == '<') && (line2[q] == '<'))
					{
						l = &line[p];
						m = &line2[q];
						if(debug>4){printf("Comparing history file line %s with extracted links line %s\n",m,l);}
						if(0==strcmp(m,l)){new = 0;}
					}
				}
				fclose(cp);
				if (new == 1){fprintf(wp,"%s",line);newlinksfound = 1;}
			}
			fclose(fp);
			fclose(wp);
		}
		//Format new links for publishing
		if (newlinksfound == 1)
		{
			if (firstpass == TRUE){wp = fopen(outputfile, "w"); firstpass = FALSE;} else {wp = fopen(outputfile, "a");}
			fp = fopen(newlinksfile, "r");
			while(fgets(line, STRING_BUFFER_ORDINARY_SIZE, fp) != NULL)
			{
				/*Extract the link and description */
				if(debug>4){printf("Unformatted output link is %s\n",line);}
				p = 0;
				while ((line[p] != '"') && (p < strlen(line))){p++;}
				q = p + 1;
				while ((line[q] != '"') && (q < strlen(line))){q++;}
				r = q + 1;
				while ((line[r] != '>') && (r < strlen(line))){r++;}
				s = r + 1;
				while ((line[s] != '<') && (s < strlen(line))){s++;}
				if ((line[p] == '"') && (line[q] == '"') && (line[r] == '>') && (line[s] == '<'))
				{
					j = 0;
					for (i = p+1; i < q; i++)
					{
						line2[j] = line[i];
						j++;
					}
					line2[j] = ' ';
					j++;
					for (i = r+1; i < s; i++)
					{
						line2[j] = line[i];
						j++;
					}
					line2[j]='\0';
					if(debug>4){printf("Formatted output link is %s\n",line2);}
					fprintf(wp,"%s\r\n",line2);
				}
			}
			fclose(fp);
			fclose(wp);
		}
	
		//clean up old history
		cp = fopen(historyfile, "r");
		wp = fopen(tempfile, "w");
		strcpy(line3,getdate());
		line3[10]='\0';
		while(fgets(line, STRING_BUFFER_ORDINARY_SIZE, cp) != NULL)
		{
			strncpy(line2,line,strlen(line3));
			line2[strlen(line3)] ='\0';
			if(debug>4){printf("Comparing history file date data line %s with getdate data line %s\n",line2,line3);}
			test3 = (int)line3 - (int)line2;
			//Keep history younger than thiry days
			if (test3 < HISTORY_LIFE_IN_SECONDS)
			{
				if(debug>4){printf("Date compare would have this line be kept %s",line);}
				fprintf(wp,"%s",line);
			}
			else
			{
				if(debug>4){printf("Date compare would have this line be deleted %s",line);}
			}
		}
		fclose(cp);
		fclose(wp);
	
		//Restore history from temp and append the new links found (if any) 
		wp = fopen(historyfile, "w");
		cp = fopen(tempfile, "r");
		strcpy(line3,getdate());
		line3[10]='\0';
		while(fgets(line, STRING_BUFFER_ORDINARY_SIZE, cp) != NULL)
		{
			fprintf(wp,"%s",line);
		}
		fclose(cp);
		if (newlinksfound == 1)
		{
			cp = fopen(newlinksfile, "r");
			while(fgets(line, STRING_BUFFER_ORDINARY_SIZE, cp) != NULL)
			{
				fprintf(wp,"%s",line);
			}
			fclose(cp);
		}
		fclose(wp);

		//remove newlinks file 
		if (remove(newlinksfile) == -1)
		{
			if(debug>3){printf("Problem removing file %s",newlinksfile);}
		}
		//remove the temp ,extracted links and input files
		if (remove(tempfile) == -1)
		{
			if(debug>3){printf("Problem removing file %s",tempfile);}
		}
		if (remove(extractedlinksfile) == -1)
		{
			if(debug>3){printf("Problem removing file %s",extractedlinksfile);}
		}
		if (remove(infile) == -1)
		{
			if(debug>3){printf("Problem removing file %s",infile);}
		}
	}
	return newlinksfound;
}

char* getuser()
{
	char systemcommand[STRING_BUFFER_ORDINARY_SIZE];
	char buffer[STRING_BUFFER_ORDINARY_SIZE];

	sprintf(systemcommand,"whoami");
 
	/*Get the stat command results into a temp file */
	FILE *lsofFile_p = popen(systemcommand, "r");
 
	/*Put the stat results in a string */
	char *line_p = fgets(buffer, sizeof(buffer), lsofFile_p);
	pclose(lsofFile_p);
	line_p[strlen(line_p)-1] = 0;
	return line_p;

}

char* getdate()
{
	char systemcommand[STRING_BUFFER_ORDINARY_SIZE];
	char buffer[STRING_BUFFER_ORDINARY_SIZE];

	//Get the year from the system
	//sprintf(systemcommand,"date +\"\%Y\"");
    	//Get date
 	//sprintf(systemcommand,"date +\"%%m%%d%%Y\"");

    	//Get epoch seconds
 	sprintf(systemcommand,"date +\"%%s\"");
    	if(debug>4){printf("The get date command string is %s\n",systemcommand);}
	/*Get the system command results into a temp file */
	FILE *lsgfFile_p = popen(systemcommand, "r");

	/*Put the stat results in a string */
	char *line_g = fgets(buffer, sizeof(buffer), lsgfFile_p);
	pclose(lsgfFile_p);
    	if(debug>4){printf("The get date command string is %s and the date retrieved is %s \n",systemcommand, line_g);}
	return line_g;
}

void signon()
{
	if (pipeplay == FALSE)
	{
		system("clear");
		printf("Assignment 4, cscrape.c  for CST240 OIT Spring 2012.\n");
		printf("  Written by James Brooks. 6/4/12.\n");
		printf("\n");
		printf("This c program, cscrape searches listings on craigs list\n");
		printf("\n");
		printf("Program useage:  cscrape [-H] [-Q] [-C] [search terms]\n");
		printf("\n");
	}
	
}

void useage()
{
	/*printf("\n     Program useage:  compdir [-R] [-D] <dir1> <dir2>\n");*/
	printf("\n     Program useage:  cscrape [-H] [-Q] [-C] [search terms]\n");
}

void pm(char message[])
{
	if (pipeplay == FALSE)
	{
		printf(message);
	}
}

