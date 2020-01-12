using System.Collections.Generic;

namespace BlueprintBreakdown
{
    sealed class Recipe<Part>
    {
        private Dictionary<Part, double> parts;

        public Recipe()
        {
            this.parts = new Dictionary<Part, double>();
        }

        public double GetPartAmount(Part part)
        {
            if (this.parts.TryGetValue(part, out var count))
            {
                return count;
            }
            return 0;
        }

        public double GetPartAmount(Part part, int efficiency)
        {
            if (this.parts.TryGetValue(part, out var count))
            {
                return count / efficiency;
            }
            return 0;
        }

        public ICollection<Part> GetParts() => this.parts.Keys;

        public ICollection<KeyValuePair<Part, double>> GetPartAmounts() => this.parts;

        public void Add(Part part, double count)
        {
            if (parts.TryGetValue(part, out var current))
            {
                parts[part] = current + count;
            }
            else
            {
                this.parts.Add(part, count);
            }
        }

        public void Set(Part part, double count)
        {
            if (parts.TryGetValue(part, out var current))
            {
                parts[part] = count;
            }
            else
            {
                this.parts.Add(part, count);
            }
        }

        public void Combine(Recipe<Part> recipe, int count)
        {
            foreach(var kvp in recipe.parts)
            {
                this.Add(kvp.Key, kvp.Value * count);
            }
        }
    }
}
