namespace Repository.Models
{
    public class Product : IEquatable<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Base64ImgOrUrl { get; set; }
        public override int GetHashCode() =>
            Id.GetHashCode() ^ (Name?.GetHashCode() ?? 0) ^ (Base64ImgOrUrl?.GetHashCode() ?? 0);

        public override bool Equals(object obj) =>
            obj is Product product && Equals(product);

        public bool Equals(Product other) =>
            other is not null && Id == other.Id && Name == other.Name && Base64ImgOrUrl == other.Base64ImgOrUrl;
    }
}
