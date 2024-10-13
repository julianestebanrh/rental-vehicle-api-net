using System.Reflection;

namespace VehicleRental.Domain.Shared
{
    public abstract class Enumeration<T> : IEquatable<Enumeration<T>> where T : Enumeration<T>
    {
        private static readonly Dictionary<int, T> Enumerations = Initialize();
        public int Id { get; protected init; }
        public string Name { get; protected set; }

        public Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static T? FromValue(int id)
        {
            return Enumerations.TryGetValue(id, out T? enumeration) ? enumeration : default;
        }

        public static T? FromName(string name)
        {
            return Enumerations.Values.SingleOrDefault(x => x.Name == name);
        }

        public static List<T> GetValues()
        {
            return Enumerations.Values.ToList();
        }

        public bool Equals(Enumeration<T>? other)
        {
            if (other is null) return false;

            return GetType() == other.GetType() && Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Enumeration<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public static Dictionary<int, T> Initialize()
        {
            var enumerationType = typeof(T);

            var fieldsFormType = enumerationType.GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.FlattenHierarchy
                ).Where(field => enumerationType.IsAssignableFrom(field.FieldType))
                .Select(field => (T)field.GetValue(default)!);

            return fieldsFormType.ToDictionary(x => x.Id);
        }
    }
}
