using System;

namespace gameofcodes
{
	public class Block
	{
		public int value;
		public int i, j;

		public Block (int i, int j, int val)
		{
			this.value = val;
			this.i = i;
			this.j = j;
		}
	}
}

