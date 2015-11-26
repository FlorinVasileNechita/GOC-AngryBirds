using System;
using System.Collections.Generic;

namespace gameofcodes
{


	public class YellowSolution
	{
		public List<Position> positions = new List<Position> ();

		public void AddPosition (int i, int j, int v)
		{
			positions.Add (new Position (i, j, v));
		}
	}
}

