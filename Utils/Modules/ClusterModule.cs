/*
ClusterModule - Methods for clustering data for unsupervised learning.
BSD license.  
by Sven Nilsen, 2013
http://www.cutoutpro.com  
Version: 0.000 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

Redistribution and use in source and binary forms, with or without  
modification, are permitted provided that the following conditions are met:  
1. Redistributions of source code must retain the above copyright notice, this  
list of conditions and the following disclaimer.  
2. Redistributions in binary form must reproduce the above copyright notice,  
this list of conditions and the following disclaimer in the documentation  
and/or other materials provided with the distribution.  
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND  
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED  
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR  
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES  
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;  
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT  
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS  
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.  
The views and conclusions contained in the software and documentation are those  
of the authors and should not be interpreted as representing official policies,  
either expressed or implied, of the FreeBSD Project.  
*/

using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// Clustering works by taking some samples
	/// and work out which groups these samples belong to.
	/// 
	/// Centroid:
	/// 
	/// 	A centroid is a vector with dimension equal to the length of a dictionary.
	/// 	There are two kinds of centroids, one derived from sets and one derived from lists and then normalized.
	/// 	If we forget to normalize the clustering will depend on the size of data.
	/// 
	/// </summary>
	public static class ClusterModule
	{
		public static List<double> FromSample(int dictionaryLength, List<int> sample)
		{
			var centroid = new List<double>(dictionaryLength);
			for (int i = 0; i < dictionaryLength; i++)
			{
				centroid.Add(0.0);
			}
			int n = sample.Count;
			for (int j = 0; j < n; j++)
			{
				centroid[sample[j]]++;
			}
			
			return centroid;
		}
		
		public static Output FromSamples
			(
				Input clusterInput)
		{
			var samples = clusterInput.Samples;
			int dictionaryLength = clusterInput.DictionaryLength;
			var indexData = clusterInput.IndexData;
			int centroidLength = samples.Count;
			var centroids = new List<List<double>>(centroidLength);
			var centroidIndices = new List<List<int>>(centroidLength);
			// Initialize random centroids.
			for (int i = 0; i < centroidLength; i++)
			{
				centroids.Add(FromSample(dictionaryLength, indexData[samples[i]]));
				centroidIndices.Add(new List<int>());
			}
			
			return new Output(){Centroids = centroids, CentroidIndices = centroidIndices};
		}
		
		public static List<double> Average(int dictionaryLength, List<int> centroidIndices, List<List<int>> indexData)
		{
			var avg = new List<double>(dictionaryLength);
			for (int i = 0; i < dictionaryLength; i++)
			{
				avg.Add(0.0);
			}
			
			foreach (var indexDataInd in centroidIndices)
			{
				var index = indexData[indexDataInd];
				int n = index.Count;
				for (int i = 0; i < n; i++)
				{
					avg[index[i]]++;
				}
			}
			
			int m = centroidIndices.Count;
			for (int i = 0; i < dictionaryLength; i++)
			{
				avg[i] /= m;
			}
			
			return avg;
		}
		
		public static double Distance(List<int> index, List<double> centroid)
		{
			int n = centroid.Count;
			double sum = 0;
			for (int i = 0; i < n; i++)
			{
				double v = index.BinarySearch(i) < 0 ? 0.0 : 1.0;
				double dx = v - centroid[i];
				sum += dx * dx;
			}
			
			return Math.Sqrt(sum);
		}
		
		public static double Distance(List<double> a, List<double> b)
		{
			int n = a.Count;
			double sum = 0;
			for (int i = 0; i < n; i++)
			{
				double dx = a[i] - b[i];
				sum += dx * dx;
			}
			
			return Math.Sqrt(sum);
		}
		
		private static void FindClosest(Input clusterInput, Output clusterOutput)
		{
			var centroidIndices = clusterOutput.CentroidIndices;
			var indexData = clusterInput.IndexData;
			var centroids = clusterOutput.Centroids;
			int centroidsLength = centroids.Count;
			// Clear the current closest.
			foreach (var list in centroidIndices)
			{
				list.Clear();
			}
			
			int indexDataLength = indexData.Count;
			for (int j = 0; j < indexDataLength; j++)
			{
				var index = indexData[j];
				double minDist = double.MaxValue;
				int minIndex = -1;
				for (int i = 0; i < centroidsLength;    i++)
				{
					var centroid = centroids[i];
					double distance = Distance(index, centroid);
					if (distance < minDist)
					{
						minDist = distance;
						minIndex = i;
					}
				}
				
				if (minIndex == -1) {continue;}
				
				centroidIndices[minIndex].Add(j);
			}
		}
		
		private static bool UpdateCentroids(Input input, Output output)
		{
			var centroids = output.Centroids;
			int dictionaryLength = input.DictionaryLength;
			var centroidIndices = output.CentroidIndices;
			var indexData = input.IndexData;
			bool converged = true;
			int centroidLength = centroids.Count;
			for (int i = 0; i < centroidLength; i++)
			{
				var newAvg = Average(dictionaryLength, centroidIndices[i], indexData);
				var diff = Distance(newAvg, centroids[i]);
				if (diff > 0.0)
				{
					converged = false;
				} 
				
				centroids[i] = newAvg;
			}
			
			return converged;
		}
		
		public static Output Cluster(Input input)
		{
			var output = FromSamples(input);
			do
			{
				FindClosest(input, output);
			} while (!UpdateCentroids(input, output));
			
			return output;
		}
		
		public class Input
		{
			public int DictionaryLength;
			public List<List<int>> IndexData;
			public List<int> Samples;
		}
		
		public class Output
		{
			public List<List<double>> Centroids;
			public List<List<int>> CentroidIndices;
		}
		
	}
}
