using System;
using System.Runtime.InteropServices;

namespace AoC.Common
{
	/// <summary>
	/// PerformanceTimer 
	/// </summary>
	/// <created>02/06/2009</created>
	/// <author>Thomas_Bates</author>
	public class PerformanceTimer
	{
		private long _timer;
		private long _count;


		#region Imported methods


		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);


		#endregion
		#region Static Support Methods


		/// <summary>
		/// Inits the specified timer.
		/// </summary>
		/// <param name="timer">The timer.</param>
		/// <created>02/06/2009</created>
		/// <author>Thomas_Bates</author>
		public static void Init(ref long timer)
		{
			timer = 0;
		}

		/// <summary>
		/// Starts the specified timer.
		/// </summary>
		/// <param name="timer">The timer.</param>
		/// <created>02/06/2009</created>
		/// <author>Thomas_Bates</author>
		public static void Start(ref long timer)
		{
			QueryPerformanceCounter(out long counter);
			timer -= counter;
		}

		/// <summary>
		/// Stops the specified timer.
		/// </summary>
		/// <param name="timer">The timer.</param>
		/// <created>02/06/2009</created>
		/// <author>Thomas_Bates</author>
		public static void Stop(ref long timer)
		{
			QueryPerformanceCounter(out long counter);
			timer += counter;
		}

		/// <summary>
		/// Shows the specified timer.
		/// </summary>
		/// <param name="timer">The timer.</param>
		/// <param name="count">The count.</param>
		/// <param name="description">The description.</param>
		/// <created>02/06/2009</created>
		/// <author>Thomas_Bates</author>
		public static string Show(long timer, long count, string description)
		{
			QueryPerformanceFrequency(out long frequency);
			count = Math.Max(count, 1);
			string result = string.Format("{0:F6} [s] / {1} [iter.] = {2:F6} [us/iter.]  -  {3}",
								timer * 1.0 / frequency, count, timer * 1000000.0 / (frequency * count), description);
			return result;
		}


		#endregion
		#region Public Access Methods


		/// <summary>
		/// Resets this instance.
		/// </summary>
		/// <created>02/06/2009</created>
		/// <author>Thomas_Bates</author>
		public void Reset()
		{
			Init(ref _timer);
			_count = 0;
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		/// <created>02/06/2009</created>
		/// <author>Thomas_Bates</author>
		public void Start()
		{
			Start(ref _timer);
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		/// <created>02/06/2009</created>
		/// <author>Thomas_Bates</author>
		public void Stop()
		{
			Stop(ref _timer);
			_count++;
		}

		/// <summary>
		/// Shows the specified description.
		/// </summary>
		/// <param name="description">The description.</param>
		/// <created>02/06/2009</created>
		/// <author>Thomas_Bates</author>
		public string Show(string description)
		{
			string result = Show(_timer, _count, description);
			return result;
		}


		#endregion
	}
}
