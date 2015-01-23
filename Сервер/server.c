#include <sys/socket.h>
#include <netinet/in.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <stdlib.h>
#include <stdio.h>
#include <errno.h>
#include <pthread.h>
#include "dir.h"

void * threadwait_func(void *arg)
{
	pid_t     killpid;
	for (;;) 
	{
		killpid = waitpid(-1, NULL, WNOHANG);
		if(killpid > 0)
		{
			(* (int *) arg)--;
			printf("Disconnection! Pid = %d, Online = %d\n", killpid, (* (int *) arg));
		}
		sleep(1);
	}
}

int Server()
{
	int counter = 0;
	pthread_t threadwait;
	int listenfd,connfd,n, result;
   	struct sockaddr_in servaddr,cliaddr;
   	socklen_t clilen;
   	pid_t     childpid;
   	char mesg[1000];
	char request[1000];

   	listenfd=socket(AF_INET,SOCK_STREAM,0);

   	bzero(&servaddr,sizeof(servaddr));
   	servaddr.sin_family = AF_INET;
   	servaddr.sin_addr.s_addr=htonl(INADDR_ANY);
  	servaddr.sin_port=htons(32000);
   	bind(listenfd,(struct sockaddr *)&servaddr,sizeof(servaddr));

   	listen(listenfd,1024);

	result = pthread_create(&threadwait, NULL, threadwait_func, &counter);
	if (result != 0)
   	{
      		printf("Error at creating the threadwait\n");
      		exit(1);
   	}

   	for(;;)
   	{
      		clilen=sizeof(cliaddr);
      		connfd = accept(listenfd,(struct sockaddr *)&cliaddr,&clilen);
		counter++;

      		if ((childpid = fork()) == 0)
      		{
         		close (listenfd);
			printf("New connection! Pid = %d Online = %d\n", getpid(), counter);

         		for(;;)
         		{
            			n = recv(connfd,mesg,50,0);
	    			if(n<=0)
            			{
               				exit(1);
            			}
				DO(mesg,request);
            			send(connfd,request,strlen(request),0);
            			mesg[n] = 0;
            			printf("(%d): ", getpid());
            			printf("%s",mesg);
         		}
         
      		}
      		close(connfd);
   	}
}
