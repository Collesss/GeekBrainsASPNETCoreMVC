namespace Repository.Models
{
    public class Product : IEquatable<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Base64Img { get; set; }
        public override int GetHashCode() =>
            Id.GetHashCode() ^ Name.GetHashCode();

        public override bool Equals(object obj) =>
            obj is Product product && Equals(product);

        public bool Equals(Product other) =>
            other is not null && Id == other.Id && Name == other.Name;
    }
}
