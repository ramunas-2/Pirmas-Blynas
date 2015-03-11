// ExternalC.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <stdio.h>





extern "C"
{
	__declspec(dllexport) void DisplayHelloFromDLL(int length, double dArray[])
	{
		//printf("Hello from DLL !\n",x);
		for (int i = 0; i < length; i++)
		{
			dArray[i] += 1.0;
		}
	}
}

