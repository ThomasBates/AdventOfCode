using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AoC.Common;

namespace AoC.Main
{
	public class TextViewModel : ViewModel
	{
		private string _text;

		public TextViewModel(string text)
		{
			_text= text;
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (string.Equals(_text, value))
				{
					return;
				}
				_text = value;
				NotifyPropertyChanged();
			}
		}
	}
}
