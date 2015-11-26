using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace gameofcodes
{
	public class Level
	{
		string path;

		public int[][] map;
		public int[][] map_copy;
		public int map_width;
		public int map_height;

		public int FinalScore = 0;

		public YellowSolution ys = new YellowSolution ();

		public List<Object> objects = new List<Object> ();

		private StreamWriter sw = null;



		public Level (string filePath, int index)
		{
			path = filePath;

			StreamReader sr = new StreamReader (this.path);
			sw = new StreamWriter ("level" + index + "solved.txt");

			string _line = sr.ReadLine ();
			string[] line = _line.Split (' ');

			map_height = Int32.Parse (line [0]);
			map_width = Int32.Parse (line [1]);
			map = new int[map_height + 1][];
			for (int i = 0; i < map.Length; i++) {
				map [i] = new int[map_width + 1];
			}

			for (int i = 1; i <= map_height; i++) {
				_line = sr.ReadLine ();
				line = _line.Split (' ');

				for (int j = 1; j <= map_width; j++)
					map [i] [j] = Int32.Parse (line [j - 1]);

			}
			DebugMap (map);
			Update ();
			Console.WriteLine ("After initial update of physics");
			UpdateAttack ();

			sr.Close ();

			for (int i = 1; i <= map_height; i++)
				for (int j = 1; j <= map_width; j++)
					if (map [i] [j] == 1)
						Attack (0, i, j);
					else if (map [i] [j] == 2) {
						Attack (0, i, j);
						Attack (0, i, j);
					}
						
		}


		public void Update ()
		{
			objects.Clear ();
			CalculateObjects ();

			while (ObjectCanFall ()) {
				foreach (Object o in objects) {
					o.UpdateFall ();
					UpdateMap ();
				}
			}
			objects.Clear ();
			CalculateObjects ();
			DebugMap (map);


		}

		private bool ObjectCanFall ()
		{
			foreach (Object o in objects) {
				if (o.canFall ())
					return true;
			}

			return false;
		}

		private void UpdateMap ()
		{
			for (int i = 1; i <= map_height; i++)
				for (int j = 1; j <= map_width; j++) {
					map [i] [j] = 0;
				}

			foreach (Object o in objects) {
				for (int i = 0; i < o.blocks.Count; i++) {
					map [o.blocks [i].i] [o.blocks [i].j] = o.blocks [i].value;
				}
			}
		}

		private void Fill (Object o, int i, int j)
		{
			if (map_copy [i] [j] != 0) {
				o.blocks.Add (new Block (i, j, map_copy [i] [j]));
				map_copy [i] [j] = 0;

				if (j - 1 > 0)
					Fill (o, i, j - 1);

				if (j + 1 <= map_width)
					Fill (o, i, j + 1);

				if (i - 1 > 0)
					Fill (o, i - 1, j);
				
				if (i + 1 <= map_height)
					Fill (o, i + 1, j);
			}
		}


		public void DebugMap (int[][] map)
		{
			Console.Write ("\n");
			for (int i = 1; i <= map_height; i++) {
				for (int j = 1; j <= map_width; j++)
					Console.Write (map [i] [j] + " ");
				Console.Write ("\n");
			}
		}

		private void CalculateObjects ()
		{
			map_copy = new int[map_height + 1][];

			for (int i = 0; i < map_copy.Length; i++) {
				map_copy [i] = new int[map_width + 1];
			}

			for (int i = 1; i <= map_height; i++) {
				for (int j = 1; j <= map_width; j++)
					map_copy [i] [j] = map [i] [j];

			}

			objects.Clear ();
			for (int i = 1; i <= map_height; i++) {
				for (int j = 1; j <= map_width; j++) {
					if (map_copy [i] [j] != 0) {
						Object o = new Object (map);
						Fill (o, i, j);
						objects.Add (o);
					}
				}
			}
		}



		public void DebugObjects ()
		{
			foreach (Object o in objects) {
				o.DebugBlocks ();
				Console.WriteLine ();
			}
		}

		#region AI

		// bird = 0 rosu
		//      = 1 yellow
		public void Attack (int bird, int i, int j)
		{
			if (map [i] [j] != 0) {
				if (bird == 0) {
					map [i] [j] -= 1;
				} else if (bird == 1) {

					map [i] [j] -= 1;


					if (j - 1 > 0 && map [i] [j - 1] != 0) {
						map [i] [j - 1] -= 1;
					}

					if (j + 1 <= map_width && map [i] [j + 1] != 0) {
						map [i] [j + 1] -= 1;
					}


					if (i - 1 > 0 && map [i - 1] [j] != 0) {
						map [i - 1] [j] -= 1;

					}

					if (i + 1 <= map_height && map [i + 1] [j] != 0) {
						map [i + 1] [j] -= 1;

					}

					// Colturi

					if ((j + 1 <= map_width && i + 1 <= map_height) &&
					    (map [i + 1] [j + 1] != 0)) {
						map [i + 1] [j + 1] -= 1;

					}

					if ((j - 1 > 0 && i + 1 <= map_height) &&
					    (map [i + 1] [j - 1] != 0)) {
						map [i + 1] [j - 1] -= 1;
					}
					if ((j + 1 <= map_width && i - 1 > 0) &&
					    (map [i - 1] [j + 1] != 0)) {
						map [i - 1] [j + 1] -= 1;
					}
					if ((j - 1 > 0 && i - 1 > 0) &&
					    (map [i - 1] [j - 1] != 0)) {
						map [i - 1] [j - 1] -= 1;
					}
				}
				Update ();


			}
		}

		public void UpdateAttack ()
		{

			while (objects.Count > 0) {
				RankYellow ();

				if (ys.positions [0].value >= 4) {
					Attack (1, ys.positions [0].i, ys.positions [0].j);
					Console.WriteLine ("After attack at: " + ys.positions [0].i + ", " + ys.positions [0].j + " with Yellow Bird");
					sw.WriteLine ("g " + ys.positions [0].i + " " + ys.positions [0].j);
					this.FinalScore += 20;
				} else {
					for (int i = map_width; i > 0; i--) {
						if (map [map_height] [i] != 0) {
							Attack (0, map_height, i);
							Console.WriteLine ("After attack at: " + map_height + ", " + i + " with Red Bird");
							sw.WriteLine ("r " + map_height + " " + i);
							this.FinalScore += 5;
						}
					}
				}



			}

			sw.Close ();
		}

		public void RankYellow ()
		{
			ys.positions.Clear ();


			for (int i = 1; i <= map_height; i++)
				for (int j = 1; j <= map_width; j++) {
					if (map [i] [j] != 0) {
						ys.AddPosition (i, j, CalculateRank (i, j));

					}
				}
			

			for (int i = 0; i < ys.positions.Count; i++) {
				for (int j = 0; j < ys.positions.Count; j++) {
					Position aux = new Position (0, 0, 0);
					if (ys.positions [i].value > ys.positions [j].value) {
						aux = ys.positions [i];
						ys.positions [i] = ys.positions [j];
						ys.positions [j] = aux;
					}
				}

			}
			/*
			for (int i = 0; i < ys.positions.Count; i++)
				Console.WriteLine (ys.positions [i].value);
				*/
		}

		public int CalculateRank (int i, int j)
		{
			int score = 1;

			if (j - 1 > 0 && map [i] [j - 1] != 0)
				score += 1;


			if (j + 1 <= map_width && map [i] [j + 1] != 0) {
				score += 1;
			}


			if (i - 1 > 0 && map [i - 1] [j] != 0)
				score += 1;

			if (i + 1 <= map_height && map [i + 1] [j] != 0)
				score += 1;


			// Colturi

			if ((j + 1 <= map_width && i + 1 <= map_height) &&
			    (map [i + 1] [j + 1] != 0))
				score += 1;

			if ((j - 1 > 0 && i + 1 <= map_height) &&
			    (map [i + 1] [j - 1] != 0))
				score += 1;

			if ((j + 1 <= map_width && i - 1 > 0) &&
			    (map [i - 1] [j + 1] != 0))
				score += 1;

			if ((j - 1 > 0 && i - 1 > 0) &&
			    (map [i - 1] [j - 1] != 0))
				score += 1;

			return score;


		}

		#endregion

	}
}

