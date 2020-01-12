using System;

namespace BlueprintBreakdown
{
    readonly struct Resource : IEquatable<Resource>
    {
        public readonly string Type;

        public readonly string SubType;

        public string DisplayName
        {
            get
            {
                switch (this.SubType)
                {
                    case "Silicon": return "Silicon Wafer";
                    case "Magnesium": return "Magnesium Powder";
                    case "Stone":

                        if (this.Type == "Ingot")
                        {
                            return "Gravel";
                        }
                        else
                        {
                            return this.SubType;
                        }

                    default: return this.ToString();
                }
            }
        }

        public Resource(string type, string subtype)
        {
            this.Type = string.Intern(type);
            this.SubType = string.Intern(subtype);
        }

        public override int GetHashCode() => this.Type.GetHashCode() ^ this.SubType.GetHashCode();

        public override string ToString() => string.IsNullOrWhiteSpace(this.SubType) ? this.Type : this.SubType + " " + this.Type;

        public override bool Equals(object obj) => obj is Resource other && this == other;

        public bool Equals(Resource other) => this == other;

        public static bool operator ==(Resource a, Resource b) => string.Equals(a.Type, b.Type) && string.Equals(a.SubType, b.SubType);

        public static bool operator !=(Resource a, Resource b) => !(a == b);
    }
}
