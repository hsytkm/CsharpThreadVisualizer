using OxyPlot;
using System;
using System.Linq;
using System.Reflection;

namespace CsharpThreadVisualizer.App.Models
{
    static class OxyColorsExtension
    {
        private static readonly OxyColor[] _exclusionColors = new[]
        {
            OxyColors.Undefined,
            OxyColors.Transparent
        };

        private static readonly OxyColor[] _oxyColors =
            typeof(OxyColors).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField)
                .Select(x => x.GetValue(null))
                .Select(x => x is null ? OxyColors.Black : (OxyColor)x)
                .Where(x => !_exclusionColors.Contains(x))
                .ToArray();

        public static ref OxyColor FromIndex(int index)
            => ref _oxyColors[index % _oxyColors.Length];
    }
}
