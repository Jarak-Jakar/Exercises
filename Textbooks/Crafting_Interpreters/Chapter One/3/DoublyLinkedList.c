/*

This implementation differs from Bob Nystrom's in at least one important way:  He only uses a Node type, whereas  I introduce both Node and DoublyLinkedList types.  In theory, at least, consumers of  the list only ever work with the list directly, and never  have to consider the Node type itself.

*/

#include <stddef.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdbool.h>

typedef struct Node
{
    struct Node *Next;
    struct Node *Prev;
    char *Value;
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
    frontNode->Value = initialStr;

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
    newNode->Value = string;
    backNode->Next = newNode;
    list->Back = newNode;
}

void Prepend(DoublyLinkedList *list, char *string)
{
    Node *frontNode = list->Front;
    Node *newNode = calloc(1, sizeof(Node));
    newNode->Next = frontNode;
    newNode->Prev = NULL;
    newNode->Value = string;
    frontNode->Prev = newNode;
    list->Front = newNode;
}

void PrintStringsForward(DoublyLinkedList *list)
{
    printf("\n"); // A further goal would be to avoid printing this if the list is empty...
    Node *node = list->Front;
    while (node != NULL)
    {
        printf("%s ", node->Value);
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
        printf("%s ", node->Value);
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
    int idx = 0;

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
            newNode->Next = node;
            newNode->Prev = node->Prev;
            newNode->Value = string;
            node->Prev = newNode;
            newNode->Prev->Next = newNode;
            return true;
        }
    }

    // This is final node
    if (idx == index)
    {
        Append(list, string);
    }

    // Otherwise, insertion failed
    return false;
}

// Returns true or false depending on whether the given element was found.  Does NOT change passed in string/pointer if the element is not found.
bool GetAt(DoublyLinkedList *list, int index, char **ptr)
{
    int idx = 0;
    Node *node = list->Front;

    while (idx < index && node != NULL)
    {
        idx++;
        node = node->Next;
    }

    if (node != NULL)
    {
        *ptr = node->Value;
        return true;
    }

    return false;
}

void DeleteList(DoublyLinkedList *list) {
    Node *node = list->Front;
    Node *nextNode = list->Front;

    while (node != NULL) {
        nextNode = node.Next;
        free(node);
        node = nextNode;
    }

    free(list);
}

bool DeleteAt(DoublyLinkedList *list, int index) {
    int idx = 0;
    Node  *node = list->Front;
    while (idx < index && node != NULL) {
        idx++;
        node = node->Next;
    }

    if (node == NULL) {
        return false;
    }

    Node *next = node->Next;
    Node *prev = node->Prev;

    prev->Next = next;
    next->Prev = prev;

    free(node);

    return true;
}

int main()
{
    DoublyLinkedList *dll = New("there");
    PrintStringsForward(dll);
    Append(dll, "children!");
    PrintStringsForward(dll);
    Prepend(dll, "Hello");
    PrintStringsForward(dll);
    PrintStringsBackward(dll);

    InsertAt(dll, "rambunctious", 2);
    PrintStringsForward(dll);

    printf("%s\n", dll->Front->Value);
    printf("%s\n", dll->Back->Value);

    PrintStringsBackward(dll);
    PrintStringsForward(dll);

    char *str = NULL;
    GetAt(dll, 2, &str);
    printf("%s\n", str);
}