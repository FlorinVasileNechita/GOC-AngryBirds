
using System;
using System.Collections.Generic;
using System.Linq;

namespace gameofcodes
{
	public class Object
	{

		public List<Block> blocks = new List<Block> ();
		private int[][] map;

		public Object (int[][] map)
		{
			this.map = map;
		}


		public bool canFall ()
		{
			int x = 0;
			for (int i = 0; i < blocks.Count; i++) {
				if (blocks [i].i != map.Length - 1) {
					if (map [blocks [i].i + 1] [blocks [i].j] == 0 || isMine (blocks [i].i + 1, blocks [i].j))
						x++;
				}
			}
			if (x == blocks.Count)
				return true;
			return false;
		}

		public void Fall ()
		{
			for (int i = 0; i < blocks.Count; i++) {
				blocks [i].i += 1;
			}
		}

		public void UpdateFall ()
		{
			while (canFall ()) {
				Fall ();
			}

		}

		private bool isMine (int i, int j)
		{
			foreach (Block _b in blocks)
				if (i == _b.i && j == _b.j)
					return true;
			return false;
		}

		public void DebugBlocks ()
		{
			for (int i = 0; i < blocks.Count; i++) {
				Console.WriteLine (blocks [i].i + ", " + blocks [i].j + ", " + blocks [i].value);

			}

		}

	}
}
