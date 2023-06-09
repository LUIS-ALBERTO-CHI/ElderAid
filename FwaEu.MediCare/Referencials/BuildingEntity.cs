using FwaEu.MediCare.MappingTransformer;

namespace FwaEu.MediCare.Referencials
{
    public class BuildingEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }


    public class BuildingEntityMappingTransformer
    {
        [MapToAttribute(nameof(BuildingEntity.Id))]
        public int Id { get; set; }
        [MapToAttribute(nameof(BuildingEntity.Name))]
        public string Nom { get; set; }
    }
}
