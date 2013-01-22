Date:		10/28/2011

From:		James Brooks

Subject:	Lab 2 Write Up
_______________________________________________________________________
Introduction

This is a writeup of the results from Lab2 - Producer Consumer Problem.
Three programmed solutions were written in C to exercise the Consumer
Producer problem of sharing a common buffer, with the Producer putting
random numbers in while the Consumer took then out.

Results

Solution 1 provided no protection for the buffer while the Producer and 
Consumer threads worked away at their jobs.  This resulted in many read
and write errors.  Here is a snippet from the Solution 1 output log.

Error reading item from buffer.  It was empty
Thread -1218085968 took a random number -1 from the buffer at location 8.
Thread -1218085968 took a random number 62 from the buffer at location 9.
Thread -1209693264 put a random number 23 in the buffer at location 9.
Error writing to buffer.  It wasn't empty. 
Thread -1209693264 put a random number 67 in the buffer at location 9.
Error reading item from buffer.  It was empty
Thread -1218085968 took a random number -1 from the buffer at location 8.

Solution 2 provided counting semaphore protection for the Buffer.  In this
case there were no errors.  Here is a snippet from the Solution 2 output
log

Thread -1218237520 took a random number 77 from the buffer at location 2.
Thread -1218237520 took a random number 86 from the buffer at location 1.
Thread -1218237520 took a random number 83 from the buffer at location 0.
Thread -1209844816 put a random number 62 in the buffer at location 0.
Thread -1209844816 put a random number 27 in the buffer at location 1.
Thread -1209844816 put a random number 90 in the buffer at location 2.
Thread -1209844816 put a random number 59 in the buffer at location 3.
Thread -1209844816 put a random number 63 in the buffer at location 4.

Solution 3 provided counting semaphore and mutex protection for the Buffer.  
Multiple producers and consumers were initiated each on their own thread.
Again there were no errors.  Here is a snippet from the Solution 3 output
log.

Thread -1210008656 put a random number 71 in the buffer at location 9.
Thread -1218401360 took a random number 20 from the buffer at location 0.
Thread -1235186768 took a random number 63 from the buffer at location 9.
Thread -1218401360 took a random number 71 from the buffer at location 9.
Thread -1235186768 took a random number 52 from the buffer at location 8.
Thread -1218401360 took a random number 15 from the buffer at location 7.

Conclusion

Protection is required for shared resources in a multithreaded program.
Semaphores can protect between two threads sharing the same resource 
while added mutex protection is required for multiple instantiations
of the same processes all sharing a common resource.

James Brooks

