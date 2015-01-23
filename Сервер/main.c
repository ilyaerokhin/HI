#include <stdio.h>
#include <unistd.h>
#include "demon.h"

int main(int argv, char *argc[])
{
	MyDemon();
	while(1)
		sleep(1);
}
