#ifndef DATA_H
#define DATA_H
#include <ostream>

using namespace std;

class data
{
public:
	data() : name(NULL) {}
	data(char const * const name);
	~data();						//destructor

	data& operator=(const data& data2);

	char const * const getName() const;
	void setName(char * name);

private:
	char * name;

	friend ostream& operator<< (ostream& out, const data& outData);
};

bool operator< (const data& d1, const data& d2);
bool operator== (const data& d1, const data& d2);

#endif