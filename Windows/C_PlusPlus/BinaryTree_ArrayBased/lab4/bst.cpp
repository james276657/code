#include "BST.h"
#include <iostream>
#include <iomanip>
#include <string>

using namespace std;

//Implement an array based binary tree and some methods

BST::BST(int capacity) :
	items(new item[capacity]),
	size(0), 
	root(0),
	capacity(capacity)

{
	int i;
	
	//Initialize the tree

	for (i = 0; i < this->capacity; i++)
	{
		this->items[i].empty = true;
	}

	//Debug watch tree
	//this->items[0];
	//this->items[1];
	//this->items[2];
	//this->items[3];
	//this->items[4];
	//this->items[5];
	//this->items[6];
	//this->items[7];
	//this->items[8];
	//this->items[9];
	//this->items[10];
	//this->items[11];
	//this->items[12];
	//this->items[13];
	//this->items[14];
	//this->items[15];
	//this->items[16];
	//this->items[17];


}

//Good old destructor 
BST::~BST()
{
	delete [] items;
}

//Started with a static array, but now we have more stuff than array, so make it bigger

void BST::grow()
{
	int	i,j;

	//grow space by making new array, deleting old and naming old to new

	//Keep the old capacity value for traverse
	j = this->capacity;

	//Double capacity. 
	this->capacity *= 2;
	item *newitems = new item[capacity];

	for (i = 0; i < j; i++) 
	{
		//Initialize the array

		newitems[i].empty = true;
	}

	for (i = 0; i < j; i++) 
	{
		//Copy old array into new

        if (!(this->items[i].empty))
        {

                newitems[i].scientist = this->items[i].scientist;
                newitems[i].empty = false;
        }
	}

	//Delete the old array and fix up the pointer
	delete [] this->items;
	this->items = newitems;
	
}

//Overloded insert to do the actual work

void BST::insert(int node, const data &aData)
{
	if(node >= capacity)
	{
		//Grow the array if index out of bounds
	
		this->grow(); 
	}


	if (this->items[node].empty) 
	{
		//Insert into an empty node

        this->items[node].scientist = aData;
        this->items[node].empty = false;
        this->size++;
	}

	else if (aData < this->items[node].scientist) 
	{
		//Calc the left child index if key less than data and recurse

		node = (2 * node) + 1; 
        this->insert(node,aData);   
	}

	else
	{
		// Calc the right child index and recurse FOR insert.

        node = (2 * node) + 2; 
        this->insert(node,aData); 
	}
}
void BST::insert (const data& aData)
{
	int node;

	//Call the overloaded insert with index pointer to root

	node = this->root;
	this->insert(node, aData);
}

bool BST::retrieve(const char *key, data& aData) const
{
	int i;
	bool ans;

	//Maybe we won't find it

	ans = false;

	//Find the data with the key

	for (i = 0; i < this->capacity; i++) 
	{
        if (!(this->items[i].empty) && (strcmp(this->items[i].scientist.getName(), key) == 0))
        {
			aData = this->items[i].scientist;
			ans = true;
			break;
        }
	}

	//Tell the caller the news.

	return ans;
}

//Here are the remove methods, adjust tree, processLeftMost,  deleteNodeItem, deleteItem and the driver remove.
//This algorithm was taken from Carrano page 551.  This is the most work in this program.

void BST::adjustTree(int child, int node)
{
	int l,r;

	//After node removal, take care of the right subtree below it, if any.

	l = child * 2 + 1;
	r = child * 2 + 2;

	if ((this->items[l].empty && this->items[r].empty) || r > this->capacity)
	{
		//No right tree so done.

		this->items[child].empty = true;
	}
	else if (!(this->items[r].empty))
	{
		//Bubble the right subtree up.
		this->items[child].scientist = this->items[r].scientist;
		this->items[child].empty = false;
		this->items[r].empty = true;
	}
	
}

void BST::processLeftMost(int child, int node)
{
	int l,r;

	//Go look for the first node with an empty left subtree

	l = child * 2 + 1;
	r = child * 2 + 2;

	if ((this->items[l].empty || (l > this->capacity)))
	{
		//Pop the data from the node into the deleted item location

		this->items[node].scientist = this->items[child].scientist;

		//Tree adjustment after replacement in case there is a right subtree

		this->adjustTree(child,node);
	}
	else
	{
		//Keep looking for that special leftless node

		this->processLeftMost(l,node);
	}
}
void BST::deleteNodeItem(int node)
{
	int l,r;

	//Find out what kind of a node you're dealing with.
	//Leaf's and one child nodes are easier than two child nodes

	l = node * 2 + 1;
	r = node * 2 + 2;

	if ((this->items[l].empty && this->items[r].empty) || (l > this->capacity))
	{
		//The node to delete is a leaf so do it now.

		this->items[node].empty = true;
	}
	else if (this->items[r].empty)
	{
		//Here's a left child only node so replace the data with its left childs 
		//and mark left child empty.

		this->items[node].scientist = this->items[l].scientist;
		this->items[l].empty = true;
	}
	else if (this->items[l].empty)
	{
		//Here's a right child only node so replace the data with its right childs 
		//and mark right child empty.

		this->items[node].scientist = this->items[r].scientist;
		this->items[r].empty = true;
	}
	else
	{
		//Do a hunt for a special leftless node to use for replace, and then fix up tree

		this->processLeftMost(r,node);
	}

}

void BST::deleteItem(int node, const char *key)
{
	//Find the node to delete by compary node data to key.

	if (node < this->capacity  && (!(this->items[node].empty)))
	{
		if(!(this->items[node].empty) && (strcmp(key, this->items[node].scientist.getName()) == 0))
		{
			//Found it.  Now go delete it.

			deleteNodeItem(node);
			this->size--;
		}
		else if (strcmp(key, this->items[node].scientist.getName()) < 0)
		{
			//Look to the left

			node = node * 2 + 1;
			BST::deleteItem(node,key);
		}
		else
		{
			//Look to the right

			node = node * 2 + 2;
			BST::deleteItem(node,key);
		}
	}
}


bool BST::remove(const char* key)
{
	int i;
	bool ans;

	//See if the data exists before deleting it

	ans = false;

	for (i = 0; i < this->capacity; i++) 
	{
        if (!(this->items[i].empty) && (strcmp(this->items[i].scientist.getName(), key) == 0))
        {
			ans = true;
			break;
        }
	}


	if (ans)
	{
		//Yep its there, so run the deletion algorithms on it

		this->deleteItem(this->root,key);
	}

	// Maybe yes maybe no only the ans knows for sure.

	return ans;
}

void BST::displayArrayOrder (ostream& out) const
{
	int	i,j,k;
	string str;

	//Compose the data for output in Array Order

	//Output the table header

	out << left << ">>> array order:" << endl << endl;
	out << "name                    leaf?  index" << endl;
	out << "----                    -----  -----" << endl;

	//Iterate through the data to collect it in the output.

	for (i = 0; i < this->capacity; i++ ) 
	{
        if (!this->items[i].empty)
        {
			
			out << left << setw(24);

			//Let the scientist object print out the object data
			cout << this->items[i].scientist;

			//Figure out if we need a leaf notation

			str = "";
			k = i * 2 + 1;
			j = i * 2 + 2;
			if ((this->items[k].empty && this->items[j].empty) || (k > this->capacity))
			{
				str = "leaf";
			}

			//Format the output properly

			out << setw(8) << str << right << setw(4) << i << endl; 
        }
	}
	
	//Finish with a item count statistic

	out << "(items printed)                      (" << this->getSize() << ')' << endl; 
}

void BST::preorder(ostream& out, int node) const
{
	int l,r;
	string str;

	//Preorder recursive routine on an array based tree.
	
	l = node * 2 + 1;
	r = node * 2 + 2;

	//Don't look beyond the array

	if (node < this->capacity)
	{
		//Start with the root

		//Format the output and figure out if its a leaf

		out << left << setw(24);
		cout << this->items[node].scientist;
		str = "";
		if ((this->items[l].empty && this->items[r].empty) || (l > this->capacity))
		{
			str = "leaf";
		}
		out << setw(8) << str << right << setw(4) << node << endl; 

		//Go to the left subtree next

		if (!(this->items[l].empty))
		{
			preorder(out,l);
		}
		
		//Now do the right subtree.

		if (!(this->items[r].empty))
		{
			preorder(out,r);
		}
	}
}

void BST::inorder(ostream& out, int node) const
{
	int l,r,j,k;
	string str;

	l = node * 2 + 1;
	r = node * 2 + 2;

	if (node < this->capacity)
	{
	
		//Start with the left subtree towards the As

		if (!(this->items[l].empty))
		{
			inorder(out,l);
		}

		//Print the most left node in the subtree

		if(!(this->items[node].empty))
		{
			out << left << setw(24);
			cout << this->items[node].scientist;
			j = node * 2 + 1;
			k = node * 2 + 2;
			str = "";
			if ((this->items[j].empty && this->items[k].empty) || (j > this->capacity))
			{
				str = "leaf";
			}
			out << setw(8) << str << right << setw(4) << node << endl; 
		}

		//Now do the right subtree toward the Zs

		if (!(this->items[r].empty))
		{
			inorder(out,r);
		}
	}
}

void BST::postorder(ostream& out, int node) const
{
	int l,r,j,k;
	string str;
	
	l = node * 2 + 1;
	r = node * 2 + 2;

	if (node < this->capacity)
	{
		//Start with the left subtree 

		if (!(this->items[l].empty))
		{
			postorder(out,l);
		}

		//Now go look right

		if (!(this->items[r].empty))
		{
			postorder(out,r);
		}

		//Now print whats right first

		if(!(this->items[node].empty))
		{
			out << left << setw(24);
			cout << this->items[node].scientist;
			j = node * 2 + 1;
			k = node * 2 + 2;
			str = "";
			if ((this->items[j].empty && this->items[k].empty) || (j > this->capacity))
			{
				str = "leaf";
			}
			out << setw(8) << str << right << setw(4) << node << endl; 
		}
	}
}

void BST::displayPreOrder (ostream& out) const
{

	//Compose the data for output in PreOrder

	//Output the table header

	out << left << ">>> preorder:" << endl << endl;
	out << "name                    leaf?  index" << endl;
	out << "----                    -----  -----" << endl;

	//Call the recursive preorder output method

	preorder(out,this->root);

	//Finish with a item count statistic

	out << "(items printed)                      (" << this->getSize() << ')' << endl; 
}

void BST::displayInOrder (ostream& out) const
{

	//Compose the data for output in InOrder  (my favorite, sorted alpha wise)

	//Output the table header

	out << left << ">>> inorder:" << endl << endl;
	out << "name                    leaf?  index" << endl;
	out << "----                    -----  -----" << endl;

	//Call the recursive inorder output method

	inorder(out,this->root);

	//Finish with a item count statistic

	out << "(items printed)                      (" << this->getSize() << ')' << endl; 
}

void BST::displayPostOrder (ostream& out) const
{

	//Compose the data for output in PostOrder

	//Output the table header

	out << left << ">>> postorder:" << endl << endl;
	out << "name                    leaf?  index" << endl;
	out << "----                    -----  -----" << endl;

	//Call the recursive postorder output method

	postorder(out,this->root);

	//Finish with a item count statistic

	out << "(items printed)                      (" << this->getSize() << ')' << endl; 
}

int BST::getSize (void) const
{
	//Somebody want to know how many items we have.

	return this->size;
}

