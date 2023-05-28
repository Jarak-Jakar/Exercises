#include "common.h"
#include "chunk.h"
#include "debug.h"

int main(int argc, char const *argv[])
{
    Chunk chunk;
    initChunk(&chunk);

    int constant = addConstant(&chunk, 1.2);
    writeChunk(&chunk, OP_CONSTANT, 123);
    writeChunk(&chunk, constant, 123);

    writeChunk(&chunk, OP_RETURN, 123);

    int constant2 = addConstant(&chunk, 2.4);
    writeChunk(&chunk, OP_CONSTANT, 124);
    writeChunk(&chunk, constant, 124);

    writeChunk(&chunk, OP_RETURN, 124);

    disassembleChunk(&chunk, "test chunk");
    freeChunk(&chunk);

    return 0;
}