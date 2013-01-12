#define _CRT_SECURE_NO_DEPRECATE	// stop deprecation warnings, needs to be on first line

#include "data.h"
#include <iostream>
#include <iomanip>
#include <string>

using namespace std;

//Make scientist data object to put into the array based binary tree.

data::data(char const * const name) : name(new char[strlen(name)+1])
{

	//Give me a name when you call my constructor or NULL

	if (name)
	{
        strcpy(this->name , name);
	}  
	else
	{
		this->name = NULL;
	}

}

data& data::operator=(const data& data2)
{
	//Operator = doesn't need to duplicate

	if ( this == &data2) 
	{
        return *this;
	}

	//But replace?, yes it does.

	delete [] name; 
	name = NULL;
	name = new char[strlen(data2.name)+1];  // allocate new space
	strcpy(name,data2.name);  //copy
	return *this;
}

data::~data()
{
	//The world is on the eve of destructor

	delete [] name; 
}

char const * const data::getName() const
{
	//Whats my name?

	return this->name;
}

void data::setName (char * name)
{
	//Give me a new name please

	strcpy(this->name, name);
}

bool operator< (const data& d1, const data& d2)
{

	// return true if d1 is "less than" d2, false otherwise

	return strcmp(d1.getName(), d2.getName()) < 0;
}

bool operator== (const data& d1, const data& d2)
{
	// return true if d1 is equal to d2, false otherwise

	return strcmp(d1.getName(), d2.getName()) == 0;
}

ostream& operator<< (ostream& out, const data& outData)
{
	//Print my name, I want to see it on the silver screen

	out << outData.name;
	return out;
}