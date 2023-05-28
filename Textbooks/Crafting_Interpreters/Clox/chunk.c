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

    // Line counting
    // If we're still on the same line, we don't need to do anything
    if (chunk->lineCount > 0 && line == chunk->lines[chunk->lineCount - 1].line)
    {
        return;
    }

    // If we're not still on the same line, we're about to add a new LineStart, so we need to ensure the relevant array has sufficient capacity
    if (chunk->lineCapacity < chunk->lineCount + 1)
    {
        int oldCapacity = chunk->lineCapacity;
        chunk->lineCapacity = GROW_CAPACITY(oldCapacity);
        chunk->lines = GROW_ARRAY(LineStart, chunk->lines, oldCapacity, chunk->lineCapacity);
    }

    LineStart *newLS = &chunk->lines[chunk->lineCount++];
    newLS->line = line;
    // The chunk count, less one, corresponds to the starting offset, I THINK
    newLS->offset = chunk->count - 1;
}

int addConstant(Chunk *chunk, Value value)
{
    writeValueArray(&chunk->constants, value);
    return chunk->constants.count - 1;
}

void writeConstant(Chunk *chunk, Value value, int line)
{
    int index = addConstant(chunk, value);

    if (index < 256)
    {
        writeChunk(chunk, OP_CONSTANT, line);
        writeChunk(chunk, (uint8_t)index, line);
    }
    else
    {
        writeChunk(chunk, OP_CONSTANT_LONG, line);
        writeChunk(chunk, (uint8_t)(index & 0xff), line);
        writeChunk(chunk, (uint8_t)((index >> 8) & 0xff), line);
        writeChunk(chunk, (uint8_t)((index >> 16) & 0xff), line);
    }
}

// This function implicitly assumes that you'll never pass it an offset that wouldn't already have been stored...
int getLine(Chunk *chunk, int instruction)
{
    int start = 0;
    int end = chunk->lineCount - 1;

    for (;;)
    {
        int mid = (start + end) / 2;
        LineStart *line = &chunk->lines[mid];
        if (instruction < line->offset)
        {
            end = mid - 1;
        }
        else if (mid == chunk->lineCount - 1 || instruction < chunk->lines[mid + 1].offset)
        {
            return line->line;
        }
        else
        {
            start = mid + 1;
        }
    }
}
