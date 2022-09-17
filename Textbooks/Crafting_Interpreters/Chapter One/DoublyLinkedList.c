#include "stddef.h"

typedef struct DoublyLinkedList
{
    struct Node *Front;
    struct Node *Back;
} DoublyLinkedList;

typedef struct Node
{
    struct Node *Next;
    struct Node *Prev;
    int Value;
} Node;

DoublyLinkedList *NewList(int initialValue)
{
    Node frontNode = {.Value = initialValue};
    DoublyLinkedList dll = {.Front = &frontNode, .Back = &frontNode};
    return &dll;
}

void AppendList(DoublyLinkedList *list, int value)
{
    Node *backNode = list->Back;
    Node newNode = {.Next = NULL, .Prev = backNode, .Value = value};
    backNode->Next = &newNode;
}