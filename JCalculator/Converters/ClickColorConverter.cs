﻿using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace JCalculator.Converters
{
	[ValueConversion(typeof(Color), typeof(Color))]
	public class ClickColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
