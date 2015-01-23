#include <sys/types.h>
#include <sys/param.h>
#include <sys/resource.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <sys/wait.h>
#include <unistd.h>
#include <stdio.h>
#include <signal.h>
#include <syslog.h>
#include "server.h"

void signal_handler(int sig)
{
	switch(sig)
	{
		case SIGUSR1:
			syslog(LOG_INFO, "Hello! I am %i signal", sig);
			break;
	}
}
void MyDemon()
{
	int fd;
	struct rlimit flim;

	if(getppid() != 1)
	{
		signal(SIGTTOU, SIG_IGN);
		signal(SIGTTIN, SIG_IGN);
		signal(SIGTSTP, SIG_IGN);
		signal(SIGUSR1, signal_handler);
		chdir("/home/server");
		
		pid_t pid;
		pid = fork();
	
		if(pid!=0)
		{
			printf("%d - Demon's pid", pid);
			exit(pid);
		}
		else
		{
			setsid();
			Server();
		}
	}

	getrlimit(RLIMIT_NOFILE, &flim);
	
	for(fd = 0; fd < flim.rlim_max; fd++)
	{
		close(fd);
	}
}

