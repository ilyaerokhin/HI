#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <sys/stat.h>
#include <sys/types.h>

#define MAXSIZEPASSWORD 20
#define MAXSIZEDOUBLE 20
#define MAXSIZENAME 20
#define MAXSIZESTATUS 256
#define MAXSIZEREQUEST 900
#define ADD_SIZE 20

// СПЕЦИАЛЬНЫЕ АЛГОРИТМЫ //////////////////////////////////////////////////

int DeleteFriend(char *path, char *user, char *password, char *userfriend)
{
	char mydir[strlen(path)+strlen(user)+ADD_SIZE];
	char frienddir[strlen(path)+strlen(userfriend)+ADD_SIZE];
	char mydir_1[strlen(path)+strlen(user)+ADD_SIZE];
	char frienddir_1[strlen(path)+strlen(userfriend)+ADD_SIZE];
	char mypassword[MAXSIZEPASSWORD];

	strcpy(mydir,path);
	strcat(mydir,"/");
	strcat(mydir,user);
	int a = access(mydir, 0);

	strcpy(mydir_1,path);
	strcat(mydir_1,"/");
	strcat(mydir_1,user);

	if(a!=0)
	{
		return 1;
	}

	strcpy(frienddir,path);
	strcat(frienddir,"/");
	strcat(frienddir,userfriend);
	a = access(frienddir, 0);

	strcpy(frienddir_1,path);
	strcat(frienddir_1,"/");
	strcat(frienddir_1,userfriend);

	if(a!=0)
	{
		return 2;
	}

	Read(path, user, mypassword, 3);
	int c = strcmp(mypassword,password);
	if(c!=0)
	{
		return 3;
	}

	char text[MAXSIZEREQUEST];
	strcat(mydir,"/friends.txt");

	FILE *f = fopen(mydir,"r");
	char *estr;
	char newtext[MAXSIZENAME];
	strcpy(text,"");

	while(1)
	{
		estr = fgets(newtext,sizeof(newtext),f);
		newtext[strlen(newtext)-1] = '\0';

		if(!estr)
		{
			fclose(f);
			break;
		}
		int c = strcmp(newtext,userfriend);
		if(c!=0)
		{
			strcat(text,newtext);
			strcat(text,"\n");
		}
	}
	Write(path, user, text, 5);

	char text2[MAXSIZEREQUEST];
	strcat(frienddir,"/friends.txt");

	FILE *f2 = fopen(frienddir,"r");

	strcpy(text2,"");

	while(1)
	{
		estr = fgets(newtext,sizeof(newtext),f2);
		newtext[strlen(newtext)-1] = '\0';

		if(!estr)
		{
			fclose(f2);
			break;
		}
		int c = strcmp(newtext,user);
		if(c!=0)
		{
			strcat(text2,newtext);
			strcat(text2,"\n");
		}
	}
	Write(path, userfriend, text2, 5);

	char text3[MAXSIZEREQUEST];
	strcat(mydir_1,"/potential.txt");

	FILE *f3 = fopen(mydir_1,"r");

	strcpy(text3,"");

	while(1)
	{
		estr = fgets(newtext,sizeof(newtext),f3);
		newtext[strlen(newtext)-1] = '\0';

		if(!estr)
		{
			fclose(f3);
			break;
		}
		int c = strcmp(newtext,userfriend);
		if(c!=0)
		{
			strcat(text3,newtext);
			strcat(text3,"\n");
		}
	}
	Write(path, user, text3, 7);

	char text4[MAXSIZEREQUEST];
	strcat(frienddir_1,"/potential.txt");

	FILE *f4 = fopen(frienddir_1,"r");

	strcpy(text4,"");

	while(1)
	{
		estr = fgets(newtext,sizeof(newtext),f4);
		newtext[strlen(newtext)-1] = '\0';

		if(!estr)
		{
			fclose(f4);
			break;
		}
		int c = strcmp(newtext,userfriend);
		if(c!=0)
		{
			strcat(text4,newtext);
			strcat(text4,"\n");
		}
	}
	Write(path, user, text4, 7);
	return 0;
}

void Write(char *path, char *user, char *text, int flag)
{
	char dir[strlen(path)+strlen(user)+ADD_SIZE];
	strcpy(dir,path);
	strcat(dir,"/");
	strcat(dir,user);

	switch(flag)
	{
		case 1:
			strcat(dir,"/latitude.txt");
			break;
		case 2:
			strcat(dir,"/longitude.txt");
			break;
		case 3:
			strcat(dir,"/password.txt");
			break;
		case 4: 
			strcat(dir,"/email.txt");
			break;
		case 5: 
			strcat(dir,"/friends.txt");
			break;
		case 6: 
			strcat(dir,"/status.txt");
			break;
		case 7: 
			strcat(dir,"/potential.txt");
			break;
		case 8: 
			strcat(dir,"/people.txt");
			break;
	}	

	FILE *file = fopen(dir,"w");
	fprintf(file,"%s",text);
	fclose(file);
}

int ListPotential(char *path, char *user, char *password, char *friends)
{
	char dir[strlen(path)+strlen(user)+ADD_SIZE];
	char mypassword[MAXSIZEPASSWORD];
	
	strcpy(dir,path);
	strcat(dir,"/");
	strcat(dir,user);
	int a = access(dir, 0);

	if(a!=0)
	{
		return 1;
	}

	Read(path, user, mypassword, 3);
	int c = strcmp(mypassword,password);
	if(c!=0)
	{
		return 2;
	}
	
	strcat(dir,"/potential.txt");

	FILE *f = fopen(dir,"r");
	char *estr;
	char newtext[MAXSIZENAME];
	strcpy(friends,"");

	estr = fgets(newtext,sizeof(newtext),f);
	newtext[strlen(newtext)-1] = '\0';
	if(!estr)
	{
		return 0;
	}

	strcat(friends,newtext);

	while(1)
	{
		estr = fgets(newtext,sizeof(newtext),f);
		newtext[strlen(newtext)-1] = '\0';
		if(!estr)
		{
			fclose(f);
			return 0;
		}
		strcat(friends,"|");
		strcat(friends,newtext);
	}
	fclose(f);
}

int ListFriends(char *path, char *user, char *password, char *friends)
{
	char dir[strlen(path)+strlen(user)+ADD_SIZE];
	char mypassword[MAXSIZEPASSWORD];
	
	strcpy(dir,path);
	strcat(dir,"/");
	strcat(dir,user);
	int a = access(dir, 0);

	if(a!=0)
	{
		return 1;
	}

	Read(path, user, mypassword, 3);
	int c = strcmp(mypassword,password);
	if(c!=0)
	{
		return 2;
	}
	
	strcat(dir,"/friends.txt");

	FILE *f = fopen(dir,"r");
	char *estr;
	char newtext[MAXSIZENAME];
	strcpy(friends,"");

	estr = fgets(newtext,sizeof(newtext),f);
	newtext[strlen(newtext)-1] = '\0';
	if(!estr)
	{
		return 0;
	}

	strcat(friends,newtext);

	while(1)
	{
		estr = fgets(newtext,sizeof(newtext),f);
		newtext[strlen(newtext)-1] = '\0';
		if(!estr)
		{
			fclose(f);
			return 0;
		}
		strcat(friends,"|");
		strcat(friends,newtext);
	}
	fclose(f);
}

int AddFriend(char *path, char *user, char *password, char *userfriend)
{
	char mydir[strlen(path)+strlen(user)+ADD_SIZE];
	char mydir_1[strlen(path)+strlen(user)+ADD_SIZE];
	char frienddir[strlen(path)+strlen(userfriend)+ADD_SIZE];
	char mypassword[MAXSIZEPASSWORD];

	strcpy(mydir,path);
	strcat(mydir,"/");
	strcat(mydir,user);
	int a = access(mydir, 0);

	strcpy(mydir_1,path);
	strcat(mydir_1,"/");
	strcat(mydir_1,user);

	if(a!=0)
	{
		return 1;
	}

	strcpy(frienddir,path);
	strcat(frienddir,"/");
	strcat(frienddir,userfriend);
	a = access(frienddir, 0);

	if(a!=0)
	{
		return 2;
	}

	Read(path, user, mypassword, 3);
	int c = strcmp(mypassword,password);
	if(c!=0)
	{
		return 3;
	}

	int l = isHere(path, user, "friends.txt", userfriend);
	if(l==0)
	{
		return 4;
	}

	l = isHere(path, userfriend, "potential.txt", user);
	if(l==0)
	{
		return 5;
	}
	
	l = isHere(path, user, "potential.txt", userfriend);
	if(l==0)
	{
		strcat(mydir,"/friends.txt");
		strcat(frienddir,"/friends.txt");

		FILE *file1 = fopen(mydir,"a");
		fprintf(file1,"%s\n",userfriend);
		fclose(file1);

		FILE *file = fopen(frienddir,"a");
		fprintf(file,"%s\n",user);
		fclose(file);
	
		char text[MAXSIZEREQUEST];
		strcat(mydir_1,"/potential.txt");

		FILE *f = fopen(mydir_1,"r");
		char *estr;
		char newtext[MAXSIZENAME];
		strcpy(text,"");

		while(1)
		{
			estr = fgets(newtext,sizeof(newtext),f);
			newtext[strlen(newtext)-1] = '\0';

			if(!estr)
			{
				fclose(f);
				break;
			}
			int c = strcmp(newtext,userfriend);
			if(c!=0)
			{
				strcat(text,newtext);
				strcat(text,"\n");
			}
		}
		Write(path, user, text, 7);

		return 0;
	}
	else
	{
		strcat(frienddir,"/potential.txt");

		FILE *file = fopen(frienddir,"a");
		fprintf(file,"%s\n",user);
		fclose(file);

		return 0;
	}
}

int AddUser(char *path, char *user, char *password, char *email)
{
	char dir[strlen(path)+strlen(user)+ADD_SIZE];
	strcpy(dir,path);
	strcat(dir,"/");
	strcat(dir,user);
	
	char cpphoto[100];
	strcpy(cpphoto,"cp /home/no_photo.jpg /var/www/html/photos/");
	strcat(cpphoto,user);
	strcat(cpphoto,".jpg");

	system(cpphoto);

	int a = access(dir, 0); 
	if(a!=0)
	{
		mkdir(dir,0777);
		char *text = "";
		Write(path, user, text, 1);
		Write(path, user, text, 2);
		Write(path, user, password, 3);
		Write(path, user, email, 4);
		Write(path, user, text, 5);
		char *status = "@";
		Write(path, user, status, 6);
		Write(path, user, text, 7);
		Write(path, user, text, 8);
		return 0; 
	}
	else
	{
		return 1;
	}
}

int CheckUser(char *path, char *user, char *password)
{
	char dir[strlen(path)+strlen(user)+ADD_SIZE];
	strcpy(dir,path);
	strcat(dir,"/");
	strcat(dir,user);

	int a = access(dir, 0); 
	if(a!=0)
	{
		return 1; 
	}

	char pasfile[strlen(dir)+ADD_SIZE];
	strcpy(pasfile,dir);
	strcat(pasfile,"/password.txt");

	char pass[MAXSIZEPASSWORD];
	FILE *file = fopen(pasfile,"r");
	fgets(pass, MAXSIZEPASSWORD,file);

	int i;
	for(i=0; i<MAXSIZEPASSWORD; i++)
	{
		if(pass[i] == '\n')
		{
			pass[i] = NULL;
			break;
		}
	}

	if(!strcmp(password,pass))
	{
		fclose(file);
		return 0;
	}
	
	fclose(file);
	return 2; // uncorrect password
}

void dateANDtime(char *text, char *date)
{
	int i=0;
	for(i=0;i<25;i++)
	{
		if(text[i] == '\n')
		{
			date[i]='\0';
			break;
		}
		date[i]=text[i];
	}
}

int isHere(char *path, char *user, char *file, char *text)
{
	char dir[strlen(path)+strlen(user)+ADD_SIZE];
	char newtext[MAXSIZENAME];
	strcpy(dir,path);
	strcat(dir,"/");
	strcat(dir,user);
	strcat(dir,"/");
	strcat(dir,file);

	FILE *f = fopen(dir,"r");
	char *estr;

	while(1)
	{
		estr = fgets(newtext,sizeof(newtext),f);
		newtext[strlen(newtext)-1] = '\0';
	
		if(!estr)
		{
			fclose(f);
			return 1;
		}
		int c = strcmp(newtext,text);
		if(c==0)
		{
			fclose(f);
			return 0;
		}
		
	}
	fclose(f);
	return 1;
}

void Read(char *path, char *user, char *text, int flag)
{
	char dir[strlen(path)+strlen(user)+ADD_SIZE];
	strcpy(dir,path);
	strcat(dir,"/");
	strcat(dir,user);

	switch(flag)
	{
		case 1:
			strcat(dir,"/latitude.txt");
			break;
		case 2:
			strcat(dir,"/longitude.txt");
			break;
		case 3:
			strcat(dir,"/password.txt");
			break;
		case 4:
			strcat(dir,"/email.txt");
			break;
		case 5: 
			strcat(dir,"/friends.txt");
			break;
		case 6: 
			strcat(dir,"/status.txt");
			break;
		case 7: 
			strcat(dir,"/potential.txt");
			break;
		case 8: 
			strcat(dir,"/people.txt");
			break;
	}
	
	FILE *file = fopen(dir,"r");
	fscanf(file,"%[^\n]",text);
	fclose(file);
}

int GetStatus(char *path, char *user, char *status)
{
	char dir[strlen(path)+strlen(user)];
	strcpy(dir,path);
	strcat(dir,"/");
	strcat(dir,user);

	int n = access(dir, 0);
	
	if(n==0)
	{
		Read(path, user, status, 6);
      		return 0;
	}
	else
	{
		return 1;
	}
}

int GetPassword(char *path, char *user, char *password, char *email)
{
	char dir[strlen(path)+strlen(user)];
	strcpy(dir,path);
	strcat(dir,"/");
	strcat(dir,user);

	int n = access(dir, 0);
	
	if(n==0)
	{
		Read(path, user, password, 3);
		Read(path, user, email, 4);
      		return 0;
	}
	else
	{
		return 1;
	}
}
// ПАРСЕРЫ ////////////////////////////////////////////////////////////////

void pars_1(char *str,char * word)
{
    short count=0;
    int flag = 0;

    while(*(str++) != '>')
    {
        if(*str == '>')
        {
           	*word=NULL;
           	return;
        }
        if(*str != '/')
        {
            if(flag == 1)
            {
               	*word=*str;
               	*(word++);
            	}
        }
        else
        {
           	flag = 1;
        }
    }
}

void pars_2(char *str,char *word_1,char *word_2)
{
	short count=0;
	while(*(str++) != '>')
	{
		if (*str=='/')
        {
			++count;
        }
        switch(count)
        {
        case 1:
			if(*str != '/')
            {
                *word_1=*str;
                *(word_1++);
            }
            break;
        case 2:
			if(*str != '/' && *str != '>')
            {
				*word_2=*str;
                *(word_2++);
            }
            break;
        default:
            	break;
		}
    }
    *word_1=NULL;
    *word_2=NULL;
}

void pars_3(char *str,char *word_1,char *word_2,char *word_3)
{
	short count=0;
	while(*(str++) != '>')
	{
		if (*str=='/')
        	{
			++count;
        	}
        	switch(count)
        	{
        	case 1:
			if(*str != '/')
            		{
                		*word_1=*str;
                		*(word_1++);
            		}
            		break;
        	case 2:
			if(*str != '/')
            		{
                    		*word_2=*str;
                    		*(word_2++);
            		}
            		break;
        	case 3:
			if(*str != '/' && *str != '>')
            		{
                    		*word_3=*str;
                    		*(word_3++);
            		}
            		break;
        	default:
            		break;
		}
    	}
    	*word_1=NULL;
    	*word_2=NULL;
    	*word_3=NULL;
}

////////////////////////////////////////////////////////////////////////////

int first2(char * str)
{
	if((str[0]=='<') && (str[1]=='u') && (str[2]=='c') && (str[3]=='/'))
	{
		return 1;
	} 
	if((str[0]=='<') && (str[1]=='f') && (str[2]=='c') && (str[3]=='/'))
   	{
      		return 2;
   	}
	if((str[0]=='<') && (str[1]=='c') && (str[2]=='u') && (str[3]=='/'))
   	{
      		return 3;
   	}
	if((str[0]=='<') && (str[1]=='a') && (str[2]=='d') && (str[3]=='/'))
   	{
      		return 4;
   	}
	if((str[0]=='<') && (str[1]=='a') && (str[2]=='f') && (str[3]=='/'))
   	{
      		return 5;
   	}
	if((str[0]=='<') && (str[1]=='l') && (str[2]=='f') && (str[3]=='/'))
   	{
      		return 6;
   	}
	if((str[0]=='<') && (str[1]=='d') && (str[2]=='f') && (str[3]=='/'))
   	{
      		return 7;
   	}
	if((str[0]=='<') && (str[1]=='s') && (str[2]=='s') && (str[3]=='/'))
   	{
      		return 8;
   	}
	if((str[0]=='<') && (str[1]=='g') && (str[2]=='s') && (str[3]=='/'))
   	{
      		return 9;
   	}
	if((str[0]=='<') && (str[1]=='g') && (str[2]=='p') && (str[3]=='/'))
   	{
      		return 10;
   	}
	if((str[0]=='<') && (str[1]=='l') && (str[2]=='p') && (str[3]=='/'))
   	{
      		return 11;
   	}
	if((str[0]=='<') && (str[1]=='l') && (str[2]=='h') && (str[3]=='/'))
   	{
      		return 12;
   	}
	if((str[0]=='<') && (str[1]=='f') && (str[2]=='d') && (str[3]=='/'))
   	{
      		return 13;
   	}
	return 0;
}

void DO(char *str, char *request)
{
	char x[MAXSIZEDOUBLE];
	char y[MAXSIZEDOUBLE];
	char user[MAXSIZENAME];
	char userfriend[MAXSIZENAME];
	char email[30];
	char password[MAXSIZEPASSWORD];
	char * path = "./users";
	char status[MAXSIZESTATUS];
	char * email_from = "hi.app@yandex.ru";

	int m = first2(str);

	if(m == 0)
	{
		sprintf(request, "<bd>\0");
	}
   	if(m == 1)
	{

      	pars_3(str,user,x,y);
		char dir[strlen(path)+strlen(user)];
		strcpy(dir,path);
		strcat(dir,"/");
		strcat(dir,user);

		int n = access(dir, 0);
		if(n==0)
		{
			Write(path, user, x, 1);
			Write(path, user, y, 2);
      		sprintf(request, "<uc/ok>\0");
		}
		else
		{
			sprintf(request, "<uc/not>\0");
		}
   	}
   	if(m == 2)
	{
      	pars_1(str,userfriend);
		char dir[strlen(path)+strlen(userfriend)];
		strcpy(dir,path);
		strcat(dir,"/");
		strcat(dir,userfriend);

		int n = access(dir, 0);
      	if(n==0)
      	{	
			Read(path, userfriend, x, 1);
			Read(path, userfriend, y, 2);
			struct stat st;
			char date[25];
			strcat(dir,"/longitude.txt");
			stat(dir,&st);
			dateANDtime((char*)ctime(&st.st_mtime),date);
         	sprintf(request, "<fc/%s/%s/%s/%s>\0", userfriend, x, y,date); 
      	}
      	else
      	{
        	sprintf(request, "<fc/not>\0");
      	} 
   	}
	if (m == 3)
	{
		pars_2(str,user,password);
		int n = CheckUser(path, user, password);
		switch(n)
        	{
        	case 0:
			sprintf(request, "<cu/ok>\0");
			break;
		case 1:
			sprintf(request, "<cu/not>\0");
			break;
		case 2:
			sprintf(request, "<cu/bad>\0");
			break;
		default:
			break;
		}
	}
	if(m == 4)
	{
		pars_3(str,user,password,email);
		int n = AddUser(path, user, password, email);
		if(n == 0)
		{
			sprintf(request, "<ad/ok>\0");	
		}
		if(n == 1)
		{
			sprintf(request, "<ad/bad>\0");	
		}
	}
	if(m == 5)
	{
		pars_3(str,user,password,userfriend);
		int n = AddFriend(path, user, password, userfriend);
		if(n == 0)
		{
			sprintf(request, "<af/ok>\0");	
		}
		if(n == 1 || n == 2 || n == 3 || n == 4 || n == 5)
		{ 
			sprintf(request, "<af/bad>\0");	
		}
	}
	if(m == 6)
	{
		pars_2(str,user,password);
		char friends[900];
		int n = ListFriends(path, user, password, friends);
		switch(n)
        	{
        	case 0:
			sprintf(request, "<lf/%s>\0",friends);
			break;
		case 1:
			sprintf(request, "<lf/not>\0");
			break;
		case 2:
			sprintf(request, "<lf/bad>\0");
			break;
		default:
			break;
		}
	}
	if(m == 7)
	{
		pars_3(str,user,password,userfriend);
		int n = DeleteFriend(path, user, password, userfriend);
		switch(n)
        {
        case 0:
			sprintf(request, "<df/ok>\0");
			break;
		case 1:
			sprintf(request, "<df/not>\0");
			break;
		case 2:
			sprintf(request, "<df/not>\0");
			break;
		case 3:
			sprintf(request, "<df/bad>\0");
			break;
		default:
			break;
		}
	}
	if(m == 8)
	{
		pars_3(str,user,password,status);
		int a = CheckUser(path, user, password);
		if(a == 0)
		{
			Write(path, user, status, 6);
			sprintf(request, "<ss/ok>\0");
		}
		else
		{
			sprintf(request, "<ss/bad>\0");
		}
	}
	if(m == 9)
	{
		pars_1(str,user);
		int a = GetStatus(path, user, status);
		if(a == 0)
		{
			sprintf(request, "<gs/%s>\0",status);
		}
		else
		{
			sprintf(request, "<gs/bad>\0");
		}
	}
	if(m == 10)
	{
		pars_1(str,user);
		int a = GetPassword(path, user, password, email);
		if(a == 0)
		{
			char email_mes[40];
			sprintf(email_mes, "Your password: %s\0", password); 
			sendmail(email, email_from, "Password recovery", email_mes);
			sprintf(request, "<gp/%s>\0", email);
		}
		else
		{
			sprintf(request, "<gp/bad>\0");
		}
		
	}
	if(m == 11)
	{
		pars_2(str,user,password);
		char friends[900];
		int n = ListPotential(path, user, password, friends);
		switch(n)
        	{
        	case 0:
			sprintf(request, "<lp/%s>\0",friends);
			break;
		case 1:
			sprintf(request, "<lp/not>\0");
			break;
		case 2:
			sprintf(request, "<lp/bad>\0");
			break;
		default:
			break;
		}
	}
	if(m == 12)
	{
		pars_3(str,user,x,y);
		char people[900];
		
		char ficha[100];
		strcpy(ficha,"./ficha ");
		strcat(ficha,user);
		strcat(ficha," ");
		strcat(ficha,x);
		strcat(ficha," ");
		strcat(ficha,y);

		system(ficha);

		printf("here");
		Read(path, user, people, 8);
		sprintf(request, "<lh/%s>\0",people);
	}
	if(m == 13)
	{
      	pars_1(str,userfriend);
		char dir[strlen(path)+strlen(userfriend)];
		strcpy(dir,path);
		strcat(dir,"/");
		strcat(dir,userfriend);

		int n = access(dir, 0);
      	if(n==0)
      	{	
			Read(path, userfriend, x, 1);
			Read(path, userfriend, y, 2);
			Read(path, userfriend, status, 6);
			struct stat st;
			char date[25];
			strcat(dir,"/longitude.txt");
			stat(dir,&st);
			dateANDtime((char*)ctime(&st.st_mtime),date);
         	sprintf(request, "<fd/%s/%s/%s/%s/%s>\0", userfriend, x, y,date,status); 
      	}
		else
      	{
        	sprintf(request, "<fd/not>\0");
      	}
	} 
}
