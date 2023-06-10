﻿using System.Reflection;

namespace InvesTime.BackEnd.Helpers;

public static class ObjectConverter
{
    public static TTarget Convert<TSource, TTarget>(TSource source)
        where TSource : class
        where TTarget : class, new()
    {
        var target = new TTarget();

        var sourceType = typeof(TSource);
        var targetType = typeof(TTarget);

        PropertyInfo[] sourceProperties = sourceType.GetProperties();
        PropertyInfo[] targetProperties = targetType.GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            var targetProperty = Array.Find(targetProperties, prop => prop.Name == sourceProperty.Name);

            if (targetProperty != null)
            {
                var value = sourceProperty.GetValue(source);
                targetProperty.SetValue(target, value);
            }
        }

        return target;
    }
}
