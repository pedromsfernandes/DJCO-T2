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

        private readonly List<int> _accumulatedTypes = new List<int>();
        internal int _type;

        public static LightColor Of(LightType type)
        {
            return new LightColor((int) type);
        }

        private LightColor(int type)
        {
            _type = type;
        }
 
        public LightColor AddColor(LightColor color)
        {
            _accumulatedTypes.Add(color._type);
            _type |= color._type;
            return this;
        }
        
        public LightColor RemoveColor(LightColor color)
        {
            _accumulatedTypes.Remove(color._type);
            _type = _accumulatedTypes.Aggregate(0, (acc, type) => acc | type);
            return this;
        }

        public Color GetColor()
        {
            return Colors[(_type & 0b100) >> 2][(_type & 0b010) >> 1][_type & 0b001];
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            switch (obj)
            {
                case LightColor lightColor :
                    return _type == lightColor._type;
                case LightType lightType:
                    return _type == (int) lightType;
                default:
                    return false;
            } 
        }
        
        public override int GetHashCode()
        {
            return _type;
        }
    }
}
