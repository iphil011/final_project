using System.Collections.Generic;

using System;
using System.Linq;

public static class LinqExtension
{
    // https://stackoverflow.com/questions/3188693/how-can-i-get-linq-to-return-the-object-which-has-the-max-value-for-a-given-prop/3188751
    public static T MinBy<T>(this IEnumerable<T> list, Func<T, float> value)
    {
        return list.Aggregate
        (
            (a, b) =>
                value(a) < value(b)
                ? a : b
        );
    }

    public static T MaxBy<T>(this IEnumerable<T> list, Func<T, float> value)
    {
        return list.Aggregate
        (
            (a, b) =>
                value(a) > value(b)
                ? a : b
        );
    }

    // https://stackoverflow.com/questions/275073/why-do-c-sharp-multidimensional-arrays-not-implement-ienumerablet
    public static IEnumerable<T> Flatten<T>(this Array target)
    {
        foreach (var item in target)
            yield return (T)item;
    }

    // https://stackoverflow.com/questions/4823467/using-linq-to-find-the-cumulative-sum-of-an-array-of-numbers-in-c-sharp
    public static IEnumerable<float> CumulativeSum(this IEnumerable<float> sequence)
    {
        float sum = 0;
        foreach (var item in sequence)
        {
            sum += item;
            yield return sum;
        }
    }

    // probability is a function that gets the absolute probability of an element T
    public static T RandomProbability<T> (this IEnumerable<T> list, Func<T, float> probability)
    {
        var cumulativeProbabilities = list
            // Replace each element with a tuple <item, probability>
            .Select(item => new Tuple<T, float>(item, probability(item)))
            // Replaces probability with cumulative probability
            .SelectAggregate
            (
                new Tuple<T, float>(default(T), 0f), // Seed
                (aggregate, next) => new Tuple<T, float>(next.first, next.second+aggregate.second)
            );

        // https://stackoverflow.com/questions/46735106/pick-random-element-from-list-with-probability
        float random = UnityEngine.Random.Range(0f, 1f);
        var selected = cumulativeProbabilities.SkipWhile(i => i.second < random).First();
        return selected.first;
    }

    // https://stackoverflow.com/questions/4823467/using-linq-to-find-the-cumulative-sum-of-an-array-of-numbers-in-c-sharp
    public static IEnumerable<TAccumulate> SelectAggregate<TSource, TAccumulate>(
    this IEnumerable<TSource> source,
    TAccumulate seed,
    Func<TAccumulate, TSource, TAccumulate> func)
    {
        return source.SelectAggregateIterator(seed, func);
    }

    private static IEnumerable<TAccumulate> SelectAggregateIterator<TSource, TAccumulate>(
        this IEnumerable<TSource> source,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func)
    {
        TAccumulate previous = seed;
        foreach (var item in source)
        {
            TAccumulate result = func(previous, item);
            previous = result;
            yield return result;
        }
    }



    // https://www.c-sharpcorner.com/forums/ranking-items-in-a-list-with-linq
    // Gets the top k
    public static IEnumerable<T> Rank<T>(this IEnumerable<T> sequence, Func<T, float> sorter, int k)
    {
        return sequence
            .OrderByDescending(item => sorter(item))
            .Take(k);
    }
}
