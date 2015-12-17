using System;

namespace gameofcodes
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			//nr de nivele totale
			int NumberOfLevels = 5;

			Level[] l = new Level[100];
		    
			/*

            Fisierul de intrare este in bin>debug>leveli.txt
            unde i este numarul hartii ( > 0)

			*/

			for (int i = 1; i <= NumberOfLevels; i++) {
				Console.WriteLine ("Started Level " + i);
				Console.WriteLine ("Initial map is: ");
				l [i] = new Level ("level" + (i) + ".txt", i);
				Console.WriteLine ();
				Console.WriteLine ("Finished Level " + i + ", Final Score is: " + l [i].FinalScore);
				Console.WriteLine ();
				Console.WriteLine ();
				Console.WriteLine ("########################################################################");
				Console.WriteLine ();
				Console.WriteLine ();
                Console.WriteLine("Level "+ i + " Finished Execution. Input any number to continue, 0 to exit.");
                if(Console.ReadLine() == "0")   break;
            }
		}
	}
}
