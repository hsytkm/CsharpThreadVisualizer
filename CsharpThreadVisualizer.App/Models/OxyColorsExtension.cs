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
            OxyColors.Transparent,
            OxyColors.Azure,
            OxyColors.Beige,
            OxyColors.Bisque,
            OxyColors.BlanchedAlmond,
            OxyColors.Cornsilk,
        };

        private static readonly (string Name, OxyColor Color)[] _oxyColors =
            typeof(OxyColors).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(x => (x.Name, Color: (OxyColor)(x.GetValue(null) ?? OxyColors.Transparent)))
                .Where(x => !_exclusionColors.Contains(x.Color))
                .ToArray();

        public static ref OxyColor GetOxyColor(int index)
            => ref _oxyColors[index % _oxyColors.Length].Color;

        public static string GetName(int index)
            => _oxyColors[index % _oxyColors.Length].Name;
    }
}
