########################################################################
# Code listing from "Advanced Linux Programming," by CodeSourcery LLC  #
# Copyright (C) 2001 by New Riders Publishing                          #
# See COPYRIGHT for license information.                               #
# Note that this makefile depends upon many "default" rules of "make"  #
########################################################################


OBJECTS		= 
LIBRARIES	=
# List all the programs to be built
PROGRAMS	= solution_1 solution_2 solution_3

# Link programs with the pthread library.
LDLIBS		+= -lpthread

# As the first target in the makefile, both the "all" and the "clean" targets
# will be run.  This is a nifty way to run 2 targets within the same makefile
.PHONY:         all clean

# Default target: build everything.
all:            $(OBJECTS) $(LIBRARIES) $(PROGRAMS)

# Clean up build products.
clean:
	rm -f *.o *.a $(PROGRAMS)
