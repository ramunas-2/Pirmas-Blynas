
#include "cuda_runtime.h"
#include "device_launch_parameters.h"

#include <stdio.h>

cudaError_t addWithCuda(int *c, const int *a, const int *b, unsigned int size);


int maxGameOverPathLengthEvolution1;

int* mask1;
int mask0;

int* matrix1;
int matrix0;

double* wonCoins;
int wonCoinsLength;

int* path1;
int path0;

int* sMask4;
int k0mask4;
int k1mask4;
int k2mask4;
int k3mask4;
int kAmask4;

int* sMask3;
int k0mask3;
int k1mask3;
int k2mask3;
int kAmask3;

int* sMask2;
int k0mask2;
int k1mask2;
int kAmask2;

int* smatrix3;
int k0matrix3;
int k1matrix3;
int k2matrix3;
int kAmatrix3;

int* smatrix2;
int k0matrix2;
int k1matrix2;
int kAmatrix2;

double* smatrixCoins2;
int k0matrixCoins2;
int k1matrixCoins2;
int kAmatrixCoins2;

int* spath2;
int k0path2;
int k1path2;
int kApath2;
















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

		double _wonCoins[],
		int _wonCoinsLength,

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

		maxGameOverPathLengthEvolution1 = _maxGameOverPathLengthEvolution1;

		mask0 = _mask0;
		mask1 = new int[mask0];
		cudaMemcpy(mask1, _mask1, sizeof(int)*mask0, cudaMemcpyHostToHost);

		matrix0 = _matrix0;
		matrix1 = new int[matrix0];
		cudaMemcpy(matrix1, _matrix1, sizeof(int)*matrix0, cudaMemcpyHostToHost);

		wonCoinsLength = _wonCoinsLength;
		wonCoins = new double[wonCoinsLength];
		cudaMemcpy(wonCoins, _wonCoins, sizeof(double)*wonCoinsLength, cudaMemcpyHostToHost);

		path0 = _path0;
		path1 = new int[path0];
		cudaMemcpy(path1, _path1, sizeof(int)*path0, cudaMemcpyHostToHost);

		k0mask4 = _k0mask4;
		k1mask4 = _k1mask4;
		k2mask4 = _k2mask4;
		k3mask4 = _k3mask4;
		kAmask4 = _kAmask4;

		sMask4 = new int[kAmask4];
		cudaMemcpy(sMask4, _sMask4, sizeof(int)*kAmask4, cudaMemcpyHostToHost);

		k0mask3 = _k0mask3;
		k1mask3 = _k1mask3;
		k2mask3 = _k2mask3;
		kAmask3 = _kAmask3;

		sMask3 = new int[kAmask3];
		cudaMemcpy(sMask3, _sMask3, sizeof(int)*kAmask3, cudaMemcpyHostToHost);

		k0mask2 = _k0mask2;
		k1mask2 = _k1mask2;
		kAmask2 = _kAmask2;

		sMask2 = new int[kAmask2];
		cudaMemcpy(sMask2, _sMask2, sizeof(int)*kAmask2, cudaMemcpyHostToHost);

		k0matrix3 = _k0matrix3;
		k1matrix3 = _k1matrix3;
		k2matrix3 = _k2matrix3;
		kAmatrix3 = _kAmatrix3;

		smatrix3 = new int[kAmatrix3];
		cudaMemcpy(smatrix3, _smatrix3, sizeof(int)*kAmatrix3, cudaMemcpyHostToHost);

		k0matrix2 = _k0matrix2;
		k1matrix2 = _k1matrix2;
		kAmatrix2 = _kAmatrix2;

		smatrix2 = new int[kAmatrix2];
		cudaMemcpy(smatrix2, _smatrix2, sizeof(int)*kAmatrix2, cudaMemcpyHostToHost);

		k0matrixCoins2 = _k0matrixCoins2;
		k1matrixCoins2 = _k1matrixCoins2;
		kAmatrixCoins2 = _kAmatrixCoins2;

		smatrixCoins2 = new double[kAmatrixCoins2];
		cudaMemcpy(smatrixCoins2, _smatrixCoins2, sizeof(double)*kAmatrixCoins2, cudaMemcpyHostToHost);

		k0path2 = _k0path2;
		k1path2 = _k1path2;
		kApath2 = _kApath2;

		spath2 = new int[kApath2];
		cudaMemcpy(spath2, _spath2, sizeof(int)*kApath2, cudaMemcpyHostToHost);





	}

	__declspec(dllexport) double calculateAntiPlayerExternal(double brainCells[], int allBrainCellsCount)
	{
		
		for (int i0 = 0; i0 < matrix0; i0++)
		{
			double sum = 0.0;
			for (int i1 = 0; i1 < matrix1[i0]; i1++)
			{
				double multiplication = smatrixCoins2[(i0 * k1matrix2) + i1];
				for (int i2 = 0; i2 < smatrix2[(i0 * k1matrix2) + i1]; i2++)
				{
					multiplication *= brainCells[smatrix3[(((i0 * k1matrix3) + i1) * k2matrix3) + i2]];
				}
				sum += multiplication;
			}
			wonCoins[i0] = sum;
		}
		
		int pathLength = maxGameOverPathLengthEvolution1;
		for (int i0 = 0; i0 < mask0; i0++)
		{
			for (int i1 = 0; i1 < mask1[i0]; i1++)
			{
				double bestOptionABenefit = -1000000000; //to fix
				int bestOptionAChoiceLocation = -1;
				for (int i2 = 0; i2 < sMask2[(i0 * k1mask2) + i1]; i2++)
				{
					double sumOfAllDiceCombinationsBenefit = 0;
					for (int i3 = 0; i3 < sMask3[(((i0 * k1mask3) + i1) * k2mask3) + i2]; i3++)
					{
						sumOfAllDiceCombinationsBenefit += wonCoins[sMask4[(((((i0 * k1mask4) + i1) * k2mask4) + i2) * k3mask4) + i3]];

					}
					//if (bestOptionABenefit == null || sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
					if (sumOfAllDiceCombinationsBenefit > bestOptionABenefit)
					{
						bestOptionABenefit = sumOfAllDiceCombinationsBenefit;
						bestOptionAChoiceLocation = i2;
					}
				}
				for (int i2 = 0; i2 < sMask2[(i0 * k1mask2) + i1]; i2++)
				{
					for (int i3 = 0; i3 < sMask3[(((i0 * k1mask3) + i1) * k2mask3) + i2]; i3++)
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
						brainCells[spath2[((sMask4[(((((i0 * k1mask4) + i1) * k2mask4) + i2) * k3mask4) + i3]) * k1path2) + pathLength - 1]] = newBrainCellValue;
						wonCoins[sMask4[(((((i0 * k1mask4) + i1) * k2mask4) + i2) * k3mask4) + i3]] *= newBrainCellValue;
					}
				}
			}
			pathLength--;
		}
		double sumOfCoins = 0;
		for (int i = 0; i < wonCoinsLength; i++)
		{
			sumOfCoins += wonCoins[i];
		}
		return sumOfCoins;
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
