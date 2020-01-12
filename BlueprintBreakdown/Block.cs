using System;

namespace BlueprintBreakdown
{
    readonly struct Block : IEquatable<Block>
    {
        public readonly string Type;

        public readonly string SubType;

        public Block(string type, string subtype)
        {
            this.Type = string.Intern(type);
            this.SubType = string.Intern(subtype);
        }

        public override int GetHashCode() => this.Type.GetHashCode() ^ this.SubType.GetHashCode();

        public override string ToString() => string.IsNullOrWhiteSpace(this.SubType) ? this.Type : this.SubType;

        public override bool Equals(object obj) => obj is Block other && this == other;

        public bool Equals(Block other) => this == other;

        public static bool operator ==(Block a, Block b) => string.Equals(a.Type, b.Type) && string.Equals(a.SubType, b.SubType);

        public static bool operator !=(Block a, Block b) => !(a == b);
    }
}
