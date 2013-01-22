/**********************************************************************
*                                                                                                                         *
*        Lab2 Solution 1 for CST352 Operating Systems Class OIT                        *
*        Fall Term 2011 by James Brooks  10/22/11                                                *
*                                                                                                                         *
***********************************************************************/

#include <pthread.h>
#include <stdio.h>
#include <time.h>

/* 
Here is a constant defining the max random number - 1 for produceItem
*/

const int MAX_NUM = 100;
const int LOOP_COUNT = 1000;

/* 
Here is a constant defining the buffer size 
*/

const BUFFER_SIZE = 10;

/* 
Here is the buffer shared by the producer and consumer 
*/

int buffer[11];

/* 
Here is global count shared by the producer and consumer 
It is initialized in main
*/

int itemCount;


/*
Program subroutines follow
*/

/* 
Here is produceItem()  It produces a random number between 0 and MAX_NUMBER - 1 
*/

int produceItem(void)
{
	int n;
	n = rand() % MAX_NUM;
	return n;
}

/* 
Here is producer()   It produces a random number and puts it in the buffer if the buffer is not full 
*/

void* producer(void* parameters)
{
	int item, itm, im, lc;

	lc = LOOP_COUNT;
	while (lc)
	{
		item = produceItem();
		while (itemCount == BUFFER_SIZE);  	/* Spin if buffer full */
        im = itemCount;
		itm = buffer[itemCount];
		buffer[itemCount] = item;		    /* Put random number in buffer */
        if (itm != -1)
		{
		 printf("Error writing to buffer.  It wasn't empty. \n");
		}
		printf("Thread %d put a random number %d in the buffer at location %d.\n",(int) pthread_self(), item, im);
		itemCount++;						/* Bump the count */
		lc--;
 	}
}

/* 
Here is the consumer()   It removes and displays  a random number from the buffer if the buffer is not empty 
*/

void* consumer(void* parameters)
{
	int item, im, lc;

	lc = LOOP_COUNT;
	while (lc)
	{	
		while (itemCount == 0);  			/* Spin if buffer empty */
		itemCount--; 						/* Decrement the count */
		im = itemCount;
		item = buffer[itemCount];			/* Take random number from buffer */
		buffer[itemCount] = -1;				/* Null out buffer item after taking it */
		if (item == -1)
		{
		 printf("Error reading item from buffer.  It was empty\n");
		}
		printf("Thread %d took a random number %d from the buffer at location %d.\n",(int) pthread_self(), item, im);
		lc--;
 	}	
}



int main ()
{

  pthread_t thread1;
  pthread_t thread2;

  int i;
  
  char *message1 = "Thread 1";
  char *message2 = "Thread 2";
 
  /* fill the buffer with with non valid numbers */
  for (i=0; i< BUFFER_SIZE; i++)
  {
    buffer[i] = -1;
  }
 
  /* Initialize the item count */
  itemCount = 0;
  

  /* Start the computing thread, up to the 5000th prime number.  */

  pthread_create (&thread1, NULL, producer, (void*) message1);
  pthread_create (&thread2, NULL, consumer, (void*) message2);

  /* Wait for the prime number threads to complete, and get the result.  */
  pthread_join (thread1, NULL);
  printf("Producer Thread1 joined.\n");
  pthread_join (thread2, NULL);
  printf("Consumer Thread2 joined.\n");
  printf("Main thread id is %d\n",(int) getpid());
  return 0;
}
