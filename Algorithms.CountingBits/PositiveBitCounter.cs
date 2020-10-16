// <copyright file="PositiveBitCounter.cs" company="Payvision">
// Copyright (c) Payvision. All rights reserved.
// </copyright>

namespace Algorithms.CountingBits
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class PositiveBitCounter
    {
        public IEnumerable<int> Count(int input)
        {
            if (input < 0) throw new ArgumentException("Parameter must be a positive integer.");
            
            var oneBitCount = 0;  // number of 1's in binary representation
            int bitPosition = 0;   // bit position being evaluated
            List<int> positionList = new List<int>();
            
            while (input > 0)
            {
                if ((input & 1) == 1)
                {
                    oneBitCount++;
                    positionList.Add(bitPosition);
                }

                bitPosition++;
                input >>= 1;
            }

            return positionList.Prepend(oneBitCount);
        }
    }
}
