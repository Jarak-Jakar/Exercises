#include <stdlib.h>

#include "chunk.h"
#include "memory.h"

void initChunk(Chunk *chunk)
{
    chunk->count = 0;
    chunk->capacity = 0;
    chunk->code = NULL;
    chunk->lineCount = 0;
    chunk->lineCapacity = 0;
    chunk->lines = NULL;
    initValueArray(&chunk->constants);
}

void freeChunk(Chunk *chunk)
{
    FREE_ARRAY(uint8_t, chunk->code, chunk->capacity);
    FREE_ARRAY(LineStart, chunk->lines, chunk->lineCapacity);
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
    }

    chunk->code[chunk->count] = byte;
    chunk->count++;

    printf("Luke\n");

    // Line counting
    // If we're still on the same line, we don't need to do anything
    if (chunk->lineCount > 0 && line == chunk->lines[chunk->lineCount - 1].line)
    {
        return;
    }

    printf("Leia\n");

    // If we're not still on the same line, we're about to add a new LineStart, so we need to ensure the relevant array has sufficient capacity
    if (chunk->lineCapacity < chunk->lineCount + 1)
    {
        printf("Jabba\n");
        int oldCapacity = chunk->lineCapacity;
        chunk->lineCapacity = GROW_CAPACITY(oldCapacity);
        chunk->lines = GROW_ARRAY(LineStart, chunk->lines, oldCapacity, chunk->lineCapacity);
    }

    printf("Han\n");

    LineStart *newLS = &chunk->lines[chunk->lineCount++];
    newLS->line = line;
    // The chunk count, less one, corresponds to the starting offset, I THINK
    newLS->offset = chunk->count - 1;

    printf("Obi-wan\n");
}

int addConstant(Chunk *chunk, Value value)
{
    writeValueArray(&chunk->constants, value);
    return chunk->constants.count - 1;
}

// This function implicitly assumes that you'll never pass it an offset that wouldn't already have been stored...
int getLine(Chunk *chunk, int instruction)
{
    LineStart *LineStart = chunk->lines;
    int startOff = 0;

    // printf("Yoda\n");

    // Seek the first line with a start offset greater than or equal to the one we're looking for
    while (instruction < chunk->lines[startOff].offset)
    {
        startOff++;
    }

    // printf("Palpatine\n");

    return chunk->lines[startOff].line;
}
