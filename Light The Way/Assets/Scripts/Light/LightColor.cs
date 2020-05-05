using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Light
{
    public enum LightType
    {
        None    = 0b000,
        Red     = 0b100,
        Green   = 0b010,
        Blue    = 0b001
    }
    public class LightColor
    {
        private static readonly Color[][][] Colors = 
        {
            // No Red
            new[]
            {
                // No Green
                new[]
                {
                    // No Blue
                    new Color(0, 0, 0, 0),
                    // Blue
                    new Color(0, 0, 0.5f, 1)
                    
                },
                // Green
                new[] 
                {
                    // No Blue
                    new Color(0, 0.5f, 0, 1),
                    // Blue
                    new Color(0, 1, 1, 1)
                }
            },
            // Red
            new[]
            {
                // No Green
                new[] 
                {
                    // No Blue
                    new Color(0.5f, 0, 0, 1),
                    // Blue
                    new Color(1, 0, 0.7f, 1)
                },
                // Green
                new[] 
                {
                    // No Blue
                    new Color(1, 1, 0, 1),
                    // Blue
                    new Color(1, 1, 1, 1)
                }
            }
        };
        
        internal int Type { get; private set; }

        public static LightColor Of(LightType type)
        {
            return new LightColor((int) type);
        }

        private LightColor(int type)
        {
            Type = type;
        }
 
        public LightColor AddColor(LightColor color)
        {
            Type |= color.Type;
            return this;
        }

        public Color GetColor()
        {
            return Colors[(Type & 0b100) >> 2][(Type & 0b010) >> 1][Type & 0b001];
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            switch (obj)
            {
                case LightColor lightColor :
                    return Type == lightColor.Type;
                case LightType lightType:
                    return Type == (int) lightType;
                default:
                    return false;
            } 
        }
        
        public override string ToString()
        {
            return $"{nameof(Type)}: {(LightType) Type}";
        }

        public override int GetHashCode()
        {
            return Type;
        }
    }
}
