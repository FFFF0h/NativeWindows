using System;

namespace TestProgramMemoryUsage
{
	public class Program
	{
		public static int Main(string[] args)
		{
			var data = new byte[1000][];
			try
			{
				for (int i = 0; i < 1000; i++)
				{
					data[i] = new byte[1024 * 1024];
					for (int j = 0; j < data[i].Length; j++)
					{
						data[i][j] = (byte) (j % 255);
					}
					Console.WriteLine("{0} MB allocated", i);
				}
			}
			catch(OutOfMemoryException)
			{
				Console.WriteLine("Out of memory");
				return -1;
			}
			return 0;
		}
	}
}
