using System.Threading;

namespace TestProgramWhileTrue
{
	public class Program
	{
		public static void Main(string[] args)
		{
			while (true)
			{
				Thread.Sleep(5000);
			}
		}
	}
}
