# This Makefile was originally derived largely from a tutorial on Makefiles found at https://cs.colby.edu/maxwell/courses/tutorials/maketutor/ on 5 March 2023
# Undoubtedly it can still be much improved, but it got me started.  Thanks to whoever wrote it!

CC=gcc
CFLAGS=
DEPS = chunk.h memory.h debug.h value.h vm.h

ODIR=obj

_OBJ = chunk.o memory.o debug.o value.o vm.o main.o
OBJ = $(patsubst %,$(ODIR)/%,$(_OBJ))

.PHONY: clean

$(ODIR)/%.o: %.c $(DEPS)
	$(CC) -c -o $@ $< $(CFLAGS)

clox: $(OBJ)
	$(CC) -o $@ $^ -Wall -Werror -std=c17

clean:
	rm -f $(ODIR)/*.o