using System;

namespace BlueprintBreakdown
{
    readonly struct Component : IEquatable<Component>
    {
        public readonly string Type;

        public readonly string SubType;
        
        public string DisplayName
        {
            get 
            { 
                switch(this.SubType)
                {
                    case "Construction": return "Construction Components";
                    case "SteelPlate": return "Steel Plate";
                    case "InteriorPlate": return "Interior Plate";
                    case "SmallTube": return "Small Tube";
                    case "LargeTube": return "Large Tube";
                    case "Motor": return "Motors";
                    case "Computer": return "Computers";
                    case "MetalGrid": return "Metal Grids";
                    case "PowerCell": return "Power Cells";
                    case "BulletproofGlass": return "Bulletproof Glass";
                    case "SolarCell": return "Solar Cells";
                    case "Display": return "Displays";
                    case "Girder": return "Girders";
                    case "Thrust": return "Thruster Components";
                    case "Medical": return "Medical Components";
                    case "Reactor": return "Reactor Components";
                    case "Detector": return "Detector Components";
                    case "RadioCommunication": return "Radio-Communication Components";
                    case "GravityGenerator": return "Gravity Generator Components";

                    default: return this.ToString();
                             
                }
            }
        }

        public Component(string type, string subtype)
        {
            this.Type = string.Intern(type);
            this.SubType = string.Intern(subtype);
        }

        public override int GetHashCode() => this.Type.GetHashCode() ^ this.SubType.GetHashCode();

        public override string ToString() => string.IsNullOrWhiteSpace(this.SubType) ? this.Type : this.SubType;

        public override bool Equals(object obj) => obj is Component other && this == other;

        public bool Equals(Component other) => this == other;

        public static bool operator ==(Component a, Component b) => string.Equals(a.Type, b.Type) && string.Equals(a.SubType, b.SubType);

        public static bool operator !=(Component a, Component b) => !(a == b);
    }
}
