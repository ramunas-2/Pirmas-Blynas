
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <stdio.h>

cudaError_t addWithCuda(int *c, const int *a, const int *b, unsigned int size);


__constant__ int* maxGameOverPathLengthEvolution1;

__constant__ int* mask1;
__constant__ int* mask0;

__constant__ int* matrix1;
__constant__ int* matrix0;

__constant__ int* path1;
__constant__ int* path0;

// int* sMask4__;
// int* kAmask4__;
 //_device__ __constant__ int sMask4[10800];
__constant__ int* sMask4;

__constant__ int* k0mask4;
__constant__ int* k1mask4;
__constant__ int* k2mask4;
__constant__ int* k3mask4;
__constant__ int* kAmask4;

__constant__ int* sMask3;
__constant__ int* k0mask3;
__constant__ int* k1mask3;
__constant__ int* k2mask3;
__constant__ int* kAmask3;

__constant__ int* sMask2;
__constant__ int* k0mask2;
__constant__ int* k1mask2;
__constant__ int* kAmask2;

__constant__ int* smatrix3;
__constant__ int* k0matrix3;
__constant__ int* k1matrix3;
__constant__ int* k2matrix3;
__constant__ int* kAmatrix3;

__constant__ int* smatrix2;
__constant__ int* k0matrix2;
__constant__ int* k1matrix2;
__constant__ int* kAmatrix2;

__constant__ double* smatrixCoins2;
__constant__ int* k0matrixCoins2;
__constant__ int* k1matrixCoins2;
__constant__ int* kAmatrixCoins2;

__constant__ int* spath2;
__constant__ int* k0path2;
__constant__ int* k1path2;
__constant__ int* kApath2;
















__global__ void addKernel(int *c, const int *a, const int *b)
{
	int i = threadIdx.x;
	//printf("Thread %d\n",i);

	long cc = 0;
	for (int i2 = 0; i2 < 1; i2++)
		for (int i1 = 0; i1 < 4000000; i1++)
		{
			cc += i1;
		}

	c[i] = cc;


}


__global__ void antiPlayerGPU(

	double *brainCells,
	const int *allBrainCellsCount,
	double *sumOfCoins,

	const int* maxGameOverPathLengthEvolution1,

	const int* mask1,
	const int* mask0,

	const int* matrix1,
	const int* matrix0,

	const int* path1,
	const int* path0,

	const int* sMask4,
	const int* k0mask4,
	const int* k1mask4,
	const int* k2mask4,
	const int* k3mask4,
	const int* kAmask4,

	const int* sMask3,
	const int* k0mask3,
	const int* k1mask3,
	const int* k2mask3,
	const int* kAmask3,

	const int* sMask2,
	const int* k0mask2,
	const int* k1mask2,
	const int* kAmask2,

	const int* smatrix3,
	const int* k0matrix3,
	const int* k1matrix3,
	const int* k2matrix3,
	const int* kAmatrix3,

	const int* smatrix2,
	const int* k0matrix2,
	const int* k1matrix2,
	const int* kAmatrix2,

	const double* smatrixCoins2,
	const int* k0matrixCoins2,
	const int* k1matrixCoins2,
	const int* kAmatrixCoins2,

	const int* spath2,
	const int* k0path2,
	const int* k1path2,
	const int* kApath2)
{
	int iThX = threadIdx.x;
	double* wonCoins = new double[*matrix0];

	for (int i0 = 0; i0 < *matrix0; i0++)
	{
		double sum = 0.0;
		for (int i1 = 0; i1 < matrix1[i0]; i1++)
		{
			double multiplication = smatrixCoins2[(i0 * (*k1matrix2)) + i1];
			for (int i2 = 0; i2 < smatrix2[(i0 * (*k1matrix2)) + i1]; i2++)
			{
				multiplication *= brainCells[((*allBrainCellsCount)*iThX) + smatrix3[(((i0 * (*k1matrix3)) + i1) * (*k2matrix3)) + i2]];
			}
			sum += multiplication;
		}
		wonCoins[i0] = sum;
	}

	int pathLength = *maxGameOverPathLengthEvolution1;
	for (int i0 = 0; i0 < *mask0; i0++)
	{
		for (int i1 = 0; i1 < mask1[i0]; i1++)
		{
			double bestOptionABenefit = -1000000000; //to fix
			int bestOptionAChoiceLocation = -1;
			for (int i2 = 0; i2 < sMask2[(i0 * (*k1mask2)) + i1]; i2++)
			{
				double sumOfAllDiceCombinationsBenefit = 0;
				for (int i3 = 0; i3 < sMask3[(((i0 * (*k1mask3)) + i1) * (*k2mask3)) + i2]; i3++)
				{
					sumOfAllDiceCombinationsBenefit += wonCoins[sMask4[(((((i0 * (*k1mask4)) + i1) * (*k2mask4)) + i2) * (*k3mask4)) + i3]];

				}
				//if (bestOptionABenefit == null || sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
				if (sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
				{
					bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
					bestOptionAChoiceLocation = i2;
				}
			}
			for (int i2 = 0; i2 < sMask2[(i0 * (*k1mask2)) + i1]; i2++)
			{
				for (int i3 = 0; i3 < sMask3[(((i0 * (*k1mask3)) + i1) * (*k2mask3)) + i2]; i3++)
				{
					double newBrainCellValue;
					if (i2 == bestOptionAChoiceLocation)
					{
						newBrainCellValue = 1.0;
					}
					else
					{
						newBrainCellValue = 0.0;
					}
					brainCells[((*allBrainCellsCount)*iThX) + spath2[((sMask4[(((((i0 * (*k1mask4)) + i1) * (*k2mask4)) + i2) * (*k3mask4)) + i3]) * (*k1path2)) + pathLength - 1]] = newBrainCellValue;
					wonCoins[sMask4[(((((i0 * (*k1mask4)) + i1) * (*k2mask4)) + i2) * (*k3mask4)) + i3]] *= newBrainCellValue;
				}
			}
		}
		pathLength--;
	}
	sumOfCoins[iThX] = 0;
	for (int i = 0; i < *matrix0; i++)
	{
		sumOfCoins[iThX] += wonCoins[i];
	}
	delete[] wonCoins;

}



extern "C"
{
	__declspec(dllexport) void CudaAdd(int path0, int path1[], int path2[][2][3])
	{
		//printf("GO Cuda! %d\n", length);
		//addWithCuda(c, a, b, length);

	}
}





extern "C"
{
	__declspec(dllexport) void DisplayHelloFromDLL(int length, double dArray[])
	{
		//printf("Hello from DLL !\n",x);
		for (int i = 0; i < length; i++)
		{
			dArray[i] += 4.0;
		}
	}
}

extern "C"
{
	__declspec(dllexport) void initialiseAntiPlayer2(

		int maxGameOverPathLengthEvolution1


		)
	{

	}



}



extern "C"
{
	__declspec(dllexport) void initialiseAntiPlayer(

		int _maxGameOverPathLengthEvolution1,

		int _mask1[],
		int _mask0,

		int _matrix1[],
		int _matrix0,

		int _path1[],
		int _path0,

		int _sMask4[],
		int _k0mask4,
		int _k1mask4,
		int _k2mask4,
		int _k3mask4,
		int _kAmask4,

		int _sMask3[],
		int _k0mask3,
		int _k1mask3,
		int _k2mask3,
		int _kAmask3,

		int _sMask2[],
		int _k0mask2,
		int _k1mask2,
		int _kAmask2,

		int _smatrix3[],
		int _k0matrix3,
		int _k1matrix3,
		int _k2matrix3,
		int _kAmatrix3,

		int _smatrix2[],
		int _k0matrix2,
		int _k1matrix2,
		int _kAmatrix2,

		double _smatrixCoins2[],
		int _k0matrixCoins2,
		int _k1matrixCoins2,
		int _kAmatrixCoins2,

		int _spath2[],
		int _k0path2,
		int _k1path2,
		int _kApath2

		)
	{
		printf("Hello from C++, initialising parameters.\n");

		/*
		maxGameOverPathLengthEvolution1 = new int;

		*maxGameOverPathLengthEvolution1 = 	_maxGameOverPathLengthEvolution1;

		mask0 = new int;
		*mask0=_mask0;
		mask1 = new int[*mask0];
		cudaMemcpy(mask1, _mask1, sizeof(int)*(*mask0), cudaMemcpyHostToHost);

		matrix0 = new int;
		*matrix0 = _matrix0;
		matrix1 = new int[*matrix0];
		cudaMemcpy(matrix1, _matrix1, sizeof(int)*(*matrix0), cudaMemcpyHostToHost);

		wonCoinsLength = new int;
		*wonCoinsLength = _wonCoinsLength;
		wonCoins = new double[*wonCoinsLength];
		cudaMemcpy(wonCoins, _wonCoins, sizeof(double)*(*wonCoinsLength), cudaMemcpyHostToHost);

		path0 = new int;
		*path0 = _path0;
		path1 = new int[*path0];
		cudaMemcpy(path1, _path1, sizeof(int)*(*path0), cudaMemcpyHostToHost);

		k0mask4 = new int;
		*k0mask4 = _k0mask4;
		k1mask4 = new int;
		*k1mask4 = _k1mask4;
		k2mask4 = new int;
		*k2mask4 = _k2mask4;
		k3mask4 = new int;
		*k3mask4 = _k3mask4;
		kAmask4 = new int;
		*kAmask4 = _kAmask4;

		sMask4 = new int[*kAmask4];
		cudaMemcpy(sMask4, _sMask4, sizeof(int)*(*kAmask4), cudaMemcpyHostToHost);

		k0mask3 = new int;
		*k0mask3 = _k0mask3;
		k1mask3 = new int;
		*k1mask3 = _k1mask3;
		k2mask3 = new int;
		*k2mask3 = _k2mask3;
		kAmask3 = new int;
		*kAmask3 = _kAmask3;

		sMask3 = new int[*kAmask3];
		cudaMemcpy(sMask3, _sMask3, sizeof(int)*(*kAmask3), cudaMemcpyHostToHost);

		k0mask2 = new int;
		*k0mask2 = _k0mask2;
		k1mask2 = new int;
		*k1mask2 = _k1mask2;
		kAmask2 = new int;
		*kAmask2 = _kAmask2;

		sMask2 = new int[*kAmask2];
		cudaMemcpy(sMask2, _sMask2, sizeof(int)*(*kAmask2), cudaMemcpyHostToHost);

		k0matrix3 = new int;
		*k0matrix3 = _k0matrix3;
		k1matrix3 = new int;
		*k1matrix3 = _k1matrix3;
		k2matrix3 = new int;
		*k2matrix3 = _k2matrix3;
		kAmatrix3 = new int;
		*kAmatrix3 = _kAmatrix3;

		smatrix3 = new int[*kAmatrix3];
		cudaMemcpy(smatrix3, _smatrix3, sizeof(int)*(*kAmatrix3), cudaMemcpyHostToHost);

		k0matrix2 = new int;
		*k0matrix2 = _k0matrix2;
		k1matrix2 = new int;
		*k1matrix2 = _k1matrix2;
		kAmatrix2 = new int;
		*kAmatrix2 = _kAmatrix2;

		smatrix2 = new int[*kAmatrix2];
		cudaMemcpy(smatrix2, _smatrix2, sizeof(int)*(*kAmatrix2), cudaMemcpyHostToHost);

		k0matrixCoins2 = new int;
		*k0matrixCoins2 = _k0matrixCoins2;
		k1matrixCoins2 = new int;
		*k1matrixCoins2 = _k1matrixCoins2;
		kAmatrixCoins2 = new int;
		*kAmatrixCoins2 = _kAmatrixCoins2;

		smatrixCoins2 = new double[*kAmatrixCoins2];
		cudaMemcpy(smatrixCoins2, _smatrixCoins2, sizeof(double)*(*kAmatrixCoins2), cudaMemcpyHostToHost);

		k0path2 = new int;
		*k0path2 = _k0path2;
		k1path2 = new int;
		*k1path2 = _k1path2;
		kApath2 = new int;
		*kApath2 = _kApath2;

		spath2 = new int[*kApath2];
		cudaMemcpy(spath2, _spath2, sizeof(int)*(*kApath2), cudaMemcpyHostToHost);
		*/



		/*
				maxGameOverPathLengthEvolution1 = new int;
				cudaMemcpy(maxGameOverPathLengthEvolution1, &_maxGameOverPathLengthEvolution1, sizeof(int), cudaMemcpyHostToHost);

				mask0 = new int;
				cudaMemcpy(mask0, &_mask0, sizeof(int), cudaMemcpyHostToHost);
				mask1 = new int[_mask0];
				cudaMemcpy(mask1, _mask1, sizeof(int)*_mask0, cudaMemcpyHostToHost);

				matrix0 = new int;
				cudaMemcpy(matrix0, &_matrix0, sizeof(int), cudaMemcpyHostToHost);
				matrix1 = new int[_matrix0];
				cudaMemcpy(matrix1, _matrix1, sizeof(int)*_matrix0, cudaMemcpyHostToHost);

				wonCoinsLength = new int;
				cudaMemcpy(wonCoinsLength, &_wonCoinsLength, sizeof(int), cudaMemcpyHostToHost);

				wonCoins = new double[_wonCoinsLength];
				cudaMemcpy(wonCoins, _wonCoins, sizeof(double)*_wonCoinsLength, cudaMemcpyHostToHost);

				path0 = new int;
				cudaMemcpy(path0, &_path0, sizeof(int), cudaMemcpyHostToHost);
				path1 = new int[_path0];
				cudaMemcpy(path1, _path1, sizeof(int)*_path0, cudaMemcpyHostToHost);

				k0mask4 = new int;
				cudaMemcpy(k0mask4, &_k0mask4, sizeof(int), cudaMemcpyHostToHost);
				k1mask4 = new int;
				cudaMemcpy(k1mask4, &_k1mask4, sizeof(int), cudaMemcpyHostToHost);
				k2mask4 = new int;
				cudaMemcpy(k2mask4, &_k2mask4, sizeof(int), cudaMemcpyHostToHost);
				k3mask4 = new int;
				cudaMemcpy(k3mask4, &_k3mask4, sizeof(int), cudaMemcpyHostToHost);
				kAmask4 = new int;
				cudaMemcpy(kAmask4, &_kAmask4, sizeof(int), cudaMemcpyHostToHost);

				sMask4 = new int[_kAmask4];
				cudaMemcpy(sMask4, _sMask4, sizeof(int)*_kAmask4, cudaMemcpyHostToHost);

				k0mask3 = new int;
				cudaMemcpy(k0mask3, &_k0mask3, sizeof(int), cudaMemcpyHostToHost);
				k1mask3 = new int;
				cudaMemcpy(k1mask3, &_k1mask3, sizeof(int), cudaMemcpyHostToHost);
				k2mask3 = new int;
				cudaMemcpy(k2mask3, &_k2mask3, sizeof(int), cudaMemcpyHostToHost);
				kAmask3 = new int;
				cudaMemcpy(kAmask3, &_kAmask3, sizeof(int), cudaMemcpyHostToHost);

				sMask3 = new int[_kAmask3];
				cudaMemcpy(sMask3, _sMask3, sizeof(int)*_kAmask3, cudaMemcpyHostToHost);

				k0mask2 = new int;
				cudaMemcpy(k0mask2, &_k0mask2, sizeof(int), cudaMemcpyHostToHost);
				k1mask2 = new int;
				cudaMemcpy(k1mask2, &_k1mask2, sizeof(int), cudaMemcpyHostToHost);
				kAmask2 = new int;
				cudaMemcpy(kAmask2, &_kAmask2, sizeof(int), cudaMemcpyHostToHost);

				sMask2 = new int[_kAmask2];
				cudaMemcpy(sMask2, _sMask2, sizeof(int)*_kAmask2, cudaMemcpyHostToHost);

				k0matrix3 = new int;
				cudaMemcpy(k0matrix3, &_k0matrix3, sizeof(int), cudaMemcpyHostToHost);
				k1matrix3 = new int;
				cudaMemcpy(k1matrix3, &_k1matrix3, sizeof(int), cudaMemcpyHostToHost);
				k2matrix3 = new int;
				cudaMemcpy(k2matrix3, &_k2matrix3, sizeof(int), cudaMemcpyHostToHost);
				kAmatrix3 = new int;
				cudaMemcpy(kAmatrix3, &_kAmatrix3, sizeof(int), cudaMemcpyHostToHost);

				smatrix3 = new int[_kAmatrix3];
				cudaMemcpy(smatrix3, _smatrix3, sizeof(int)*_kAmatrix3, cudaMemcpyHostToHost);

				k0matrix2 = new int;
				cudaMemcpy(k0matrix2, &_k0matrix2, sizeof(int), cudaMemcpyHostToHost);
				k1matrix2 = new int;
				cudaMemcpy(k1matrix2, &_k1matrix2, sizeof(int), cudaMemcpyHostToHost);
				kAmatrix2 = new int;
				cudaMemcpy(kAmatrix2, &_kAmatrix2, sizeof(int), cudaMemcpyHostToHost);

				smatrix2 = new int[_kAmatrix2];
				cudaMemcpy(smatrix2, _smatrix2, sizeof(int)*_kAmatrix2, cudaMemcpyHostToHost);

				k0matrixCoins2 = new int;
				cudaMemcpy(k0matrixCoins2, &_k0matrixCoins2, sizeof(int), cudaMemcpyHostToHost);
				k1matrixCoins2 = new int;
				cudaMemcpy(k1matrixCoins2, &_k1matrixCoins2, sizeof(int), cudaMemcpyHostToHost);
				kAmatrixCoins2 = new int;
				cudaMemcpy(kAmatrixCoins2, &_kAmatrixCoins2, sizeof(int), cudaMemcpyHostToHost);

				smatrixCoins2 = new double[_kAmatrixCoins2];
				cudaMemcpy(smatrixCoins2, _smatrixCoins2, sizeof(double)*_kAmatrixCoins2, cudaMemcpyHostToHost);

				k0path2 = new int;
				cudaMemcpy(k0path2, &_k0path2, sizeof(int), cudaMemcpyHostToHost);
				k1path2 = new int;
				cudaMemcpy(k1path2, &_k1path2, sizeof(int), cudaMemcpyHostToHost);
				kApath2 = new int;
				cudaMemcpy(kApath2, &_kApath2, sizeof(int), cudaMemcpyHostToHost);

				spath2 = new int[_kApath2];
				cudaMemcpy(spath2, _spath2, sizeof(int)*_kApath2, cudaMemcpyHostToHost);
				*/


		/*
		CPU_maxGameOverPathLengthEvolution1 = new int;
		cudaMemcpy(maxGameOverPathLengthEvolution1, &_maxGameOverPathLengthEvolution1, sizeof(int), cudaMemcpyHostToHost);

		CPU_mask0 = new int;
		cudaMemcpy(mask0, &_mask0, sizeof(int), cudaMemcpyHostToHost);
		CPU_mask1 = new int[_mask0];
		cudaMemcpy(mask1, _mask1, sizeof(int)*_mask0, cudaMemcpyHostToHost);

		CPU_matrix0 = new int;
		cudaMemcpy(matrix0, &_matrix0, sizeof(int), cudaMemcpyHostToHost);
		CPU_matrix1 = new int[_matrix0];
		cudaMemcpy(matrix1, _matrix1, sizeof(int)*_matrix0, cudaMemcpyHostToHost);

		CPU_path0 = new int;
		cudaMemcpy(path0, &_path0, sizeof(int), cudaMemcpyHostToHost);
		CPU_path1 = new int[_path0];
		cudaMemcpy(path1, _path1, sizeof(int)*_path0, cudaMemcpyHostToHost);

		CPU_k0mask4 = new int;
		cudaMemcpy(k0mask4, &_k0mask4, sizeof(int), cudaMemcpyHostToHost);
		CPU_k1mask4 = new int;
		cudaMemcpy(k1mask4, &_k1mask4, sizeof(int), cudaMemcpyHostToHost);
		CPU_k2mask4 = new int;
		cudaMemcpy(k2mask4, &_k2mask4, sizeof(int), cudaMemcpyHostToHost);
		CPU_k3mask4 = new int;
		cudaMemcpy(k3mask4, &_k3mask4, sizeof(int), cudaMemcpyHostToHost);
		CPU_kAmask4 = new int;
		cudaMemcpy(kAmask4, &_kAmask4, sizeof(int), cudaMemcpyHostToHost);

		CPU_sMask4 = new int[_kAmask4];
		cudaMemcpy(sMask4, _sMask4, sizeof(int)*_kAmask4, cudaMemcpyHostToHost);

		CPU_k0mask3 = new int;
		cudaMemcpy(k0mask3, &_k0mask3, sizeof(int), cudaMemcpyHostToHost);
		CPU_k1mask3 = new int;
		cudaMemcpy(k1mask3, &_k1mask3, sizeof(int), cudaMemcpyHostToHost);
		CPU_k2mask3 = new int;
		cudaMemcpy(k2mask3, &_k2mask3, sizeof(int), cudaMemcpyHostToHost);
		CPU_kAmask3 = new int;
		cudaMemcpy(kAmask3, &_kAmask3, sizeof(int), cudaMemcpyHostToHost);

		CPU_sMask3 = new int[_kAmask3];
		cudaMemcpy(sMask3, _sMask3, sizeof(int)*_kAmask3, cudaMemcpyHostToHost);

		CPU_k0mask2 = new int;
		cudaMemcpy(k0mask2, &_k0mask2, sizeof(int), cudaMemcpyHostToHost);
		CPU_k1mask2 = new int;
		cudaMemcpy(k1mask2, &_k1mask2, sizeof(int), cudaMemcpyHostToHost);
		CPU_kAmask2 = new int;
		cudaMemcpy(kAmask2, &_kAmask2, sizeof(int), cudaMemcpyHostToHost);

		CPU_sMask2 = new int[_kAmask2];
		cudaMemcpy(sMask2, _sMask2, sizeof(int)*_kAmask2, cudaMemcpyHostToHost);

		CPU_k0matrix3 = new int;
		cudaMemcpy(k0matrix3, &_k0matrix3, sizeof(int), cudaMemcpyHostToHost);
		CPU_k1matrix3 = new int;
		cudaMemcpy(k1matrix3, &_k1matrix3, sizeof(int), cudaMemcpyHostToHost);
		CPU_k2matrix3 = new int;
		cudaMemcpy(k2matrix3, &_k2matrix3, sizeof(int), cudaMemcpyHostToHost);
		CPU_kAmatrix3 = new int;
		cudaMemcpy(kAmatrix3, &_kAmatrix3, sizeof(int), cudaMemcpyHostToHost);

		CPU_smatrix3 = new int[_kAmatrix3];
		cudaMemcpy(smatrix3, _smatrix3, sizeof(int)*_kAmatrix3, cudaMemcpyHostToHost);

		CPU_k0matrix2 = new int;
		cudaMemcpy(k0matrix2, &_k0matrix2, sizeof(int), cudaMemcpyHostToHost);
		CPU_k1matrix2 = new int;
		cudaMemcpy(k1matrix2, &_k1matrix2, sizeof(int), cudaMemcpyHostToHost);
		CPU_kAmatrix2 = new int;
		cudaMemcpy(kAmatrix2, &_kAmatrix2, sizeof(int), cudaMemcpyHostToHost);

		CPU_smatrix2 = new int[_kAmatrix2];
		cudaMemcpy(smatrix2, _smatrix2, sizeof(int)*_kAmatrix2, cudaMemcpyHostToHost);

		CPU_k0matrixCoins2 = new int;
		cudaMemcpy(k0matrixCoins2, &_k0matrixCoins2, sizeof(int), cudaMemcpyHostToHost);
		CPU_k1matrixCoins2 = new int;
		cudaMemcpy(k1matrixCoins2, &_k1matrixCoins2, sizeof(int), cudaMemcpyHostToHost);
		CPU_kAmatrixCoins2 = new int;
		cudaMemcpy(kAmatrixCoins2, &_kAmatrixCoins2, sizeof(int), cudaMemcpyHostToHost);

		CPU_smatrixCoins2 = new double[_kAmatrixCoins2];
		cudaMemcpy(smatrixCoins2, _smatrixCoins2, sizeof(double)*_kAmatrixCoins2, cudaMemcpyHostToHost);

		CPU_k0path2 = new int;
		cudaMemcpy(k0path2, &_k0path2, sizeof(int), cudaMemcpyHostToHost);
		CPU_k1path2 = new int;
		cudaMemcpy(k1path2, &_k1path2, sizeof(int), cudaMemcpyHostToHost);
		CPU_kApath2 = new int;
		cudaMemcpy(kApath2, &_kApath2, sizeof(int), cudaMemcpyHostToHost);

		CPU_spath2 = new int[_kApath2];
		cudaMemcpy(spath2, _spath2, sizeof(int)*_kApath2, cudaMemcpyHostToHost);
		*/


		cudaMalloc((void**)&maxGameOverPathLengthEvolution1, sizeof(int));
		cudaMemcpy(maxGameOverPathLengthEvolution1, &_maxGameOverPathLengthEvolution1, sizeof(int), cudaMemcpyHostToDevice);

		cudaMalloc((void**)&mask0, sizeof(int));
		cudaMemcpy(mask0, &_mask0, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&mask1, _mask0 * sizeof(int));
		cudaMemcpy(mask1, _mask1, sizeof(int)*_mask0, cudaMemcpyHostToDevice);

		cudaMalloc((void**)&matrix0, sizeof(int));
		cudaMemcpy(matrix0, &_matrix0, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&matrix1, _matrix0 * sizeof(int));
		cudaMemcpy(matrix1, _matrix1, sizeof(int)*_matrix0, cudaMemcpyHostToDevice);

		cudaMalloc((void**)&path0, sizeof(int));
		cudaMemcpy(path0, &_path0, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&path1, _path0 * sizeof(int));
		cudaMemcpy(path1, _path1, sizeof(int)*_path0, cudaMemcpyHostToDevice);

		cudaMalloc((void**)&k0mask4, sizeof(int));
		cudaMemcpy(k0mask4, &_k0mask4, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k1mask4, sizeof(int));
		cudaMemcpy(k1mask4, &_k1mask4, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k2mask4, sizeof(int));
		cudaMemcpy(k2mask4, &_k2mask4, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k3mask4, sizeof(int));
		cudaMemcpy(k3mask4, &_k3mask4, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&kAmask4, sizeof(int));
		cudaMemcpy(kAmask4, &_kAmask4, sizeof(int), cudaMemcpyHostToDevice);





		cudaMalloc((void**)&sMask4, _kAmask4 * sizeof(int));
		cudaMemcpy(sMask4, _sMask4, sizeof(int)*_kAmask4, cudaMemcpyHostToDevice);


		/*
		sMask4__ = new int[_kAmask4];
		cudaMemcpy(sMask4__, _sMask4, sizeof(int)*_kAmask4, cudaMemcpyHostToHost);

		
		kAmask4__ = new int;
		cudaMemcpy(kAmask4__, &_kAmask4, sizeof(int), cudaMemcpyHostToHost);
		*/






		


		cudaMalloc((void**)&k0mask3, sizeof(int));
		cudaMemcpy(k0mask3, &_k0mask3, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k1mask3, sizeof(int));
		cudaMemcpy(k1mask3, &_k1mask3, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k2mask3, sizeof(int));
		cudaMemcpy(k2mask3, &_k2mask3, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&kAmask3, sizeof(int));
		cudaMemcpy(kAmask3, &_kAmask3, sizeof(int), cudaMemcpyHostToDevice);

		cudaMalloc((void**)&sMask3, _kAmask3 * sizeof(int));
		cudaMemcpy(sMask3, _sMask3, sizeof(int)*_kAmask3, cudaMemcpyHostToDevice);

		cudaMalloc((void**)&k0mask2, sizeof(int));
		cudaMemcpy(k0mask2, &_k0mask2, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k1mask2, sizeof(int));
		cudaMemcpy(k1mask2, &_k1mask2, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&kAmask2, sizeof(int));
		cudaMemcpy(kAmask2, &_kAmask2, sizeof(int), cudaMemcpyHostToDevice);

		cudaMalloc((void**)&sMask2, _kAmask2 * sizeof(int));
		cudaMemcpy(sMask2, _sMask2, sizeof(int)*_kAmask2, cudaMemcpyHostToDevice);

		cudaMalloc((void**)&k0matrix3, sizeof(int));
		cudaMemcpy(k0matrix3, &_k0matrix3, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k1matrix3, sizeof(int));
		cudaMemcpy(k1matrix3, &_k1matrix3, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k2matrix3, sizeof(int));
		cudaMemcpy(k2matrix3, &_k2matrix3, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&kAmatrix3, sizeof(int));
		cudaMemcpy(kAmatrix3, &_kAmatrix3, sizeof(int), cudaMemcpyHostToDevice);

		cudaMalloc((void**)&smatrix3, _kAmatrix3 * sizeof(int));
		cudaMemcpy(smatrix3, _smatrix3, sizeof(int)*_kAmatrix3, cudaMemcpyHostToDevice);

		cudaMalloc((void**)&k0matrix2, sizeof(int));
		cudaMemcpy(k0matrix2, &_k0matrix2, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k1matrix2, sizeof(int));
		cudaMemcpy(k1matrix2, &_k1matrix2, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&kAmatrix2, sizeof(int));
		cudaMemcpy(kAmatrix2, &_kAmatrix2, sizeof(int), cudaMemcpyHostToDevice);

		cudaMalloc((void**)&smatrix2, _kAmatrix2 * sizeof(int));
		cudaMemcpy(smatrix2, _smatrix2, sizeof(int)*_kAmatrix2, cudaMemcpyHostToDevice);

		cudaMalloc((void**)&k0matrixCoins2, sizeof(int));
		cudaMemcpy(k0matrixCoins2, &_k0matrixCoins2, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k1matrixCoins2, sizeof(int));
		cudaMemcpy(k1matrixCoins2, &_k1matrixCoins2, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&kAmatrixCoins2, sizeof(int));
		cudaMemcpy(kAmatrixCoins2, &_kAmatrixCoins2, sizeof(int), cudaMemcpyHostToDevice);

		cudaMalloc((void**)&smatrixCoins2, _kAmatrixCoins2 * sizeof(double));
		cudaMemcpy(smatrixCoins2, _smatrixCoins2, sizeof(double)*_kAmatrixCoins2, cudaMemcpyHostToDevice);

		cudaMalloc((void**)&k0path2, sizeof(int));
		cudaMemcpy(k0path2, &_k0path2, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&k1path2, sizeof(int));
		cudaMemcpy(k1path2, &_k1path2, sizeof(int), cudaMemcpyHostToDevice);
		cudaMalloc((void**)&kApath2, sizeof(int));
		cudaMemcpy(kApath2, &_kApath2, sizeof(int), cudaMemcpyHostToDevice);

		cudaMalloc((void**)&spath2, _kApath2 * sizeof(int));
		cudaMemcpy(spath2, _spath2, sizeof(int)*_kApath2, cudaMemcpyHostToDevice);



	}

	__declspec(dllexport) double calculateAntiPlayerExternal(double _brainCells[], int _allBrainCellsCount, double _sumOfCoins[], int _count)
	{


		double* brainCells;
		cudaMalloc((void**)&brainCells, _allBrainCellsCount*_count*sizeof(double));
		cudaMemcpy(brainCells, _brainCells, _allBrainCellsCount*_count*sizeof(double), cudaMemcpyHostToDevice);

		int* allBrainCellsCount;
		cudaMalloc((void**)&allBrainCellsCount, sizeof(int));
		cudaMemcpy(allBrainCellsCount, &_allBrainCellsCount, sizeof(int), cudaMemcpyHostToDevice);


		double* sumOfCoins;
		cudaMalloc((void**)&sumOfCoins, sizeof(double)*_count);


		
		//__constant__ int* sMask4;

		//__constant__ int sMask4[10800];




		/*
		int* sMask4;

		cudaMalloc((void**)&sMask4, (*kAmask4__) * sizeof(int));
		cudaMemcpy(sMask4, sMask4__, sizeof(int)*(*kAmask4__), cudaMemcpyHostToDevice);
		*/

		/*
		cudaMemcpyToSymbol(sMask4, sMask4__, sizeof(int)*(*kAmask4__), cudaMemcpyHostToDevice);

		cudaError_t cudaStatus = cudaMemcpyToSymbol(sMask4, sMask4__, sizeof(int)*(*kAmask4__), cudaMemcpyHostToDevice);
		if (cudaStatus != cudaSuccess) {
			fprintf(stderr, "cudaMallocToSymbol failed!");
			printf("cudaMallocToSymbol failed!\n");
			printf(cudaStatus);

		}
		*/


		//antiPlayerGPU << <1, _count >> >(brainCells, allBrainCellsCount, sumOfCoins,
		antiPlayerGPU << <1, _count >> >(brainCells, allBrainCellsCount, sumOfCoins,
			maxGameOverPathLengthEvolution1,

			mask1,
			mask0,

			matrix1,
			matrix0,

			/*
			wonCoins,
			wonCoinsLength,
			*/

			path1,
			path0,

			sMask4,
			k0mask4,
			k1mask4,
			k2mask4,
			k3mask4,
			kAmask4,

			sMask3,
			k0mask3,
			k1mask3,
			k2mask3,
			kAmask3,

			sMask2,
			k0mask2,
			k1mask2,
			kAmask2,

			smatrix3,
			k0matrix3,
			k1matrix3,
			k2matrix3,
			kAmatrix3,

			smatrix2,
			k0matrix2,
			k1matrix2,
			kAmatrix2,

			smatrixCoins2,
			k0matrixCoins2,
			k1matrixCoins2,
			kAmatrixCoins2,

			spath2,
			k0path2,
			k1path2,
			kApath2

			);

		cudaDeviceSynchronize();

		cudaMemcpy(_sumOfCoins, sumOfCoins, _count*sizeof(double), cudaMemcpyDeviceToHost);
		cudaMemcpy(_brainCells, brainCells, _allBrainCellsCount*_count*sizeof(double), cudaMemcpyDeviceToHost);

		cudaFree(brainCells);
		cudaFree(allBrainCellsCount);
		cudaFree(sumOfCoins);

		return 0;

	}
}



int main()
{
	const int arraySize = 5;
	const int a[arraySize] = { 1, 2, 3, 4, 5 };
	const int b[arraySize] = { 10, 20, 30, 40, 50 };
	int c[arraySize] = { 0 };

	// Add vectors in parallel.
	cudaError_t cudaStatus = addWithCuda(c, a, b, arraySize);
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "addWithCuda failed!");
		return 1;
	}

	printf("{1,2,3,4,5} + {10,20,30,40,50} = {%d,%d,%d,%d,%d}\n",
		c[0], c[1], c[2], c[3], c[4]);

	// cudaDeviceReset must be called before exiting in order for profiling and
	// tracing tools such as Nsight and Visual Profiler to show complete traces.
	cudaStatus = cudaDeviceReset();
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaDeviceReset failed!");
		return 1;
	}

	return 0;
}

// Helper function for using CUDA to add vectors in parallel.
cudaError_t addWithCuda(int *c, const int *a, const int *b, unsigned int size)
{
	int *dev_a = 0;
	int *dev_b = 0;
	int *dev_c = 0;
	cudaError_t cudaStatus;

	// Choose which GPU to run on, change this on a multi-GPU system.
	cudaStatus = cudaSetDevice(0);
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaSetDevice failed!  Do you have a CUDA-capable GPU installed?");
		goto Error;
	}

	// Allocate GPU buffers for three vectors (two input, one output)    .
	cudaStatus = cudaMalloc((void**)&dev_c, size * sizeof(int));
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaMalloc failed!");
		goto Error;
	}

	cudaStatus = cudaMalloc((void**)&dev_a, size * sizeof(int));
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaMalloc failed!");
		goto Error;
	}

	cudaStatus = cudaMalloc((void**)&dev_b, size * sizeof(int));
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaMalloc failed!");
		goto Error;
	}

	// Copy input vectors from host memory to GPU buffers.
	cudaStatus = cudaMemcpy(dev_a, a, size * sizeof(int), cudaMemcpyHostToDevice);
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaMemcpy failed!");
		goto Error;
	}

	cudaStatus = cudaMemcpy(dev_b, b, size * sizeof(int), cudaMemcpyHostToDevice);
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaMemcpy failed!");
		goto Error;
	}

	// Launch a kernel on the GPU with one thread for each element.
	addKernel << <1, size >> >(dev_c, dev_a, dev_b);

	// Check for any errors launching the kernel
	cudaStatus = cudaGetLastError();
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "addKernel launch failed: %s\n", cudaGetErrorString(cudaStatus));
		goto Error;
	}

	// cudaDeviceSynchronize waits for the kernel to finish, and returns
	// any errors encountered during the launch.
	cudaStatus = cudaDeviceSynchronize();
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaDeviceSynchronize returned error code %d after launching addKernel!\n", cudaStatus);
		goto Error;
	}

	// Copy output vector from GPU buffer to host memory.
	cudaStatus = cudaMemcpy(c, dev_c, size * sizeof(int), cudaMemcpyDeviceToHost);
	if (cudaStatus != cudaSuccess) {
		fprintf(stderr, "cudaMemcpy failed!");
		goto Error;
	}

Error:
	cudaFree(dev_c);
	cudaFree(dev_a);
	cudaFree(dev_b);

	return cudaStatus;
}
