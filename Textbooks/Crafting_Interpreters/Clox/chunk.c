#include <stdlib.h>

#include "chunk.h"
#include "memory.h"

void initChunk(Chunk *chunk)
{
    chunk->count = 0;
    chunk->capacity = 0;
    chunk->code = NULL;
    chunk->lines = NULL;
    chunk->lineStartCount = 0;
    chunk->lineStartCapacity = 0;
    initValueArray(&chunk->constants);
}

void freeChunk(Chunk *chunk)
{
    FREE_ARRAY(uint8_t, chunk->code, chunk->capacity);
    FREE_ARRAY(LineStarts, chunk->lines, chunk->lineStartCapacity);
    freeValueArray(&chunk->constants);
    initChunk(chunk);
}

void writeChunk(Chunk *chunk, uint8_t byte, int line)
{
    if (chunk->capacity < chunk->count + 1)
    {
        int oldCapacity = chunk->capacity;
        chunk->capacity = GROW_CAPACITY(oldCapacity);
        chunk->code = GROW_ARRAY(uint8_t, chunk->code, oldCapacity, chunk->capacity);
        // chunk->lines = GROW_ARRAY(int, chunk->lines, oldCapacity, chunk->capacity);
    }

    chunk->code[chunk->count] = byte;
    // chunk->lines[chunk->count] = line;
    chunk->count++;

    printf("Luke\n");

    // If we're not still on the same line, we're about to add a new LineStart, so we need to ensure the relevant array has sufficient capacity
    if (chunk->lineStartCapacity < chunk->lineStartCount + 1)
    {
        printf("Jabba\n");
        int oldCapacity = chunk->lineStartCapacity;
        chunk->lineStartCapacity = GROW_CAPACITY(oldCapacity);
        chunk->lines = GROW_ARRAY(LineStarts, chunk->lines, oldCapacity, chunk->lineStartCapacity);
    }

    printf("Leia\n");

    // Line counting
    // If we're still on the same line, we don't need to do anything
    if (line == chunk->lines[chunk->lineStartCount - 1].lineNumber)
    {
        return;
    }

    printf("Han\n");

    LineStarts *newLS = &chunk->lines[chunk->lineStartCount++];
    newLS->lineNumber = line;
    // The chunk count corresponds to the starting offset, I THINK
    newLS->startingOffset = chunk->count;

    printf("Obi-wan\n");
}

int addConstant(Chunk *chunk, Value value)
{
    writeValueArray(&chunk->constants, value);
    return chunk->constants.count - 1;
}

// This function implicitly assumes that you'll never pass it an offset that wouldn't already have been stored...
int getLine(Chunk *chunk, int offset)
{
    LineStarts *LineStarts = chunk->lines;
    int startOff = 0;

    // printf("Yoda\n");

    // Seek the first line with a start offset greater than or equal to the one we're looking for
    while (startOff < chunk->capacity && offset < chunk->lines[startOff].startingOffset)
    {
        startOff++;
    }

    // printf("Palpatine\n");

    return chunk->lines[startOff].lineNumber;
}
