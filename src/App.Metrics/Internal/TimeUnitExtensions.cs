﻿// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// ReSharper disable CheckNamespace
namespace App.Metrics
// ReSharper restore CheckNamespace
{
    internal static class TimeUnitExtensions
    {
        private static readonly long[,] ConversionFactors = BuildConversionFactorsMatrix();

        private static readonly IReadOnlyDictionary<string, TimeUnit> TimeUnitValueMapping =
            new ReadOnlyDictionary<string, TimeUnit>(new Dictionary<string, TimeUnit>
            {
                { "ns", TimeUnit.Nanoseconds },
                { "us", TimeUnit.Microseconds },
                { "ms", TimeUnit.Milliseconds },
                { "s", TimeUnit.Seconds },
                { "min", TimeUnit.Minutes },
                { "h", TimeUnit.Hours },
                { "days", TimeUnit.Days }
            });

        private static readonly IReadOnlyDictionary<TimeUnit, string> ValueTimeUnitMapping =
            new ReadOnlyDictionary<TimeUnit, string>(new Dictionary<TimeUnit, string>
            {
                { TimeUnit.Nanoseconds, "ns" },
                { TimeUnit.Microseconds, "us" },
                { TimeUnit.Milliseconds, "ms" },
                { TimeUnit.Seconds, "s" },
                { TimeUnit.Minutes, "min" },
                { TimeUnit.Hours, "h" },
                { TimeUnit.Days, "days" }
            });

        public static long Convert(this TimeUnit sourceUnit, TimeUnit targetUnit, long value)
        {
            if (sourceUnit == targetUnit)
            {
                return value;
            }

            return System.Convert.ToInt64(value * sourceUnit.ScalingFactorFor(targetUnit));
        }

        public static TimeUnit FromUnit(string unit)
        {
            if (!TimeUnitValueMapping.ContainsKey(unit))
            {
                throw new ArgumentOutOfRangeException(nameof(unit));
            }

            return TimeUnitValueMapping[unit];
        }

        public static double ScalingFactorFor(this TimeUnit sourceUnit, TimeUnit targetUnit)
        {
            if (sourceUnit == targetUnit)
            {
                return 1.0;
            }

            var sourceIndex = (int)sourceUnit;
            var targetIndex = (int)targetUnit;

            if (sourceIndex < targetIndex)
            {
                return 1 / (double)ConversionFactors[targetIndex, sourceIndex];
            }
            else
            {
                return ConversionFactors[sourceIndex, targetIndex];
            }
        }

        public static long ToDays(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Days, value);
        }

        public static long ToHours(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Hours, value);
        }

        public static long ToMicroseconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Microseconds, value);
        }

        public static long ToMilliseconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Milliseconds, value);
        }

        public static long ToMinutes(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Minutes, value);
        }

        public static long ToNanoseconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Nanoseconds, value);
        }

        public static long ToSeconds(this TimeUnit unit, long value)
        {
            return Convert(unit, TimeUnit.Seconds, value);
        }

        public static string Unit(this TimeUnit unit)
        {
            if (!ValueTimeUnitMapping.ContainsKey(unit))
            {
                throw new ArgumentOutOfRangeException(nameof(unit));
            }

            return ValueTimeUnitMapping[unit];
        }

        private static long[,] BuildConversionFactorsMatrix()
        {
            var count = Enum.GetValues(typeof(TimeUnit)).Length;

            var matrix = new long[count, count];
            var timingFactors = new[]
            {
                1000L, // Nanoseconds to microseconds
                1000L, // Microseconds to milliseconds
                1000L, // Milliseconds to seconds
                60L, // Seconds to minutes
                60L, // Minutes to hours
                24L // Hours to days
            };

            for (var source = 0; source < count; source++)
            {
                var cumulativeFactor = 1L;
                for (var target = source - 1; target >= 0; target--)
                {
                    cumulativeFactor *= timingFactors[target];
                    matrix[source, target] = cumulativeFactor;
                }
            }
            return matrix;
        }
    }
}