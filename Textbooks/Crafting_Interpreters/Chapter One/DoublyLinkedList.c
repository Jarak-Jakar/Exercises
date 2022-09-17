#include <stddef.h>
#include <stdlib.h>
#include <stdio.h>

typedef struct DoublyLinkedList
{
    struct Node *Front;
    struct Node *Back;
} DoublyLinkedList;

typedef struct Node
{
    struct Node *Next;
    struct Node *Prev;
    char *String;
} Node;

DoublyLinkedList *NewList(char *initialStr)
{
    Node frontNode = {.String = initialStr};
    DoublyLinkedList *ptr = calloc(1, sizeof(DoublyLinkedList));
    DoublyLinkedList dll = {.Front = &frontNode, .Back = &frontNode};
    *ptr = dll;
    return ptr;
}

void AppendList(DoublyLinkedList *list, char *string)
{
    Node *backNode = list->Back;
    Node newNode = {.Next = NULL, .Prev = backNode, .String = string};
    backNode->Next = &newNode;
}

int main()
{
    return printf("\nHello World!\n\n");
}