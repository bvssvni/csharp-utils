/*

Utils.PathFinding - Methods for making path finding easier.
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
using Utils;
using Play;
using Cairo;

namespace Utils.PathFinding
{
	public class PathFindingHelper
	{
		public PathFindingHelper()
		{
		}
		
		private PixelMap m_map;
		private PathFindingTask m_pathFinding;
		private bool[] m_mask;
		private IndexHeap m_heap;
		private PathNodes m_pathNodes;
		
		public void Step1_SetMap(PixelMap map) {
			m_map = map;
			m_mask = m_map.CreateMask();
			m_heap = m_map.CreateHeap();
			m_pathNodes = m_map.CreatePathNodes();
		}
		
		public void Step2_SetPathFinding(PathFindingTask pathFinding) {
			m_pathFinding = pathFinding;
		}
		
		public List<int> Step3_Search() {
			return PathFindingModule.SearchMap(m_map, m_pathFinding, m_mask, m_heap, m_pathNodes);
		}
	}
	
	public class PathFindingTask
	{
		public int StartX;
		public int StartY;
		public int TargetX;
		public int TargetY;
	}
	
	public class PixelMap
	{
		public int Width;
		public int Height;
		public Group Walls;
		
		public PixelMap(ImageSurface Image)
		{
			this.Width = Image.Width;
			this.Height = Image.Height;
			Walls = Utils.PixelGroupModule.FromColor(Image, new Color(0, 0, 0, 1));
		}
		
		public bool[] CreateMask() {
			return new bool[Width * Height];
		}
		
		public IndexHeap CreateHeap() {
			return new IndexHeap(Width * Height);
		}
		
		public PathNodes CreatePathNodes() {
			return new PathNodes(Width, Height);
		}
	}
	
	public class PathNodes
	{
		public struct Info
		{
			public int PreviousId;
			public int Travelled;
			public int DistanceToGoal;
		}
		
		public Info[] Data;
		public int Width;
		public int Height;
		
		public PathNodes(int width, int height)
		{
			this.Data = new Info[width * height];
			this.Width = width;
			this.Height = height;
		}
	}
	
	public class PathFindingModule
	{
		
		public static PathFindingTask TaskFromImage(Cairo.ImageSurface image) {
			var task = new PathFindingTask();
			var start = Utils.CairoPixelPositionModule.First(image, new Cairo.Color(0, 0, 1, 1));
			var goal = Utils.CairoPixelPositionModule.First(image, new Cairo.Color(1, 0, 0, 1));
			task.StartX = start.X;
			task.StartY = start.Y;
			task.TargetX = goal.X;
			task.TargetY = goal.Y;
			return task;
		}
		
		private delegate bool IsVisitedDelegate(int x, int y);
		private delegate int AddDelegate(int x, int y);
		
		public static List<int> SearchMap(PixelMap map, PathFindingTask pathFinding, bool[] mask, IndexHeap heap, PathNodes nodes) {
			var width = map.Width;
			var height = map.Height;
			var goalPosX = pathFinding.TargetX;
			var goalPosY = pathFinding.TargetY;
			
			heap.Compare = (int a, int b) => 
				(nodes.Data[a].Travelled + nodes.Data[a].DistanceToGoal).CompareTo(
					nodes.Data[b].Travelled + nodes.Data[b].DistanceToGoal);
			
			
			IsVisitedDelegate IsVisited = (int x, int y) =>
				x < 0 || y < 0 || x >= width || y >= height || mask[x + y * width];
			
			// Contains the id of previous node.
			int node = -1;
			
			AddDelegate Add = delegate(int x, int y) {
				int id = x + y * width;
				nodes.Data[id].PreviousId = node;
				
				int dx = goalPosX - x;
				int dy = goalPosY - y;
				
				// Use square of length because it is faster.
				int dist = dx * dx + dy * dy;
				nodes.Data[id].DistanceToGoal = dist;
				
				if (node == -1) {
					nodes.Data[id].Travelled = 0;
				} else {
					dx = x - node % width;
					dy = y - node / width;
					
					// Use square of length because it is faster.
					dist = dx * dx + dy * dy;
					
					nodes.Data[id].Travelled = nodes.Data[node].Travelled + dist;
				}
				
				return id;
			};
			
			var start = Add(pathFinding.StartX, pathFinding.StartY);
			heap.Push(start);
			
			// Set values from the walls to the mask.
			mask.Initialize();
			var walls = map.Walls;
			int walls_length = walls.Count >> 1;
			int i, j, e, s;
			for (i = walls_length - 1; i >= 0; i--) {
				s = walls[i << 1];
				e = walls[(i << 1) + 1];
				for (j = e - 1; j >= s; j--) {
					mask[j] = true;
				}
			}
			
			mask[start] = true;
			
			int n;
			int nodex;
			int nodey;
			while (heap.Cursor > 1) {
				node = heap.Pop();
				nodey = node / width;
				nodex = node - nodey * width;
				if (nodex == goalPosX &&
				    nodey == goalPosY) {
					var list = new List<int>();
					while (nodes.Data[node].PreviousId != -1) {
						list.Add(node);
						node = nodes.Data[node].PreviousId;
					}
					return list;
				}
				
				if (!IsVisited(nodex + 1, nodey)) {
					n = Add(nodex + 1, nodey);
					heap.Push(n);
					mask[n] = true;
				}
				if (!IsVisited(nodex, nodey + 1)) {
					n = Add(nodex, nodey + 1);
					heap.Push(n);
					mask[n] = true;
				}
				if (!IsVisited(nodex - 1, nodey)) {
					n = Add(nodex - 1, nodey);
					heap.Push(n);
					mask[n] = true;
				}
				if (!IsVisited(nodex, nodey - 1)) {
					n = Add(nodex, nodey - 1);
					heap.Push(n);
					mask[n] = true;
				}
			}
			
			return null;
		}
	}
}

