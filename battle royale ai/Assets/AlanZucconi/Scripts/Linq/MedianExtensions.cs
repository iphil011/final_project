using System;
using System.Collections.Generic;
using System.Linq;

// https://gist.github.com/axelheer/b1cb9d7c267d6762b244
public static class MedianExtensions
{
    // --- by Alan ---
    //public static T Percentile<T>(this IEnumerable<T> source, float percentile = 0.5f)
    public static float Percentile(this IEnumerable<float> source, float percentile = 0.5f)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.OrderBy(n => n).ToArray();
        if (data.Length == 0)
            throw new InvalidOperationException();

        float index = data.Length * percentile;
        if (IsInteger(index))
            return (data[(int)index - 1] + data[(int)index]) / 2.0f;
            //return ((dynamic)data[(int)index - 1] + data[(int)index]) / 2.0;
        return data[(int)index];
    }
    public static bool IsInteger (float f)
    {
        return (int)f == f;
    }

    public static float Percentile<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float percentile = 0.5f)
    {
        return source.Select(selector).Percentile(percentile);
    }
    /*
    public static T Percentile<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> selector, float percentile = 0.5f)
    {
        return source.Select(selector).Percentile(percentile);
    }
    */
    // -------------

    public static double Median(this IEnumerable<int> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.OrderBy(n => n).ToArray();
        if (data.Length == 0)
            throw new InvalidOperationException();
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0;
        return data[data.Length / 2];
    }
    
    public static double? Median(this IEnumerable<int?> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.Where(n => n.HasValue).Select(n => n.Value).OrderBy(n => n).ToArray();
        if (data.Length == 0)
            return null;
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0;
        return data[data.Length / 2];
    }
    
    public static double Median(this IEnumerable<long> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.OrderBy(n => n).ToArray();
        if (data.Length == 0)
            throw new InvalidOperationException();
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0;
        return data[data.Length / 2];
    }
    
    public static double? Median(this IEnumerable<long?> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.Where(n => n.HasValue).Select(n => n.Value).OrderBy(n => n).ToArray();
        if (data.Length == 0)
            return null;
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0;
        return data[data.Length / 2];
    }
    
    public static float Median(this IEnumerable<float> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.OrderBy(n => n).ToArray();
        if (data.Length == 0)
            throw new InvalidOperationException();
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0f;
        return data[data.Length / 2];
    }
    
    public static float? Median(this IEnumerable<float?> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.Where(n => n.HasValue).Select(n => n.Value).OrderBy(n => n).ToArray();
        if (data.Length == 0)
            return null;
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0f;
        return data[data.Length / 2];
    }
    
    public static double Median(this IEnumerable<double> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.OrderBy(n => n).ToArray();
        if (data.Length == 0)
            throw new InvalidOperationException();
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0;
        return data[data.Length / 2];
    }
    
    public static double? Median(this IEnumerable<double?> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.Where(n => n.HasValue).Select(n => n.Value).OrderBy(n => n).ToArray();
        if (data.Length == 0)
            return null;
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0;
        return data[data.Length / 2];
    }
    
    public static decimal Median(this IEnumerable<decimal> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.OrderBy(n => n).ToArray();
        if (data.Length == 0)
            throw new InvalidOperationException();
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0m;
        return data[data.Length / 2];
    }
    
    public static decimal? Median(this IEnumerable<decimal?> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        var data = source.Where(n => n.HasValue).Select(n => n.Value).OrderBy(n => n).ToArray();
        if (data.Length == 0)
            return null;
        if (data.Length % 2 == 0)
            return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2.0m;
        return data[data.Length / 2];
    }
    
    public static double Median<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
        return source.Select(selector).Median();
    }
    
    public static double? Median<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
        return source.Select(selector).Median();
    }
    
    public static double Median<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
    {
        return source.Select(selector).Median();
    }
    
    public static double? Median<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
        return source.Select(selector).Median();
    }
    
    public static float Median<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    {
        return source.Select(selector).Median();
    }
    
    public static float? Median<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
        return source.Select(selector).Median();
    }
    
    public static double Median<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
        return source.Select(selector).Median();
    }
    
    public static double? Median<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
        return source.Select(selector).Median();
    }
    
    public static decimal Median<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
        return source.Select(selector).Median();
    }
    
    public static decimal? Median<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
    {
        return source.Select(selector).Median();
    }
}