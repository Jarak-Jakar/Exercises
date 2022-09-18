#include <stddef.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdbool.h>

typedef struct Node
{
    struct Node *Next;
    struct Node *Prev;
    char *String;
} Node;

typedef struct DoublyLinkedList
{
    struct Node *Front;
    struct Node *Back;
} DoublyLinkedList;

DoublyLinkedList *New(char *initialStr)
{
    Node *frontNode = calloc(1, sizeof(Node));
    frontNode->Next = NULL;
    frontNode->Prev = NULL;
    frontNode->String = initialStr;

    DoublyLinkedList *ptr = calloc(1, sizeof(DoublyLinkedList));
    ptr->Front = frontNode;
    ptr->Back = frontNode;

    return ptr;
}

void Append(DoublyLinkedList *list, char *string)
{
    Node *backNode = list->Back;
    Node *newNode = calloc(1, (sizeof(Node)));
    newNode->Next = NULL;
    newNode->Prev = backNode;
    newNode->String = string;
    backNode->Next = newNode;
    list->Back = newNode;
}

void Prepend(DoublyLinkedList *list, char *string)
{
    Node *frontNode = list->Front;
    Node *newNode = calloc(1, sizeof(Node));
    newNode->Next = frontNode;
    newNode->Prev = NULL;
    newNode->String = string;
    frontNode->Prev = newNode;
    list->Front = newNode;
}

void PrintStringsForward(DoublyLinkedList *list)
{
    printf("\n"); // A further goal would be to avoid printing this if the list is empty...
    Node *node = list->Front;
    while (node != NULL)
    {
        printf("%s ", node->String);
        node = node->Next;
    }
    printf("\n");
}

void PrintStringsBackward(DoublyLinkedList *list)
{
    printf("\n");
    Node *node = list->Back;
    while (node != NULL)
    {
        printf("%s ", node->String);
        node = node->Prev;
    }
    printf("\n");
}

// Returns true if the node  string was inserted, or false if it couldn't be for whatever reason.
// This assumes zero-based indexing
bool InsertAt(DoublyLinkedList *list, char *string, int index)
{

    if (index == 0)
    {
        Prepend(list, string);
        return true;
    }

    Node *node = list->Front;
    int idx = 1;
    bool success = false;

    while (node != NULL)
    {
        if (idx < index)
        {
            node = node->Next;
            idx++;
        }
        else
        {
            Node *newNode = calloc(1, sizeof(Node));
            newNode->Next = node->Next;
            newNode->Prev = node;
            newNode->String = string;
            node->Next = newNode;
            success = true;
            break;
        }
    }

    return success;
}

int main()
{
    DoublyLinkedList *dll = New("Hello");
    PrintStringsForward(dll);
    Append(dll, "there");
    PrintStringsForward(dll);
    Append(dll, "children!");
    PrintStringsForward(dll);
    PrintStringsBackward(dll);

    InsertAt(dll, "rambunctious", 1);
    PrintStringsForward(dll);
}