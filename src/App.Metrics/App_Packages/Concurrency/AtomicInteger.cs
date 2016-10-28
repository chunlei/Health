// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported to .NET Standard Library by Allan Hardy

// This is a collection of .NET concurrency utilities, inspired by the classes
// available in java. This utilities are Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET as described here
// https://github.com/etishor/ConcurrencyUtilities
// 
//
// Striped64 & LongAdder classes were ported from Java and had this copyright:
// 
// Written by Doug Lea with assistance from members of JCP JSR-166
// Expert Group and released to the public domain, as explained at
// http://creativecommons.org/publicdomain/zero/1.0/
// 
// Source: http://gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/jsr166e/Striped64.java?revision=1.8

//
// By default all added classes are internal to your assembly. 
// To make them public define you have to define the conditional compilation symbol CONCURRENCY_UTILS_PUBLIC in your project properties.
//

#pragma warning disable 1591

// ReSharper disable All

using System.Threading;

namespace App.Metrics.App_Packages.Concurrency
{
    /// <summary>
    ///     Atomic int value. Operations exposed on this class are performed using System.Threading.Interlocked class and are
    ///     thread safe.
    ///     For AtomicInt values that are stored in arrays PaddedAtomicInt is recommended.
    /// </summary>
    /// <remarks>
    ///     The AtomicInteger is a struct not a class and members of this type should *not* be declared readonly or changes
    ///     will not be reflected in the member instance.
    /// </remarks>
#if CONCURRENCY_UTILS_PUBLIC
public
#else
    internal
#endif
        struct AtomicInteger
    {
        private int _value;

        /// <summary>
        ///     Initializes a new instance with the specified <paramref name="value" />.
        /// </summary>
        /// <param name="value">Initial value of the instance.</param>
        public AtomicInteger(int value)
        {
            _value = value;
        }

        /// <summary>
        ///     Returns the latest value of this instance written by any processor.
        /// </summary>
        /// <returns>The latest written value of this instance.</returns>
        public int GetValue()
        {
            return Volatile.Read(ref _value);
        }

        /// <summary>
        ///     Returns the current value of the instance without using Volatile.Read fence and ordering.
        /// </summary>
        /// <returns>The current value of the instance in a non-volatile way (might not observe changes on other threads).</returns>
        public int NonVolatileGetValue()
        {
            return _value;
        }

        /// <summary>
        ///     Write a new value to this instance. The _value is immediately seen by all processors.
        /// </summary>
        /// <param name="value">The new value for this instance.</param>
        public void SetValue(int value)
        {
            Volatile.Write(ref _value, value);
        }

        /// <summary>
        ///     From the Java Version:
        ///     Eventually sets to the given value.
        ///     The semantics are that the write is guaranteed not to be re-ordered with any previous write,
        ///     but may be reordered with subsequent operations (or equivalently, might not be visible to other threads)
        ///     until some other volatile write or synchronizing action occurs).
        /// </summary>
        /// <remarks>
        ///     Currently implemented by calling Volatile.Write which is different from the java version.
        ///     Not sure if it is possible on CLR to implement
        /// </remarks>
        /// <param name="value">The new value for this instance.</param>
        public void LazySetValue(int value)
        {
            Volatile.Write(ref _value, value);
        }

        /// <summary>
        ///     Set the value without using Volatile.Write fence and ordering.
        /// </summary>
        /// <param name="value">The new value for this instance.</param>
        public void NonVolatileSetValue(int value)
        {
            _value = value;
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the resulting value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance + the amount added.</returns>
        public int Add(int value)
        {
            return Interlocked.Add(ref _value, value);
        }

        /// <summary>
        ///     Add <paramref name="value" /> to this instance and return the value this instance had before the add operation.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        /// <returns>The value of this instance before the amount was added.</returns>
        public int GetAndAdd(int value)
        {
            return Add(value) - value;
        }

        /// <summary>
        ///     Increment this instance and return the value the instance had before the increment.
        /// </summary>
        /// <returns>The value of the instance *before* the increment.</returns>
        public int GetAndIncrement()
        {
            return Increment() - 1;
        }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value the instance had before the increment.
        /// </summary>
        /// <returns>The value of the instance *before* the increment.</returns>
        public int GetAndIncrement(int value)
        {
            return Increment(value) - value;
        }

        /// <summary>
        ///     Decrement this instance and return the value the instance had before the decrement.
        /// </summary>
        /// <returns>The value of the instance *before* the decrement.</returns>
        public int GetAndDecrement()
        {
            return Decrement() + 1;
        }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value the instance had before the decrement.
        /// </summary>
        /// <returns>The value of the instance *before* the decrement.</returns>
        public int GetAndDecrement(int value)
        {
            return Decrement(value) + value;
        }

        /// <summary>
        ///     Increment this instance and return the value after the increment.
        /// </summary>
        /// <returns>The value of the instance *after* the increment.</returns>
        public int Increment()
        {
            return Interlocked.Increment(ref _value);
        }

        /// <summary>
        ///     Increment this instance with <paramref name="value" /> and return the value after the increment.
        /// </summary>
        /// <returns>The value of the instance *after* the increment.</returns>
        public int Increment(int value)
        {
            return Add(value);
        }

        /// <summary>
        ///     Decrement this instance and return the value after the decrement.
        /// </summary>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public int Decrement()
        {
            return Interlocked.Decrement(ref _value);
        }

        /// <summary>
        ///     Decrement this instance with <paramref name="value" /> and return the value after the decrement.
        /// </summary>
        /// <returns>The value of the instance *after* the decrement.</returns>
        public int Decrement(int value)
        {
            return Add(-value);
        }

        /// <summary>
        ///     Returns the current value of the instance and sets it to zero as an atomic operation.
        /// </summary>
        /// <returns>The current value of the instance.</returns>
        public int GetAndReset()
        {
            return GetAndSet(0);
        }

        /// <summary>
        ///     Returns the current value of the instance and sets it to <paramref name="newValue" /> as an atomic operation.
        /// </summary>
        /// <returns>The current value of the instance.</returns>
        public int GetAndSet(int newValue)
        {
            return Interlocked.Exchange(ref _value, newValue);
        }

        /// <summary>
        ///     Replace the value of this instance, if the current _value is equal to the <paramref name="expected" /> _value.
        /// </summary>
        /// <param name="expected">Value this instance is expected to be equal with.</param>
        /// <param name="updated">Value to set this instance to, if the current value is equal to the expected _value</param>
        /// <returns>True if the update was made, false otherwise.</returns>
        public bool CompareAndSwap(int expected, int updated)
        {
            return Interlocked.CompareExchange(ref _value, updated, expected) == expected;
        }
    }
}