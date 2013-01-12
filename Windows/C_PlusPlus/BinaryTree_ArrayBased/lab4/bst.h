// do not change this file except within the private section

#ifndef BST_H
#define BST_H

#include "data.h"

class BST
{
public:
	BST(int capacity = 5);							
	BST(const BST& aTable);
	~BST();

	void insert(const data& aData);
	bool remove(const char *key);
	bool retrieve(const char *key, data& aData) const;
	void displayArrayOrder(ostream& out) const;
	void displayPreOrder(ostream& out) const;
	void displayInOrder(ostream& out) const;
	void displayPostOrder(ostream& out) const;
	int getSize(void) const;						// returns number of data items contained in the BST

private:
	struct item
	{
		bool	empty;
		data	scientist;							//Computer Scientist. - I like the ring of that!
	};
	item *items;

	int		size;
	int		root;
	int		capacity;

	//You need a lot of methods for BST - binary search trees.  

	//Some for inserting

	void grow();
	void insert(int node, const data & aData); 

	//And some for printing

	void preorder(ostream& out, int node) const;
	void inorder(ostream& out, int node) const;
	void postorder(ostream& out, int node) const;

	//And especially some when you delete stuff

	void deleteNodeItem(int node);
	void deleteItem(int node, const char *key);
	void processLeftMost(int child, int node);
	void adjustTree(int child, int node);
};


#endif