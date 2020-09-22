using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Component
{
    public class ProductoDescriptionSearchFilter : ModelLocalSearchFilter<Producto>
    {
        public override bool Filter(Producto element, string query)
        {
            return element.Description?.ToLower().Contains(query.ToLower()) ?? false; ;
        }
    }
}