#include <sys/socket.h>
#include <netinet/in.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <stdlib.h>
#include <stdio.h>
#include <errno.h>
#include <pthread.h>

#include <string.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <netinet/in.h>

#include <sys/stat.h>
#include <fcntl.h>
#include <sys/sendfile.h>

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
	ssize_t len;
	
	char buffer[BUFSIZ];
	char *str = "/var/www/html/photos/";
	//char *str = "/home/Ilya/";

        int file_size;
        FILE *received_file;
        int remain_data = 0;
	struct stat file_stat;

   	listenfd=socket(AF_INET,SOCK_STREAM,0);

   	bzero(&servaddr,sizeof(servaddr));
   	servaddr.sin_family = AF_INET;
   	servaddr.sin_addr.s_addr=htonl(INADDR_ANY);
  	servaddr.sin_port=htons(5000);
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
			printf("\nNew connection! Pid = %d Online = %d\n", getpid(), counter);
			char FILENAME[BUFSIZ];

			/* Receiving file size */
        		recv(connfd, buffer, 1024, 0);
        		file_size = atoi(buffer);
        		fprintf(stdout, "File size : %d\n", file_size);

			char *request = "OK";
			send(connfd,request,strlen(request),0);

			/* Receiving file name */
        		recv(connfd, buffer, BUFSIZ, 0);
			send(connfd,request,strlen(request),0);
			strcpy(FILENAME,str);
			strcat(FILENAME,buffer);
			
			char str1[4] = ".jpg";
			char *istr;
			istr = strstr(FILENAME,str1);

			if(istr == NULL)
			{
				printf("NULL\n");
				exit(1);				
			}
			else
			{
				printf("NOT NULL\n");
				//FILENAME[(int)(istr-FILENAME+1)] = NULL;
			}
			
			int i=0;
			while(FILENAME[i]!=NULL)
			{
				if(FILENAME[i]=='.' && FILENAME[i+1]=='j' && FILENAME[i+2]=='p' && FILENAME[i+3]=='g')
				{
					FILENAME[i+4] = NULL;
					printf("HERE\n");
					break;
				}
				i++;
			}

			printf("filename = %s\n",FILENAME);

        		received_file = fopen(FILENAME, "w");		
			remain_data = file_size;

        		while (((len = recv(connfd, buffer, 1024, 0)) > 0) && (remain_data > len))
       			{
				remain_data -= len;
				fwrite(buffer, sizeof(char), len, received_file);
				fprintf(stdout, "Receive %d bytes and we hope :- %d bytes\n", len, remain_data);
        		}
			fwrite(buffer, sizeof(char), remain_data, received_file);
			fprintf(stdout, "!Receive %d bytes and we hope :- %d bytes\n", remain_data, 0);
				
        		fclose(received_file);

			close(connfd);
			exit(1);
         
      		}
      		close(connfd);
   	}
}
