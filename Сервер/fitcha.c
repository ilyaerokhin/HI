#include <sys/types.h>
#include <stdlib.h>
#include <unistd.h>
#include <stdio.h>
#include <string.h>
#include <math.h>

#define DIRECT_SIZE 12
#define BUFER_SIZE 25

int main(int argc, char *argv[])
{
	people_near(argv[1],argv[2],argv[3]);
	return 0;
}

double distance(double mylatitude, double mylongitude, double lat, double lon)
{
	static const double DEG_TO_RAD = 0.017453292519943295769236907684886; 
	static const double EARTH_RADIUS_IN_METERS = 6372797.560856; 
	printf("distance function");
	double latitudeArc = (mylatitude - lat) * DEG_TO_RAD * 0.5;
	double longitudeArc = (mylongitude - lon) * DEG_TO_RAD * 0.5;
	double latitudeH = sin(latitudeArc);
	latitudeH *= latitudeH;
	double longitudeH = sin(longitudeArc);
	longitudeH *= longitudeH;
	double tmp = cos(mylatitude * DEG_TO_RAD) * cos(lat * DEG_TO_RAD);
	return EARTH_RADIUS_IN_METERS * 2.0 * asin(sqrt(latitudeH + tmp * longitudeH));
}

void people_near(char* myname, char* mylatitude, char* mylongitude)
{ 
	char mlatitude[BUFER_SIZE];
	char mlongitude[BUFER_SIZE];
	int j = 0;
	double mylat;
	double mylon;
	
	//char dot=',';	
	//char dot2='.';
	while(*mylatitude)
	{
		mlatitude[j]=*mylatitude;
		j++;
		mylatitude++;
	}
	j=0;
	while(*mylongitude)
	{
		mlongitude[j]=*mylongitude;
		j++;
		mylongitude++;
	}
	for(j =0; mlatitude[j] != '\0'; j++)
		if(mlatitude[j] == ',')
			mlatitude[j] = '.';
	printf("%s\n",mlatitude);
	mylat = atof(mlatitude);
	printf("%f\n",mylat);
	for(j =0; mlongitude[j] != '\0'; j++)
		if(mlongitude[j] == ',')
			mlongitude[j] = '.';
	printf("%s\n",mlongitude);
	mylon = atof(mlongitude);
	printf("%f\n",mylon);
	chdir("/home/users");
	system("rm ufile");
	system("touch ufile");
	system("chmod -R 777 /home/users");
	system("chmod 777 ufile");
	system("ls -d */ > ufile");
	char* user_name = NULL;
	user_name = malloc(25); 
	char file_name[BUFER_SIZE] = "ufile";
	char* path = NULL;
	path = malloc(50);
	strcpy(path, "/home/users/");
	//const char* latitude_file, longitude_file;
	char latitude[BUFER_SIZE],longitude[BUFER_SIZE];
	double lat, lon, delta;
	int i=0;
	
	strcat(path, myname);
	strcat(path, "/people.txt");
	printf("%s\n", path);
	FILE *people;
	people = fopen(path, "w");
	if( people == NULL )
	{
		perror("Error: can't open people file.\n");
		exit(EXIT_FAILURE);
	}
	strcpy(path, "/home/users/");
	FILE *fp;
	fp = fopen(file_name, "r");
	if( fp == NULL )
	{
		perror("Error: can't open file.\n");
		return;
	}
	while(fgets(user_name,BUFER_SIZE,fp))
	{
		//user_name[strlen(user_name)] = '\0';
		printf("Data from file: %s\n", user_name);
		for(i =0; user_name[i] != '\0'; i++)
			if(user_name[i] == '\n')
				user_name[i-1] = '\0';
		strcpy(path,"/home/users/");
		strcat(path,user_name);
		strcat(path,"/latitude.txt");
		printf("%s\n", path);
		FILE *lati;
		lati = fopen(path, "r");
		if( lati == NULL )
		{
			perror("Error: can't open lati file.\n");
			return;
		}
		fgets(latitude,BUFER_SIZE,lati);
		for(i =0; latitude[i] != '\0'; i++)
			if(latitude[i] == ',')
				latitude[i] = '.';
		printf("%s\n", latitude);
		lat = atof(latitude);
		printf("%f\n", lat);
		fclose(lati);
		strcpy(path, "/home/users/");
		strcat(path, user_name);
		strcat(path, "/longitude.txt");
		FILE *longi;
		longi = fopen(path, "r");
		if( longi == NULL )
		{
			perror("Error: can't open longi file.\n");
			return;
		}
		fgets(longitude,BUFER_SIZE,longi);
		for(i =0; longitude[i] != '\0'; i++)
			if(longitude[i] == ',')
				longitude[i] = '.';
		lon = atof(longitude);
		fclose(longi);
		strcpy(path, "/home/users/");
		delta = distance(mylat, mylon, lat, lon);
		printf("%f", delta); 
		if (delta <= 500)
		{
			fprintf(people, "%s|", user_name);
		}
		*latitude = NULL;
		*longitude = NULL;
		*user_name = NULL;
	}
	free(user_name);
	free(path);
	fclose(fp);
	fclose(people);
}
